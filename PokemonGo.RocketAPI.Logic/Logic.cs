using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllEnum;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Extensions;
using PokemonGo.RocketAPI.GeneratedCode;
using PokemonGo.RocketAPI.Logic.Utils;

namespace PokemonGo.RocketAPI.Logic
{
    public class Logic
    {
        private readonly Client _client;
        private readonly ISettings _clientSettings;
        private readonly Inventory _inventory;
        private readonly Statistics _stats;

        public Logic(ISettings clientSettings)
        {
            _clientSettings = clientSettings;
            _client = new Client(_clientSettings);
            _inventory = new Inventory(_client);
            _stats = new Statistics();
        }

        public async void Execute()
        {
            Logger.Normal(ConsoleColor.DarkGreen, $"Starting Execute on login server: {_clientSettings.AuthType}");

            if (_clientSettings.AuthType == AuthType.Ptc)
                await _client.DoPtcLogin(_clientSettings.PtcUsername, _clientSettings.PtcPassword);
            else if (_clientSettings.AuthType == AuthType.Google)
                await _client.DoGoogleLogin();
            Logger.Normal(ConsoleColor.DarkGreen, $"Client logged in");

            while (true)
            {
                try
                {
                    await _client.SetServer();
                    await TransferDuplicatePokemon(true);
                    await RecycleItems();
                    await RepeatAction(10, async () => await ExecuteFarmingPokestopsAndPokemons(_client));

                    /*
                * Example calls below
                *
                var profile = await _client.GetProfile();
                var settings = await _client.GetSettings();
                var mapObjects = await _client.GetMapObjects();
                var inventory = await _client.GetInventory();
                var pokemons = inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.Pokemon).Where(p => p != null && p?.PokemonId > 0);
                */
                }
                catch (Exception ex)
                {
                    Logger.Error($"Exception: {ex}");
                }

                await Task.Delay(10000);
            }
        }

        public async Task RepeatAction(int repeat, Func<Task> action)
        {
            for (int i = 0; i < repeat; i++)
                await action();
        }

        private async Task ExecuteFarmingPokestopsAndPokemons(Client client)
        {
            var mapObjects = await client.GetMapObjects();
            var pokeStops = mapObjects.MapCells.SelectMany(i => i.Forts).Where(i => i.Type == FortType.Checkpoint && i.CooldownCompleteTimestampMs < DateTime.UtcNow.ToUnixTime());
            Logger.Normal(ConsoleColor.Green, $"Found {pokeStops.Count()} pokestops");

            foreach (var pokeStop in pokeStops)
            {
                await ExecuteCatchAllNearbyPokemons(client);
                await TransferDuplicatePokemon(true);
                
                var update = await client.UpdatePlayerLocation(pokeStop.Latitude, pokeStop.Longitude);
                var fortInfo = await client.GetFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);
                var fortSearch = await client.SearchFort(pokeStop.Id, pokeStop.Latitude, pokeStop.Longitude);

                _stats.addExperience(fortSearch.ExperienceAwarded);
                _stats.updateConsoleTitle();

                Logger.Normal(ConsoleColor.Cyan, $"Using Pokestop: {fortInfo.Name}");
                Logger.Normal(ConsoleColor.Cyan, $"Received XP: {fortSearch.ExperienceAwarded}, Gems: { fortSearch.GemsAwarded}, Eggs: {fortSearch.PokemonDataEgg} Items: {StringUtils.GetSummedFriendlyNameOfItemAwardList(fortSearch.ItemsAwarded)}");

                await Task.Delay(8000);
            }
            await RecycleItems();
        }

        private async Task ExecuteCatchAllNearbyPokemons(Client client)
        {
            var mapObjects = await client.GetMapObjects();
            var pokemons = mapObjects.MapCells.SelectMany(i => i.CatchablePokemons);
            if (pokemons != null && pokemons.Any())
                Logger.Normal(ConsoleColor.Green, $"Found {pokemons.Count()} catchable Pokemon");

            foreach (var pokemon in pokemons)
            {
                var update = await client.UpdatePlayerLocation(pokemon.Latitude, pokemon.Longitude);
                var encounterPokemonResponse = await client.EncounterPokemon(pokemon.EncounterId, pokemon.SpawnpointId);
                var pokemonCP = encounterPokemonResponse?.WildPokemon?.PokemonData?.Cp;
                var pokeball = await GetBestBall(pokemonCP);
                if (pokeball == MiscEnums.Item.ITEM_UNKNOWN)
                {
                    Logger.Normal($"You don't own any Pokeballs :( - We missed a {pokemon.PokemonId} with CP {encounterPokemonResponse?.WildPokemon?.PokemonData?.Cp}");
                    return;
                }
                var balls_used = 0;

                CatchPokemonResponse caughtPokemonResponse;
                do
                {
                    if (encounterPokemonResponse?.CaptureProbability.CaptureProbability_.First() < 0.4)
                        await UseBerry(pokemon.EncounterId, pokemon.SpawnpointId);
                    caughtPokemonResponse = await client.CatchPokemon(pokemon.EncounterId, pokemon.SpawnpointId, pokemon.Latitude, pokemon.Longitude, pokeball);
                    balls_used++;
                }
                while (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed);

                foreach (int xp in caughtPokemonResponse.Scores.Xp)
                    _stats.addExperience(xp);
                if (caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                    _stats.increasePokemons();
                _stats.updateConsoleTitle();

                Logger.Normal(ConsoleColor.Yellow, caughtPokemonResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess ? $"We caught a {pokemon.PokemonId} with CP {encounterPokemonResponse?.WildPokemon?.PokemonData?.Cp}, used {balls_used} x {pokeball} and received XP {caughtPokemonResponse.Scores.Xp.Sum()}" : $"{pokemon.PokemonId} with CP {encounterPokemonResponse?.WildPokemon?.PokemonData?.Cp} got away while using a {pokeball}..");
                await Task.Delay(5000);
            }
        }

        private async Task EvolveAllGivenPokemons(IEnumerable<Pokemon> pokemonToEvolve)
        {
            foreach (var pokemon in pokemonToEvolve)
            {
                EvolvePokemonOut evolvePokemonOutProto;
                do
                {
                    evolvePokemonOutProto = await _client.EvolvePokemon((ulong)pokemon.Id);

                    if (evolvePokemonOutProto.Result == EvolvePokemonOut.Types.EvolvePokemonStatus.PokemonEvolvedSuccess)
                        Logger.Normal($"Evolved {pokemon.PokemonType} successfully for {evolvePokemonOutProto.ExpAwarded}xp");
                    else
                        Logger.Normal($"Failed to evolve {pokemon.PokemonType}. EvolvePokemonOutProto.Result was {evolvePokemonOutProto.Result}, stopping evolving {pokemon.PokemonType}");

                    await Task.Delay(3000);
                }
                while (evolvePokemonOutProto.Result == EvolvePokemonOut.Types.EvolvePokemonStatus.PokemonEvolvedSuccess);

                _stats.increasePokemonsTransfered();
                _stats.updateConsoleTitle();

                await Task.Delay(3000);
            }
        }

        private async Task TransferDuplicatePokemon(bool keepPokemonsThatCanEvolve = false)
        {
            var duplicatePokemons = await _inventory.GetDuplicatePokemonToTransfer();
            if (duplicatePokemons != null && duplicatePokemons.Any())
                Logger.Normal(ConsoleColor.DarkYellow, $"Transfering duplicate Pokemon");

            foreach (var duplicatePokemon in duplicatePokemons)
            {
                var transfer = await _client.TransferPokemon(duplicatePokemon.Id);
                Logger.Normal(ConsoleColor.DarkYellow, $"Transfer {duplicatePokemon.PokemonId} with {duplicatePokemon.Cp} CP");
                await Task.Delay(500);
            }
        }

        private async Task RecycleItems()
        {
            var items = await _inventory.GetItemsToRecycle(_clientSettings);

            foreach (var item in items)
            {
                var transfer = await _client.RecycleItem((AllEnum.ItemId)item.Item_, item.Count);
                Logger.Normal($"Recycled {item.Count}x {(AllEnum.ItemId)item.Item_}");

                _stats.addItemsRemoved(item.Count);
                _stats.updateConsoleTitle();

                await Task.Delay(500);
            }
        }

        private async Task<MiscEnums.Item> GetBestBall(int? pokemonCp)
        {
            var pokeBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_POKE_BALL);
            var greatBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_GREAT_BALL);
            var ultraBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_ULTRA_BALL);
            var masterBallsCount = await _inventory.GetItemAmountByType(MiscEnums.Item.ITEM_MASTER_BALL);

            if (masterBallsCount > 0 && pokemonCp >= 1000)
                return MiscEnums.Item.ITEM_MASTER_BALL;
            else if (ultraBallsCount > 0 && pokemonCp >= 1000)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            else if (greatBallsCount > 0 && pokemonCp >= 1000)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (ultraBallsCount > 0 && pokemonCp >= 600)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            else if (greatBallsCount > 0 && pokemonCp >= 600)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (greatBallsCount > 0 && pokemonCp >= 350)
                return MiscEnums.Item.ITEM_GREAT_BALL;

            if (pokeBallsCount > 0)
                return MiscEnums.Item.ITEM_POKE_BALL;
            if (greatBallsCount > 0)
                return MiscEnums.Item.ITEM_GREAT_BALL;
            if (ultraBallsCount > 0)
                return MiscEnums.Item.ITEM_ULTRA_BALL;
            if (masterBallsCount > 0)
                return MiscEnums.Item.ITEM_MASTER_BALL;

            return MiscEnums.Item.ITEM_UNKNOWN; // returning null to notify handler
        }

        public async Task UseBerry(ulong encounterId, string spawnPointId)
        {
            var inventoryBalls = await _inventory.GetItems();
            var berry = inventoryBalls.Where(p => (ItemId)p.Item_ == ItemId.ItemRazzBerry).FirstOrDefault();

            if (berry == null)
                return;

            var useRaspberry = await _client.UseCaptureItem(encounterId, AllEnum.ItemId.ItemRazzBerry, spawnPointId);
            Logger.Normal($"Use Rasperry. Remaining: {berry.Count}");
            await Task.Delay(3000);
        }
    }
}
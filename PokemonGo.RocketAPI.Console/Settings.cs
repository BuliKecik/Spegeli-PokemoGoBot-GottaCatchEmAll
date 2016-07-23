#region

using PokemonGo.RocketAPI.Enums;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using AllEnum;
#endregion


namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {
        public AuthType AuthType => (AuthType)Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType, true);
        public string PtcUsername => UserSettings.Default.PtcUsername;
        public string PtcPassword => UserSettings.Default.PtcPassword;
        public double DefaultLatitude => UserSettings.Default.DefaultLatitude;
        public double DefaultLongitude => UserSettings.Default.DefaultLongitude;
        public double DefaultAltitude => UserSettings.Default.DefaultAltitude;

        public float KeepMinIVPercentage => UserSettings.Default.KeepMinIVPercentage;
        public int KeepMinCP => UserSettings.Default.KeepMinCP;
        public double WalkingSpeedInKilometerPerHour => UserSettings.Default.WalkingSpeedInKilometerPerHour;
        public bool EvolveAllPokemonWithEnoughCandy => UserSettings.Default.EvolveAllPokemonWithEnoughCandy;
        public bool TransferDuplicatePokemon => UserSettings.Default.TransferDuplicatePokemon;
        public bool UsePokemonToNotCatchFilter => UserSettings.Default.UsePokemonToNotCatchFilter;

        private ICollection<PokemonId> _pokemonsToEvolve;
        private ICollection<PokemonId> _pokemonsNotToTransfer;
        private ICollection<PokemonId> _pokemonsNotToCatch;

        public string GoogleRefreshToken
        {
            get { return UserSettings.Default.GoogleRefreshToken; }
            set
            {
                UserSettings.Default.GoogleRefreshToken = value;
                UserSettings.Default.Save();
            }
        }

        ICollection<KeyValuePair<ItemId, int>> ISettings.ItemRecycleFilter
        {
            get
            {
                //Items to Recylce but keep X amount
                return new[]
                {
                    new KeyValuePair<ItemId, int>(ItemId.ItemUnknown, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPokeBall, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemGreatBall, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemUltraBall, 50),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMasterBall, 100),

                    new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 0),
                    new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMaxPotion, 50),

                    new KeyValuePair<ItemId, int>(ItemId.ItemRevive, 10),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMaxRevive, 50),

                    new KeyValuePair<ItemId, int>(ItemId.ItemLuckyEgg, 200),

                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseOrdinary, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseSpicy, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseCool, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncenseFloral, 100),

                    new KeyValuePair<ItemId, int>(ItemId.ItemTroyDisk, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemXAttack, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemXDefense, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemXMiracle, 100),

                    new KeyValuePair<ItemId, int>(ItemId.ItemRazzBerry, 20),
                    new KeyValuePair<ItemId, int>(ItemId.ItemBlukBerry, 10),
                    new KeyValuePair<ItemId, int>(ItemId.ItemNanabBerry, 10),
                    new KeyValuePair<ItemId, int>(ItemId.ItemWeparBerry, 30),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPinapBerry, 30),

                    new KeyValuePair<ItemId, int>(ItemId.ItemSpecialCamera, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasicUnlimited, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemIncubatorBasic, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemPokemonStorageUpgrade, 100),
                    new KeyValuePair<ItemId, int>(ItemId.ItemItemStorageUpgrade, 100),
                };
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private ICollection<PokemonId> LoadPokemonList(string filename)
        {
            ICollection<PokemonId> result = new List<PokemonId>();
            string path = Directory.GetCurrentDirectory() + "\\Configs\\";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            if (!File.Exists(path + filename + ".txt"))
            {
                string pokemonName = Properties.Resources.ResourceManager.GetString(filename);
                Logger.Normal($"File: {filename} not found, creating new...");
                File.WriteAllText(path + filename + ".txt", pokemonName);
            }
            if (File.Exists(path + filename + ".txt"))
            {
                string[] _locallist = File.ReadAllLines(path + filename + ".txt");
                foreach (string pokemonName in _locallist)
                {
                    var pokemon = Enum.Parse(typeof(PokemonId), pokemonName, true);
                    if (pokemonName != null) result.Add((PokemonId)pokemon);
                }
            }
            return result;
        }

        public ICollection<PokemonId> PokemonsToEvolve
        {
            get
            {
                //Type of pokemons to evolve
                _pokemonsToEvolve = _pokemonsToEvolve != null ? _pokemonsToEvolve : LoadPokemonList("PokemonsToEvolve");
                return _pokemonsToEvolve;
            }
        }

        public ICollection<PokemonId> PokemonsNotToTransfer
        {
            get
            {
                //Type of pokemons not to transfer
                _pokemonsNotToTransfer = _pokemonsNotToTransfer != null ? _pokemonsNotToTransfer : LoadPokemonList("PokemonsNotToTransfer");
                return _pokemonsNotToTransfer;
            }
        }

        public ICollection<PokemonId> PokemonsNotToCatch
        {
            get
            {
                //Type of pokemons not to catch
                _pokemonsNotToCatch = _pokemonsNotToCatch != null ? _pokemonsNotToCatch : LoadPokemonList("PokemonsNotToCatch");
                return _pokemonsNotToCatch;
            }
        }

    }
}

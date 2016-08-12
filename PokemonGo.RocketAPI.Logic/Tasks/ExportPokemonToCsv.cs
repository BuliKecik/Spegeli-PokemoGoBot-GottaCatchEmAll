using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Logic.Utils;
using POGOProtos.Data;
using Logger = PokemonGo.RocketAPI.Logic.Logging.Logger;
using LogLevel = PokemonGo.RocketAPI.Logic.Logging.LogLevel;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public class ExportPokemonToCsv
    {
        private static readonly string _exportPath = Path.Combine(Directory.GetCurrentDirectory(), "Export");
        public static DateTime _lastExportTime;

        public static async Task Execute(PlayerData player, string filename = "PokeList.csv")
        {
            if (player == null)
                return;
            var stats = await Inventory.GetPlayerStats();
            var stat = stats.FirstOrDefault();
            if (stat == null)
                return;

            if (!Directory.Exists(_exportPath))
                Directory.CreateDirectory(_exportPath);
            if (Directory.Exists(_exportPath))
            {
                try
                {
                    var pokelistFile = Path.Combine(_exportPath, $"Profile_{player.Username}_{filename}");
                    if (File.Exists(pokelistFile))
                        File.Delete(pokelistFile);

                    CsvExport myExport = new CsvExport();
                    var allPokemon = await Inventory.GetHighestsIv();
                    var myPokemonSettings = await Inventory.GetPokemonSettings();
                    var pokemonSettings = myPokemonSettings.ToList();
                    var myPokemonFamilies = await Inventory.GetPokemonFamilies();
                    var pokemonFamilies = myPokemonFamilies.ToArray();
                    var trainerLevel = stat.Level;
                    foreach (var pokemon in allPokemon)
                    {
                        var toEncode = $"{(int)pokemon.PokemonId}" + "," + trainerLevel + "," + PokemonInfo.GetLevel(pokemon) + "," + pokemon.Cp + "," + pokemon.Stamina;
                        //Generate base64 code to make it viewable here http://poke.isitin.org/#MTUwLDIzLDE3LDE5MDIsMTE4
                        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(toEncode));

                        var settings = pokemonSettings.Single(x => x.PokemonId == pokemon.PokemonId);
                        var familiecandies = pokemonFamilies.Single(x => settings.FamilyId == x.FamilyId).Candy_;

                        myExport.AddRow();
                        myExport["PokemonID"] = (int)pokemon.PokemonId;
                        myExport["Name"] = pokemon.PokemonId;
                        myExport["NickName"] = pokemon.Nickname;
                        myExport["Level"] = PokemonInfo.GetLevel(pokemon).ToString("0.0");
                        myExport["CP"] = pokemon.Cp;
                        myExport["MaxCP"] = PokemonInfo.CalculateMaxCp(pokemon);
                        myExport["IV Perfection in %"] = PokemonInfo.CalculatePokemonPerfection(pokemon).ToString("0.00");
                        myExport["Attack 1"] = pokemon.Move1;
                        myExport["Attack 2"] = pokemon.Move2;
                        myExport["HP"] = pokemon.Stamina;
                        myExport["Attk"] = pokemon.IndividualAttack;
                        myExport["Def"] = pokemon.IndividualDefense;
                        myExport["Stamina"] = pokemon.IndividualStamina;
                        myExport["Familie Candies"] = familiecandies;
                        myExport["IsInGym"] = pokemon.DeployedFortId != string.Empty ? "Yes" : "No";
                        myExport["IsFavorite"] = pokemon.Favorite != 0 ? "Yes" : "No";
                        myExport["previewLink"] = $"http://poke.isitin.org/#{encoded}";
                    }
                    myExport.ExportToFile(pokelistFile);
                    Logger.Write($"Export Player Infos and all Pokemon to \"\\Export\\Profile_{player.Username}_{filename}\"", LogLevel.Info);
                }
                catch
                {
                    Logger.Write("Export Player Infos and all Pokemons to CSV not possible. File seems be in use!", LogLevel.Warning);
                }
                _lastExportTime = DateTime.Now;
                if (Logic._client.Settings.ExportPokemonToCsvEveryMinutes > 0)
                    Logger.Write($"Next Export in {Logic._client.Settings.ExportPokemonToCsvEveryMinutes} Minutes", LogLevel.Info);
            }
        }
    }
}

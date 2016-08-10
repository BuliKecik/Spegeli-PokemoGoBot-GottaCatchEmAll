using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Logic.Logging;
using POGOProtos.Data;
using POGOProtos.Inventory.Item;

namespace PokemonGo.RocketAPI.Logic.Tasks
{
    public class HatchEggsTask
    {
        public static int _hatchUpdateDelay = 0;
        public static int _hatchUpdateDelayGPX = 0;

        public static async Task Execute()
        {
            try
            {
                var playerStats = (await Inventory.GetPlayerStats()).FirstOrDefault();
                if (playerStats == null)
                    return;
                var kmWalked = playerStats.KmWalked;

                await Inventory.GetCachedInventory(true);

                var incubators = (await Inventory.GetEggIncubators())
                    .Where(x => x.UsesRemaining > 0 || x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                    .OrderByDescending(x => x.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                    .ThenBy(x => x.UsesRemaining)
                    .ToList();

                var unusedEggs = (await Inventory.GetEggs())
                    .Where(x => string.IsNullOrEmpty(x.EggIncubatorId))
                    .OrderBy(x => x.EggKmWalkedTarget - x.EggKmWalkedStart)
                    .ToList();

                foreach (var incubator in incubators)
                {
                    if (incubator.PokemonId == 0)
                    {
                        if (incubator.ItemId == ItemId.ItemIncubatorBasic && Logic._client.Settings.UseOnlyBasicIncubator)
                            continue;

                        //Lowest egg for Unlimited, Highest egg for Limited Incubators
                        PokemonData egg = null;
                        if (incubator.ItemId == ItemId.ItemIncubatorBasicUnlimited)
                            egg = unusedEggs.FirstOrDefault();
                        if (incubator.ItemId == ItemId.ItemIncubatorBasic)
                            egg = unusedEggs.LastOrDefault();
                        // Don't use limited incubators for under 5km eggs
                        if (egg == null || (egg.EggKmWalkedTarget < 5 && incubator.ItemId == ItemId.ItemIncubatorBasic))
                            continue;

                        var response = await Logic._client.Inventory.UseItemEggIncubator(incubator.Id, egg.Id);
                        Logger.Write($"Adding Egg #{unusedEggs.IndexOf(egg)} with {egg.EggKmWalkedTarget}km to Incubator #{incubators.IndexOf(incubator)}: {response.Result}!", LogLevel.Incubation);

                        unusedEggs.Remove(egg);
                    }
                    else
                    {
                        var kmToWalk = incubator.TargetKmWalked - incubator.StartKmWalked;
                        var kmRemaining = incubator.TargetKmWalked - kmWalked;

                        Logger.Write($"Incubator #{incubators.IndexOf(incubator)} needs {kmRemaining.ToString("N2")}km/{kmToWalk.ToString("N2")}km to hatch.", LogLevel.Egg);
                    }
                }

                if (_hatchUpdateDelay >= 15 || _hatchUpdateDelayGPX >= 5)
                {
                    _hatchUpdateDelay = 0;
                    _hatchUpdateDelayGPX = 0;
                }

            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}

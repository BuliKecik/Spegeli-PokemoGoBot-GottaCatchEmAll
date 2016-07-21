using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Utils
{
    class Statistics
    {
        private int _totalExperience;
        private int _totalPokemons;
        private int _totalItemsRemoved;
        private int _totalPokemonsTransfered;
        private int _totalStardust;
        public static string _getLevelInfos;
        public static int Currentlevel = -1;

        private DateTime _initSessionDateTime = DateTime.Now;

        private double _getSessionRuntime()
        {
            return ((DateTime.Now - _initSessionDateTime).TotalSeconds) / 3600;
        }

        public void addExperience(int xp)
        {
            _totalExperience += xp;
        }

        public static async Task<string> _getcurrentLevelInfos(Client _client)
        {
            var inventory = await _client.GetInventory();
            var stats = inventory.InventoryDelta.InventoryItems.Select(i => i.InventoryItemData?.PlayerStats).ToArray();
            var output = string.Empty;
            foreach (var v in stats)
                if (v != null)
                {
                    Currentlevel = v.Level;
                    output = $"{v.Level} ({v.Experience}/{v.NextLevelXp})";
                }
            return output;
        }

        public void increasePokemons()
        {
            _totalPokemons += 1;
        }

        public void getStardust(int stardust)
        {
            _totalStardust = stardust;
        }

        public void addItemsRemoved(int count)
        {
            _totalItemsRemoved += count;
        }

        public void increasePokemonsTransfered()
        {
            _totalPokemonsTransfered += 1;
        }

        public async void updateConsoleTitle(Client _client)
        {
            _getLevelInfos = await _getcurrentLevelInfos(_client);
            Console.Title = ToString();
        }

        public override string ToString()
        {           
            return string.Format("{0} - LvL: {1:0}    EXP Exp/H: {2:0.0} EXP   P/H: {3:0.0} Pokemon(s)   Stardust: {4:0}   Pokemon Transfered: {5:0}   Items Removed: {6:0}", "Statistics", _getLevelInfos, _totalExperience / _getSessionRuntime(), _totalPokemons / _getSessionRuntime(), _totalStardust, _totalPokemonsTransfered, _totalItemsRemoved);
        }
    }
}
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

        private DateTime _initSessionDateTime = DateTime.Now;

        private double _getSessionRuntime()
        {
            return ((DateTime.Now - _initSessionDateTime).TotalSeconds) / 3600;
        }

        public void addExperience(int xp)
        {
            _totalExperience += xp;
        }

        public void increasePokemons()
        {
            _totalPokemons += 1;
        }

        public void addItemsRemoved(int count)
        {
            _totalItemsRemoved += count;
        }

        public void increasePokemonsTransfered()
        {
            _totalPokemonsTransfered += 1;
        }

        public void updateConsoleTitle()
        {
            Console.Title = ToString();
        }

        public override string ToString()
        {
            return string.Format("{0} - Exp/H: {1:0.0} EXP   P/H: {2:0.0} Pokemon(s)    Items Removed: {3:0.0}    Items Transfered: {4:0.0}", "Statistics", _totalExperience / _getSessionRuntime(), _totalPokemons / _getSessionRuntime(), _totalItemsRemoved, _totalPokemonsTransfered);
        }
    }
}
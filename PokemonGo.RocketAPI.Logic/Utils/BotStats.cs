using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGo.RocketAPI.Logic.Utils
{
    public class BotStats
    {
        private int _totalExperience;
        private int _totalPokemons;
        private DateTime _initialSessionDateTime = DateTime.Now;

        private double _getBottingSessionTime()
        {
            return ((DateTime.Now - _initialSessionDateTime).TotalSeconds) / 3600;
        }

        public void addExperience(int exp)
        {
            _totalExperience += exp;
        }

        public void addPokemon(int count)
        {
            _totalPokemons += count;
        }

        public override string ToString()
        {
            return string.Format("{0} - Experience/Hour: {1:0.0} EXP     Pokemon/Hour: {2:0.0} Pokemon(s)",
                "Pokemon GO",
                _totalExperience / _getBottingSessionTime(),
                _totalPokemons / _getBottingSessionTime());
        }
    }
}

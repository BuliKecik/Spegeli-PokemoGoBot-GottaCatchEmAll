#region

using PokemonGo.RocketAPI.Enums;
using System.Collections.Generic;

#endregion


namespace PokemonGo.RocketAPI
{
    public interface ISettings
    {
        AuthType AuthType { get; }
        double DefaultLatitude { get; }
        double DefaultLongitude { get; }
        double DefaultAltitude { get; }
        string GoogleRefreshToken { get; set; }
        string PtcPassword { get; }
        string PtcUsername { get; }
        float KeepMinIVPercentage { get; }
        int KeepMinCP { get; }
        double WalkingSpeedInKilometerPerHour { get; }
        bool EvolveAllPokemonWithEnoughCandy { get; }
        bool TransferDuplicatePokemon { get; }
        bool UsePokemonToNotCatchFilter { get; }

        ICollection<KeyValuePair<AllEnum.ItemId, int>> ItemRecycleFilter { get; set; }

        ICollection<AllEnum.PokemonId> PokemonsToEvolve { get; }

        ICollection<AllEnum.PokemonId> PokemonsNotToTransfer { get; }

        ICollection<AllEnum.PokemonId> PokemonsNotToCatch { get; }
    }
}
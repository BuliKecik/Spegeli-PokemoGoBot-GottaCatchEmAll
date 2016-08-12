#region

using System;
using System.IO;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.HttpClient;
using POGOProtos.Networking.Envelopes;

#endregion


namespace PokemonGo.RocketAPI
{
    public class Client : IDisposable
    {
        public Rpc.Login Login;
        public Rpc.Player Player;
        public Rpc.Download Download;
        public Rpc.Inventory Inventory;
        public Rpc.Map Map;
        public Rpc.Fort Fort;
        public Rpc.Encounter Encounter;
        public Rpc.Misc Misc;

        public ISettings Settings { get; }
        public string AuthToken { get; set; }

        public double CurrentLatitude { get; internal set; }
        public double CurrentLongitude { get; internal set; }
        public double CurrentAltitude { get; internal set; }

        public AuthType AuthType => Settings.AuthType;

        internal readonly PokemonHttpClient PokemonHttpClient = new PokemonHttpClient();
        internal string ApiUrl { get; set; }
        internal AuthTicket AuthTicket { get; set; }
        internal static string DeviceId { get; set; }
        internal static string AndroidBoardName { get; set; }
        internal static string AndroidBootloader { get; set; }
        internal static string DeviceBrand { get; set; }
        internal static string DeviceModel { get; set; }
        internal static string DeviceModelBoot { get; set; }
        internal static string DeviceModelIdentifier { get; set; }
        internal static string FirmwareBrand { get; set; }
        internal static string FirmwareFingerprint { get; set; }
        internal static string FirmwareTags { get; set; }
        internal static string FirmwareType { get; set; }
        internal static string HardwareManufacturer { get; set; }
        internal static string HardwareModel { get; set; }

        public Client(ISettings settings)
        {
            DeviceId = RequestBuilder.GetDeviceId();
            RequestBuilder.SetDevice(settings);

            Settings = settings;

            Login = new Rpc.Login(this);
            Player = new Rpc.Player(this);
            Download = new Rpc.Download(this);
            Inventory = new Rpc.Inventory(this);
            Map = new Rpc.Map(this);
            Fort = new Rpc.Fort(this);
            Encounter = new Rpc.Encounter(this);
            Misc = new Rpc.Misc(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                PokemonHttpClient?.Dispose();
            }
        }
    }
}

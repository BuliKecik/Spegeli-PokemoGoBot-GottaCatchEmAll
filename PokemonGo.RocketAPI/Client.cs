#region

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using C5;
using Google.Protobuf;
using DankMemes.GPSOAuthSharp;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.Exceptions;
using PokemonGo.RocketAPI.Extensions;

using PokemonGo.RocketAPI.Helpers;
using PokemonGo.RocketAPI.HttpClient;
using PokemonGo.RocketAPI.Login;
using PokemonGo.RocketAPI.Logging;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Envelopes;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;
using Logger = PokemonGo.RocketAPI.Logging.Logger;

#endregion


namespace PokemonGo.RocketAPI
{
    public class Client
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

        private Random _rand;

        private static readonly string ConfigsPath = Path.Combine(Directory.GetCurrentDirectory(), "Settings");
        private static readonly string LastcoordsFile = Path.Combine(ConfigsPath, "LastCoords.ini");

        public Client(ISettings settings)
        {
            Settings = settings;

            Login = new Rpc.Login(this);
            Player = new Rpc.Player(this);
            Download = new Rpc.Download(this);
            Inventory = new Rpc.Inventory(this);
            Map = new Rpc.Map(this);
            Fort = new Rpc.Fort(this);
            Encounter = new Rpc.Encounter(this);
            Misc = new Rpc.Misc(this);

            var latLngFromFile = GetLatLngFromFile();

            if (latLngFromFile != null && Math.Abs(latLngFromFile.Item1) > 0 && Math.Abs(latLngFromFile.Item2) > 0)
            {
                Player.SetCoordinates(latLngFromFile.Item1, latLngFromFile.Item2, Settings.DefaultAltitude);
            }
            else
            {
                if (!File.Exists(LastcoordsFile) || !File.ReadAllText(LastcoordsFile).Contains(":"))
                    Logger.Write("Missing Settings File \"LastCoords.ini\", using default settings for coordinates and create a new one...");
                Player.SetCoordinates(Settings.DefaultLatitude, Settings.DefaultLongitude, Settings.DefaultAltitude);
            }
        }

        /// <summary>
        /// Gets the lat LNG from file.
        /// </summary>
        /// <returns>Tuple&lt;System.Double, System.Double&gt;.</returns>
        public static Tuple<double, double> GetLatLngFromFile()
        {
            if (!Directory.Exists(ConfigsPath))
                Directory.CreateDirectory(ConfigsPath);
            if (File.Exists(LastcoordsFile) && File.ReadAllText(LastcoordsFile).Contains(":"))
            {
                var latlngFromFile = File.ReadAllText(LastcoordsFile);
                var latlng = latlngFromFile.Split(':');
                if (latlng[0].Length != 0 && latlng[1].Length != 0)
                {
                    try
                    {
                        var tempLat = Convert.ToDouble(latlng[0]);
                        var tempLong = Convert.ToDouble(latlng[1]);

                        if (tempLat >= -90 && tempLat <= 90 && tempLong >= -180 && tempLong <= 180)
                        {
                            //SetCoordinates(Convert.ToDouble(latlng[0]), Convert.ToDouble(latlng[1]), Settings.DefaultAltitude);
                            return new Tuple<double, double>(tempLat, tempLong);
                        }
                        else
                        {
                            Logger.Write("Coordinates in \"\\Settings\\LastCoords.ini\" file are invalid, using the default coordinates", LogLevel.Error);
                            return null;
                        }
                    }
                    catch (FormatException)
                    {
                        Logger.Write("Coordinates in \"\\Settings\\LastCoords.ini\" file are invalid, using the default coordinates", LogLevel.Error);
                        return null;
                    }
                }
            }
            return null;
        }
    }
}

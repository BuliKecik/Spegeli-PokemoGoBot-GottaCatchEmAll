using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;

namespace PokemonGo.RocketAPI.Logic.Utils
{
    class CheckAndDownloadVersion
    {
        public static void CheckVersion()
        {
            try
            {
                var match =
                    new Regex(
                        @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                        .Match(DownloadServerVersion());

                if (!match.Success) return;
                var gitVersion =
                    new Version(
                        string.Format(
                            "{0}.{1}.{2}.{3}",
                            match.Groups[1],
                            match.Groups[2],
                            match.Groups[3],
                            match.Groups[4]));
                if (gitVersion <= Assembly.GetExecutingAssembly().GetName().Version)
                {
                    Logger.Normal(ConsoleColor.Green, "Awesome! You have already got the newest version! " + Assembly.GetExecutingAssembly().GetName().Version);
                    return;
                }
                ;

                Logger.Normal(ConsoleColor.Red, "There is a new Version available: " + gitVersion + " downloading.. ");
                Thread.Sleep(1000);
                Process.Start("https://github.com/Spegeli/Pokemon-Go-Rocket-API");
            }
            catch (Exception)
            {
                Logger.Error($"Unable to check for updates now...");
            }
        }

        private static string DownloadServerVersion()
        {
            using (var wC = new WebClient())
                return
                    wC.DownloadString(
                        "https://raw.githubusercontent.com/Spegeli/Pokemon-Go-Rocket-API/master/PokemonGo/RocketAPI/Console/Properties/AssemblyInfo.cs");
        }
    }
}

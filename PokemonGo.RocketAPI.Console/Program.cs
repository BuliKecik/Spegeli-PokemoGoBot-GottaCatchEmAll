using System;
using System.Threading.Tasks;
using PokemonGo.RocketAPI.Exceptions;

namespace PokemonGo.RocketAPI.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetLogger(new Logging.ConsoleLogger(LogLevel.Info));

            Task.Run(() =>
            {
                try
                {
                    new Logic.Logic(new Settings()).Execute();
                }
                catch (PtcOfflineException)
                {
                    Logger.Normal("PTC Servers are probably down OR your credentials are wrong. Try google");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Unhandled exception: {ex}");
                }
            });
             System.Console.ReadLine();
        }
    }
}
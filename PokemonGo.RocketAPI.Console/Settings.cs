using System.Configuration;
using PokemonGo.RocketAPI.Enums;
using PokemonGo.RocketAPI.GeneratedCode;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using AllEnum;

namespace PokemonGo.RocketAPI.Console
{
    public class Settings : ISettings
    {
        public AuthType AuthType => (AuthType)Enum.Parse(typeof(AuthType), UserSettings.Default.AuthType);
        public string PtcUsername => UserSettings.Default.PtcUsername;
        public string PtcPassword => UserSettings.Default.PtcPassword;
        public double DefaultLatitude => UserSettings.Default.DefaultLatitude;
        public double DefaultLongitude => UserSettings.Default.DefaultLongitude;

        ICollection<KeyValuePair<ItemId, int>> ISettings.itemRecycleFilter
        {
            get
            {
                //Items to Recylce but keep X amount
                return new[]
                {
                    new KeyValuePair<ItemId, int>(ItemId.ItemPotion, 5),
                    new KeyValuePair<ItemId, int>(ItemId.ItemSuperPotion, 5),
                    new KeyValuePair<ItemId, int>(ItemId.ItemHyperPotion, 5),
                    new KeyValuePair<ItemId, int>(ItemId.ItemMaxPotion, 5),
                    new KeyValuePair<ItemId, int>(ItemId.ItemRevive, 5),
                    new KeyValuePair<ItemId, int>(ItemId.ItemRazzBerry, 5)
                };
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string GoogleRefreshToken
        {
            get { return UserSettings.Default.GoogleRefreshToken; }
            set
            {
                UserSettings.Default.GoogleRefreshToken = value;
                UserSettings.Default.Save();
            }
        }
    }
}

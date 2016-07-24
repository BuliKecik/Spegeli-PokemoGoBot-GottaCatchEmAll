# Pokemon-Go-Rocket-API

![alt tag](https://github.com/Spegeli/Pokemon-Go-Rocket-API/blob/master/Screenshot.png)

A Pokemon Go bot in C#

## About

Chat about this Repository via Discord: https://discord.gg/VsVrjgr

**GitHub issues will be deleted if they are not API related.**

## Features
* PTC Login / Google
* Use Humanlike Walking with 10 km/h (instead of Teleport) (Speed is configurable via UserSettings)
* Farm Pokestops (use always the nearest from the current location) (Optional: keep within specific distance to Start Point) (MaxDistance configurable via UserSettings)
* Farm all Pokemon near your (Optional: PokemonsNotToCatch List. Disabled by default, can be Enabled via UserSettings, configurable Names via File in Config Folder)
* Transfer duplicate Pokemon (ignore favorite/gym marked) (Optional: Enabled by default, can be Disabled via UserSettings. Keep X amount of everyone and order by CP or IV configurable via UserSettings) (Optional: PokemonsNotToTransfer List configurable via File in Config Folder)
* Evolve Pokemon (Optional: Disabled by default, can be Enabled via UserSettings. PokemonsToEvolve List configurable via File in Config Folder)
* Throws away unneeded items (configurable via UserSettings)
* Use best Pokeball & Berry (depending on Pokemon CP)
* Creates Excel CSV File on Startup with your current Pokemon (including Number, Name, CP & Perfection) (can be found in the Export Folder)
* Log File System (all activity will be tracked in a Log File)
* Random Task Delays
* Statistic in the Header:
![alt tag](https://github.com/Spegeli/Pokemon-Go-Rocket-API/blob/master/StatisticScreenshot.png)
* Very color and useful Logging (so you every time up2date what currently happened)
* and many more ;-)

## ToDo
* Auto Update the Bot

## Setting it up
Note: You need some basic Computer Expierience, if you need help somewhere, ask the community and do not spam us via private messages. **The Issue Tracker is not for help!**


1. Download and Install [Visual Studio 2015](https://go.microsoft.com/fwlink/?LinkId=691979&clcid=0x409)
2. Download [this Repository](https://github.com/Spegeli/Pokemon-Go-Rocket-API/archive/master.zip)
3. Open Pokemon Go Rocket API.sln
4. On the right hand side, double click on UserSettings.settings
5. Select the AuthType (Google or Ptc for Pokémon Trainer Club)
6. If selected Ptc , enter the Username and Password of your Account
7. Enter the DefaultLatitude and DefaultLongitude [can be found here](http://mondeca.com/index.php/en/any-place-en)
8. Optional you can set up the other Settings (when you know what you're doing)
9. Right click on PokemonGo.RocketAPI.Console and Set it as Startup Project
10. Press CTRL + F5 and follow the Instructions
11. Have fun! 

## License
This Project is licensed as GNU (GNU GENERAL PUBLIC LICENSE v3) 

You can find all necessary Information [here](https://github.com/Spegeli/Pokemon-Go-Rocket-API/blob/master/LICENSE.md)

## Credits
Thanks to Ferox hard work on the API & Console we are able to manage something like this. Without him that would have been nothing. <3

Thanks to everyone who contributed via Pull Requests!
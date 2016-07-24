<!-- define warning icon -->
[1.1]: http://i.imgur.com/M4fJ65n.png (ATTENTION)
[1.2]: http://i.imgur.com/NNcGs1n.png (BTC)
<!-- title -->
<h1>Pokemon Go Bot based on FeroxRevs API</h1>
<br/>
![alt tag](https://github.com/Spegeli/Pokemon-Go-Rocket-API/blob/master/Screenshot.png)
<!-- disclaimer -->
![alt text][1.1] <strong><em> The contents of this repo are a proof of concept and are for educational use only </em></strong> ![alt text][1.1]
<br/>
<br/>

<h2>Table of Contents</h2>

- [Chat](#chat)
- [Features](#features)
  - [Screenshots](#screenshots)
- [Getting Started](#getting-started)
  - [Installation & Configuration](#install-config)
  - [Changing Location](#changing-location)
- [License](#license)
- [Credits](#credits)

<hr/>

<h2><a name="chat">Chat</a></h2>

Chatting about this Repository can be done on our Discord: https://discord.gg/VsVrjgr <br/>
Please keep your conversations in the designated channels.
<br/>
<hr/>
<br/>
<h2><a name="features">Features</a></h2>
 
 - [PTC Login / Google]
 - [Humanlike Walking]<br />
   (Speed in km/h is configurable via UserSettings)
 - [Farm Pokestops]<br />
   (use always the nearest from the current location) (Optional: keep within specific distance to Start Point) (MaxDistance configurable via UserSettings)
 - [Farm all Pokemon near your]<br />
   (Optional: PokemonsNotToCatch List. Disabled by default, can be Enabled via UserSettings, configurable Names via File in Config Folder)
 - Transfer duplicate Pokemon (ignore favorite/gym marked) (Optional: Enabled by default, can be Disabled via UserSettings. Keep X amount of everyone and order by CP or IV configurable via UserSettings) (Optional: PokemonsNotToTransfer List configurable via File in Config Folder)
 - Evolve Pokemon (Optional: Disabled by default, can be Enabled via UserSettings. PokemonsToEvolve List configurable via File in Config Folder)
 - Throws away unneeded items (configurable via UserSettings)
 - Use best Pokeball & Berry (depending on Pokemon CP)
 - Creates Excel CSV File on Startup with your current Pokemon (including Number, Name, CP & Perfection) (can be found in the Export Folder)
 - Log File System (all activity will be tracked in a Log File)
 - Random Task Delays
 - Statistic in the Header: ![alt tag](https://github.com/Spegeli/Pokemon-Go-Rocket-API/blob/master/StatisticScreenshot.png)
 - Very color and useful Logging (so you every time up2date what currently happened)
 - and many more ;-)

<br/>
<h2><a name="screenshots">Screenshots</a></h2><br/>
- coming soon -<br/>
<hr/>

<h2><a name="getting-started">Getting Started</a></h2>
Note: You will need some basic Computer Expierience.<br/>
Need help? <a name="chat">Join the Chat!</a> **The Issue Tracker is not for help!**<br/>
<br/>
<h2><a name="install-config">Installation & Configuration</a></h2><br/>

1. Download and Install [Visual Studio 2015](https://go.microsoft.com/fwlink/?LinkId=691979&clcid=0x409)
2. Download [this Repository](https://github.com/Spegeli/Pokemon-Go-Rocket-API/archive/master.zip)
3. Open Pokemon Go Rocket API.sln
4. On the right hand side, double click on "UserSettings.settings"
5. Enter the DefaultLatitude and DefaultLongitude [can be found here](http://mondeca.com/index.php/en/any-place-en)
6. Select the AuthType (Google or Ptc for Pokémon Trainer Club)
7. If selected Ptc , enter the Username and Password of your Account
8. Right click on "PokemonGo.RocketAPI.Console" and Set it as Startup Project
9. Press CTRL + F5 and follow the Instructions
10. Have fun!<br/>

<h2><a name="changing-location">Changing Location of the Bot</a></h2><br/>

1. Get new latitude and longitude
2. Delete `LastCoords.ini` from folder `PokemonGo.RocketAPI.Console\bin\Debug\Configs`
3. Change the value of `DefaultLatitude` and `DefaultLongitude` in `UserSettings.settings`
4. Compile and run (CTRL + F5)<br/>

<hr/>
<br/>
<h2><a name="license">License</a></h2><br/>
This Project is licensed as GNU (GNU GENERAL PUBLIC LICENSE v3) 
<br/>
You can find all necessary Information [HERE](https://github.com/Spegeli/Pokemon-Go-Bot/blob/master/LICENSE.md)
<br/>
<hr/>
<br/>

<h2><a name="credits">Credits</a></h2><br/>
Thanks to Feroxs' hard work on the API & Console we are able to manage something like this.<br/>
Without him, this would not have been available. <3
<br/>
Thanks to everyone who voluntaired by contributing to the Pull Requests!

<!-- define warning icon -->
[1.1]: http://i.imgur.com/M4fJ65n.png (ATTENTION)
[1.2]: http://i.imgur.com/NNcGs1n.png (BTC)
[1.3]: http://i.epvpimg.com/ZTsdb.png (SCREENSHOT)
<!-- title -->
<h1>A Pokemon Go Bot based on FeroxRevs API</h1>
<!-- disclaimer -->
![alt text][1.1]<strong><em> The contents of this repo are a proof of concept and are for educational use only </em></strong>![alt text][1.1]<br />
<br />
Chatting about this Repository can be done on our Discord: https://discord.gg/KzJUjDE <br/>
<br/>
<strong><em>UPDATE (8/7/2016)</em></strong> - Back online!<br />
<br />
In order for the bot to function. You will need to supply a encrypt.dll file. Due to to legal reasons, We will not supply this file. <br/>
The dll needs to be placed within the folder:<br/>
PokemoGoBot-GottaCatchEmAll-master\PokemonGo.RocketAPI.Console\bin\Debug<br/>
<br/>
The file is not hard to find and any questions about this or other common items can be found within our Discord community in the channel #readme<br/>
<br />

<h2><a name="features">Features</a></h2>
 
 - [PTC Login / Google]
 - [Humanlike Walking]<br />
 - [Configurable Custom Pathing]<br />
   (Speed in km/h is configurable via UserSettings)
 - [Farm Pokestops]<br />
   (use always the nearest from the current location)<br />
   (Optional: keep within specific MaxTravelDistanceInMeters to Start Point) (MaxTravelDistanceInMeters configurable via UserSettings)
 - [Farm all Pokemon near your]<br />
   (Optional: PokemonsNotToCatch List. Disabled by default, can be Enabled via UserSettings, configurable Names via File in Config Folder)
 - [Evolve Pokemon]<br />
   (Optional: Enabled by default, can be Disabled via UserSettings)<br />
   (Optional): UsePokemonToEvolveList = When False bot will not use the EvolveList so it will Evolve ALL Pokemon<br />
   (Optional: EvolveOnlyPokemonAboveIV - Will Evolve only Pokemon with IV > EvolveAboveIVValue, Disabled by default, can be Enabled vis UserSettings)
 - [Transfer Pokemon]<br />
   (ignore favorite/gym marked)<br />
   (Optional: Enabled by default, can be Disabled via UserSettings.)<br />
   (Optional: UseTransferPokemonKeepAboveCP - Keeps all Pokemon with CP > TransferPokemonKeepAboveCP, Enabled by default, can be Disabled vis UserSettings)
   (Optional: UseTransferPokemonKeepAboveIV - Keeps all Pokemon with IV > TransferPokemonKeepAboveIVPercentage, Enabled by default, can be Disabled vis UserSettings)
   (Optional: PrioritizeIVOverCP - Determines the sorting sequence - CP or IV, Enabled by default, can be Disabled via UserSettings.)<br />
 - TransferPokemonKeepAmountHighestCP = Keep X Highest Pokemon based on CP (when doing Transfer) - Default 0 (because CP are useless)<br />
 - TransferPokemonKeepAmountHighestIV = Keep X Highest Pokemon based on IV (when doing Transfer) - Default 1<br />
   (Optional: PokemonsNotToTransfer List. Enabled by default, can be Disabled via UserSettings, configurable Names via File in Config Folder)
   (Optional: NotTransferPokemonsThatCanEvolve - Will keep ALL Pokemons which can be Evolve not matter if they on PokemonsToEvolve List or not, Disabled by default, can be Enabled via UserSettings)
 - CatchIncensePokemon & CatchLuredPokemon = The Name says all, disabled by Default because it seems it slows down the EP    instead of increase it<br />
   UseIncense gets removed from the Settings Options, when CatchIncensePokemon is enabled he will use Incense<br />
 - In the Settings.cs you found now the following Options:<br />
    private const int MaxBalls = 100;<br />
    private const int MaxBerries = 20;<br />
    private const int MaxPotions = 30;<br />
   Instead of setup every ball, berry & potions you can set now a complete amount, he will always destory the badest and keep to best.<br />
 - [Throws away unneeded items]<br />
   (configurable via Settings.cs)
 - [Use Lucky Eggs]<br />
   (Disbaled by default, can be Enabled via UserSettings)<br />
 - [Use best Pokeball & Berry]<br />
   (depending on Pokemon CP and IV)
 - [Creates Excel CSV File on Startup with your current Pokemon]<br />
   (including Number, Name, CP,IV Perfection in % and many more) (can be found in the Export Folder)<br />
 - [Softban bypass]<br />
 - [Log File System]<br />
   (all activity will be tracked in a Log File)<br />
 - [Statistic in the Header] - ![spegelibot_statisticbar](https://cloud.githubusercontent.com/assets/20632191/17541376/bcfdfad6-5e8c-11e6-9340-003f44653def.png)<br />
 - Pokedex count: Captured / Saw added to the Status Bar<br />
 - [Very color and useful Logging]<br />
   (so you every time up2date what currently happened)
 - and many more ;-)
<br/>

<h2><a name="screenshots">Screenshots</a></h2>
![spegelibot_screen](https://cloud.githubusercontent.com/assets/20632191/17541319/4abcef36-5e8c-11e6-8bce-bd0d593a01c9.png)
<br/>

<h2><a name="getting-started">Getting Started</a></h2>
Make sure you check out our [Wiki](https://github.com/Spegeli/PokemoGoBot-GottaCatchEmAll/wiki) to get started.
<br/>

<h2><a name="credits">Credits</a></h2>
A big thank you goes to Feroxs' hard work on the API & Console. Without him, this would not have been possible. <3
<br/>
Thanks to everyone who volunteered by contributing via Pull Requests!

<h2><a name="donating">Donating</a></h2>
<a name="Donating">All infos about how you can do a donate can you find in the [Wiki](https://github.com/Spegeli/PokemoGoBot-GottaCatchEmAll/wiki/Donations#donating)</a><br/>
<hr/>

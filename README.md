# Idle-Civilization

To do:

	- fancy cursors for normal mouse and scrolling
	- add informations for enemytiles
	- finish HUD
		o coloured text for pos/neg demand
		o add +/- to demand
		o make backgroundtextures
		o add button for speed
	- Spritesheets to Dictionary
	- Add an sprite-animation for testing
	- add difficulty-levels to enemy-ai
	
done lately:

	- EnemyBases now start with armystrength of 100
	- Added Enemy-Class to keep track of enemy-values
	- Codesektions for adding/subing extended with add/sub tile to enemy
	- Added Enum to determine enemy-playstile
	- added pause-key
		o added text to hud to visualice pause
	- added Enemyparameter to Globals and Configfile
	- added Enemyvalues to HUD for debugging
	- first version of enemy-ai implemented
	
done earlier:

	- added animation to tileMenu
	- updated demand-calc to use tiles under city controll
		o added Values to Globals and Configfile
	- added Alpha-Value to tiles around selected tile to aid visibility of TileMenu
	-.....
	- GraphicsDevice is a static global now
		o changed all using function to use the globals
	- Added Animation-Class
	- Textures are now stored as statics in Globals
	- optimised enemygeneration
	- got rid of some exceptions from city building
	- added buttons for zoom in/out
	- added buttons to deploy/take army to/from a tile
	-.....
	- done a super early version for the HUD
		o plane white text, no fancyness
	- put values for mapgeneration and demand-calc into static global-class
	- added function to load certain values from file while game is running
	- add function to reload map
	- add helptext for hotkeys (above functions) to HUD
	-.....
	- started programming of HUD
	- added offset to map-drawing to make room for HUD
		o inside Session and Tile
	- updated map-drawing to be more dynamic to screenresolution
	- added background-transparency to TextBox-Class
	- added fontColor to TextBox-Class
	-....
	- added remaining tileMenuFunction-Executions
	- added Conquer-Function to Tile-class
	- advanced tile-update-function to calculate ressource-demand
	- advanced session-update-function to subtract demand from player-ressources
	-....
	- added addTile-Function
	-....
	- debounce of button fixed
	- prepared all buttonfunctions for execution
	- created rules for city-creation
	- created public enum TileControllType and function (in Session) AdjacentTo to check if something specific is around a tile
	- if city is created neighboring tiles get part of it
	- added border to tile
	- deleted MyEventArgs
	- added static class Globals for global static attribues
		o added primitive texture for lines
	- added border-texture-sheet and respective enum
	- added global texture-lists for borders
	- added function to session to fill border-texture-lists
	- added drawing of borders to tiles
	- added Enemies to Mapgeneration


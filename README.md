# Idle-Civilization

To do:

	- fancy cursors for normal mouse and scrolling
	- add informations for enemytiles
	- finish HUD
		o coloured text for pos/neg demand
		o add +/- to demand
		o make backgroundtextures
		o add buttons for speed, pause 
	- Add a simple AI to the EnemyCitys so they start creeping around and deploy army to there tiles
		o Act like a player or set ressources after time (ease (less) to hard (many))?
	- Spritesheets to Dictionary
	- Add an animation for testing
	- update demand-calc to use tiles under city controll
	
done lately:
	
	- GraphicsDevice is a static global now
		o changed all using function to use the globals
	- Added Animation-Class
	- Textures are now stored as statics in Globals
	- optimised enemygeneration
	- got rid of some exceptions from city building
	- added buttons for zoom in/out
	- added buttons to deploy/take army to/from a tile

done earlier:

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


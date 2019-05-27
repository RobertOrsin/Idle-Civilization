# Idle-Civilization

To do:

	- fancy cursors for normal mouse and scrolling
	- some exceptions
	- add informations for enemytiles
	- add demand-calc for army in tile-update
	- add army deployment to tiles
	- store all textures as statics
	- finish HUD
	

done lately:
	- started programming of HUD
	- added offset to map-drawing to make room for HUD
		o inside Session and Tile
	- updated map-drawing to be more dynamic to screenresolution
	- added background-transparency to TextBox-Class
	- added fontColor to TextBox-Class

done earlier:

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


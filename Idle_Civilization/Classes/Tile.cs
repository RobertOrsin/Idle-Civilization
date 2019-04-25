using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class Tile
    {
        #region VARS
        public TileBaseType tileBaseType;
        private TileNumber tiletype;
        private int x, y; //koordinates on map
        Texture2D tile_texture;
        bool empty = false;
        Rectangle clickArea;

        //Delay for Ressourceupdate
        double timer = 5000; //in ms
        const double TIME = 5000;
        #endregion

        public Tile()
        {
            empty = true;
        }
        /// <summary>
        /// STD-Construktor
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public Tile(GraphicsDevice GraphicsDevice, Texture2D tileMap, TileNumber _tiletype, int _x, int _y)
        {
            tiletype = _tiletype;
            x = _x;
            y = _y;

            Color[] data;
            Rectangle rect = GetTileSpritePosition(tiletype);

            tile_texture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            tileMap.GetData(0, rect, data, 0, data.Length);
            tile_texture.SetData(data);

            clickArea = GetClickArea();
        }
        /// <summary>
        /// Update Inputs on Map and its tiles
        /// </summary>
        /// <param name="mouseState"></param>
        public void Update(MouseState mouseState, GameTime gameTime)
        {
            #region GameValue Update
            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            timer -= elapsed;

            if (timer < 0)
            {
                timer = TIME;
                //Ressourceupdate
            }
            #endregion
        }
        /// <summary>
        /// Draw Part of Map
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="mapPosition"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 mapPosition)
        { 
            if(!empty)
                spriteBatch.Draw(tile_texture, GetTileMapPosition(mapPosition), Color.Wheat);
        }
        /// <summary>
        /// Set Tiletype bei Enum-TileType; Update Texture of tile
        /// </summary>
        /// <param name="_tileType"></param>
        /// <param name="GraphicsDevice"></param>
        /// <param name="tileMap"></param>
        public void SetTileType(TileNumber _tileType, GraphicsDevice GraphicsDevice, Texture2D tileMap)
        {
            tiletype = _tileType;

            Color[] data;
            Rectangle rect = GetTileSpritePosition(tiletype);

            tile_texture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            tileMap.GetData(0, rect, data, 0, data.Length);
            tile_texture.SetData(data);
        }
        /// <summary>
        /// Set Tiletype bei Enum-TileBaseType; Update Texture of tile; used bei Map-Generation
        /// </summary>
        /// <param name="_tilebasetype"></param>
        /// <param name="GraphicsDevice"></param>
        /// <param name="tileMap"></param>
        public void SetTileType(TileBaseType _tilebasetype, GraphicsDevice GraphicsDevice, Texture2D tileMap)
        {
            tileBaseType = _tilebasetype;
            switch (tileBaseType)
            {
                case TileBaseType.Mountain:
                    tiletype = TileNumber.mountain;
                    break;
                case TileBaseType.Wood:
                    tiletype = TileNumber.wood;
                    break;
                case TileBaseType.Water:
                    tiletype = TileNumber.flatwater;
                    break;
            }

            Color[] data;
            Rectangle rect = GetTileSpritePosition(tiletype);

            tile_texture = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            tileMap.GetData(0, rect, data, 0, data.Length);
            tile_texture.SetData(data);
        }
        /// <summary>
        /// Returns a Rectangle descriping position and dimension of sprite on a spritemap
        /// </summary>
        /// <param name="tileNumber"></param>
        /// <returns></returns>
        private Rectangle GetTileSpritePosition(TileNumber tileNumber)
        {
            return new Rectangle(Constants.tile_width * ((int)tileNumber % Constants.tiles_per_row), Constants.tile_height * ((int)tileNumber / Constants.tiles_per_row), Constants.tile_width, Constants.tile_height);
        }
        /// <summary>
        /// Calculate Position of Tilesprite for x,y of this tile
        /// </summary>
        /// <returns></returns>
        private Rectangle GetTileMapPosition(Vector2 mapPosition)
        {
            int rect_x;
            int rect_y;

            if((x - (int)mapPosition.X)%2 == 0)
            {
                rect_x = ((x - (int)mapPosition.X) / 2) * (Constants.tile_x_offset + Constants.tile_x_offset / 3);
            }
            else
            {
                rect_x = (Constants.tile_x_offset / 3 * 2) + (((x - (int)mapPosition.X) - 1)/2) * (Constants.tile_x_offset + Constants.tile_x_offset / 3);
            }

            if((y-mapPosition.Y)%2 == 0)
            {
                rect_y = ((y - (int)mapPosition.Y) / 2) * Constants.tile_y_offset;
            }
            else
            {
                rect_y = (Constants.tile_y_offset/2) + (((y - (int)mapPosition.Y) - 1) / 2) * Constants.tile_y_offset;
            }
                    
            return new Rectangle(rect_x * Constants.tile_stretch_factor, rect_y * Constants.tile_stretch_factor, Constants.tile_width * Constants.tile_stretch_factor, Constants.tile_height * Constants.tile_stretch_factor);
        }
        /// <summary>
        /// Calculate ClickArea of this tile, depending on x,y of this tile
        /// </summary>
        /// <returns></returns>
        private Rectangle GetClickArea()
        {
            return new Rectangle();
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class Tile
    {
        #region VARS
        public TileNumber tiletype;
        public int x, y; //koordinates on map
        Texture2D tile_texture;
        bool empty = false;


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
        }
    
        public void Update(MouseState mouseState)
        {
            
        }
        public void Draw(SpriteBatch spriteBatch)
        { 
            if(!empty)
                spriteBatch.Draw(tile_texture, GetTileMapPosition(), Color.Wheat);
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

        private Rectangle GetTileMapPosition()
        {
            Rectangle rect = new Rectangle(Constants.tile_x_offset * x, Constants.tile_y_offset * y, 32, 48);

            int rect_x;
            int rect_y;

            if(x%2 == 0)
            {
                rect_x = (x/2) * (Constants.tile_x_offset + Constants.tile_x_offset / 3);
            }
            else
            {
                rect_x = (Constants.tile_x_offset / 3 * 2) + ((x-1)/2) * (Constants.tile_x_offset + Constants.tile_x_offset / 3);
            }

            if(y%2 == 0)
            {
                rect_y = (y / 2) * Constants.tile_y_offset;
            }
            else
            {
                rect_y = (Constants.tile_y_offset/2) + ((y-1) / 2) * Constants.tile_y_offset;
            }
                    



            return new Rectangle(rect_x, rect_y, 32, 48);
        }
    }
}

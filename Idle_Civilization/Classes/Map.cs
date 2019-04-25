using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class Map
    {
        private List<List<Tile>> map;
        int width, height;
        Vector2 mapPosition;

        #region Map-Characteristics and Generation
        private int mountain_density = 10;
        private int mountain_spread = 20;
        private int wood_density = 15;
        private int wood_spread = 20;
        private int water_density = 10;
        private int water_spread = 0;
        private int enemy_density = 20;
        private int enemy_spread = 40;

        Random rand = new Random();
        int spread = 0;
        #endregion

        #region mapscrolling
        //Updatetimer
        double timer = 100; //500ms
        const double TIME = 100; //to init
        //Rectangle-Boundaries 
        Rectangle top_bar, bottom_bar, left_bar, right_bar, upper_left, upper_right, lower_left, lower_right;

        #endregion

        public Map(GraphicsDevice GraphicsDevice, Texture2D tileMap, int _width, int _height, int screen_width, int screen_height)
        {
            width = _width;
            height = _height;

            GenerateMap(GraphicsDevice, tileMap);

            mapPosition = new Vector2(0, 0);

            top_bar = new Rectangle(50, 0, screen_width - 100, 50);
            bottom_bar = new Rectangle(50, screen_height - 50, screen_width - 100, 50);
            left_bar = new Rectangle(0, 50, 50, screen_height - 100);
            right_bar = new Rectangle(screen_width - 50, 50, 50, screen_height - 100);

            upper_left = new Rectangle(0, 0, 50, 50);
            upper_right = new Rectangle(screen_width - 50, 0, 50, 50);
            lower_left = new Rectangle(0, screen_height - 50, 50, 50);
            lower_right = new Rectangle(screen_width - 50, screen_height - 50, 50, 50);
        }

        public void Update(MouseState mouseState, GameTime gameTime)
        {
            #region scrolling
            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            timer -= elapsed;

            if(timer < 0)
            {
                timer = TIME;

                //check if mouse is inside boundaries
                if(Utility.mouseInBounds(top_bar, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.Y > 0)
                        mapPosition.Y -= 1;
                }
                else if (Utility.mouseInBounds(bottom_bar, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.Y < height)
                        mapPosition.Y += 1;
                }
                else if (Utility.mouseInBounds(left_bar, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.X > 0)
                        mapPosition.X -= 1;
                }
                else if (Utility.mouseInBounds(right_bar, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.X < width)
                        mapPosition.X += 1;
                }
                else if (Utility.mouseInBounds(upper_right, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.Y > 0)
                        mapPosition.Y -= 1;
                    if(mapPosition.X < width)
                        mapPosition.X += 1;
                }
                else if (Utility.mouseInBounds(upper_left, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.Y > 0)
                        mapPosition.Y -= 1;
                    if(mapPosition.X > 0)
                        mapPosition.X -= 1;
                }
                else if (Utility.mouseInBounds(lower_right, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.Y < height)
                        mapPosition.Y += 1;
                    if(mapPosition.X < width)
                        mapPosition.X += 1;
                }
                else if (Utility.mouseInBounds(lower_left, new Vector2(mouseState.X, mouseState.Y)))
                {
                    if(mapPosition.Y < height)
                        mapPosition.Y += 1;
                    if(mapPosition.X > 0)
                        mapPosition.X -= 1;
                }
            }
            #endregion
        }
    
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = (int)mapPosition.X; x < width; x++)
            {
                for (int y = (int)mapPosition.Y; y < height; y++)
                {
                    map[x][y].Draw(spriteBatch, mapPosition);
                }
            }
        }

        #region Map-Generation
        public void SetMapParameters(int _mountain_density, int _mountain_spread, int _wood_density, int _wood_spread, int _water_density, int _water_spread, int _enemy_density, int _enemy_spread)
        {

        }
        private void GenerateMap(GraphicsDevice GraphicsDevice, Texture2D tileMap)
        {
            map = new List<List<Tile>>();

            //Generate blank map with gras
            for (int x = 0; x < width; x++)
            {
                map.Add(new List<Tile>());
                for (int y = 0; y < height; y++)
                {
                    //tile on even x-value with even y-value => visible tile
                    if (x % 2 == 0 && y % 2 == 0)
                    {
                        map[x].Add(new Tile(GraphicsDevice, tileMap, Classes.TileNumber.gras, x, y));
                    }
                    //tile on odd x-value with odd y-value ==> visible tile
                    else if (x % 2 != 0 && y % 2 != 0)
                    {
                        map[x].Add(new Tile(GraphicsDevice, tileMap, Classes.TileNumber.gras, x, y));
                    }
                    //empty tile
                    else
                    {
                        map[x].Add(new Tile());
                    }
                }
            }

            //Add Mountains
            for (int i = 0; i < mountain_density; i++)
            {
                int start_x, start_y;
                do
                {
                    start_x = rand.Next(0, width);
                    start_y = rand.Next(0, height);
                } while (!IsValidTile(start_x, start_y));

                map[start_x][start_y].SetTileType(TileNumber.mountain, GraphicsDevice, tileMap);

                SetNeighbors(start_x, start_y, TileBaseType.Mountain, GraphicsDevice, tileMap);
            }

            //Add Woods
            for (int i = 0; i < wood_density; i++)
            {
                int start_x, start_y;
                do
                {
                    start_x = rand.Next(0, width);
                    start_y = rand.Next(0, height);
                } while (!IsValidTile(start_x, start_y));

                map[start_x][start_y].SetTileType(TileNumber.wood, GraphicsDevice, tileMap);

                SetNeighbors(start_x, start_y, TileBaseType.Wood, GraphicsDevice, tileMap);
            }

            //Add Water
            for (int i = 0; i < water_density; i++)
            {
                int start_x, start_y;
                do
                {
                    start_x = rand.Next(0, width);
                    start_y = rand.Next(0, height);
                } while (!IsValidTile(start_x, start_y));

                map[start_x][start_y].SetTileType(TileNumber.flatwater, GraphicsDevice, tileMap);

                SetNeighbors(start_x, start_y, TileBaseType.Water, GraphicsDevice, tileMap);
            }
        }
        private bool IsValidTile(int x, int y)
        {
            return (x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0);
        }

        private void SetNeighbors(int _x, int _y, TileBaseType tileBaseType, GraphicsDevice GraphicsDevice, Texture2D tileMap)
        {
            int x = _x, y = _y;
            //get spread of tileBaseType
            switch(tileBaseType)
            {
                case TileBaseType.Mountain:
                    spread = mountain_spread; break;
                case TileBaseType.Wood:
                    spread = wood_spread; break;
                case TileBaseType.Water:
                    spread = water_spread; break;
            }

            //tile on top (x; y-=2)
            if (y - 2 >= 0 && map[x][y - 2].tileBaseType != tileBaseType)
            {
                if (rand.Next(0, 100) <= spread)
                {
                    map[x][y - 2].SetTileType(tileBaseType, GraphicsDevice, tileMap);
                    SetNeighbors(x, y - 2, tileBaseType, GraphicsDevice, tileMap);
                }
            }

            //tile upper right (x++;y--)
            if (y-1 >= 0 && x+1 < width && map[x + 1][y - 1].tileBaseType != tileBaseType)
            {
                if (rand.Next(0, 100) <= spread)
                {
                    map[x + 1][y - 1].SetTileType(tileBaseType, GraphicsDevice, tileMap);
                    SetNeighbors(x + 1, y - 1, tileBaseType, GraphicsDevice, tileMap);
                }
            }

            //tile lower right (x++, y++)
            if (y + 1 < height && x + 1 < width && map[x + 1][y + 1].tileBaseType != tileBaseType)
            {
                if (rand.Next(0, 100) <= spread)
                {
                    map[x + 1][y + 1].SetTileType(tileBaseType, GraphicsDevice, tileMap);
                    SetNeighbors(x + 1, y + 1, tileBaseType, GraphicsDevice, tileMap);
                }
            }

            //tile bottom (x, y+=2)
            if (y + 2 < height && map[x][y + 2].tileBaseType != tileBaseType)
            {
                if (rand.Next(0, 100) <= spread)
                {
                    map[x][y + 2].SetTileType(tileBaseType, GraphicsDevice, tileMap);
                    SetNeighbors(x, y + 2, tileBaseType, GraphicsDevice, tileMap);
                }
            }

            //tile lower left (x--, y++)
            if (y + 1 < height && x - 1 >= 0 && map[x - 1][y + 1].tileBaseType != tileBaseType)
            {
                if (rand.Next(0, 100) <= spread)
                {
                    map[x - 1][y + 1].SetTileType(tileBaseType, GraphicsDevice, tileMap);
                    SetNeighbors(x - 1, y + 1, tileBaseType, GraphicsDevice, tileMap);
                }
            }

            //tile upper left (x--, y--)
            if (y - 1 >= 0 && x - 1 >= 0 && map[x - 1][y - 1].tileBaseType != tileBaseType)
            {
                if (rand.Next(0, 100) <= spread)
                {
                    map[x - 1][y - 1].SetTileType(tileBaseType, GraphicsDevice, tileMap);
                    SetNeighbors(x - 1, y - 1, tileBaseType, GraphicsDevice, tileMap);
                }
            }
        }
        #endregion
    }

    enum TileBaseType
    {
        None,
        Mountain,
        Wood,
        Water
    }
}

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

        Random rand = new Random();
        int spread = 0;
        #region Map-Characteristics
        private int mountain_density = 10;
        private int mountain_spread = 20;
        private int wood_density = 15;
        private int wood_spread = 20;
        private int water_density = 10;
        private int water_spread = 0;
        private int enemy_density = 20;
        private int enemy_spread = 40;
        #endregion


        public Map(GraphicsDevice GraphicsDevice, Texture2D tileMap, int _width, int _height)
        {
            width = _width;
            height = _height;

            map = new List<List<Tile>>();

            GenerateMap(GraphicsDevice, tileMap);
        }

        public void Update()
        { }
    
        public void Draw(SpriteBatch spriteBatch)
        {
            //for(int x = width - 1; x >= 0; x--)
            //{
            //    for(int y=height- 1; y >= 0; y--)
            //    {
            //        map[x][y].Draw(spriteBatch);
            //    }
            //}

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x][y].Draw(spriteBatch);
                }
            }
        }

        #region Map-Generation
        private void SetMapParameters(int _mountain_density, int _mountain_spread, int _wood_density, int _wood_spread, int _water_density, int _water_spread, int _enemy_density, int _enemy_spread)
        {

        }
        private void GenerateMap(GraphicsDevice GraphicsDevice, Texture2D tileMap)
        {
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

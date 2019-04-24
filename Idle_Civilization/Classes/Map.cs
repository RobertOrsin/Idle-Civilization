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


        public Map(GraphicsDevice GraphicsDevice, Texture2D tileMap)
        {
            map = new List<List<Tile>>();

            //Generate samplemap
            for (int x = 0; x < 10; x++)
            {
                map.Add(new List<Tile>());
                for(int y = 0; y < 20; y++)
                {
                    //tile on even x-value with even y-value => visible tile
                    if(x%2 == 0 && y%2 == 0)
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
                
        }

        public void Update()
        { }
    
        public void Draw(SpriteBatch spriteBatch)
        {
            for(int x = 9; x >= 0; x--)
            {
                for(int y=19; y >= 0; y--)
                {
                    map[x][y].Draw(spriteBatch);
                }
            }
        }
    }
}

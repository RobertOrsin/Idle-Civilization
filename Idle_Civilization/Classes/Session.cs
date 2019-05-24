﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;

using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class Session
    {
        TileMenu tileMenu;
        Point selectedTile;
        
        int width, height;  // of map
        Vector2 mapPosition; //upper left corner

        Player player;
        HUD hud;
        UpgradeMenu upgradeMenu;

        bool debounce_function = false;

        Texture2D tileMap;
        GraphicsDevice GraphicsDevice;
        private List<List<Tile>> map;
        #region Map-Characteristics and Generation
        private int mountain_density = 10;
        private int mountain_spread = 20;
        private int wood_density = 15;
        private int wood_spread = 20;
        private int water_density = 10;
        private int water_spread = 0;
        private int enemy_density = 5;
        private int enemy_spread = 20;

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

        #region Upgrades/Costs
        List<Upgrade> wood_Upgrades, food_Upgrades, ore_Upgrades, army_Upgrades, global_Upgrades;
        List<Ressources> buildCosts;
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="tileMap"></param>
        /// <param name="_width"></param>
        /// <param name="_height"></param>
        /// <param name="screen_width"></param>
        /// <param name="screen_height"></param>
        public Session(GraphicsDevice _GraphicsDevice, Texture2D _tileMap, Texture2D mediumButtons, Texture2D smallButtons, int _width, int _height, int screen_width, int screen_height)
        {
            width = _width;
            height = _height;

            tileMap = _tileMap;
            GraphicsDevice = _GraphicsDevice;
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

            tileMenu = new TileMenu(GraphicsDevice, mediumButtons, smallButtons);

            player = new Player(new Ressources(100000, 100000, 100000, 0));
            LoadUpgradesCosts();
        }
        /// <summary>
        /// Updatefunction
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="gameTime"></param>
        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            #region scrolling
            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            timer -= elapsed;

            if(timer < 0)
            {
                timer = TIME;
                Vector2 mapPosOld = mapPosition;

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

                if (mapPosOld != mapPosition)
                    tileMenu.visible = false;
            }
            #endregion

            TileUpdateData tileUpdateData;
            TileMenuUpdateData tileMenuUpdateData = new TileMenuUpdateData(false);

            tileMenu.selectedTile = map[selectedTile.X][selectedTile.Y];
            tileMenuUpdateData = tileMenu.Update(keyboardState, mouseState);

            if (tileMenuUpdateData.clickDetected)
            {
                ExecuteFunction(tileMenuUpdateData.tileMenuFunction, map[selectedTile.X][selectedTile.Y]);
            }

            for (int x = 0; x < map.Count; x++)
            {
                for (int y = 0; y < map[0].Count; y++)
                {
                    tileUpdateData = map[x][y].Update(mouseState, gameTime, GetNeighbors(map[x][y]));

                    if (tileUpdateData.clickDetected && !tileMenuUpdateData.clickDetected)
                    {
                        tileMenu.visible = true;
                        selectedTile = new Point(x, y);
                    }
                }
            }
        }
        /// <summary>
        /// Drawfunction
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            for (int x = (int)mapPosition.X; x < width; x++)
            {
                for (int y = (int)mapPosition.Y; y < height; y++)
                {
                    map[x][y].Draw(spriteBatch, mapPosition);
                }
            }

            tileMenu.Draw(spriteBatch, spriteFont);
        }
        /// <summary>
        /// Execute a function depending on clicked Button from TileMenu
        /// </summary>
        /// <param name="function"></param>
        /// <param name="tile"></param>
        public void ExecuteFunction(TileMenuFunction function, Tile tile)
        {
            switch(function)
            {
                case TileMenuFunction.foundCity:
                    if (FoundCityValid(map[selectedTile.X][selectedTile.Y]) && player.CanAfford(buildCosts[(int)Buildcosts.CreateCity]))
                    {
                        //Create City on tile
                        map[selectedTile.X][selectedTile.Y].SetAsCity(GraphicsDevice, tileMap, player.cityCount);
                        //Add adjacent tiles to this city
                        MakeNeighborsToPartsOfCity(map[selectedTile.X][selectedTile.Y]);

                        player.cityCount++;
                    }
                    break;
                case TileMenuFunction.addPeople:
                    if (tile.hasCity && player.CanAfford(buildCosts[(int)Buildcosts.Worker]))
                    {
                        map[selectedTile.X][selectedTile.Y].population++;
                        player.ressources.SubRessources(buildCosts[(int)Buildcosts.Worker]);
                    }
                    break;
                case TileMenuFunction.addArmy:
                    break;
                case TileMenuFunction.subArmy:
                    break;
                case TileMenuFunction.addFood:
                    break;
                case TileMenuFunction.subFood:
                    break;
                case TileMenuFunction.addOre:
                    break;
                case TileMenuFunction.subOre:
                    break;
                case TileMenuFunction.addWood:
                    break;
                case TileMenuFunction.subWood:
                    break;
                case TileMenuFunction.addTile:
                    break;
                case TileMenuFunction.attackTile:
                    break;
                default: break;
            }
        }

        #region Map-Generation
        /// <summary>
        /// Set Map-Parameters to generate different mapstyles
        /// </summary>
        /// <param name="_mountain_density"></param>
        /// <param name="_mountain_spread"></param>
        /// <param name="_wood_density"></param>
        /// <param name="_wood_spread"></param>
        /// <param name="_water_density"></param>
        /// <param name="_water_spread"></param>
        /// <param name="_enemy_density"></param>
        /// <param name="_enemy_spread"></param>
        public void SetMapParameters(int _mountain_density, int _mountain_spread, int _wood_density, int _wood_spread, int _water_density, int _water_spread, int _enemy_density, int _enemy_spread)
        {

        }
        /// <summary>
        /// Generate Map
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="tileMap"></param>
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

            //Add Enemys
            for(int i = 0; i < enemy_density; i++)
            {
                int start_x, start_y;
                do
                {
                    start_x = rand.Next(0, width);
                    start_y = rand.Next(0, height);
                } while (!IsValidTile(start_x, start_y) && map[start_x][start_y].tileBaseType != TileBaseType.None);

                map[start_x][start_y].SetAsEnemyBase(GraphicsDevice, tileMap);

                SetNeighbors(start_x, start_y, TileBaseType.Enemy, GraphicsDevice, tileMap);
            }
        }
        /// <summary>
        /// Check if a Tile is Valid to be set to TileType
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool IsValidTile(int x, int y)
        {
            return (x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0);
        }
        /// <summary>
        /// Rekursive function to spread a tiletype across the map
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        /// <param name="tileBaseType"></param>
        /// <param name="GraphicsDevice"></param>
        /// <param name="tileMap"></param>
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
                case TileBaseType.Enemy:
                    spread = enemy_spread; break;
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
        /// <summary>
        /// Stores Border-Textures in static List
        /// </summary>
        public void SerializeBorderTextures(GraphicsDevice GraphicsDevice, Texture2D borders)
        {
            Globals.enemyBorders = new List<Texture2D>();
            Globals.playerBorders = new List<Texture2D>();

            for(int i = 0; i < Enum.GetNames(typeof(BorderNumber)).Length; i++)
            {
                //enemy border
                Rectangle rect = new Rectangle(32 * i, 0, 32, 48);
                Texture2D tex = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                Color[] data = new Color[rect.Width * rect.Height];

                borders.GetData(0, rect, data, 0, data.Length);
                tex.SetData(data);

                Globals.enemyBorders.Add(tex);

                //player border
                rect = new Rectangle(32 * i, 48, 32, 48);
                tex = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                data = new Color[rect.Width * rect.Height];

                borders.GetData(0, rect, data, 0, data.Length);
                tex.SetData(data);

                Globals.playerBorders.Add(tex);
            }
        }
        #endregion

        #region controllfunctions
        /// <summary>
        /// Check if a city can be founded on a tile.
        /// Citys may only be founded on grasstiles and no other city adjacent
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool FoundCityValid(Tile tile)
        {
            if (!tile.hasCity && tile.tileBaseType == TileBaseType.None && !AdjacentTo(TileControllType.cityTile,tile))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Check if a given tile is adjacent to a tile with given TileControllType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool AdjacentTo(TileControllType type, Tile tile)
        {
            bool result = false;
            int x = tile.x, y = tile.y;
            switch(type)
            {
                case TileControllType.cityTile:
                    result = map[x - 1][y - 1].isCitypart ||
                            map[x][y - 2].isCitypart ||
                            map[x + 1][y - 1].isCitypart ||
                            map[x + 1][y + 1].isCitypart ||
                            map[x][y + 2].isCitypart ||
                            map[x - 1][y + 1].isCitypart;
                    break;
                case TileControllType.enemyTile:
                    break;
                default: break;

            }
            return result;
        }

        private void MakeNeighborsToPartsOfCity(Tile tile)
        {
            int x = tile.x, y = tile.y;

            map[x - 1][y - 1].SetAsCityPart(tile.cityID);
            map[x][y - 2].SetAsCityPart(tile.cityID);
            map[x + 1][y - 1].SetAsCityPart(tile.cityID);
            map[x + 1][y + 1].SetAsCityPart(tile.cityID);
            map[x][y + 2].SetAsCityPart(tile.cityID);
            map[x - 1][y + 1].SetAsCityPart(tile.cityID);
        }
        #endregion

        #region setter/getter
        private List<Tile> GetNeighbors(Tile tile)
        {
            List<Tile> result = new List<Tile>();
            int x = tile.x, y = tile.y;

            if (x > 0 && y > 0)
                result.Add(map[x - 1][y - 1]);
            else
                result.Add(new Tile());

            if (y > 1)
                result.Add(map[x][y - 2]);
            else
                result.Add(new Tile());

            if(x < width - 1 && y > 0)
                result.Add(map[x + 1][y - 1]);
            else
                result.Add(new Tile());

            if(x < width - 1 && y < height - 1)
                result.Add(map[x + 1][y + 1]);
            else
                result.Add(new Tile());

            if(y < height - 2)
                result.Add(map[x][y + 2]);
            else
                result.Add(new Tile());

            if(x > 0 && y < height - 1)
                result.Add(map[x - 1][y + 1]);
            else
                result.Add(new Tile());

            return result;
        }
        #endregion

        #region config-file
        /// <summary>
        /// load upgrade-texts from xml-file
        /// </summary>
        public void LoadUpgradesCosts()
        {
            //Clear Lists
            wood_Upgrades = new List<Upgrade>();
            food_Upgrades = new List<Upgrade>();
            ore_Upgrades = new List<Upgrade>();
            army_Upgrades = new List<Upgrade>();
            global_Upgrades = new List<Upgrade>();
            buildCosts = new List<Ressources>();


            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string filepath = directory + "\\config.xml";

            XmlTextReader reader = new XmlTextReader(filepath);

            while (reader.Read())
            {
                string nodeName = reader.Name;

                if (nodeName == "Wood_Upgrades")
                {
                    reader.Read();
                    do
                    {
                        nodeName = reader.Name;
                        if (nodeName == "Wood_Upgrade")
                        {
                            wood_Upgrades.Add(new Upgrade(reader.GetAttribute("Name"), reader.GetAttribute("Tooltip"), reader.GetAttribute("Cost"), reader.GetAttribute("Modifier")));
                        }
                        reader.Read();
                    } while (nodeName != "Wood_Upgrades");
                }
                else if (nodeName == "Food_Upgrades")
                {
                    reader.Read();
                    do
                    {
                        nodeName = reader.Name;
                        if (nodeName == "Food_Upgrade")
                        {
                            food_Upgrades.Add(new Upgrade(reader.GetAttribute("Name"), reader.GetAttribute("Tooltip"), reader.GetAttribute("Cost"), reader.GetAttribute("Modifier")));
                        }
                        reader.Read();
                    } while (nodeName != "Food_Upgrades");
                }
                else if (nodeName == "Ore_Upgrades")
                {
                    reader.Read();
                    do
                    {
                        nodeName = reader.Name;
                        if (nodeName == "Ore_Upgrade")
                        {
                            ore_Upgrades.Add(new Upgrade(reader.GetAttribute("Name"), reader.GetAttribute("Tooltip"), reader.GetAttribute("Cost"), reader.GetAttribute("Modifier")));
                        }
                        reader.Read();
                    } while (nodeName != "Ore_Upgrades");
                }
                else if (nodeName == "Army_Upgrades")
                {
                    reader.Read();
                    do
                    {
                        nodeName = reader.Name;
                        if (nodeName == "Army_Upgrade")
                        {
                            army_Upgrades.Add(new Upgrade(reader.GetAttribute("Name"), reader.GetAttribute("Tooltip"), reader.GetAttribute("Cost"), reader.GetAttribute("Modifier")));
                        }
                        reader.Read();
                    } while (nodeName != "Army_Upgrades");
                }
                else if (nodeName == "Global_Upgrades")
                {
                    reader.Read();
                    do
                    {
                        nodeName = reader.Name;
                        if (nodeName == "Global_Upgrade")
                        {
                            global_Upgrades.Add(new Upgrade(reader.GetAttribute("Name"), reader.GetAttribute("Tooltip"), reader.GetAttribute("Cost"), reader.GetAttribute("Modifier")));
                        }
                        reader.Read();
                    } while (nodeName != "Global_Upgrades");
                }
                else if (nodeName == "BuildCosts")
                {
                    reader.Read();
                    do
                    {
                        nodeName = reader.Name;
                        if (nodeName == "BuildCost")
                        {
                            buildCosts.Add(new Ressources(reader.GetAttribute("Cost")));
                        }
                        reader.Read();
                    } while (nodeName != "BuildCosts");
                }
            }
        }
        /// <summary>
        /// create dummy xml-file for upgrades
        /// </summary>
        public void SaveUpgradesCosts()
        {
            #region generate dummylist
            wood_Upgrades = new List<Upgrade>();

            wood_Upgrades.Add(new Upgrade("Upgrade1", "Tooltip 1", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            wood_Upgrades.Add(new Upgrade("Upgrade2", "Tooltip 2", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            wood_Upgrades.Add(new Upgrade("Upgrade3", "Tooltip 3", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            wood_Upgrades.Add(new Upgrade("Upgrade4", "Tooltip 4", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));

            food_Upgrades = new List<Upgrade>();

            food_Upgrades.Add(new Upgrade("Upgrade1", "Tooltip 1", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            food_Upgrades.Add(new Upgrade("Upgrade2", "Tooltip 2", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            food_Upgrades.Add(new Upgrade("Upgrade3", "Tooltip 3", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            food_Upgrades.Add(new Upgrade("Upgrade4", "Tooltip 4", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));

            ore_Upgrades = new List<Upgrade>();

            ore_Upgrades.Add(new Upgrade("Upgrade1", "Tooltip 1", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            ore_Upgrades.Add(new Upgrade("Upgrade2", "Tooltip 2", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            ore_Upgrades.Add(new Upgrade("Upgrade3", "Tooltip 3", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            ore_Upgrades.Add(new Upgrade("Upgrade4", "Tooltip 4", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));

            army_Upgrades = new List<Upgrade>();

            army_Upgrades.Add(new Upgrade("Upgrade1", "Tooltip 1", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            army_Upgrades.Add(new Upgrade("Upgrade2", "Tooltip 2", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            army_Upgrades.Add(new Upgrade("Upgrade3", "Tooltip 3", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            army_Upgrades.Add(new Upgrade("Upgrade4", "Tooltip 4", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));


            global_Upgrades = new List<Upgrade>();

            global_Upgrades.Add(new Upgrade("Upgrade1", "Tooltip 1", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            global_Upgrades.Add(new Upgrade("Upgrade2", "Tooltip 2", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            global_Upgrades.Add(new Upgrade("Upgrade3", "Tooltip 3", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));
            global_Upgrades.Add(new Upgrade("Upgrade4", "Tooltip 4", new Ressources(100, 100, 100, 0), new Ressources(0.5, 0, 10.0, 0)));

            buildCosts = new List<Ressources>();

            buildCosts.Add(new Ressources(200, 200, 200, 0));
            buildCosts.Add(new Ressources(100, 100, 100, 0));
            buildCosts.Add(new Ressources(0, 100, 0, 0));
            buildCosts.Add(new Ressources(0, 0, 0, 200));
            #endregion


            //Path of File on folder of executeable
            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            #region create XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("IDLECIV_Upgrades");
            xmlDoc.AppendChild(rootNode);

            //wood upgrades
            XmlNode Node_wood_Upgrades = xmlDoc.CreateElement("Wood_Upgrades");
            foreach (Upgrade upgrade in wood_Upgrades)
            {
                XmlNode node_wood_upgrade = xmlDoc.CreateElement("Wood_Upgrade");

                XmlAttribute node_wood_attribute_name = xmlDoc.CreateAttribute("Name");
                node_wood_attribute_name.Value = upgrade.name;
                node_wood_upgrade.Attributes.Append(node_wood_attribute_name);

                XmlAttribute node_wood_attribute_tooltip = xmlDoc.CreateAttribute("Tooltip");
                node_wood_attribute_tooltip.Value = upgrade.tooltip;
                node_wood_upgrade.Attributes.Append(node_wood_attribute_tooltip);

                XmlAttribute node_wood_attribute_cost = xmlDoc.CreateAttribute("Cost");
                node_wood_attribute_cost.Value = upgrade.cost.AsString();
                node_wood_upgrade.Attributes.Append(node_wood_attribute_cost);

                XmlAttribute node_wood_attribute_modifier = xmlDoc.CreateAttribute("Modifier");
                node_wood_attribute_modifier.Value = upgrade.modifier.AsString();
                node_wood_upgrade.Attributes.Append(node_wood_attribute_modifier);

                Node_wood_Upgrades.AppendChild(node_wood_upgrade);
            }
            rootNode.AppendChild(Node_wood_Upgrades);

            //food upgrades
            XmlNode Node_food_Upgrades = xmlDoc.CreateElement("Food_Upgrades");
            foreach (Upgrade upgrade in food_Upgrades)
            {
                XmlNode node_food_upgrade = xmlDoc.CreateElement("Food_Upgrade");

                XmlAttribute node_food_attribute_name = xmlDoc.CreateAttribute("Name");
                node_food_attribute_name.Value = upgrade.name;
                node_food_upgrade.Attributes.Append(node_food_attribute_name);

                XmlAttribute node_food_attribute_tooltip = xmlDoc.CreateAttribute("Tooltip");
                node_food_attribute_tooltip.Value = upgrade.tooltip;
                node_food_upgrade.Attributes.Append(node_food_attribute_tooltip);

                XmlAttribute node_food_attribute_cost = xmlDoc.CreateAttribute("Cost");
                node_food_attribute_cost.Value = upgrade.cost.AsString();
                node_food_upgrade.Attributes.Append(node_food_attribute_cost);

                XmlAttribute node_food_attribute_modifier = xmlDoc.CreateAttribute("Modifier");
                node_food_attribute_modifier.Value = upgrade.modifier.AsString();
                node_food_upgrade.Attributes.Append(node_food_attribute_modifier);

                Node_food_Upgrades.AppendChild(node_food_upgrade);
            }
            rootNode.AppendChild(Node_food_Upgrades);

            //ore upgrades
            XmlNode Node_ore_Upgrades = xmlDoc.CreateElement("Ore_Upgrades");
            foreach (Upgrade upgrade in ore_Upgrades)
            {
                XmlNode node_ore_upgrade = xmlDoc.CreateElement("Ore_Upgrade");

                XmlAttribute node_ore_attribute_name = xmlDoc.CreateAttribute("Name");
                node_ore_attribute_name.Value = upgrade.name;
                node_ore_upgrade.Attributes.Append(node_ore_attribute_name);

                XmlAttribute node_ore_attribute_tooltip = xmlDoc.CreateAttribute("Tooltip");
                node_ore_attribute_tooltip.Value = upgrade.tooltip;
                node_ore_upgrade.Attributes.Append(node_ore_attribute_tooltip);

                XmlAttribute node_ore_attribute_cost = xmlDoc.CreateAttribute("Cost");
                node_ore_attribute_cost.Value = upgrade.cost.AsString();
                node_ore_upgrade.Attributes.Append(node_ore_attribute_cost);

                XmlAttribute node_ore_attribute_modifier = xmlDoc.CreateAttribute("Modifier");
                node_ore_attribute_modifier.Value = upgrade.modifier.AsString();
                node_ore_upgrade.Attributes.Append(node_ore_attribute_modifier);

                Node_ore_Upgrades.AppendChild(node_ore_upgrade);
            }
            rootNode.AppendChild(Node_ore_Upgrades);

            //army upgrades
            XmlNode Node_army_Upgrades = xmlDoc.CreateElement("Army_Upgrades");
            foreach (Upgrade upgrade in army_Upgrades)
            {
                XmlNode node_army_upgrade = xmlDoc.CreateElement("Army_Upgrade");

                XmlAttribute node_army_attribute_name = xmlDoc.CreateAttribute("Name");
                node_army_attribute_name.Value = upgrade.name;
                node_army_upgrade.Attributes.Append(node_army_attribute_name);

                XmlAttribute node_army_attribute_tooltip = xmlDoc.CreateAttribute("Tooltip");
                node_army_attribute_tooltip.Value = upgrade.tooltip;
                node_army_upgrade.Attributes.Append(node_army_attribute_tooltip);

                XmlAttribute node_army_attribute_cost = xmlDoc.CreateAttribute("Cost");
                node_army_attribute_cost.Value = upgrade.cost.AsString();
                node_army_upgrade.Attributes.Append(node_army_attribute_cost);

                XmlAttribute node_army_attribute_modifier = xmlDoc.CreateAttribute("Modifier");
                node_army_attribute_modifier.Value = upgrade.modifier.AsString();
                node_army_upgrade.Attributes.Append(node_army_attribute_modifier);

                Node_army_Upgrades.AppendChild(node_army_upgrade);
            }
            rootNode.AppendChild(Node_army_Upgrades);

            //global upgrades
            XmlNode Node_global_Upgrades = xmlDoc.CreateElement("Global_Upgrades");
            foreach (Upgrade upgrade in global_Upgrades)
            {
                XmlNode node_global_upgrade = xmlDoc.CreateElement("Global_Upgrade");

                XmlAttribute node_global_attribute_name = xmlDoc.CreateAttribute("Name");
                node_global_attribute_name.Value = upgrade.name;
                node_global_upgrade.Attributes.Append(node_global_attribute_name);

                XmlAttribute node_global_attribute_tooltip = xmlDoc.CreateAttribute("Tooltip");
                node_global_attribute_tooltip.Value = upgrade.tooltip;
                node_global_upgrade.Attributes.Append(node_global_attribute_tooltip);

                XmlAttribute node_global_attribute_cost = xmlDoc.CreateAttribute("Cost");
                node_global_attribute_cost.Value = upgrade.cost.AsString();
                node_global_upgrade.Attributes.Append(node_global_attribute_cost);

                XmlAttribute node_global_attribute_modifier = xmlDoc.CreateAttribute("Modifier");
                node_global_attribute_modifier.Value = upgrade.modifier.AsString();
                node_global_upgrade.Attributes.Append(node_global_attribute_modifier);

                Node_global_Upgrades.AppendChild(node_global_upgrade);
            }
            rootNode.AppendChild(Node_global_Upgrades);

            //buildCosts
            XmlNode Node_BuildCosts = xmlDoc.CreateElement("BuildCosts");
            foreach (Ressources cost in buildCosts)
            {
                XmlNode node_BuildCost = xmlDoc.CreateElement("BuildCost");

                XmlAttribute node_cost = xmlDoc.CreateAttribute("Cost");
                node_cost.Value = cost.AsString();
                node_BuildCost.Attributes.Append(node_cost);

                Node_BuildCosts.AppendChild(node_BuildCost);
            }
            rootNode.AppendChild(Node_BuildCosts);

            xmlDoc.Save(directory + "\\" + "config.xml");

            #endregion
        }
        #endregion
    }
    /// <summary>
    /// Basetypes for tiles, used by MapGeneration
    /// </summary>
    enum TileBaseType
    {
        None,
        Mountain,
        Wood,
        Water,
        Enemy
    }
}

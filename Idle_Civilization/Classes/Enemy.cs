using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace Idle_Civilization.Classes
{
    class Enemy
    {
        public int controlled_wood = 0, controlled_grass = 0, controlled_mountain = 0;
        public Ressources ressources = new Ressources(0.0,0.0,0.0,0.0);

        List<Ressources> buildCosts;

        //Delay for Enemyupdate
        double timer = 100; //in ms
        const double TIME = 100;
        AI_Preference aI_Preference;

        #region AI-VARS
        private int cycles_Without_Action = 0; //
        private int change_strat_after_empty_loops = 21;
        #endregion

        public Enemy(AI_Preference _aI_Preference, List<Ressources> _buildCosts)
        {
            aI_Preference = _aI_Preference;
            buildCosts = _buildCosts;
        }

        public void Update(GameTime gameTime, List<List<Tile>> map)
        {
            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            timer -= elapsed;

            if (timer < 0)
            {
                timer = TIME;

                ressources.wood += ((controlled_wood * Globals.tileControllFactor_wood) * Globals.enemy_ressource_factor) + Globals.enemy_base_production_wood;
                ressources.ore += ((controlled_mountain * Globals.tileControllFactor_ore) * Globals.enemy_ressource_factor) + Globals.enemy_base_production_ore;
                ressources.food += ((controlled_grass * Globals.tileControllFactor_food) * Globals.enemy_ressource_factor) + Globals.enemy_base_production_food;


                switch (aI_Preference)
                {
                    case AI_Preference.conquering: //try to get as many tiles under controll as possible
                        #region conquer
                        if (CanAfford(buildCosts[(int)Buildcosts.addTile])) //check if ressources are enough to conquer a new tile
                        {
                            cycles_Without_Action = 0;
                            ConquerBestTile(map);
                        }
                        else
                        {
                            cycles_Without_Action++;

                            if (cycles_Without_Action >= change_strat_after_empty_loops)
                            {
                                if (CanAfford(buildCosts[(int)Buildcosts.enemyAddArmy])) //Add Army if possible
                                {
                                    ressources.SubRessources(buildCosts[(int)Buildcosts.enemyAddArmy]);
                                    ressources.army++;
                                }

                                cycles_Without_Action = 0;
                            }
                        }

                        if (ressources.army > 0) //check if army is in ressources
                        {
                            bool break_value = false;
                            foreach (List<Tile> row in map) //check for tile without 50 army
                            {
                                foreach (Tile tile in row)
                                {
                                    if (tile.hasEnemy && tile.armystrength < 50)
                                    {
                                        do //add army to tile and sub from ressources
                                        {
                                            tile.armystrength++;
                                            ressources.army--;
                                        } while (tile.armystrength < 50 && ressources.army > 0);
                                    }

                                    if (ressources.army <= 0)
                                    {
                                        break_value = true;
                                        break;
                                    }
                                }
                                if (break_value)
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case AI_Preference.stronghold:
                        #region stronghold
                        if (ressources.army > 0) //check if army is in ressources
                        {
                            bool break_value = false;
                            foreach (List<Tile> row in map) //check for tile without 50 army
                            {
                                foreach (Tile tile in row)
                                {
                                    if (tile.hasEnemy && tile.armystrength < 50)
                                    {
                                        do //add army to tile and sub from ressources
                                        {
                                            tile.armystrength++;
                                            ressources.army--;
                                        } while (tile.armystrength < 50 && ressources.army > 0);
                                    }

                                    if (ressources.army <= 0)
                                    {
                                        break_value = true;
                                        break;
                                    }
                                }
                                if (break_value)
                                    break;
                            }
                        }

                        //produce as much army as possible
                        while (CanAfford(buildCosts[(int)Buildcosts.enemyAddArmy]) && ressources.army < 10)
                        {
                            ressources.army++;
                            ressources.SubRessources(buildCosts[(int)Buildcosts.enemyAddArmy]);
                        }

                        if (CanAfford(buildCosts[(int)Buildcosts.addTile])) //check if ressources are enough to conquer a new tile
                        {
                            ConquerBestTile(map);
                        }
                        #endregion
                        break;
                    case AI_Preference.evenly:
                        break;
                    default: break;
                }

            }
        }

        public void AddTileToControll(Tile tile)
        {
            switch(tile.tileBaseType)
            {
                case TileBaseType.None:
                    controlled_grass++;
                    break;
                case TileBaseType.Mountain:
                    controlled_mountain++;
                    break;
                case TileBaseType.Wood:
                    controlled_wood++;
                    break;
                default: break;
            }
        }
        public void SubTileFromControll(Tile tile)
        {
            switch (tile.tileBaseType)
            {
                case TileBaseType.None:
                    controlled_grass--;
                    break;
                case TileBaseType.Mountain:
                    controlled_mountain--;
                    break;
                case TileBaseType.Wood:
                    controlled_wood--;
                    break;
                default: break;
            }
        }

        private bool CanAfford(Ressources price)
        {
            if (ressources.food >= price.food &&
               ressources.wood >= price.wood &&
               ressources.ore >= price.ore &&
               ressources.army >= price.army)
                return true;
            else
                return false;
        }
        private void ConquerBestTile(List<List<Tile>> map)
        {
            //check which ressource is lowest
            TileBaseType wanted_tile = new TileBaseType();
            bool break_value = false;
            bool tileAdded = false;

            if (ressources.wood < ressources.food && ressources.wood < ressources.ore)
                wanted_tile = TileBaseType.Wood;
            else if (ressources.food < ressources.wood && ressources.food < ressources.ore)
                wanted_tile = TileBaseType.None;
            else if (ressources.ore < ressources.wood && ressources.ore < ressources.food)
                wanted_tile = TileBaseType.Mountain;
            else
                wanted_tile = TileBaseType.None;

            foreach(List<Tile> row in map)
            {
                foreach(Tile tile in row)
                {
                    if(!tile.hasEnemy && tile.aNeighborisEnemy && tile.tileBaseType == wanted_tile && !tile.empty)
                    {
                        if(tile.isCitypart)
                        {
                            if (ressources.army >= tile.armystrength)
                            {
                                ressources.army -= tile.armystrength;
                                tile.SetAsEnemyTile();
                                AddTileToControll(tile);
                                break_value = true;
                                tileAdded = true;
                                break;
                            }
                        }
                        else
                        {
                            ressources.SubRessources(buildCosts[(int)Buildcosts.addTile]);
                            tile.SetAsEnemyTile();
                            AddTileToControll(tile);
                            break_value = true;
                            tileAdded = true;
                            break;
                        }
                    }
                }

                if (break_value)
                    break;
            }
            //Add any tile which is not gras if no tile with right condition is found
            if(!tileAdded)
            {
                foreach (List<Tile> row in map)
                {
                    foreach (Tile tile in row)
                    {
                        if (!tile.hasEnemy && tile.aNeighborisEnemy && !tile.empty && tile.tileBaseType != TileBaseType.None)
                        {
                            if (tile.isCitypart)
                            {
                                if (ressources.army >= tile.armystrength)
                                {
                                    ressources.army -= tile.armystrength;
                                    tile.SetAsEnemyTile();
                                    AddTileToControll(tile);
                                    break_value = true;
                                    tileAdded = true;
                                    break;
                                }
                            }
                            else
                            {
                                ressources.SubRessources(buildCosts[(int)Buildcosts.addTile]);
                                tile.SetAsEnemyTile();
                                AddTileToControll(tile);
                                break_value = true;
                                tileAdded = true;
                                break;
                            }
                        }
                    }

                    if (break_value)
                        break;
                }
            }

            //Add any tile if no tile with right condition is found
            if (!tileAdded)
            {
                foreach (List<Tile> row in map)
                {
                    foreach (Tile tile in row)
                    {
                        if (!tile.hasEnemy && tile.aNeighborisEnemy && !tile.empty)
                        {
                            if (tile.isCitypart)
                            {
                                if (ressources.army >= tile.armystrength)
                                {
                                    ressources.army -= tile.armystrength;
                                    tile.SetAsEnemyTile();
                                    AddTileToControll(tile);
                                    break_value = true;
                                    tileAdded = true;
                                    break;
                                }
                            }
                            else
                            {
                                ressources.SubRessources(buildCosts[(int)Buildcosts.addTile]);
                                tile.SetAsEnemyTile();
                                AddTileToControll(tile);
                                break_value = true;
                                tileAdded = true;
                                break;
                            }
                        }
                    }

                    if (break_value)
                        break;
                }
            }


        }
    }
    /// <summary>
    /// Sets how the AI is playing
    /// </summary>
    public enum AI_Preference
    {
        stronghold, //fortify tiles to the max before expanding
        conquering, //expanding as fast as possible
        evenly      //expand and forfeit evenly
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Idle_Civilization.Classes
{
    public class Constants
    {
        public const int tile_width = 32, tile_height = 48;
        public const int tile_x_offset = 36, tile_y_offset = 30;
        public const int tiles_per_row = 8;
        public const int tile_y_space = 24;
        public const int tile_x_click_space = 16;

       // public const int tile_stretch_factor = 3;

        public const int medium_button_diameter = 24;
        public const int small_button_diameter = 16;
    }

    public static class Globals
    {
        public static GraphicsDevice GraphicsDevice;

        public static Texture2D primitive;

        public static Texture2D tileMap;
        public static Texture2D buttons_medium;
        public static Texture2D buttons_small;
        public static List<Texture2D> playerBorders;
        public static List<Texture2D> enemyBorders;

        public static int tile_stretch_factor = 3;

        //player parameters
        public static int player_starting_food = 10000;
        public static int player_starting_wood = 10000;
        public static int player_starting_ore = 10000;
        public static int player_starting_army = 10000;

        //enemy parameters
        public static double enemy_ressource_factor = 2.0;
        public static AI_Preference AI_Preference = AI_Preference.evenly;
        public static double enemy_base_production_food = 5.0;
        public static double enemy_base_production_wood = 5.0;
        public static double enemy_base_production_ore = 5.0;

        //Demand-Calculation
        public static double baseproduction_wood = 0.3;
        public static double baseproduction_ore = 0.3;
        public static double baseproduction_food = 1.2;
        public static double baseFoodconsumption_poeple = 1.0;
        public static double baseFoodconsumption_wood = 0.1;
        public static double baseFoodconsumption_ore = 0.2;
        public static double tileControllFactor_wood = 1.0;
        public static double tileControllFactor_food = 1.0;
        public static double tileControllFactor_ore = 1.0;

        //Mapgeneration
        public static int map_width = 30; //tiles in x
        public static int map_height = 45; //tiles in y
            
        public static int mountain_density = 10;
        public static int mountain_spread = 20;
        public static int wood_density = 15;
        public static int wood_spread = 20;
        public static int water_density = 10;
        public static int water_spread = 0;
        public static int enemy_density = 5;
        public static int enemy_spread = 0;

        //Builddurations
        public static double buildCityDuration = 5.0;
        public static double addTileDuration = 3.0;
        public static double conquerTileDuration = 5.0;
    }


    public struct Ressources
    {
        public Ressources(double _wood, double _ore, double _food, double _army)
        {
            wood = _wood;
            ore = _ore;
            food = _food;
            army = _army;
        }
        /// <summary>
        /// wood;food;ore
        /// </summary>
        /// <param name="str"></param>
        public Ressources(string str)
        {
            string[] splits = str.Split(';');

            wood = Convert.ToDouble(splits[0]);
            food = Convert.ToDouble(splits[1]);
            ore = Convert.ToDouble(splits[2]);
            army = Convert.ToDouble(splits[3]);
        }

        public void AddRessources(Ressources adder)
        {
            wood += adder.wood;
            food += adder.food;
            ore += adder.ore;
            army += adder.army;
        }
        public void SubRessources(Ressources subtractor)
        {
            wood -= subtractor.wood;
            food -= subtractor.food;
            ore -= subtractor.ore;
            army -= subtractor.army;
        }

        public string AsString()
        {
            return wood.ToString() + ";" + food.ToString() + ";" + ore.ToString() + ";" + army.ToString();
        }

        public double wood;
        public double ore;
        public double food;
        public double army;
    }
    public enum WorkerAdding
    {
        AddFood,
        RemoveFood,
        AddWood,
        RemoveWood,
        AddOre,
        RemoveOre
    }
    public enum GlobalUpgrade
    {
        something
    }
    public enum RessourceType
    {
        Wood,
        Food,
        Ore
    }
    public enum Buildcosts
    {
        CreateCity,
        addTile,
        Worker,
        enemyAddArmy
    }

    /// <summary>
    /// 8 in a row; 6 rows
    /// for Spritesheet
    /// </summary>
    public enum TileNumber
    { 
        gras,
        sometrees,
        wood,
        somerocks,
        somerocksandwood,
        mountain,
        flatwater,
        deepwater,
        town,
        townwithwall,
        townwithstrongwall,
        cropfield,
        swampwood,
        swamp,
        oldgras,
        downknow,
        snow,
        snowsometrees,
        snowwood,
        snowsomerocks,
        snowsomerocksandwood,
        flatwaterwithice,
        snowtown,
        snowcastle,
        dessert,
        dessertsomerocks,
        desserthighdunes,
        dessermountain,
        dessertoasis,
        dessertsmalltown,
        desserttown,
        desserttownwithwall,
        wood2,
        grascave,
        snowcave,
        dessertcave,
        portleft,
        portright,
        lighthouse,
        oldcastle,
        oldvillage       
    }
    /// <summary>
    /// Row of Button on Spritesheet
    /// </summary>
    public enum MediumButtonNumber
    {
        clear,
        people,
        wood,
        food,
        ore,
        army,
        foundCity,
        addTile,
        attackTile,
        upgradeCity
    }
    /// <summary>
    /// Row of Button on Spritesheet
    /// </summary>
    public enum SmallButtonNumber
    {
        minus,
        plus
    }
    /// <summary>
    /// Colum of Border in Spritesheet
    /// </summary>
    public enum BorderNumber
    {
        upperLeft,
        top,
        upperRight,
        lowerRight,
        bottom,
        lowerLeft
    }
    /// <summary>
    /// Stuff a tile can be adjacent to
    /// </summary>
    public enum TileControllType
    {
        cityTile,
        enemyTile,       
    }
    /// <summary>
    /// the duration of an action on a tile is expired, session fullfills an coresponding order
    /// </summary>
    public enum TileOrder
    {
        none,
        buildCity,
        addTile,
        conquerTile

    }
}

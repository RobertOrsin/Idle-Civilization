﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idle_Civilization.Classes
{
    public class Constants
    {
        public const int tile_width = 32, tile_height = 48;
        public const int tile_x_offset = 36, tile_y_offset = 30;
        public const int tiles_per_row = 8;
        public const int tile_y_space = 24;
        public const int tile_x_click_space = 16;

        public const int tile_stretch_factor = 3;

        public const int medium_button_diameter = 24;
        public const int small_button_diameter = 16;
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
        AddCityTile,
        Worker,
        addTile
    }

    /// <summary>
    /// 8 in a row; 6 rows
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
}

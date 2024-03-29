﻿using System;
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
    class HUD
    {
        #region VARS
        public static int hud_height = 50;
        public static int hud_width = 0; //defined in contructor

        private Pushbutton label_food, label_wood, label_ore, label_army;
        private TextBox player_ressources_food, player_ressources_wood, player_ressources_ore, player_ressources_army;
        private TextBox player_demand_food, player_demand_wood, player_demand_ore, player_demand_army;

        private TextBox enemy_food, enemy_wood, enemy_ore, enemy_army;

        private TextBox helpstring;

        private TextBox pauseString;

        #endregion

        public HUD(int screen_width, int screen_height)
        {
            hud_width = screen_width;

            player_ressources_food = new TextBox(new Vector2(0, 0), new Point(75, 15), 1, false, true, null, new Color(255, 255, 255));
            player_ressources_wood = new TextBox(new Vector2(0, 0), new Point(75, 15), 1, false, true, null, new Color(255, 255, 255));
            player_ressources_ore = new TextBox(new Vector2(0, 0), new Point(75, 15), 1, false, true, null, new Color(255, 255, 255));
            player_ressources_army = new TextBox(new Vector2(0, 0), new Point(75, 15), 1, false, true, null, new Color(255, 255, 255));

            player_demand_food = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));
            player_demand_wood = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));
            player_demand_ore = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));
            player_demand_army = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));

            enemy_food = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));
            enemy_wood = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));
            enemy_ore = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));
            enemy_army = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 255, 255));

            player_demand_food.textArray[0] = "0";
            player_demand_wood.textArray[0] = "0";
            player_demand_ore.textArray[0] = "0";
            player_demand_army.textArray[0] = "0";

            helpstring = new TextBox(new Vector2(0, 0), new Point(100, 15), 1, false, true, null, new Color(255, 255, 255));
            helpstring.textArray[0] = "I - load GameValues from File";
            helpstring.textArray.Add("R - reload Game");
            helpstring.textArray.Add("plus - zoom in");
            helpstring.textArray.Add("minus - zoom out");
            helpstring.textArray.Add("P - pause");


            pauseString = new TextBox(new Vector2(0, 0), new Point(100, 15), 1, false, true, null, new Color(255, 0, 0));
            pauseString.textArray[0] = "Pause active";
            pauseString.visible = true;

            SetPositions();
        }

        public void Update(MouseState mouseState, GameTime gameTime, Player player, Enemy enemy)
        {
            #region write player-values to textboxes
            player_ressources_food.textArray[0] = player.ressources.food.ToString();
            player_ressources_wood.textArray[0] = player.ressources.wood.ToString();
            player_ressources_ore.textArray[0] = player.ressources.ore.ToString();
            player_ressources_army.textArray[0] = player.ressources.army.ToString();

            if (player.ressource_demand.food != 0 || player.ressource_demand.wood != 0 || player.ressource_demand.ore != 0 || player.ressource_demand.army != 0)
            {
                player_demand_food.textArray[0] = player.ressource_demand.food.ToString();
                player_demand_wood.textArray[0] = player.ressource_demand.wood.ToString();
                player_demand_ore.textArray[0] = player.ressource_demand.ore.ToString();
                player_demand_army.textArray[0] = player.ressource_demand.army.ToString();
            }
            #endregion

            #region write enemy-values to textboxes
            enemy_army.textArray[0] = enemy.ressources.army.ToString();
            enemy_food.textArray[0] = enemy.ressources.food.ToString();
            enemy_wood.textArray[0] = enemy.ressources.wood.ToString();
            enemy_ore.textArray[0] = enemy.ressources.ore.ToString();
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            player_ressources_food.Draw(spriteBatch, spriteFont);
            player_ressources_wood.Draw(spriteBatch, spriteFont);
            player_ressources_ore.Draw(spriteBatch, spriteFont);
            player_ressources_army.Draw(spriteBatch, spriteFont);

            player_demand_food.Draw(spriteBatch, spriteFont);
            player_demand_wood.Draw(spriteBatch, spriteFont);
            player_demand_ore.Draw(spriteBatch, spriteFont);
            player_demand_army.Draw(spriteBatch, spriteFont);

            enemy_ore.Draw(spriteBatch, spriteFont);
            enemy_food.Draw(spriteBatch, spriteFont);
            enemy_wood.Draw(spriteBatch, spriteFont);
            enemy_army.Draw(spriteBatch, spriteFont);

            helpstring.Draw(spriteBatch, spriteFont);

            pauseString.Draw(spriteBatch, spriteFont);
        }
        /// <summary>
        /// Set positions for all elements on HUD
        /// </summary>
        private void SetPositions()
        {
            int offset = hud_width / 5;

            player_ressources_food.position = new Vector2(0, 0);
            player_demand_food.position = new Vector2(player_ressources_food.dimension.X + 5, 0);

            player_ressources_wood.position = new Vector2(offset * 1 + 5, 0);
            player_demand_wood.position = new Vector2(offset * 1 + player_ressources_wood.dimension.X + 5, 0);

            player_ressources_ore.position = new Vector2(offset * 2 + 5, 0);
            player_demand_ore.position = new Vector2(offset * 2 + player_ressources_ore.dimension.X + 5, 0);

            player_ressources_army.position = new Vector2(offset * 3 + 5, 0);
            player_demand_army.position = new Vector2(offset * 3 + player_ressources_army.dimension.X + 5, 0);

            enemy_food.position = new Vector2(0, 30);
            enemy_wood.position = new Vector2(offset * 1 + 5, 30);
            enemy_ore.position = new Vector2(offset * 2 + 5, 30);
            enemy_army.position = new Vector2(offset * 3 + 5, 30);

            helpstring.position = new Vector2(offset * 4 + 5, 0);

            pauseString.position = new Vector2(hud_width / 2 - pauseString.dimension.X / 2, hud_height / 2);
        }

        public void Pause()
        {
            pauseString.visible = !pauseString.visible;
        }


    }
}

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
    class HUD
    {
        #region VARS
        public static int hud_height = 50;
        public static int hud_width = 0; //defined in contructor

        private Pushbutton label_food, label_wood, label_ore, label_army;
        private TextBox player_ressources_food, player_ressources_wood, player_ressources_ore, player_ressources_army;
        private TextBox player_demand_food, player_demand_wood, player_demand_ore, player_demand_army;

        #endregion

        public HUD(GraphicsDevice GraphicsDevice,int screen_width, int screen_height)
        {
            hud_width = screen_width;

            player_ressources_food = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));
            player_ressources_wood = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));
            player_ressources_ore = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));
            player_ressources_army = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));

            player_demand_food = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));
            player_demand_wood = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));
            player_demand_ore = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));
            player_demand_army = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(0, 0, 0));

            SetPositions();
        }

        public void Update(MouseState mouseState, GameTime gameTime, Player player)
        {
            #region write player-values to textboxes
            player_ressources_food.textArray[0] = player.ressources.food.ToString();
            player_ressources_wood.textArray[0] = player.ressources.wood.ToString();
            player_ressources_ore.textArray[0] = player.ressources.ore.ToString();
            player_ressources_army.textArray[0] = player.ressources.army.ToString();

            player_demand_food.textArray[0] = player.ressource_demand.food.ToString();
            player_demand_wood.textArray[0] = player.ressource_demand.wood.ToString();
            player_demand_ore.textArray[0] = player.ressource_demand.ore.ToString();
            player_demand_army.textArray[0] = player.ressource_demand.army.ToString();
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
        }

        private void SetPositions()
        {

        }


    }
}

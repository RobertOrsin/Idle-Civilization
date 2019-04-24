using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class TileMenu
    {
        #region MenuItems
        Pushbutton addPeople;
        TextBox Population;

        Pushbutton found_add_attack;

        Pushbutton subOreWorker, addOreWorker;
        Icon oreIcon;
        TextBox oreworkers;

        Pushbutton subWoodWorker, addWoodWorker;
        Icon woodIcon;
        TextBox woodWorkers;

        Pushbutton subFoodWorker, addFoodWorker;
        Icon FoodIcon;
        TextBox foodWorker;

        Pushbutton subArmyWorker, addArmyWorker;
        Icon armyIcon;
        TextBox armyWorker;
        #endregion

        public TileMenu()
        {

        }

        public void Update(KeyboardState currentKeyboardState, MouseState mouseState)
        {

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {

        }
    }
}

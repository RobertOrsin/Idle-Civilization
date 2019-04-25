using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class TileMenu
    {
        Texture2D mediumButtonSheet, smallButtonSheet;

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

        public TileMenu(GraphicsDevice GraphicsDevice, Texture2D _mediumButtonSheet, Texture2D _smallButtonSheet)
        {
            mediumButtonSheet = _mediumButtonSheet;
            smallButtonSheet = _smallButtonSheet;
        }

        public void Update(KeyboardState currentKeyboardState, MouseState mouseState)
        {

        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {

        }


        private Texture2D GetTexture(GraphicsDevice GraphicsDevice, MediumButtonNumber position, ButtonStateType buttonStateType)
        {
            Texture2D return_tex;
            Color[] data;
            Rectangle rect = GetTileSpritePosition(position, buttonStateType);

            return_tex = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            mediumButtonSheet.GetData(0, rect, data, 0, data.Length);
            return_tex.SetData(data);


            return return_tex;
        }

        private Texture2D GetTexture(GraphicsDevice GraphicsDevice, SmallButtonNumber position, ButtonStateType buttonStateType)
        {
            Texture2D return_tex;
            Color[] data;
            Rectangle rect = GetTileSpritePosition(position, buttonStateType);

            return_tex = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            smallButtonSheet.GetData(0, rect, data, 0, data.Length);
            return_tex.SetData(data);


            return return_tex;
        }
        /// <summary>
        ///  Returns a Rectangle descriping position and dimension of sprite on a spritemap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buttonStateType"></param>
        /// <returns></returns>
        private Rectangle GetTileSpritePosition(MediumButtonNumber position, ButtonStateType buttonStateType)
        {
            return new Rectangle(((int)buttonStateType) * Constants.medium_button_diameter, ((int)position) * Constants.medium_button_diameter, Constants.medium_button_diameter, Constants.medium_button_diameter);
        }
        /// <summary>
        ///  Returns a Rectangle descriping position and dimension of sprite on a spritemap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="buttonStateType"></param>
        /// <returns></returns>
        private Rectangle GetTileSpritePosition(SmallButtonNumber position, ButtonStateType buttonStateType)
        {
            return new Rectangle(((int)buttonStateType) * Constants.small_button_diameter, ((int)position) * Constants.small_button_diameter, Constants.small_button_diameter, Constants.small_button_diameter);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Utility_Functions;

namespace Idle_Civilization.Classes
{
    class TileMenu
    {
        Texture2D mediumButtonSheet, smallButtonSheet;

        public bool visible = false;
        Vector2 centerPosition = new Vector2(5,5);

        public Rectangle tileArea;

        #region MenuItems
        Pushbutton addPeople;
        TextBox Population;

        Pushbutton foundCity, addTile, attackTile;

        Pushbutton subOreWorker, addOreWorker;
        Pushbutton oreIcon;
        TextBox oreworkers;

        Pushbutton subWoodWorker, addWoodWorker;
        Pushbutton woodIcon;
        TextBox woodWorkers;

        Pushbutton subFoodWorker, addFoodWorker;
        Pushbutton FoodIcon;
        TextBox foodWorker;

        Pushbutton subArmyWorker, addArmyWorker;
        Pushbutton armyIcon;
        TextBox armyWorker;
        #endregion

        Dictionary<MenuElement, Vector2> menuElementOffsets;

        public TileMenu(GraphicsDevice GraphicsDevice, Texture2D _mediumButtonSheet, Texture2D _smallButtonSheet)
        {
            mediumButtonSheet = _mediumButtonSheet;
            smallButtonSheet = _smallButtonSheet;

            InitMenuItemOffsets();


            #region Init Elements
            List<string> emptyStringList = new List<string>();
            emptyStringList.Add("0000");

            List<Texture2D> minusButton, plusButton;
            minusButton = new List<Texture2D>();
            minusButton.Add(GetTexture(GraphicsDevice, SmallButtonNumber.minus, ButtonStateType.idle));
            minusButton.Add(GetTexture(GraphicsDevice, SmallButtonNumber.minus, ButtonStateType.hoover));
            minusButton.Add(GetTexture(GraphicsDevice, SmallButtonNumber.minus, ButtonStateType.pressed));
            plusButton = new List<Texture2D>();
            plusButton.Add(GetTexture(GraphicsDevice, SmallButtonNumber.plus, ButtonStateType.idle));
            plusButton.Add(GetTexture(GraphicsDevice, SmallButtonNumber.plus, ButtonStateType.hoover));
            plusButton.Add(GetTexture(GraphicsDevice, SmallButtonNumber.plus, ButtonStateType.pressed));

            addPeople = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.people, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.people, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.people, ButtonStateType.pressed),"",Color.AliceBlue);
            Population = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);

            oreIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.pressed), "", Color.AliceBlue);
            oreIcon.locked = true;
            oreworkers = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addOreWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subOreWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            woodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.pressed), "", Color.AliceBlue);
            woodIcon.locked = true;
            woodWorkers = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addWoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subWoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            FoodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.pressed), "", Color.AliceBlue);
            FoodIcon.locked = true;
            foodWorker = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addFoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subFoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            armyIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.pressed), "", Color.AliceBlue);
            armyIcon.locked = true;
            armyWorker = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addArmyWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subArmyWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            foundCity = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.foundCity, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.foundCity, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.foundCity, ButtonStateType.pressed), "", Color.AliceBlue);

            addTile = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.addTile, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.addTile, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.addTile, ButtonStateType.pressed), "", Color.AliceBlue);

            attackTile = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.attackTile, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.attackTile, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.attackTile, ButtonStateType.pressed), "", Color.AliceBlue);
            #endregion
        }

        public void Update(KeyboardState currentKeyboardState, MouseState mouseState)
        {
            #region update elements
            foundCity.update(mouseState);
            addTile.update(mouseState);
            attackTile.update(mouseState);

            subOreWorker.update(mouseState);
            addOreWorker.update(mouseState);
            oreIcon.update(mouseState);
            oreworkers.update(currentKeyboardState, mouseState);

            subWoodWorker.update(mouseState);
            addWoodWorker.update(mouseState);
            woodIcon.update(mouseState);
            woodWorkers.update(currentKeyboardState, mouseState);

            subFoodWorker.update(mouseState);
            addFoodWorker.update(mouseState);
            FoodIcon.update(mouseState);
            foodWorker.update(currentKeyboardState, mouseState);

            subArmyWorker.update(mouseState);
            addArmyWorker.update(mouseState);
            armyIcon.update(mouseState);
            armyWorker.update(currentKeyboardState, mouseState);
            #endregion
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            #region draw elements
            if (visible)
            {
                SetPositions();

                addPeople.draw(spriteBatch, spriteFont);
                Population.draw(spriteBatch, spriteFont);

                foundCity.draw(spriteBatch, spriteFont);
                addTile.draw(spriteBatch, spriteFont);
                attackTile.draw(spriteBatch, spriteFont);

                subOreWorker.draw(spriteBatch, spriteFont);
                addOreWorker.draw(spriteBatch, spriteFont);
                oreIcon.draw(spriteBatch, spriteFont);
                oreworkers.draw(spriteBatch, spriteFont);

                subWoodWorker.draw(spriteBatch, spriteFont);
                addWoodWorker.draw(spriteBatch, spriteFont);
                woodIcon.draw(spriteBatch, spriteFont);
                woodWorkers.draw(spriteBatch, spriteFont);

                subFoodWorker.draw(spriteBatch, spriteFont);
                addFoodWorker.draw(spriteBatch, spriteFont);
                FoodIcon.draw(spriteBatch, spriteFont);
                foodWorker.draw(spriteBatch, spriteFont);

                subArmyWorker.draw(spriteBatch, spriteFont);
                addArmyWorker.draw(spriteBatch, spriteFont);
                armyIcon.draw(spriteBatch, spriteFont);
                armyWorker.draw(spriteBatch, spriteFont);
            }
            #endregion
        }

        /// <summary>
        /// Get Texture of medium button for a buttonstate
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="position"></param>
        /// <param name="buttonStateType"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get Texture of small button for a buttonstate
        /// </summary>
        /// <param name="GraphicsDevice"></param>
        /// <param name="position"></param>
        /// <param name="buttonStateType"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Set Screenposition for MenuElements depending on Centerpoint of selected tile
        /// </summary>
        private void SetPositions()
        {
            Vector2 centerpoint = new Vector2(tileArea.X + (Constants.tile_width * Constants.tile_stretch_factor / 2),
                                             tileArea.Y + ((Constants.tile_height - Constants.tile_y_offset)*Constants.tile_stretch_factor) + (Constants.tile_y_space * Constants.tile_stretch_factor / 2));

            #region set position of elements
            addPeople.position = centerpoint + menuElementOffsets[MenuElement.TopButton];
            Population.position = centerpoint + menuElementOffsets[MenuElement.TopTextBox];

            foundCity.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];
            addTile.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];
            attackTile.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];

            subOreWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerRightMinus];
            addOreWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerRightPlus];
            oreIcon.position = centerpoint + menuElementOffsets[MenuElement.LowerRightButton];
            oreworkers.position = centerpoint + menuElementOffsets[MenuElement.LowerRightTextBox];

            subWoodWorker.position = centerpoint + menuElementOffsets[MenuElement.BottomMinus];
            addWoodWorker.position = centerpoint + menuElementOffsets[MenuElement.BottomPlus];
            woodIcon.position = centerpoint + menuElementOffsets[MenuElement.BottomButton];
            woodWorkers.position = centerpoint + menuElementOffsets[MenuElement.BottomTextBox];

            subFoodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftMinus];
            addFoodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftPlus];
            FoodIcon.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftButton];
            foodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftTextBox];

            subArmyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftMinus];
            addArmyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftPlus];
            armyIcon.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftButton];
            armyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftTextBox];
            #endregion
        }
        /// <summary>
        /// Initialze Offsetvalues of all Menu-Elements
        /// See https://github.com/RobertOrsin/GameButtons.git 
        /// </summary>
        private void InitMenuItemOffsets()
        {
            menuElementOffsets = new Dictionary<MenuElement, Vector2>();

            menuElementOffsets.Add(MenuElement.TopButton, new Vector2(-11, -86));
            menuElementOffsets.Add(MenuElement.TopTextBox, new Vector2(-25, -58));

            menuElementOffsets.Add(MenuElement.UpperRightButton, new Vector2(41, -48));

            menuElementOffsets.Add(MenuElement.LowerRightMinus, new Vector2(37, 29));
            menuElementOffsets.Add(MenuElement.LowerRightPlus, new Vector2(67, 29));
            menuElementOffsets.Add(MenuElement.LowerRightButton, new Vector2(48, 42));
            menuElementOffsets.Add(MenuElement.LowerRightTextBox, new Vector2(35, 69));

            menuElementOffsets.Add(MenuElement.BottomMinus, new Vector2(-22, 49));
            menuElementOffsets.Add(MenuElement.BottomPlus, new Vector2(8, 49));
            menuElementOffsets.Add(MenuElement.BottomButton, new Vector2(-11, 62));
            menuElementOffsets.Add(MenuElement.BottomTextBox, new Vector2(-24, 89));

            menuElementOffsets.Add(MenuElement.LowerLeftMinus, new Vector2(-82, 29));
            menuElementOffsets.Add(MenuElement.LowerLeftPlus, new Vector2(-52, 29));
            menuElementOffsets.Add(MenuElement.LowerLeftButton, new Vector2(-71, 42));
            menuElementOffsets.Add(MenuElement.LowerLeftTextBox, new Vector2(-86, 69));

            menuElementOffsets.Add(MenuElement.UpperLeftMinus, new Vector2(-92, -64));
            menuElementOffsets.Add(MenuElement.UpperLeftPlus, new Vector2(-62, -64));
            menuElementOffsets.Add(MenuElement.UpperLeftButton, new Vector2(-81, -51));
            menuElementOffsets.Add(MenuElement.UpperLeftTextBox, new Vector2(-94, -24));
        }
        private enum MenuElement
        {
            TopButton,
            TopTextBox,
            UpperRightButton,
            LowerRightMinus,
            LowerRightPlus,
            LowerRightButton,
            LowerRightTextBox,
            BottomMinus,
            BottomPlus,
            BottomButton,
            BottomTextBox,
            LowerLeftMinus,
            LowerLeftPlus,
            LowerLeftButton,
            LowerLeftTextBox,
            UpperLeftMinus,
            UpperLeftPlus,
            UpperLeftButton,
            UpperLeftTextBox
        }

    }
}

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

        bool visible = true;
        Vector2 centerPosition = new Vector2(5,5);

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

        public TileMenu(GraphicsDevice GraphicsDevice, Texture2D _mediumButtonSheet, Texture2D _smallButtonSheet)
        {
            mediumButtonSheet = _mediumButtonSheet;
            smallButtonSheet = _smallButtonSheet;

            List<string> emptyStringList = new List<string>();
            emptyStringList.Add("");

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
            Population = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 10), 1, false, emptyStringList);

            oreIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.pressed), "", Color.AliceBlue);
            oreIcon.locked = true;
            oreworkers = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 10), 1, false, emptyStringList);
            addOreWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subOreWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            woodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.pressed), "", Color.AliceBlue);
            woodIcon.locked = true;
            woodWorkers = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 10), 1, false, emptyStringList);
            addWoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subWoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            FoodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.pressed), "", Color.AliceBlue);
            FoodIcon.locked = true;
            foodWorker = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 10), 1, false, emptyStringList);
            addFoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue);
            subFoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue);

            armyIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.pressed), "", Color.AliceBlue);
            armyIcon.locked = true;
            armyWorker = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 10), 1, false, emptyStringList);
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
        }

        public void Update(KeyboardState currentKeyboardState, MouseState mouseState)
        {
            SetPositions();

            //Set Data

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

        private void SetPositions()
        {
            Vector2 centerpoint = new Vector2();

            #region calculate centerpoint
            if ((int)centerPosition.X % 2 == 0)
            {
                centerpoint.X = (((int)centerPosition.X) / 2) * (Constants.tile_x_offset + Constants.tile_x_offset / 3) * Constants.tile_stretch_factor;
            }
            else
            {
                centerpoint.X = (Constants.tile_x_offset / 3 * 2) + (( (int)centerPosition.X - 1) / 2) * (Constants.tile_x_offset + Constants.tile_x_offset / 3) * Constants.tile_stretch_factor;
            }

            if ((int)centerPosition.Y % 2 == 0)
            {
                centerpoint.Y = ((int)centerPosition.Y) / 2 * Constants.tile_y_offset * Constants.tile_stretch_factor;
            }
            else
            {
                centerpoint.Y = (Constants.tile_y_offset / 2) + (((int)centerPosition.Y - 1) / 2) * Constants.tile_y_offset * Constants.tile_stretch_factor;
            }
            #endregion


            #region set position of elements
            addPeople.position = centerpoint;
            #endregion


        }

    }
}

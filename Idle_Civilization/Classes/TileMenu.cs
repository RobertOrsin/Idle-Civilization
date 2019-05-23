using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Utility_Functions;
using Microsoft.CSharp.RuntimeBinder;

namespace Idle_Civilization.Classes
{
    class TileMenu
    {
        Texture2D mediumButtonSheet, smallButtonSheet;
        public bool visible = false;
        Vector2 centerPosition = new Vector2(5,5);
        //public Rectangle tileArea;
        public Tile selectedTile;
        public TileMenuUpdateData tileMenuUpdateData = new TileMenuUpdateData(false);

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

        Dictionary<MenuElement, Vector2> menuElementOffsets;
        #endregion

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
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.people, ButtonStateType.pressed),"",Color.AliceBlue, TileMenuFunction.addPeople);
            addPeople.onClick += TileMenuClick;
            Population = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);

            oreIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.ore, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            oreIcon.locked = true;
            oreworkers = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addOreWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addOre);
            addOreWorker.onClick += TileMenuClick;
            subOreWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subOre);
            subOreWorker.onClick += TileMenuClick;

            woodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.wood, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            woodIcon.locked = true;
            woodWorkers = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addWoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addWood);
            addWoodWorker.onClick += TileMenuClick;
            subWoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subWood);
            subWoodWorker.onClick += TileMenuClick;

            FoodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.food, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            FoodIcon.locked = true;
            foodWorker = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addFoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addFood);
            addFoodWorker.onClick += TileMenuClick;
            subFoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subFood);
            subFoodWorker.onClick += TileMenuClick;

            armyIcon = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.army, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            armyIcon.locked = true;
            armyWorker = new TextBox(GraphicsDevice, new Vector2(0, 0), new Point(50, 15), 1, false, emptyStringList);
            addArmyWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addArmy);
            addArmyWorker.onClick += TileMenuClick;
            subArmyWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subArmy);
            subArmyWorker.onClick += TileMenuClick;

            foundCity = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.foundCity, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.foundCity, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.foundCity, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.foundCity);
            foundCity.onClick += TileMenuClick;

            addTile = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.addTile, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.addTile, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.addTile, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.addTile);
            addTile.onClick += TileMenuClick;

            attackTile = new Pushbutton(new Vector2(0, 0), GetTexture(GraphicsDevice, MediumButtonNumber.attackTile, ButtonStateType.idle),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.attackTile, ButtonStateType.hoover),
                                                        GetTexture(GraphicsDevice, MediumButtonNumber.attackTile, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.attackTile);
            attackTile.onClick += TileMenuClick;
            #endregion
        }

        public void TileMenuClick(TileMenuFunction function)
        {
            tileMenuUpdateData.tileMenuFunction = function;
        }

        public TileMenuUpdateData Update(KeyboardState currentKeyboardState, MouseState mouseState)
        {
            #region update elements

            tileMenuUpdateData = new TileMenuUpdateData(false);

            addPeople.Update(mouseState);
                
            foundCity.Update(mouseState);
            addTile.Update(mouseState);
            addTile.visible = false;
            attackTile.Update(mouseState);
            attackTile.visible = false;

            subOreWorker.Update(mouseState);
            addOreWorker.Update(mouseState);
            oreIcon.Update(mouseState);
            oreworkers.update(currentKeyboardState, mouseState);

            subWoodWorker.Update(mouseState);
            addWoodWorker.Update(mouseState);
            woodIcon.Update(mouseState);
            woodWorkers.update(currentKeyboardState, mouseState);

            subFoodWorker.Update(mouseState);
            addFoodWorker.Update(mouseState);
            FoodIcon.Update(mouseState);
            foodWorker.update(currentKeyboardState, mouseState);

            subArmyWorker.Update(mouseState);
            addArmyWorker.Update(mouseState);
            armyIcon.Update(mouseState);
            armyWorker.update(currentKeyboardState, mouseState);

            #endregion

            Population.textArray[0] = selectedTile.population.ToString();

            return tileMenuUpdateData;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            #region draw elements
            if (visible)
            {
                SetPositions();

                addPeople.Draw(spriteBatch, spriteFont);
                Population.draw(spriteBatch, spriteFont);


                foundCity.Draw(spriteBatch, spriteFont);
                addTile.Draw(spriteBatch, spriteFont);
                attackTile.Draw(spriteBatch, spriteFont);

                subOreWorker.Draw(spriteBatch, spriteFont);
                addOreWorker.Draw(spriteBatch, spriteFont);
                oreIcon.Draw(spriteBatch, spriteFont);
                oreworkers.draw(spriteBatch, spriteFont);

                subWoodWorker.Draw(spriteBatch, spriteFont);
                addWoodWorker.Draw(spriteBatch, spriteFont);
                woodIcon.Draw(spriteBatch, spriteFont);
                woodWorkers.draw(spriteBatch, spriteFont);

                subFoodWorker.Draw(spriteBatch, spriteFont);
                addFoodWorker.Draw(spriteBatch, spriteFont);
                FoodIcon.Draw(spriteBatch, spriteFont);
                foodWorker.draw(spriteBatch, spriteFont);

                subArmyWorker.Draw(spriteBatch, spriteFont);
                addArmyWorker.Draw(spriteBatch, spriteFont);
                armyIcon.Draw(spriteBatch, spriteFont);
                armyWorker.draw(spriteBatch, spriteFont);
            }
            #endregion
        }

        #region getter/setter
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
            Vector2 centerpoint = new Vector2(selectedTile.drawArea.X + (Constants.tile_width * Constants.tile_stretch_factor / 2),
                                             selectedTile.drawArea.Y + ((Constants.tile_height - Constants.tile_y_offset)*Constants.tile_stretch_factor) + (Constants.tile_y_space * Constants.tile_stretch_factor / 2));

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
        #endregion

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
    }
    /// <summary>
    /// Enum-List for all TileMenuItems
    /// </summary>
    public enum MenuElement
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
    /// <summary>
    /// Data to eval TileMenu in calling function
    /// </summary>
    public struct TileMenuUpdateData
    {
        public TileMenuFunction tileMenuFunction;

        public TileMenuUpdateData(bool what)
        {
            tileMenuFunction = TileMenuFunction._void_;
        }
    }
    /// <summary>
    /// Enum-List of valid TileMenu-Functions
    /// </summary>
    public enum TileMenuFunction
    {
        _void_,
        none,
        addPeople,
        foundCity,
        addTile,
        attackTile,
        subOre,
        addOre,
        subWood,
        addWood,
        subFood,
        addFood,
        subArmy,
        addArmy
    }
}

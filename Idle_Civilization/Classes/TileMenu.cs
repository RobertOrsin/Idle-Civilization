﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Utility_Functions;
using Microsoft.CSharp.RuntimeBinder;

namespace Idle_Civilization.Classes
{
    class TileMenu
    {
        public bool visible = false;
        Vector2 centerPosition = new Vector2(5,5);
        //public Rectangle tileArea;
        public Tile selectedTile, oldselectedTile;
        public TileMenuUpdateData tileMenuUpdateData = new TileMenuUpdateData(false);

        private bool animation_finished = false;
        private int animation_steps = 10;
        private int animation_step = 0;
        double timer = 20; //in ms
        const double TIME = 20;


        #region MenuItems
        Pushbutton addPeople;
        TextBox population;

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

        Pushbutton subArmy, addArmy;
        Pushbutton armyIcon_Deploy;
        TextBox armyDeployed;

        Dictionary<MenuElement, Vector2> menuElementOffsets;
        #endregion

        public TileMenu()
        {
            InitMenuItemOffsets();

            #region Init Elements
            List<Texture2D> minusButton, plusButton;
            minusButton = new List<Texture2D>();
            minusButton.Add(GetTexture(SmallButtonNumber.minus, ButtonStateType.idle));
            minusButton.Add(GetTexture(SmallButtonNumber.minus, ButtonStateType.hoover));
            minusButton.Add(GetTexture(SmallButtonNumber.minus, ButtonStateType.pressed));
            plusButton = new List<Texture2D>();
            plusButton.Add(GetTexture(SmallButtonNumber.plus, ButtonStateType.idle));
            plusButton.Add(GetTexture(SmallButtonNumber.plus, ButtonStateType.hoover));
            plusButton.Add(GetTexture(SmallButtonNumber.plus, ButtonStateType.pressed));

            addPeople = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.people, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.people, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.people, ButtonStateType.pressed),"",Color.AliceBlue, TileMenuFunction.addPeople);
            addPeople.onClick += TileMenuClick;
            population = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255,0,0));

            oreIcon = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.ore, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.ore, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.ore, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            oreIcon.locked = true;
            oreworkers = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 0, 0));
            addOreWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addOre);
            addOreWorker.onClick += TileMenuClick;
            subOreWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subOre);
            subOreWorker.onClick += TileMenuClick;

            woodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.wood, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.wood, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.wood, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            woodIcon.locked = true;
            woodWorkers = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 0, 0));
            addWoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addWood);
            addWoodWorker.onClick += TileMenuClick;
            subWoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subWood);
            subWoodWorker.onClick += TileMenuClick;

            FoodIcon = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.food, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.food, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.food, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            FoodIcon.locked = true;
            foodWorker = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 0, 0));
            addFoodWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addFood);
            addFoodWorker.onClick += TileMenuClick;
            subFoodWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subFood);
            subFoodWorker.onClick += TileMenuClick;

            armyIcon = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.army, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.army, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.army, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            armyIcon.locked = true;
            armyWorker = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 0, 0));
            addArmyWorker = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addArmy);
            addArmyWorker.onClick += TileMenuClick;
            subArmyWorker = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subArmy);
            subArmyWorker.onClick += TileMenuClick;

            foundCity = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.foundCity, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.foundCity, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.foundCity, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.foundCity);
            foundCity.onClick += TileMenuClick;

            addTile = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.addTile, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.addTile, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.addTile, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.addTile);
            addTile.onClick += TileMenuClick;

            attackTile = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.attackTile, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.attackTile, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.attackTile, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.attackTile);
            attackTile.onClick += TileMenuClick;

            armyIcon_Deploy = new Pushbutton(new Vector2(0, 0), GetTexture(MediumButtonNumber.army, ButtonStateType.idle),
                                                        GetTexture(MediumButtonNumber.army, ButtonStateType.hoover),
                                                        GetTexture(MediumButtonNumber.army, ButtonStateType.pressed), "", Color.AliceBlue, TileMenuFunction.none);
            armyIcon_Deploy.locked = true;
            addArmy = new Pushbutton(new Vector2(0, 0), plusButton, "", Color.AliceBlue, TileMenuFunction.addArmyDeploy);
            addArmy.onClick += TileMenuClick;
            subArmy = new Pushbutton(new Vector2(0, 0), minusButton, "", Color.AliceBlue, TileMenuFunction.subArmyDeploy);
            subArmy.onClick += TileMenuClick;
            armyDeployed = new TextBox(new Vector2(0, 0), new Point(50, 15), 1, false, true, null, new Color(255, 0, 0));

            #endregion
        }

        public void TileMenuClick(TileMenuFunction function)
        {
            tileMenuUpdateData.tileMenuFunction = function;
        }

        public TileMenuUpdateData Update(KeyboardState currentKeyboardState, MouseState mouseState, GameTime gameTime)
        {
            tileMenuUpdateData = new TileMenuUpdateData(false);
            #region update elements

            UpdateButtonVisibility();

            addPeople.Update(mouseState);
                
            foundCity.Update(mouseState);
            addTile.Update(mouseState);
            attackTile.Update(mouseState);

            subOreWorker.Update(mouseState);
            addOreWorker.Update(mouseState);
            oreIcon.Update(mouseState);
            oreworkers.Update(currentKeyboardState, mouseState);

            subWoodWorker.Update(mouseState);
            addWoodWorker.Update(mouseState);
            woodIcon.Update(mouseState);
            woodWorkers.Update(currentKeyboardState, mouseState);

            subFoodWorker.Update(mouseState);
            addFoodWorker.Update(mouseState);
            FoodIcon.Update(mouseState);
            foodWorker.Update(currentKeyboardState, mouseState);

            subArmyWorker.Update(mouseState);
            addArmyWorker.Update(mouseState);
            armyIcon.Update(mouseState);
            armyWorker.Update(currentKeyboardState, mouseState);

            subArmy.Update(mouseState);
            addArmy.Update(mouseState);
            armyIcon_Deploy.Update(mouseState);
            armyDeployed.Update(currentKeyboardState, mouseState);

            #endregion

            #region write tile-values to menu
            population.textArray[0] = selectedTile.population.ToString();
            oreworkers.textArray[0] = selectedTile.ore_worker.ToString();
            woodWorkers.textArray[0] = selectedTile.wood_worker.ToString();
            foodWorker.textArray[0] = selectedTile.food_worker.ToString();
            armyWorker.textArray[0] = selectedTile.army_worker.ToString();
            armyDeployed.textArray[0] = selectedTile.armystrength.ToString();
            #endregion

            tileMenuUpdateData.clickDetected = tileMenuUpdateData.tileMenuFunction != TileMenuFunction.none && tileMenuUpdateData.tileMenuFunction != TileMenuFunction._void_;

            if (selectedTile != oldselectedTile)
            {
                animation_finished = false;
                oldselectedTile = selectedTile;
                animation_step = 0;
            }

            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            timer -= elapsed;

            if (timer < 0)
            {
                timer = TIME;

                if (!animation_finished)
                {
                    animation_step++;

                    if (animation_step >= animation_steps)
                    {
                        animation_finished = true;
                        
                    }
                }
            }

            return tileMenuUpdateData;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            #region draw elements
            if (visible)
            {
                SetPositions();

                addPeople.Draw(spriteBatch, spriteFont);
                population.Draw(spriteBatch, spriteFont);

                foundCity.Draw(spriteBatch, spriteFont);
                addTile.Draw(spriteBatch, spriteFont);
                attackTile.Draw(spriteBatch, spriteFont);

                subOreWorker.Draw(spriteBatch, spriteFont);
                addOreWorker.Draw(spriteBatch, spriteFont);
                oreIcon.Draw(spriteBatch, spriteFont);
                oreworkers.Draw(spriteBatch, spriteFont);

                subWoodWorker.Draw(spriteBatch, spriteFont);
                addWoodWorker.Draw(spriteBatch, spriteFont);
                woodIcon.Draw(spriteBatch, spriteFont);
                woodWorkers.Draw(spriteBatch, spriteFont);

                subFoodWorker.Draw(spriteBatch, spriteFont);
                addFoodWorker.Draw(spriteBatch, spriteFont);
                FoodIcon.Draw(spriteBatch, spriteFont);
                foodWorker.Draw(spriteBatch, spriteFont);

                subArmyWorker.Draw(spriteBatch, spriteFont);
                addArmyWorker.Draw(spriteBatch, spriteFont);
                armyIcon.Draw(spriteBatch, spriteFont);
                armyWorker.Draw(spriteBatch, spriteFont);

                subArmy.Draw(spriteBatch, spriteFont);
                addArmy.Draw(spriteBatch, spriteFont);
                armyIcon_Deploy.Draw(spriteBatch, spriteFont);
                armyDeployed.Draw(spriteBatch, spriteFont);
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
        private Texture2D GetTexture(MediumButtonNumber position, ButtonStateType buttonStateType)
        {
            Texture2D return_tex;
            Color[] data;
            Rectangle rect = GetTileSpritePosition(position, buttonStateType);

            return_tex = new Texture2D(Globals.GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            Globals.buttons_medium.GetData(0, rect, data, 0, data.Length);
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
        private Texture2D GetTexture(SmallButtonNumber position, ButtonStateType buttonStateType)
        {
            Texture2D return_tex;
            Color[] data;
            Rectangle rect = GetTileSpritePosition(position, buttonStateType);

            return_tex = new Texture2D(Globals.GraphicsDevice, rect.Width, rect.Height);
            data = new Color[rect.Width * rect.Height];

            Globals.buttons_small.GetData(0, rect, data, 0, data.Length);
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
            Vector2 centerpoint = new Vector2(selectedTile.drawArea.X + (Constants.tile_width * Globals.tile_stretch_factor / 2),
                                             selectedTile.drawArea.Y + ((Constants.tile_height - Constants.tile_y_offset)* Globals.tile_stretch_factor) + (Constants.tile_y_space * Globals.tile_stretch_factor / 2));

            #region set position of elements
            addPeople.position = centerpoint + menuElementOffsets[MenuElement.TopButton] / animation_steps * animation_step;
            population.position = centerpoint + menuElementOffsets[MenuElement.TopTextBox] / animation_steps * animation_step;

            foundCity.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton] / animation_steps * animation_step;
            addTile.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton] / animation_steps * animation_step;
            attackTile.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton] / animation_steps * animation_step;

            subOreWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerRightMinus] / animation_steps * animation_step;
            addOreWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerRightPlus] / animation_steps * animation_step;
            oreIcon.position = centerpoint + menuElementOffsets[MenuElement.LowerRightButton] / animation_steps * animation_step;
            oreworkers.position = centerpoint + menuElementOffsets[MenuElement.LowerRightTextBox] / animation_steps * animation_step;

            subWoodWorker.position = centerpoint + menuElementOffsets[MenuElement.BottomMinus] / animation_steps * animation_step;
            addWoodWorker.position = centerpoint + menuElementOffsets[MenuElement.BottomPlus] / animation_steps * animation_step;
            woodIcon.position = centerpoint + menuElementOffsets[MenuElement.BottomButton] / animation_steps * animation_step;
            woodWorkers.position = centerpoint + menuElementOffsets[MenuElement.BottomTextBox] / animation_steps * animation_step;

            subFoodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftMinus] / animation_steps * animation_step;
            addFoodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftPlus] / animation_steps * animation_step;
            FoodIcon.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftButton] / animation_steps * animation_step;
            foodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftTextBox] / animation_steps * animation_step;

            subArmyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftMinus] / animation_steps * animation_step;
            addArmyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftPlus] / animation_steps * animation_step;
            armyIcon.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftButton] / animation_steps * animation_step;
            armyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftTextBox] / animation_steps * animation_step;

            subArmy.position = centerpoint + menuElementOffsets[MenuElement.UpperRightMinus] / animation_steps * animation_step;
            addArmy.position = centerpoint + menuElementOffsets[MenuElement.UpperRightPlus] / animation_steps * animation_step;
            armyIcon_Deploy.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton] / animation_steps * animation_step;
            armyDeployed.position = centerpoint + menuElementOffsets[MenuElement.UpperRightTextBox] / animation_steps * animation_step;
            #endregion

            //#region set position of elements
            //addPeople.position = centerpoint + menuElementOffsets[MenuElement.TopButton];
            //population.position = centerpoint + menuElementOffsets[MenuElement.TopTextBox];

            //foundCity.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];
            //addTile.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];
            //attackTile.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];

            //subOreWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerRightMinus];
            //addOreWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerRightPlus];
            //oreIcon.position = centerpoint + menuElementOffsets[MenuElement.LowerRightButton];
            //oreworkers.position = centerpoint + menuElementOffsets[MenuElement.LowerRightTextBox];

            //subWoodWorker.position = centerpoint + menuElementOffsets[MenuElement.BottomMinus];
            //addWoodWorker.position = centerpoint + menuElementOffsets[MenuElement.BottomPlus];
            //woodIcon.position = centerpoint + menuElementOffsets[MenuElement.BottomButton];
            //woodWorkers.position = centerpoint + menuElementOffsets[MenuElement.BottomTextBox];

            //subFoodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftMinus];
            //addFoodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftPlus];
            //FoodIcon.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftButton];
            //foodWorker.position = centerpoint + menuElementOffsets[MenuElement.LowerLeftTextBox];

            //subArmyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftMinus];
            //addArmyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftPlus];
            //armyIcon.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftButton];
            //armyWorker.position = centerpoint + menuElementOffsets[MenuElement.UpperLeftTextBox];

            //subArmy.position = centerpoint + menuElementOffsets[MenuElement.UpperRightMinus];
            //addArmy.position = centerpoint + menuElementOffsets[MenuElement.UpperRightPlus];
            //armyIcon_Deploy.position = centerpoint + menuElementOffsets[MenuElement.UpperRightButton];
            //armyDeployed.position = centerpoint + menuElementOffsets[MenuElement.UpperRightTextBox];
            //#endregion
        }

        public void ResetAnimation()
        {
            animation_finished = false;
            oldselectedTile = selectedTile;
            animation_step = 0;
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

            menuElementOffsets.Add(MenuElement.UpperRightMinus, new Vector2(37, -64));
            menuElementOffsets.Add(MenuElement.UpperRightPlus, new Vector2(67, -64));
            menuElementOffsets.Add(MenuElement.UpperRightButton, new Vector2(48, -51));
            menuElementOffsets.Add(MenuElement.UpperRightTextBox, new Vector2(35, -24));

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
        /// <summary>
        /// Depending on tile-attributes set MenuItems in/visible
        /// </summary>
        private void UpdateButtonVisibility()
        {
            addPeople.visible = selectedTile.hasCity;
            population.visible = selectedTile.hasCity;

            subOreWorker.visible = selectedTile.hasCity;
            addOreWorker.visible = selectedTile.hasCity;
            oreIcon.visible = selectedTile.hasCity;
            oreworkers.visible = selectedTile.hasCity;

            subWoodWorker.visible = selectedTile.hasCity;
            addWoodWorker.visible = selectedTile.hasCity;
            woodIcon.visible = selectedTile.hasCity;
            woodWorkers.visible = selectedTile.hasCity;

            subFoodWorker.visible = selectedTile.hasCity;
            addFoodWorker.visible = selectedTile.hasCity;
            FoodIcon.visible = selectedTile.hasCity;
            foodWorker.visible = selectedTile.hasCity;

            subArmyWorker.visible = selectedTile.hasCity;
            addArmyWorker.visible = selectedTile.hasCity;
            armyIcon.visible = selectedTile.hasCity;
            armyWorker.visible = selectedTile.hasCity;

            foundCity.visible = !selectedTile.hasCity && !selectedTile.isCitypart && !selectedTile.aNeighborisCity && !selectedTile.aNeighborisEnemy;
            addTile.visible = !selectedTile.hasCity && !selectedTile.isCitypart && selectedTile.aNeighborisCity;
            attackTile.visible = selectedTile.hasEnemy;

            subArmy.visible = selectedTile.isCitypart;
            addArmy.visible = selectedTile.isCitypart;
            armyIcon_Deploy.visible = selectedTile.isCitypart;
            armyDeployed.visible = selectedTile.isCitypart;
        }
    }
    /// <summary>
    /// Enum-List for all TileMenuItems
    /// </summary>
    public enum MenuElement
    {
        TopButton,
        TopTextBox,
        UpperRightMinus,
        UpperRightPlus,
        UpperRightButton,
        UpperRightTextBox,
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
        public bool clickDetected;

        public TileMenuUpdateData(bool what)
        {
            tileMenuFunction = TileMenuFunction._void_;
            clickDetected = false;
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
        addArmy,
        addArmyDeploy,
        subArmyDeploy
    }
}

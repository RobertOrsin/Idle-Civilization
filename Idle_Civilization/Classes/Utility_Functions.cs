﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Idle_Civilization.Classes;

namespace Utility_Functions
{
    /// <summary>
    /// Some Static Funktions
    /// </summary>
    class Utility
    {
        /// <summary>
        /// Check if MouseCursor is inside rectangle
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool mouseInBounds(Rectangle bounds, Vector2 position)
        {
            if (position.X >= bounds.X && position.X <= (bounds.X + bounds.Width) && position.Y >= bounds.Y && position.Y <= (bounds.Y + bounds.Height))
                return true;

            return false;
        }
        /// <summary>
        /// Draw a Frame
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="frame"></param>
        /// <param name="Bounds"></param>
        /// <param name="lineWidth"></param>
        /// <param name="color"></param>
        public static void drawFrame(SpriteBatch spriteBatch, Texture2D frame, Rectangle Bounds, int lineWidth, Color color)
        {
            spriteBatch.Draw(frame, new Rectangle(Bounds.X, Bounds.Y, lineWidth, Bounds.Height + lineWidth), color);
            spriteBatch.Draw(frame, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(frame, new Rectangle(Bounds.X + Bounds.Width, Bounds.Y, lineWidth, Bounds.Height + lineWidth), color);
            spriteBatch.Draw(frame, new Rectangle(Bounds.X, Bounds.Y + Bounds.Height, Bounds.Width + lineWidth, lineWidth), color);
        }
    }
    /// <summary>
    /// Represents a Textbox
    /// </summary>
    public class TextBox
    {
        public List<string> textArray; //Intilize Array Bellow, or in declaration
        public bool visible = true;
        public bool selected = false;
        public Vector2 position;
        public Point dimension;

        private KeyboardState previousKeyboardState;
        private int maxRows;
        private bool Caps_detected = false;
        private Texture2D frame;
        private Rectangle bounds;
        private String displayName;
        
        private bool editingEnables;
        private bool background_transparent;
        private Color fontColor;

        public TextBox(Vector2 _position, Point _dimension, int _maxRows, bool _editingEnables, bool _background_transparent, List<string> _textArray, Color _fontColor)
        {
            position = _position;
            dimension = _dimension;
            maxRows = _maxRows;

            if (_textArray == null)
            {
                textArray = new List<string>();
                textArray.Add("");
            }
            else
            {
                textArray = _textArray;
            }
            bounds = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), _dimension.X, _dimension.Y);
            frame = new Texture2D(Globals.GraphicsDevice, 1, 1);
            frame.SetData<Color>(new Color[] { Color.White });
            displayName = "type here";
            editingEnables = _editingEnables;
            background_transparent = _background_transparent;
            fontColor = _fontColor;
        }

        public void Update(KeyboardState currentKeyboardState, MouseState mouseState)
        {
            if (visible)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && Utility.mouseInBounds(bounds, new Vector2(mouseState.X, mouseState.Y)))
                {
                    selected = true;
                }
                else if (mouseState.LeftButton == ButtonState.Pressed && !Utility.mouseInBounds(bounds, new Vector2(mouseState.X, mouseState.Y)))
                {
                    selected = false;
                }

                if (selected && editingEnables)
                {
                    Keys[] pressedKeys;
                    pressedKeys = currentKeyboardState.GetPressedKeys();

                    foreach (Keys key in pressedKeys)
                    {
                        if (previousKeyboardState.IsKeyUp(key))
                        {
                            if (key == Keys.Back)
                            {
                                if (textArray[textArray.Count - 1].Length > 0)
                                    textArray[textArray.Count - 1] = textArray[textArray.Count - 1].Remove(textArray[textArray.Count - 1].Length - 1, 1);
                            }
                            else if (key == Keys.Space)
                            {
                                textArray[textArray.Count - 1] = textArray[textArray.Count - 1].Insert(textArray[textArray.Count - 1].Length, " ");
                            }
                            else if (key == Keys.Enter)
                            {
                                if (textArray.Count < maxRows)
                                    textArray.Add("");
                            }
                            //Numbers
                            else if ((key >= Keys.D0 && key <= Keys.D9) || (key >= Keys.NumPad0 && key <= Keys.NumPad9))
                            {
                                textArray[textArray.Count - 1] += key.ToString()[key.ToString().Length - 1];
                            }
                            //Line and Unterline
                            else if (key == Keys.OemMinus)
                            {
                                if (Caps_detected)
                                {
                                    textArray[textArray.Count - 1] += "_";
                                    Caps_detected = false;
                                }
                                else
                                {
                                    textArray[textArray.Count - 1] += "-";
                                }
                            }
                            //Shift
                            else if (key == Keys.LeftShift || key == Keys.RightShift)
                            {
                                Caps_detected = true;
                            }
                            //Not used
                            else if (key == Keys.CapsLock || key == Keys.LeftControl || key == Keys.RightControl || key == Keys.LeftWindows || key == Keys.RightWindows || key == Keys.LeftAlt || key == Keys.RightAlt) { }
                            //Anything else
                            else
                            {
                                if (Caps_detected)
                                {
                                    textArray[textArray.Count - 1] += key.ToString();
                                    Caps_detected = false;
                                }
                                else
                                {
                                    textArray[textArray.Count - 1] += key.ToString().ToLower();
                                }
                            }

                        }
                    }
                    previousKeyboardState = currentKeyboardState;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            bounds = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), dimension.X, dimension.Y);
            if (visible)
            {
                Color color = new Color();

                if (background_transparent)
                    color = new Color(0, 0, 0, 0);
                else
                    color = new Color(255, 255, 255, 255);


                //Draw Frame      
                spriteBatch.Draw(frame, bounds, color);
                //Displayname if no text typed yet
                if (textArray.Count <= 1 && textArray[0].Count() < 1)
                    spriteBatch.DrawString(spriteFont, displayName, new Vector2(bounds.X, bounds.Y), Color.LightGray);
                //Draw for each line
                int i;
                for (i = 0; i < textArray.Count; i++)
                {
                    spriteBatch.DrawString(spriteFont, textArray[i], new Vector2(position.X, position.Y + 20 * i), fontColor);
                }
                //draw prompt
                if (selected)
                {

                }
            }
        }
    }
    /// <summary>
    /// Represents a Pushbutton
    /// </summary>
    class Pushbutton
    {
        public Vector2 position = new Vector2();
        private Dictionary<ButtonStateType, Texture2D> textures = new Dictionary<ButtonStateType, Texture2D>();
        private ButtonStateType state = new ButtonStateType();
        private Rectangle bounds = new Rectangle();
        private string text;
        private Color color;
        public bool visible = true;
        public bool locked = false;
        private TileMenuFunction buttonFunction = new TileMenuFunction();

        public delegate void ClickEventHandler(TileMenuFunction function);
        public event ClickEventHandler onClick;

        private MouseState old_mouseState;

        #region constructors
        /// <summary>
        /// Creates a new Pushbutton-objekt
        /// </summary>
        /// <param name="_position">where the button is drawn, upper left corner</param>
        /// <param name="_textures">3 textures, idle, hoover, pressed</param>
        public Pushbutton(Vector2 _position, List<Texture2D> _textures, string _text, Color _color, TileMenuFunction _buttonFunction)
        {
            state = ButtonStateType.idle;
            position = _position;

            textures.Add(ButtonStateType.idle, _textures[0]);
            textures.Add(ButtonStateType.hoover, _textures[1]);
            textures.Add(ButtonStateType.pressed, _textures[2]);

            bounds = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), textures[0].Width, textures[0].Height);

            text = _text;
            color = _color;
            buttonFunction = _buttonFunction;
        }
        /// <summary>
        /// Creates a new Pushbutton-objekt
        /// </summary>
        /// <param name="_position">where the button is drawn, upper left corner</param>
        /// <param name="idletex">texture of idlebutton</param>
        /// <param name="hoovertex">texture of hoovering button</param>
        /// <param name="presstex">texture of pressed button</param>
        public Pushbutton(Vector2 _position, Texture2D idletex, Texture2D hoovertex, Texture2D presstex, string _text, Color _color, TileMenuFunction _buttonFunction)
        {
            state = ButtonStateType.idle;
            position = _position;

            textures.Add(ButtonStateType.idle, idletex);
            textures.Add(ButtonStateType.hoover, hoovertex);
            textures.Add(ButtonStateType.pressed, presstex);

            bounds = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), textures[0].Width, textures[0].Height);

            text = _text;
            color = _color;
            buttonFunction = _buttonFunction;
        }
        /// <summary>
        /// Creates a new Pushbutton-objekt
        /// </summary>
        /// <param name="_position">where the button is drawn, upper left corner</param>
        /// <param name="_textures">dictionary of statetype and textures</param>
        public Pushbutton(Vector2 _position, Dictionary<ButtonStateType, Texture2D> _textures, string _text, Color _color, TileMenuFunction _buttonFunction)
        {
            state = ButtonStateType.idle;
            position = _position;

            foreach (KeyValuePair<ButtonStateType, Texture2D> tex in _textures)
            {
                textures.Add(tex.Key, tex.Value);
            }

            bounds = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), textures[0].Width, textures[0].Height);

            text = _text;
            color = _color;
            buttonFunction = _buttonFunction;
        }
        #endregion

        public void Update(MouseState mouseState)
        {
            if (visible && !locked)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && old_mouseState.LeftButton == ButtonState.Released && Utility.mouseInBounds(bounds, new Vector2(mouseState.X, mouseState.Y)))
                {
                    state = ButtonStateType.pressed;
                    onClick?.Invoke(buttonFunction);
                }
                else if (mouseState.LeftButton == ButtonState.Released && Utility.mouseInBounds(bounds, new Vector2(mouseState.X, mouseState.Y)))
                {
                    state = ButtonStateType.hoover;
                }
                else
                {
                    state = ButtonStateType.idle;
                }

                bounds = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), textures[0].Width, textures[0].Height);
            }

            old_mouseState = mouseState;
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (visible)
            {
                spriteBatch.Draw(textures[state], position, Color.Wheat);

                if (text.Length > 0)
                {
                    Vector2 size = font.MeasureString(text);
                    Point pos = bounds.Center;
                    Vector2 textpos = new Vector2(pos.X - size.X / 2, pos.Y - size.Y / 2);

                    spriteBatch.DrawString(font, text, textpos, color);
                }
            }
        }
    }

    class Animation
    {
        private Texture2D animation_sheet;
        private int width, height, count_per_row, rows, animation_index = 0;
        private double timer, time;

        public Animation(Texture2D _animation_sheet, int _width, int _height, int _count_per_row, int _rows, double _time)
        {
            animation_sheet = _animation_sheet;
            width = _width;
            height = _height;
            count_per_row = _count_per_row;
            rows = _rows;
            time = _time;
            timer = time;
        }

        public void Update(MouseState mouseState, GameTime gameTime, List<Tile> neighbors)
        {
            double elapsed = gameTime.ElapsedGameTime.TotalMilliseconds;
            timer -= elapsed;

            if (timer < 0)
            {
                timer = time;
                animation_index++;

                if (animation_index >= count_per_row * rows)
                    animation_index = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.Draw(GetActualTexture(), position, null, Color.Wheat, 0.0f, new Vector2(), SpriteEffects.None, 0.0f);
        }

        private Texture2D GetActualTexture()
        {
            Color[] data;
            Rectangle rect = GetTileSpritePosition();

            Texture2D return_texture = new Texture2D(Globals.GraphicsDevice, rect.Width, rect.Height);

            data = new Color[rect.Width * rect.Height];

            animation_sheet.GetData(0, rect, data, 0, data.Length);
            return_texture.SetData(data);

            return return_texture;
        }

        private Rectangle GetTileSpritePosition()
        {
            return new Rectangle(width * (animation_index % count_per_row), height * (animation_index / count_per_row), width, height);
        }
    }

    public enum ButtonStateType
    {
        idle,
        pressed,
        hoover,  
    }
}

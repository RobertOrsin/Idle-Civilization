using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Idle_Civilization.Classes;


namespace Idle_Civilization
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        Classes.Session session;

        Texture2D tileMap, buttons_medium, buttons_small, borders;

        public static MouseState mouseState, oldMouseState;
        public static KeyboardState keyboardState, oldKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;

            Globals.primitive = new Texture2D(GraphicsDevice, 1, 1);

            spriteFont = Content.Load<SpriteFont>("std_font");
            tileMap = Content.Load<Texture2D>("basetiles");
            buttons_medium = Content.Load<Texture2D>("Buttons_Medium_Spritesheet");
            buttons_small = Content.Load<Texture2D>("Buttons_Small_Spritesheet");
            borders = Content.Load<Texture2D>("borders");

            session = new Classes.Session(GraphicsDevice, tileMap, buttons_medium, buttons_small, 30, 45, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            session.SerializeBorderTextures(GraphicsDevice, borders);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            session.Update(keyboardState, mouseState, gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            session.Draw(spriteBatch, spriteFont);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

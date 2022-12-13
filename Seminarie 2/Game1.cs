using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Seminarie_2.GameObjects;
using SharpDX.Direct2D1;
using System;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Seminarie_2
{
    public class Game1 : Game
    {
        // Int
        int Height = 700;
        int Width = 700;
        int CannonHeight = 22;
        int CannonWidth = 30;
        int CarHeight;
        int CarWidth;

        // Textrue2D
        Texture2D BallTex;
        Texture2D CarTex;

        // Others
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        Color BackgroundColor = Color.Green;
        Cannon Cannon;
        
        bool IsKeyPressed;
        ProgramState CurrentState = 0;
        Car Car;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            SetWindowDimensions();
            InitializeTextures();

            CarHeight = CarTex.Height * 4;
            CarWidth = CarTex.Width * 4;

            Cannon = CreateCannon();
            Car = CreateCar();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().GetPressedKeyCount() == 0)
                IsKeyPressed = false;

            Cannon.Update(deltaTime);

            Car.Update(CurrentState);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !IsKeyPressed)
            {
                Cannon.Shoot();
                IsKeyPressed = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, Microsoft.Xna.Framework.Graphics.SamplerState.PointWrap);
            Car.Draw(SpriteBatch, BallTex);
            Cannon.Draw(SpriteBatch);
            
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        void InitializeTextures()
        {
            BallTex = Content.Load<Texture2D>("Pool ball");
            CarTex = Content.Load<Texture2D>("Car");
        }

        void SetWindowDimensions()
        {
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.ApplyChanges();
        }

        Cannon CreateCannon()
        {
            Rectangle destRec = new(0, Height - CannonHeight/ 2, CannonWidth, CannonHeight);
            Vector2 drawOrigin = new(0,1f);
            Vector2 launchOrigin = new(0, Height);

            Cannon newCannon = new(SpriteBatch, BallTex, destRec, drawOrigin, launchOrigin);

            return newCannon;
        }

        Car CreateCar()
        {
            Rectangle destRec = new(0,0,CarWidth, CarHeight);

            Car newCar = new(CarTex,destRec, new(Width/2,Height/2), new(),100.0f);
            return newCar;
        }
    }
}
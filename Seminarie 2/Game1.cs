using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Seminarie_2.GameObjects;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        int DisplayTimesAmount = 1;

        // Textrue2D
        Texture2D BallTex;
        Texture2D CarTex;

        // Bool
        bool IsKeyPressed;
        bool PauseGame;
        bool IsMouseButtonPressed;

        // String
        string TimeFormat = "{0:00}:{1:00}.{2:000}";
        List<string> TimeOfCollisions = new List<string>();
        string ElapsedTime;

        // Others
        private GraphicsDeviceManager Graphics;
        private SpriteBatch SpriteBatch;
        Color BackgroundColor = Color.Green;
        Cannon Cannon;
        ProgramState CurrentState = 0;
        Car Car;
        float CirclePathRadius = 100.0f;
        Stopwatch Timer;
        SpriteFont GameFont;

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
            GameFont = Content.Load<SpriteFont>("File");

            CarHeight = CarTex.Height * 4;
            CarWidth = CarTex.Width * 4;

            Cannon = CreateCannon();
            Cannon.SetBallOnCollisionFunction(SavePositions);
            Car = CreateCar();

            Timer = new();
            Timer.Start();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard.GetPressedKeyCount() == 0)
                IsKeyPressed = false;

            if (mouse.LeftButton == ButtonState.Released)
                IsMouseButtonPressed = false;

            if (!IsKeyPressed)
            {
                if (keyboard.IsKeyDown(Keys.P))
                {
                    PauseGame = !PauseGame;
                    IsKeyPressed = true;
                }

                if (keyboard.IsKeyDown(Keys.C))
                    switch (CurrentState)
                    {
                        case ProgramState.CirclePath:
                            CurrentState = ProgramState.FunctionPath;
                            IsKeyPressed = true;
                            break;
                        case ProgramState.FunctionPath:
                            CurrentState = ProgramState.CirclePath;
                            IsKeyPressed = true;
                            break;
                    }
            }

            if (!PauseGame)
            {
                if (!IsMouseButtonPressed)
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        Cannon.Shoot();
                        IsMouseButtonPressed = true;
                    }
                }

                Cannon.AimAt(mouse.X, mouse.Y);

                Cannon.Update(deltaTime, Car);

                Car.Update(CurrentState);
            }

            if (PauseGame)
                Timer.Stop();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, Microsoft.Xna.Framework.Graphics.SamplerState.PointWrap);
            Car.Draw(SpriteBatch, BallTex);
            Cannon.Draw(SpriteBatch);

            if (TimeOfCollisions.Count >= 1)
                for (int x = 1; x <= DisplayTimesAmount; x++)
                {
                    if (TimeOfCollisions.Count - x < 0)
                        continue;

                    SpriteBatch.DrawString(GameFont, TimeOfCollisions[TimeOfCollisions.Count - x], new Vector2(Width - GameFont.MeasureString(TimeOfCollisions[TimeOfCollisions.Count - x]).X, 0), Color.White);
                }
                

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

            Car newCar = new(CarTex,destRec, new(Width/2,Height/2), new(0, Height / 2), CirclePathRadius);
            newCar.SetScreenHeight(Height);
            return newCar;
        }

        public void SavePositions(Ball ball)
        {
            TimeOfCollisions.Add(ElapsedTime + " : BALL" + ball.Pos + " : Car" + Car.Pos);
        }
    }
}
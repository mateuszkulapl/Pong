using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public enum Side
    {
        Left,
        Right,
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D background;
        Texture2D lPadTexture;
        Texture2D rPadTexture;
        Texture2D ballTexture;

        private Paddle lPad;
        private Paddle rPad;
        private Ball ball;
        bool active = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            this.Window.AllowUserResizing = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>(@"pongBackground");
            ballTexture = Content.Load<Texture2D>(@"pongBall");

            lPadTexture = Content.Load<Texture2D>(@"paddle1");
            rPadTexture = Content.Load<Texture2D>(@"paddle2");

            lPad = new Paddle(lPadTexture, GraphicsDevice.Viewport, Side.Left, Keys.Q, Keys.A);
            rPad = new Paddle(rPadTexture, GraphicsDevice.Viewport, Side.Right, Keys.P, Keys.L);

            ball = new Ball(ballTexture, GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState kb = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                Exit();

            if (kb.IsKeyDown(Keys.Space) && active == false)
            {
                StartGame();
            }

            ball.Move(lPad, rPad);
            rPad.CheckMove(kb);
            lPad.CheckMove(kb);


            if (ball.IsEnd(lPad, rPad) && active == true)
            {
                EndGame();
            }

            base.Update(gameTime);
        }



        private void StartGame()
        {
            active = true;
            ball.Start();
            rPad.Start();
            lPad.Start();
        }

        private void EndGame()
        {
            active = false;
            ball.End();
            rPad.End();
            lPad.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            ball.Draw(_spriteBatch);
            lPad.Draw(_spriteBatch);
            rPad.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

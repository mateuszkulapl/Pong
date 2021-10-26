using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{

    public enum position
    {
        Left,
        Right,
    }

    public class Ball
    {
        Texture2D texture;
        Vector2 position;
        Vector2 speed;
        Viewport graphicViewport;
        float r;
        

        public Ball(Texture2D texture, Viewport graphicViewport)
        {
            this.texture = texture;
            this.speed = new Vector2(0,0);
            this.graphicViewport = graphicViewport;

            this.position = new Vector2((int)graphicViewport.Width / 2,(int) graphicViewport.Height / 2);
            this.r = 20;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            Rectangle screenPosition=new Rectangle((int)(position.X-r),(int)(position.Y-r),(int)(2*r),(int)(2*r));
            spriteBatch.Draw(texture, screenPosition, Color.White);
        }
        internal void Start(int speed=5)
        {
            this.speed = new Vector2(speed, speed/2);
        }
        internal void Move(Paddle l, Paddle r)
        {
            position += speed;
            checkCollision(l,r);
        }
        private void checkCollision(Paddle l, Paddle r)
        {
            paddleColider(l);

            paddleColider(r);

            screenCollider();

        }

        
        private void paddleColider(Paddle paddle)
        {
            Vector2 offfetVector = position + new Vector2(r, 0);
            if(paddle.getPosition().Contains(offfetVector))
            {
                speed.X *= -1;
            }
            else
            {
                offfetVector = position + new Vector2(-r, 0);
                if (paddle.getPosition().Contains(offfetVector))
                {
                    speed.X *= -1;
                }
            }

        }
        private void screenCollider()
        {
            Vector2 offfetVector = position + new Vector2(0, r);
            if ((offfetVector).Y > graphicViewport.Height)
            {
                speed.Y *= -1;
            }
            else
            {
                offfetVector = position + new Vector2(0, -r);
                if ((offfetVector).Y <= 0)
                {
                    speed.Y *= -1;
                }
            }

        }

        public bool isEnd(Paddle lPad, Paddle rPad)
        {
            Vector2 offfetVector = position + new Vector2(r, 0);
            if ((offfetVector).X > graphicViewport.Width)
            {
                lPad.addPoints();
                return true;
            }
            else
            {
                offfetVector = position + new Vector2(-r, 0);
                if ((offfetVector).X <= 0)
                {
                    rPad.addPoints();
                    return true;
                }
            }


            return false;
        }

    }
    public class Paddle
    {
        Texture2D texture;
        position position;
        Rectangle screenPosition;
        int points;
        bool active;

        Keys upKey;
        Keys downKey;
        Viewport graphicViewport;

        public Paddle(Texture2D texture, Viewport graphicViewport, position pos, Keys upKey, Keys downKey)
        {
            this.graphicViewport = graphicViewport;
            this.texture = texture;
            this.points = 0;
            this.active = false;
            this.upKey = upKey;
            this.downKey = downKey;
            this.position = pos;
            int width = 20;
            int height = 100;

            if(pos == position.Left)
            {
                screenPosition = new Rectangle(0, graphicViewport.Height / 2 - height / 2, width, height);
            }
            if (pos == position.Right)
            {
                screenPosition = new Rectangle(graphicViewport.Width-width, graphicViewport.Height / 2 - height / 2, width,height);
            }

        }
        public void addPoints(int points=1)
        {
            this.points += points;
        }
        public Rectangle getPosition()
        {
            return this.screenPosition;
        }
        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenPosition, Color.White);
        }

        public void chechMove(KeyboardState kb)
        {
            if (active)
            {
                if (kb.IsKeyDown(upKey))
                {
                    moveUp();
                }
                if (kb.IsKeyDown(downKey))
                {
                    moveDown();
                }
            }
        }
        public void moveUp()
        {
            int yOffset = -3;
            if (screenPosition.Top + yOffset >= 0)
                screenPosition.Offset(0, yOffset);
        }

        public void moveDown()
        {
            int yOffset = 3;
            if (screenPosition.Bottom + yOffset <= graphicViewport.Height)
                screenPosition.Offset(0, yOffset);
        }
        public void start()
        {
            this.active = true;
        }
        public void end()
        {
            this.active = false;
        }


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

            lPadTexture= Content.Load<Texture2D>(@"paddle1");
            rPadTexture = Content.Load<Texture2D>(@"paddle2");

            lPad = new Paddle(lPadTexture, GraphicsDevice.Viewport, position.Left,Keys.Q, Keys.A);
            rPad = new Paddle(rPadTexture, GraphicsDevice.Viewport, position.Right, Keys.P, Keys.L);

            ball = new Ball(ballTexture, GraphicsDevice.Viewport);
        }

        protected override void Update(GameTime gameTime)
        {
            
            KeyboardState kb = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                Exit();




            if (kb.IsKeyDown(Keys.Space))
            {
                ball.Start();
                rPad.start();
                lPad.start();
            }

            
            ball.Move(lPad,rPad);
            rPad.chechMove(kb);
            lPad.chechMove(kb);

            bool isEnd=ball.isEnd(lPad,rPad);//todo: implement

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            // TODO: Add your drawing code here
            ball.Draw(_spriteBatch);
            lPad.Draw(_spriteBatch);
            rPad.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{

    public class Paddle
    {
        Texture2D texture;
        Side side;
        Rectangle screenPosition;
        int points;
        bool active;


        Keys upKey;
        Keys downKey;
        Viewport graphicViewport;

        public Paddle(Texture2D texture, Viewport graphicViewport, Side side, Keys upKey, Keys downKey)
        {
            this.graphicViewport = graphicViewport;
            this.texture = texture;
            this.points = 0;
            this.active = false;
            this.upKey = upKey;
            this.downKey = downKey;
            this.side = side;
            resetPosition();

        }
        private void resetPosition()
        {
            int width = 5;
            int height = 100;
            int gap = 10;
            if (side == Side.Left)
            {
                screenPosition = new Rectangle(0 + gap, graphicViewport.Height / 2 - height / 2, width, height);
            }
            if (side == Side.Right)
            {
                screenPosition = new Rectangle(graphicViewport.Width - gap - width, graphicViewport.Height / 2 - height / 2, width, height);
            }

        }
        public void Start()
        {
            resetPosition();
            this.active = true;

        }
        public void End()
        {
            this.active = false;
        }

        public int GetPoints()
        {
            return points;
        }
        internal Side GetSide()
        {
            return side;
        }

        public void AddPoints(int points = 1)
        {
            this.points += points;
        }
        public Rectangle GetScreenPosition()
        {
            return this.screenPosition;
        }
        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, screenPosition, Color.White);
        }

        public void CheckMove(KeyboardState kb)
        {
            if (active)
            {
                if (kb.IsKeyDown(upKey))
                    MoveUp();

                if (kb.IsKeyDown(downKey))
                    MoveDown();
            }
        }
        public void MoveUp()
        {
            int yOffset = -3;
            if (screenPosition.Top + yOffset >= 0)
                screenPosition.Offset(0, yOffset);
            else
                screenPosition.Y = 0;
        }

        public void MoveDown()
        {
            int yOffset = 3;
            if (screenPosition.Bottom + yOffset <= graphicViewport.Height)
                screenPosition.Offset(0, yOffset);
            else
                screenPosition.Y = graphicViewport.Height - screenPosition.Height;
        }

    }
}

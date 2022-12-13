using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Seminarie_2.GameObjects
{
    public class Car
    {
        // Float
        float CirclePathRadius;
        float CurrentAngle = MathF.PI * 3 / 2;
        float Speed = 0.01f;

        // Vector2
        Vector2 Pos;
        Vector2 CircleCenter;
        Vector2 FunctionOrigin;

        // Other
        Texture2D Tex;
        Rectangle DestRec;
        Color DrawColor = Color.White;
        bool DrawCirclePath = true;

        public Car(Texture2D tex, Rectangle destRec,Vector2 circleCenter, Vector2 functionOrigin, float circleRadius)
        {
            Tex = tex;
            DestRec = destRec;
            CircleCenter = circleCenter;
            FunctionOrigin = functionOrigin;
            CirclePathRadius = circleRadius;
        }

        public void Update(ProgramState state)
        {
            switch (state)
            {
                case ProgramState.CirclePath:
                    MoveAroundCirlce();
                    break;
                case ProgramState.FunctionPath:
                    MoveFunctionPath();
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D circle)
        {
            if (DrawCirclePath)
            spriteBatch.Draw(circle, new((int)(CircleCenter.X - CirclePathRadius), (int)(CircleCenter.Y - CirclePathRadius), (int)CirclePathRadius * 2, (int)CirclePathRadius * 2), null, Color.Green, 0.0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            spriteBatch.Draw(Tex, DestRec, null, DrawColor,CurrentAngle+ MathF.PI/2, new(Tex.Width / 2, Tex.Height / 2), SpriteEffects.None, 0.6f);
        }

        public void MoveAroundCirlce()
        {
            Pos.X = CircleCenter.X + CirclePathRadius * (float)Math.Cos(CurrentAngle);
            Pos.Y = CircleCenter.Y + CirclePathRadius * (float)Math.Sin(CurrentAngle);
            CurrentAngle += Speed;

            if (CurrentAngle >= MathF.PI * 2)
                CurrentAngle -= MathF.PI * 2;

            SetDestRec();
        }

        public void MoveFunctionPath()
        {
            Pos.X += 1f;
            Pos.Y = 400 + 100 * (float)Math.Sin(Pos.X / 40);
            SetDestRec();
        }

        void SetDestRec()
        {
            DestRec.X = (int)Pos.X;
            DestRec.Y = (int)Pos.Y;
        }
    }
}

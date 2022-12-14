using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Seminarie_2.GameObjects
{
    public class Car
    {
        // Float
        float CirclePathRadius;
        float CurrentAngle = MathF.PI * 3 / 2;
        float DefaultAndgle = MathF.PI * 3 / 2;
        float CircleSpeed = 0.01f;
        float FunctionSpeed = 1.0f;

        // Vector2
        public Vector2 Pos;
        Vector2 CircleCenter;
        Vector2 FunctionOrigin;

        // Bool
        bool DrawCollisionVectors = true;
        bool DrawCirclePath = false;

        // Other
        Texture2D Tex;
        Rectangle DestRec;
        Color DrawColor = Color.White;
        ProgramState LastState;
        int ScreenHeight;

        public Car(Texture2D tex, Rectangle destRec, Vector2 circleCenter, Vector2 functionOrigin, float circleRadius)
        {
            Tex = tex;
            DestRec = destRec;
            CircleCenter = circleCenter;
            FunctionOrigin = functionOrigin;
            CirclePathRadius = circleRadius;
        }

        public void Update(ProgramState state)
        {
            if (LastState != state)
            {
                switch (state)
                {
                    case ProgramState.CirclePath:
                        CurrentAngle = DefaultAndgle;
                        break;
                    case ProgramState.FunctionPath:
                        Pos = FunctionOrigin;
                        CurrentAngle = DefaultAndgle;
                        break;
                }
            }

            switch (state)
            {
                case ProgramState.CirclePath:
                    MoveAroundCirlce();
                    break;
                case ProgramState.FunctionPath:
                    MoveFunctionPath();
                    break;
            }

            LastState = state;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D circle)
        {
            if (DrawCirclePath && LastState == ProgramState.CirclePath)
                spriteBatch.Draw(circle, new((int)(CircleCenter.X - CirclePathRadius), (int)(CircleCenter.Y - CirclePathRadius), (int)CirclePathRadius * 2, (int)CirclePathRadius * 2), null, Color.Green, 0.0f, Vector2.Zero, SpriteEffects.None, 0.6f);
            spriteBatch.Draw(Tex, DestRec, null, DrawColor, CurrentAngle + MathF.PI / 2, new(Tex.Width / 2, Tex.Height / 2), SpriteEffects.None, 0.6f);

            if (DrawCollisionVectors)
            foreach (Vector2 vec in GetCollisionVectors())
            {
                float temp = 0.3f;
                Rectangle tempRec = new((int)(vec.X - DestRec.Width * temp / 2), (int)(vec.Y- DestRec.Height * temp / 2), (int) (DestRec.Width* temp), (int)(DestRec.Height * temp));
                spriteBatch.Draw(Tex, tempRec, null, DrawColor, 0, Vector2.Zero, SpriteEffects.None, 0.6f);
            }
        }

        void MoveAroundCirlce()
        {
            Pos.X = CircleCenter.X + CirclePathRadius * (float)Math.Cos(CurrentAngle);
            Pos.Y = CircleCenter.Y + CirclePathRadius * (float)Math.Sin(CurrentAngle);
            CurrentAngle += CircleSpeed;

            if (CurrentAngle >= MathF.PI * 2)
                CurrentAngle -= MathF.PI * 2;

            SetDestRec();
        }

        void MoveFunctionPath()
        {
            Pos.X += FunctionSpeed;

            if (Pos.X > ScreenHeight)
                Pos.X = 0;

            Pos.Y = 400 + 100 * (float)Math.Sin(Pos.X / 40);
            SetDestRec();
        }

        void SetDestRec()
        {
            DestRec.X = (int)Pos.X/* - DestRec.Width / 2*/;
            DestRec.Y = (int)Pos.Y/* - DestRec.Height / 2*/;
        }

        public void SetScreenHeight(int screenHeight)
        {
            ScreenHeight = screenHeight;
        }

        public Vector2[] GetCollisionVectors()
        {
            Vector2[] vectors = new Vector2[4];

            Vector2 v1 = new(DestRec.X- DestRec.Width / 2, DestRec.Y - DestRec.Height / 2);
            Vector2 v2 = new(DestRec.X - DestRec.Width / 2 + DestRec.Width, DestRec.Y - DestRec.Height / 2);
            Vector2 v3 = new(DestRec.X - DestRec.Width / 2, DestRec.Y - DestRec.Height / 2 + DestRec.Height);
            Vector2 v4 = new(DestRec.X - DestRec.Width / 2 + DestRec.Width, DestRec.Y - DestRec.Height / 2 + DestRec.Height);

            float rotation = CurrentAngle + MathF.PI / 2;

            vectors[0] = Vector2.Transform(v1, Matrix.CreateRotationZ(rotation));
            vectors[1] = Vector2.Transform(v2, Matrix.CreateRotationZ(rotation));
            vectors[2] = Vector2.Transform(v3, Matrix.CreateRotationZ(rotation));
            vectors[3] = Vector2.Transform(v4, Matrix.CreateRotationZ(rotation));

            return vectors;
        }
    }
}

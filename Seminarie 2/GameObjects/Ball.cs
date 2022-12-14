using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminarie_2.GameObjects
{
    public class Ball
    {
        // float
        float Mass = 0.4f;
        float Radius;
        float DragCoeficient = 0.9f;
        float Gravity = 400.0f;

        // Vector2
        public Vector2 Pos;
        Vector2 Vel;
        Vector2 ObjectForce;

        // Other
        Texture2D Tex;
        Rectangle DestRec;
        Color DrawColor = Color.White;
        public bool UseAdvancedPhysics = true;
        public Timer LifeTime;
        Action<Ball> OnCollision;

        public Ball(Texture2D tex, float radius, Vector2 pos)
        {
            Tex = tex;
            Pos = pos;
            Radius = radius;
            Mass = 1.0f;
            SetConstants();
        }

        public Ball(Texture2D tex, float radius, Vector2 pos, Vector2 vel)
        {
            Tex = tex;
            Pos = pos;
            Vel = vel;
            Radius = radius;
            Mass = 1.0f;
            SetConstants();
        }

        public Ball(Texture2D tex, float radius, float mass, Vector2 pos)
        {
            Tex = tex;
            Pos = pos;
            Radius = radius;
            Mass = mass;
            SetConstants();
        }

        public Ball(Texture2D tex, float radius, float mass, Vector2 pos, Vector2 vel)
        {
            Tex = tex;
            Pos = pos;
            Vel = vel;
            Radius = radius;
            Mass = mass;
            SetConstants();
        }

        public void Update(float deltaTime, Car car)
        {
            LifeTime.Update(deltaTime);

            if (UseAdvancedPhysics)
            {
                AddForce(0, Gravity);
                AddForce(-Vel.X * DragCoeficient, 0);

                Vel += ObjectForce / Mass * deltaTime;
                Pos += Vel * deltaTime;

                ObjectForce = Vector2.Zero;
            }

            else
                Pos += Vel;

            UpdateDestRec();

            if (car.GetCollisionVectors().Any(v => Vector2.Distance(Pos, v) <= Radius))
            {
                OnCollision.Invoke(this);
                LifeTime.StartTimer(0);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Tex, DestRec, null, DrawColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.6f);
        }

        void SetConstants()
        {
            DestRec = new((int)(Pos.X - Radius), (int)(Pos.Y - Radius), (int)Radius * 2, (int)Radius * 2);
            LifeTime = new();
            LifeTime.StartTimer(float.PositiveInfinity);
        }

        public void SetColor(Color newColor)
        {
            DrawColor = newColor;
        }

        public void UpdateDestRec()
        {
            DestRec.X = (int)(Pos.X - Radius);
            DestRec.Y = (int)(Pos.Y - Radius);
        }

        /// <summary>
        /// Adds force to player
        /// </summary>
        /// <param name="x">x value of vector</param>
        /// <param name="y">y value of vector</param>
        public void AddForce(float x, float y)
        {
            ObjectForce += new Vector2(x, y);
        }

        /// <summary>
        /// Adds force to player
        /// </summary>
        /// <param name="f">vector force</param>
        public void AddForce(Vector2 f)
        {
            ObjectForce += f;
        }

        public void SetLifeTime(float lifeTime)
        {
            LifeTime.StartTimer(lifeTime);
        }

        public void SetVel(float x, float y)
        {
            Vel = new(x,y);
        }

        public void SetVel(Vector2 f)
        {
            Vel = f;
        }

        public void SetCollisionFunction(Action<Ball> function)
        {
            OnCollision = function;
        }
    }
}

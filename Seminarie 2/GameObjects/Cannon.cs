using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Seminarie_2.GameObjects
{
    public class Cannon
    {
        // Float
        float CurrentAngle;
        float LaunchVel = 800.0f;
        float ProjectileRadius = 10.0f;
        float ProjectileLifeTime = 6.0f;

        // Vector2
        Vector2 DrawOrigin;
        Vector2 LaunchOrigin;
        Vector2 LaunchVector;

        // Texture2D
        Texture2D Tex;
        Texture2D BallTex;

        // Other
        public List<Ball> Projectiles = new();
        Rectangle DestRec;
        Color DrawColor = Color.Black;

        public Cannon(SpriteBatch spriteBatch, Texture2D ballTex,Rectangle destRec, Vector2 drawOrigin, Vector2 launchOrigin,float angle = MathF.PI*2 - MathF.PI/3)
        {
            Tex = new(spriteBatch.GraphicsDevice, 2, 2);
            Tex.SetData(new Color[] {Color.White, Color.White , Color.White , Color.White });
            BallTex = ballTex;
            DestRec = destRec;
            DrawOrigin = drawOrigin;
            LaunchOrigin = launchOrigin;
            CurrentAngle = angle;
        }

        public void Update(float deltaTime)
        {
            for (int x = 0; x < Projectiles.Count; x++)
                Projectiles[x].Update(deltaTime);

            Projectiles.RemoveAll(b => b.LifeTime.IsDone());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Projectiles.Count; x++)
                Projectiles[x].Draw(spriteBatch);

            spriteBatch.Draw(Tex, DestRec, null, DrawColor, CurrentAngle, DrawOrigin, SpriteEffects.None, 0.6f);
        }

        public void Shoot()
        {
            Ball newShot = new(BallTex,ProjectileRadius,LaunchOrigin, LaunchVector * LaunchVel);
            LaunchVector = new(MathF.Cos(CurrentAngle), MathF.Sin(CurrentAngle));

            newShot.SetLifeTime(ProjectileLifeTime);

            Projectiles.Add(newShot);
        }

        public void SetAngle(float newAngle)
        {
            CurrentAngle = newAngle;
        }
    }
}

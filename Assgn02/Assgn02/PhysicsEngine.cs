using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn02
{
    class PhysicsEngine
    {
        float ScreenWidth { get; set; }
        float ScreenHeight { get; set; }

        public PhysicsEngine(float w, float h)
        {
            ScreenHeight = h;
            ScreenWidth = w;
        }

        public void CheckWithScreenBounds(Sprite sprite)
        {
            Vector2 dst = sprite.Position + sprite.Velocity;


            if (sprite.Velocity.X < 0 && dst.X <= 0)
                sprite.CollisionV += new Vector2(1,0);
            if (sprite.Velocity.X > 0 && dst.X + sprite.Texture.Width > ScreenWidth)
                sprite.CollisionV += new Vector2(-1, 0);
            if (sprite.Velocity.Y < 0 && dst.Y <= 0)
                sprite.CollisionV += new Vector2(0, 1);
            if (sprite.Velocity.Y > 0 && dst.Y + sprite.Texture.Height > ScreenHeight)
                sprite.CollisionV += new Vector2(0, -1);
        }

        public void CheckWithEachOther(List<Sprite> sprites, int deltaTime)
        {           

            foreach(Sprite sprite1 in sprites)
            {
                foreach(Sprite sprite2 in sprites)
                {
                    if (sprite1 == sprite2) continue;
                    Vector2 dst1 = sprite1.Position + new Vector2(sprite1.Width/2, sprite1.Height/2) + sprite1.Velocity * deltaTime;
                    Vector2 dst2 = sprite2.Position + new Vector2(sprite2.Width/2, sprite2.Height/2) + sprite2.Velocity * deltaTime;
                    Vector2 dstBetween = dst1 - dst2;

                    if (Math.Abs(dstBetween.Y) <= sprite1.Height/2 + sprite2.Height/2 && Math.Abs(dstBetween.X) <= sprite1.Width/2 + sprite2.Width/2)
                    {
                        sprite1.Collide();
                        sprite1.Velocity = Vector2.Normalize(dstBetween) * sprite1.Velocity.Length();
                    }
                }
            }
             
        }

        public void CheckWithEachOther(Sprite sprite1, List<Sprite> sprites, int deltaTime)
        {
            foreach (Sprite sprite2 in sprites)
            {
                if (sprite1 == sprite2) continue;
                Vector2 dst1 = sprite1.Position + new Vector2(sprite1.Width / 2, sprite1.Height / 2) + sprite1.Velocity * deltaTime;
                Vector2 dst2 = sprite2.Position + new Vector2(sprite2.Width / 2, sprite2.Height / 2) + sprite2.Velocity * deltaTime;
                Vector2 dstBetween = dst1 - dst2;

                if (Math.Abs(dstBetween.Y) <= sprite1.Height / 2 + sprite2.Height / 2 && Math.Abs(dstBetween.X) <= sprite1.Width / 2 + sprite2.Width / 2)
                {
                    sprite2.Collide();
                    sprite1.Velocity = Vector2.Normalize(dstBetween) * sprite1.Velocity.Length();
                }
            }
        }

        public void CheckWithEachOther(Sprite sprite1, Sprite sprite2, int deltaTime)
        {
            Vector2 dst1 = sprite1.Position + new Vector2(sprite1.Width / 2, sprite1.Height / 2) + sprite1.Velocity * deltaTime;
            Vector2 dst2 = sprite2.Position + new Vector2(sprite2.Width / 2, sprite2.Height / 2) + sprite2.Velocity * deltaTime;
            Vector2 dstBetween = dst1 - dst2;

            if (Math.Abs(dstBetween.Y) <= sprite1.Height / 2 + sprite2.Height / 2 && Math.Abs(dstBetween.X) <= sprite1.Width / 2 + sprite2.Width / 2)
            {
                sprite1.Collide();
                sprite1.Velocity = Vector2.Normalize(dstBetween) * sprite1.Velocity.Length();
            }

        }

        public static void SolidCollision(PhysicsTransform sprite)
        {
            Vector2 cV = sprite.CollisionV;



            if (cV.X != 0) sprite.Velocity *= new Vector2(0, 1);
            if (cV.Y != 0) sprite.Velocity *= new Vector2(1, 0);
        }
        public static void BounceCollision(PhysicsTransform sprite)
        {
            Vector2 cV = sprite.CollisionV;

            if (cV.X != 0) sprite.Velocity *= new Vector2(-1, 1);
            if (cV.Y != 0) sprite.Velocity *= new Vector2(1, -1);


        }
    }
}

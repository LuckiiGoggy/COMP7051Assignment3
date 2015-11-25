using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assgn02
{
    public abstract class PhysicsTransform : ICollidable
    {
        public enum PhysicsType { Ghost, Solid, Elastic, Fragile };

        Vector2 position;
        Vector2 velocity;
        Vector2 collisionV;
        PhysicsType physType;

        bool isNonStop;

        public bool IsNonStop
        {
            get { return isNonStop; }
            set { isNonStop = value; }
        }


        public Vector2 Position
        {
            get { return position;  }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 CollisionV
        {
            get { return collisionV; }
            set { collisionV = value; }
        }

        public PhysicsType PhysType
        {
            get { return physType; }
            set { physType = value; }
        }

        public virtual void Move(Vector2 delta)
        {
            velocity += delta;
        }

        public virtual void Update(int timeDelta)
        {

            switch (physType)
            {
                case PhysicsType.Solid:
                    PhysicsEngine.SolidCollision(this);
                    break;
                case PhysicsType.Elastic:
                    PhysicsEngine.BounceCollision(this);
                    break;
                case PhysicsType.Fragile:
                    if(CollisionV != Vector2.Zero) Game1.Remove((Sprite)this);
                    break;
                default:
                    break;
            }


            position += velocity * timeDelta;

            if(!isNonStop) velocity = Vector2.Zero;
            collisionV = Vector2.Zero;
        }

        public virtual void Collide()
        {
            if (physType == PhysicsType.Fragile) Game1.Remove((Sprite)this);
        }
    }
}

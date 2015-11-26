using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.MazeObjects
{
    class Frog: GameObjects.GameObject3D
    {
        protected Vector2 m_MapPos;
        protected Vector2 m_PrevMapPos;
        protected bool isMoving;
        protected float moveDelayTimer = 0;
        protected float moveDelay = float.Epsilon;
        protected float movingTimer = 0;
        protected float movingTime = 1;

        public Frog()
            : base(Game1.ModelLib.Get("DerpyFrog"), Game1.TexLib.Get("eye texture"))
        {
        }

        public Frog(Vector3 startPos, Vector2 MapPos)
            : base(Game1.ModelLib.Get("DerpyFrog"), Game1.TexLib.Get("eye texture"))
        {
            Position = startPos;
            m_MapPos = MapPos;
            m_PrevMapPos = MapPos;
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if(moveDelayTimer >= moveDelay)
            {
                if(movingTimer == 0)
                {
                    //Console.WriteLine("Derp: MovingTimer==0");
                    m_PrevMapPos = m_MapPos;
                    m_MapPos = ChooseNextPos();
                    movingTimer += timeDelta;
                }
                else if(movingTimer >= movingTime)
                {
                    //Console.WriteLine("Derp: movingTimer >= movingTime");
                    movingTimer = 0;
                    moveDelayTimer = 0;
                    Position = new Vector3(2 * m_MapPos.Y, 2 * m_MapPos.X, 0);
                }
                else
                {
                    //Console.WriteLine("Derp: Moving");
                    Vector3 distance = new Vector3(2 * m_MapPos.Y, 2 * m_MapPos.X, 0) - new Vector3(2 * m_PrevMapPos.Y, 2 * m_PrevMapPos.X, 0);
                    Translate(distance * (timeDelta / movingTime));
                    movingTimer += timeDelta;
                }



            }
            else
            {
                moveDelayTimer += timeDelta;

                //Console.WriteLine("Derp: " + moveDelayTimer);
            }
            
            base.Update(gameTime);
        }

        public Vector2 ChooseNextPos()
        {
            Vector2 pos = m_MapPos;
            List<Vector2> adjCells = ((Maze)Game1.view).GetAdjacentAvailableCells(pos);
            if(adjCells.Count > 0)
                pos = ChooseRandomPos(adjCells);

            return pos;
        }

        public Vector2 ChooseRandomPos(List<Vector2> adjCells)
        {
            //Console.WriteLine("Adj Count: " + adjCells.Count);
            int ndex = Game1.rand.Next(adjCells.Count);

            //Console.WriteLine("Rand Result: " + ndex);

            return adjCells[ndex];
        }
    }
}

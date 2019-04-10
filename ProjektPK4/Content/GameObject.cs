using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPK4.Content
{
    class GameObject
    {
        private Rectangle Body;

        public GameObject(int positionX, int positionY, int width, int height)
        {
            Body = new Rectangle(positionX, positionY, width, height);
        }

        public Rectangle GetRectangle()
        {
            return Body;
        }

        public int GetPosX()
        {
            return Body.X;
        }
        
        public int GetPosY()
        {
            return Body.Y;
        }

        public void MoveBody(int posX, int posY)
        {
            Body.X = Body.X+posX;
            Body.Y = Body.Y+posY;
        }

        public bool CheckColisionWidth(int posX, int posXAndBody)
        {
            if (posX > Body.X && posX < Body.X + Body.Width || posXAndBody > Body.X && posXAndBody < Body.X + Body.Width || posX == Body.X && posXAndBody == Body.X + Body.Width)
            {
                return true;
            }
            return false;
        }
        public bool CheckColisionHeight(int posY, int posYAndBody, int shade)
        {
            if (posY > Body.Y && posY < Body.Y + Body.Height-shade || posYAndBody > Body.Y && posYAndBody < Body.Y + Body.Height-shade || posY==Body.Y && posYAndBody == Body.Y+Body.Height-shade)
            {
                return true;
            }
            return false;
        }
    }
}

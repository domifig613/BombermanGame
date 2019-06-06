using Microsoft.Xna.Framework;

namespace ProjektPK4.game
{
    class GameObject
    {
        private Rectangle Body;

        public GameObject(int positionX, int positionY)
        {
            Body = new Rectangle(positionX, positionY, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight+ProgramParameters.AreaYShade);
        }
        public GameObject(int positionX, int positionY, int shade)
        {
            Body = new Rectangle(positionX, positionY, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + shade);
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

        public bool ChceckColision(int posX, int posY)
        {
            if(CheckColisionWidth(posX) && CheckColisionHeight(posY))
            {
                return true;
            }
            return false;
        }

        public bool CheckColisionWidth(int posX)
        {
            if (posX > Body.X && posX < Body.X + Body.Width || posX + ProgramParameters.OneAreaWidth > Body.X && posX + ProgramParameters.OneAreaWidth < Body.X + Body.Width ||
                posX == Body.X && posX + ProgramParameters.OneAreaWidth == Body.X + Body.Width)
            {
                return true;
            }
            return false;
        }
        public bool CheckColisionHeight(int posY)
        {
            if ((posY > Body.Y && posY < Body.Y + Body.Height- ProgramParameters.AreaYShade) ||
                (posY + ProgramParameters.OneAreaHeight > Body.Y && posY + ProgramParameters.OneAreaHeight < Body.Y + Body.Height- ProgramParameters.AreaYShade) ||
                (posY==Body.Y && posY + ProgramParameters.OneAreaHeight == Body.Y+Body.Height- ProgramParameters.AreaYShade))
            {
                return true;
            }
            return false;
        }
    }
}

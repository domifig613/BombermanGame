using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPK4.Content
{
    class Powerups: GameObject
    {
        int indestructible;//cant destroy and pick frame
        int textureNumber = 0;
        int frameWithOneTexture;
        int countTexture=0;
        int type;//1 more bombs, 2 large area bomb, 3 speed, 4 indestructible for some frame

        public Powerups(int _indestructibleTime,int _type, int _frameWithOneTexture, int posX, int posY, int Width, int height):base(posX, posY, Width, height)
        {
            frameWithOneTexture = _frameWithOneTexture;
            indestructible = _indestructibleTime;
            type = _type;
        }
        public void setNextTexture(int max)
        {
            if(countTexture % frameWithOneTexture == 0)
            {
                countTexture = 1;
                if (textureNumber == max - 1)
                {
                    textureNumber = 0;
                }
                else
                {
                    textureNumber++;
                }
            }
            else
            {
                countTexture++;
            }
        }

        public int getTypePowerups()
        {
            return type;
        }

        public int getNumberTexture()
        {
            return textureNumber;
        }

        public int getIndestructible()
        {
            return indestructible;
        }
        public void IndestructibleDecrement()
        {
            if (indestructible > 0)
            {
                indestructible--;
            }
        }
        public void AddPower(Character player)
        {
            switch (type)
            {
                case 1:
                    player.ShortenMaxDelayBettwenPutBomb();
                    break;
                case 2:
                    player.AddBombPower();
                    break;
                case 3:
                    player.AddSpeed();
                    break;
                case 4:
                    break;
            }
        }
    }
}

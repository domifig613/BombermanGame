using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPK4.game
{
    class Powerups: GameObject
    {
        int indestructible;//cant destroy and pick frame
        int textureNumber = 0;
        int FrameWithOneTexture { get; }
        int countTexture=0;
        int Type { get; }//1 more bombs, 2 large area bomb, 3 speed, 4 indestructible for some frame

        public Powerups(int _indestructibleTime,int _type, int _frameWithOneTexture, int posX, int posY):base(posX, posY)
        {
            FrameWithOneTexture = _frameWithOneTexture;
            indestructible = _indestructibleTime;
            Type = _type;
        }
        public void SetNextTexture(int max)
        {
            if(countTexture % FrameWithOneTexture == 0)
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

        public int GetTypePowerups()
        {
            return Type;
        }

        public int GetNumberTexture()
        {
            return textureNumber;
        }

        public int GetIndestructible()
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
            switch (Type)
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

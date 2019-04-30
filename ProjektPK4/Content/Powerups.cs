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
        int type;//1 more bombs, 2 speed, 3 larger area bomb

        public Powerups(int _indestructibleTime,int _type, int posX, int posY, int Width, int height):base(posX, posY, Width, height)
        {
            indestructible = _indestructibleTime;
            type = _type;
        }
        public void setNextTexture()
        {
            if (textureNumber == 17*4)
            {
                textureNumber = 0;
            }
            else
            {
                textureNumber++;
            }
        }

        public int getType()
        {
            return type;
        }

        public int getNumberTexture()
        {
            return textureNumber/4;
        }
    }
}

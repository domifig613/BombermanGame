using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPK4.Content
{
    class Bomb : GameObject
    {
        private int range;
        private int maxTimeToDestroy = 120;
        private int timerToDestroy;//frame
        int textureNumber = 0;
        public Bomb(int posX, int posY, int Width, int height, int _range) : base(posX, posY, Width, height)
        {
            timerToDestroy = maxTimeToDestroy;
            range = _range;
        }
        public void shortenTheTimer()
        {
            timerToDestroy--;
        }

        public int getMaxTime()
        {
            return maxTimeToDestroy;
        }
        public int getTimer()
        {
            return timerToDestroy;
        }

        public void setTimeToZero()
        {
            timerToDestroy = 0;
        }

        public bool checkDestroyTimer()
        {
            if(timerToDestroy <= 0)
            {
                return true;
            }
            return false;
        }

        public void DestroyBombCreateFire(ref List<GameObject> fireList, int Width, int height, int[] distance, bool[] destroyable)
        {
            fireList.Add(new Fire(range, GetPosX(), GetPosY(), Width, height, ref fireList, distance, destroyable));
        }

        public int getTextureNumber()
        {
            return textureNumber;
        }
        public void SetNextTexture()
        {
            textureNumber++;
        }

        ~Bomb()
        {

        }
    }
}

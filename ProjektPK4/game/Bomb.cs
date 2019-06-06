using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPK4.game
{
    class Bomb : GameObject
    {
        readonly private int range;
        readonly private int maxTimeToDestroy = 120;
        private int timerToDestroy;//frame
        int textureNumber = 0;
        public Bomb(int posX, int posY,int _range) : base(posX, posY)
        {
            timerToDestroy = maxTimeToDestroy;
            range = _range;
        }
        public void ShortenTheTimer()
        {
            timerToDestroy--;
        }

        public int GetMaxTime()
        {
            return maxTimeToDestroy;
        }
        public int GetTimer()
        {
            return timerToDestroy;
        }

        public void SetTimeToZero()
        {
            timerToDestroy = 0;
        }

        public bool CheckDestroyTimer()
        {
            if(timerToDestroy <= 0)
            {
                return true;
            }
            return false;
        }

        public void DestroyBombCreateFire(ref List<GameObject> fireList, int Width, int height, int[] distance, bool[] destroyable)
        {
            fireList.Add(new Fire(range, GetPosX(), GetPosY(), ref fireList, distance, destroyable));
        }

        public int GetTextureNumber()
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

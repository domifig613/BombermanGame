using System.Collections.Generic;

namespace ProjektPK4.game
{
    class Bomb : GameObject
    {
        readonly private int Range;
        readonly private int MaxTimeToDestroy = 120;
        private int TimerToDestroy;//frame
        int TextureNumber = 0;
        public Bomb(int posX, int posY,int range) : base(posX, posY)
        {
            TimerToDestroy = MaxTimeToDestroy;
            Range = range;
        }
        public void ShortenTheTimer()
        {
            TimerToDestroy--;
        }

        public int GetMaxTime()
        {
            return MaxTimeToDestroy;
        }
        public int GetTimer()
        {
            return TimerToDestroy;
        }

        public void SetTimeToZero()
        {
            TimerToDestroy = 0;
        }

        public bool CheckDestroyTimer()
        {
            if(TimerToDestroy <= 0)
            {
                return true;
            }
            return false;
        }

        public void DestroyBombCreateFire(ref List<GameObject> fireList, int Width, int height, int[] distance, bool[] destroyable)
        {
            fireList.Add(new Fire(Range, GetPosX(), GetPosY(), ref fireList, distance, destroyable));
        }

        public int GetTextureNumber()
        {
            return TextureNumber;
        }
        public void SetNextTexture()
        {
            TextureNumber++;
        }

        ~Bomb()
        {

        }
    }
}

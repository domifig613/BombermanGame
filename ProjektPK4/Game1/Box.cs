using System;

namespace ProjektPK4.game
{
    class Box : GameObject
    {
        static Random Rnd = new Random();
        readonly int ChanceDropPowerups;//percent 
        public Box(int positionX, int positionY,int chanceDrop) : base(positionX, positionY) {
            ChanceDropPowerups = chanceDrop;
        }

        public int GetChanceToDrop()
        {
            return ChanceDropPowerups;
        }

        public Powerups TrySpawnPowerup()
        {
            int type;
            if (Rnd.Next(1, 10) == 2)
            {
                type = 4;
            }
            else
            {
                type = Rnd.Next(1, 4);
            }
            int speedTexture;
            if(type == 1 || type == 3)
            {
                speedTexture = 4;
            }
            else
            {
                speedTexture = 5;
            }
            return new Powerups(20, type,speedTexture, GetPosX(), GetPosY());

        }

        public int RandomDrop()
        {
            return Rnd.Next(100);
        }
    }
}

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
    class Box : GameObject
    {
        static Random rand = new Random();
        int chanceDropPowerups;//percent 
        public Box(int positionX, int positionY, int widht, int height, int chanceDrop) : base(positionX, positionY, widht, height) {
            chanceDropPowerups = chanceDrop;
        }

        public int GetChanceToDrop()
        {
            return chanceDropPowerups;
        }

        public Powerups TrySpawnPowerup()
        {
            int type;
            if (rand.Next(1, 10) == 2)
            {
                type = rand.Next(1, 5);
            }
            else
            {
                type = rand.Next(1, 4);
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
            return new Powerups(20, type,speedTexture, GetPosX(), GetPosY(), GetRectangle().Width, GetRectangle().Height);

        }

        public int randomDrop()
        {
            return rand.Next(100);
        }
    }
}

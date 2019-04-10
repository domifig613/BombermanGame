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
    class Box :  GameObject
    {
        float chanceDropPowerups;
        public Box(int positionX, int positionY,int widht,int height ,float chanceDrop) : base(positionX, positionY,widht,height) {
            chanceDropPowerups = chanceDrop;
        }

        public float GetChanceToDrop()
        {
            return chanceDropPowerups;
        }
    }
}

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
    class Player : Character
    {
        Keys[] MoveKeys = { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.C};
        
        public Player(int posX, int posY,int width,int height ,int speedTexture) : base(posX, posY,width,height ,speedTexture) {}

        public Keys GetKey(int control) //0=Up, 1=Down, 2=Left, 3=Right
        {
            return MoveKeys[control];
        }


    }
}

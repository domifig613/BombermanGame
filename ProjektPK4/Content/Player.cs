using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPK4.game
{
    class Player : Character
    {
        readonly Keys[] MoveKeys = new Keys[5];
        
        public Player(int posX, int posY, Keys up, Keys down, Keys left, Keys right, Keys bomb) : base(posX, posY)
        {
            MoveKeys[0] = up;
            MoveKeys[1] = down;
            MoveKeys[2] = left;
            MoveKeys[3] = right;
            MoveKeys[4] = bomb;
        }

        public Keys GetKey(int control) //0=Up, 1=Down, 2=Left, 3=Right
        {
            return MoveKeys[control];
        }
    }
}

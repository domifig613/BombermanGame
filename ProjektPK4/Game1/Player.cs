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
        private readonly Keys[] MoveKeys = new Keys[5];
        
        public Player(int posX, int posY, Keys[] keys ,int playerNumber) : base(posX, posY, playerNumber)
        {
           for(int i=0; i<keys.Length; i++)
           {
                MoveKeys[i] = keys[i];
           }
        }

        public Keys GetKey(int control) //0=Up, 1=Down, 2=Left, 3=Right
        {
            return MoveKeys[control];
        }

        public void CheckMove(GameObject[][] Objects)
        {
            if (Keyboard.GetState().IsKeyDown(GetKey(0)))//up
            {
                CheckMoveUp(0, Objects);
            }
            else if (Keyboard.GetState().IsKeyDown(GetKey(1)))//down
            {
                CheckMoveDown(0, Objects);
            }
            else if (Keyboard.GetState().IsKeyDown(GetKey(2)))//left
            {
                CheckMoveLeft(0, Objects);
            }
            else if (Keyboard.GetState().IsKeyDown(GetKey(3)))//right
            {
                CheckMoveRight(0, Objects);
            }
        }
    }
}

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
    abstract class Character:GameObject
    {
        protected bool MoveControl = false;//for loading good texture
        readonly private Texture2D[,] CharacterTextures;// [3,4]   x:0 Up x:1 Down x:2 Left x: 3 Right
        protected int ControlTextures = 3;
        protected int SpeedTexture=0;
        protected int MaxTextureSpeed;
        protected int DelayBettwenPutBomb;//frame
        readonly private int MaxDelayBettwenPutBomb = 120;
        public int ActuallDelayBomb { get; set; } = 0;//frame
        public int Speed { get; set; } = 3;
        public int BombPower = 2;
        

        public Character(int posX, int posY) : base(posX, posY) {
            CharacterTextures = new Texture2D[3, 4];
            MaxTextureSpeed = ProgramParameters.CharacterSlowerAnimation;
            DelayBettwenPutBomb = MaxDelayBettwenPutBomb;
        }
        //{ bomb
        public Bomb PutBomb(int Width, int height, int shade)
        { 
            ActuallDelayBomb = DelayBettwenPutBomb;
            
            int BombPosX;
            int BombPosY;
            if (GetPosX() % Width >= Width/2)
            {
                BombPosX = GetPosX() + Width - (GetPosX() % Width);
            }
            else
            {
                BombPosX = GetPosX() - (GetPosX() % Width);
            }
            if (GetPosY() % height > height/2)
            {
                BombPosY = GetPosY() + height - GetPosY() % height;
            }
            else
            {
                BombPosY = GetPosY() - GetPosY() % height;
            }

            return new Bomb(BombPosX, BombPosY, BombPower);
        }
        public void ShortenTheDelay()
        {
            if (ActuallDelayBomb > 0)
            {
                ActuallDelayBomb--;
            }
        }
        //}
        public void SetTexture(Texture2D texture, int x, int y)
        {
            CharacterTextures[x, y] = texture;
        }

        public Texture2D GetTexture()
        {
            return CharacterTextures[ControlTextures%3,ControlTextures/3];
        }

        public void SetControlTextures(int control)
        {
            ControlTextures = control;
        }

        public void MoveCharacter(int wayX, int wayY, int number)//way: 0 up, 1 down, 2 left, 3 right
        {
            MoveBody(wayX, wayY);
            ChangeControlTexture(number);
        }

        private void ChangeControlTexture(int number)
        {
            if (ControlTextures >= number && ControlTextures <= number + 2)
            {
                if (SpeedTexture == MaxTextureSpeed)
                {
                    SpeedTexture = 0;
                    if (ControlTextures == number)
                    {
                        if (MoveControl)
                        {
                            ControlTextures = number + 1;
                        }
                        else
                        {
                            ControlTextures = number + 2;
                        }
                        return;
                    }
                    else if (ControlTextures == number + 1 || ControlTextures == number + 2)
                    {
                        MoveControl = !MoveControl;
                    }
                    ControlTextures = number;
                }
                else
                {
                    SpeedTexture++;
                }
            }
            else
            {
                ControlTextures = number;
            }
        }

        public void AddBombPower()
        {
            BombPower++;
        }
        public void AddSpeed()
        {
            if (Speed <= 8)
            {
                Speed++;
            }
        }
        public void ShortenMaxDelayBettwenPutBomb()
        {
            int count = MaxDelayBettwenPutBomb / DelayBettwenPutBomb;
            count++;
            DelayBettwenPutBomb = MaxDelayBettwenPutBomb / count;
        }

    }
}

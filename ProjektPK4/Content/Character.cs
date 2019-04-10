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
    class Character:GameObject
    {
        protected bool MoveControl = false;//for loading good texture
        private Texture2D[,] CharacterTextures;// [3,4]   x:0 Up x:1 Down x:2 Left x: 3 Right
        protected int controlTextures = 3;
        protected int SpeedTexture=0;
        protected int MaxTextureSpeed;
        public int Speed { get; } = 5;

        public Character(int posX, int posY,int width,int height ,int TexturSpeed) : base(posX, posY, width,height) {
            CharacterTextures = new Texture2D[3, 4];
            MaxTextureSpeed = TexturSpeed;
        }

        public void SetTexture(Texture2D texture, int x, int y)
        {
            CharacterTextures[x, y] = texture;
        }

        public Texture2D GetTexture()
        {
            return CharacterTextures[controlTextures%3,controlTextures/3];
        }


        public void SetControlTextures(int control)
        {
            controlTextures = control;
        }

        public void MoveCharacter(int wayX, int wayY, int number)//way: 0 up, 1 down, 2 left, 3 right
        {
            MoveBody(wayX, wayY);
            ChangeControlTexture(number);
        }


        private void ChangeControlTexture(int number)
        {
            if (controlTextures >= number && controlTextures <= number + 2)
            {
                if (SpeedTexture == MaxTextureSpeed)
                {
                    SpeedTexture = 0;
                    if (controlTextures == number)
                    {
                        if (MoveControl)
                        {
                            controlTextures = number + 1;
                        }
                        else
                        {
                            controlTextures = number + 2;
                        }
                        return;
                    }
                    else if (controlTextures == number + 1 || controlTextures == number + 2)
                    {
                        MoveControl = !MoveControl;
                    }
                    controlTextures = number;
                }
                else
                {
                    SpeedTexture++;
                }
            }
            else
            {
                controlTextures = number;
            }
        }
    }
}

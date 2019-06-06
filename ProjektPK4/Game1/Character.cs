using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ProjektPK4.game
{
    abstract class Character : GameObject
    {
        protected bool MoveControl = false;//for loading good texture
        readonly private Texture2D[,] CharacterTextures;// [3,4]   x:0 Up x:1 Down x:2 Left x: 3 Right
        protected int ControlTextures = 3;
        protected int SpeedTexture = 0;
        protected int MaxTextureSpeed;
        protected int DelayBettwenPutBomb;//frame
        readonly private int MaxDelayBettwenPutBomb = 120;
        private List<int> BombsDelay = new List<int>();
        private int Speed = 3;
        private int BombPower = 2;
        readonly public int PlayerNumber;
        private int Indestructible = 0;//frame, if > 0 is indestructible
        readonly static private Texture2D[,] GhostTextures = new Texture2D[3,4];

        public Character(int posX, int posY, int _playerNumber) : base(posX, posY)
        {
            CharacterTextures = new Texture2D[3, 4];
            MaxTextureSpeed = ProgramParameters.CharacterSlowerAnimation;
            DelayBettwenPutBomb = MaxDelayBettwenPutBomb;
            PlayerNumber = _playerNumber;
            AddOneBomb();
        }
        //{ bomb
        public Bomb PutBomb(int Width, int height, int shade)
        {
            for (int i = BombsDelay.Count - 1; i >= 0; i--)
            {
                if (BombsDelay[i] == 0)
                {
                    BombsDelay[i] = MaxDelayBettwenPutBomb;
                    break;
                }
            }

            int BombPosX;
            int BombPosY;
            if (GetPosX() % Width >= Width / 2)
            {
                BombPosX = GetPosX() + Width - (GetPosX() % Width);
            }
            else
            {
                BombPosX = GetPosX() - (GetPosX() % Width);
            }
            if (GetPosY() % height > height / 2)
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
            for(int i=BombsDelay.Count-1; i>=0; i--)
            {
                if(BombsDelay[i]>0)
                {
                    BombsDelay[i]--;
                }
            }

            if(Indestructible>0)
            {
                Indestructible--;
            }
        }
        //}
        public void SetTexture(Texture2D texture, int x, int y)
        {
            CharacterTextures[x, y] = texture;
        }

        public static void SetGhostTexture(Texture2D texture)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if(GhostTextures[j,k]==null)
                    {
                        GhostTextures[j, k] = texture;
                        return;
                    }
                }
            }
        }

        public Texture2D GetTexture()
        {   
            if(Indestructible >=100 || Indestructible <=90 && Indestructible >=60 || Indestructible <= 40 && Indestructible >= 20)
            {
                return GhostTextures[ControlTextures % 3, ControlTextures / 3];
            }

            return CharacterTextures[ControlTextures % 3, ControlTextures / 3];
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
            if (Speed <= 6)
            {
                Speed++;
            }
        }
        public void AddOneBomb()
        {
            BombsDelay.Add(0);
        }

        public int GetSpeed()
        {
            return Speed;
        }

        public bool ChceckBombDelay()
        {
            foreach (int bomb1 in BombsDelay)
            {
                if (bomb1 >= MaxDelayBettwenPutBomb - 10)
                {
                    return false;
                }
            }
            foreach (int bomb1 in BombsDelay)
            {
                if(bomb1 == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsIndestructible()
        {
            if(Indestructible >0)
            {
                return true;
            }
            return false;
        }

        public void SetIndestuctible(int frames)
        {
            Indestructible += frames;
        }

        protected void CheckMoveUp(int control, GameObject[][] ObjectsPosition)
        {
            int distance = GetPosY() - (ObjectsPosition[0][2].GetPosY() + ProgramParameters.OneAreaHeight);

            if (control != 0 &&
               (GetPosY() - GetSpeed() + ProgramParameters.OneAreaHeight <= ObjectsPosition[0][0].GetPosY() 
                || GetPosY() - GetSpeed() + ProgramParameters.OneAreaHeight <= ObjectsPosition[0][1].GetPosY()))
            {
                MoveCharacter(0, -1, 3);
            }
            else if (distance > GetSpeed())
            {
                MoveCharacter(0, -GetSpeed(), 0);
            }
            else if (distance > 0)
            {
                MoveCharacter(0, -distance, 0);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(3, true, ObjectsPosition);
            }
        }
        protected void CheckMoveDown(int control, GameObject[][] ObjectsPosition)
        {
            int distance = ObjectsPosition[0][3].GetPosY() - (GetPosY() + ProgramParameters.OneAreaHeight);

            if (control != 0 &&
                (GetPosY() + GetSpeed() - ProgramParameters.OneAreaHeight >= ObjectsPosition[0][0].GetPosY() 
                || GetPosY() + GetSpeed() - ProgramParameters.OneAreaHeight >= ObjectsPosition[0][1].GetPosY()))
            {
                MoveCharacter(0, 1, 3);
            }
            else if (distance > GetSpeed())
            {
                MoveCharacter(0, GetSpeed(), 3);
            }
            else if (distance > 0)
            {
                MoveCharacter(0, distance, 3);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(4, true,ObjectsPosition);
            }
        }
        protected void CheckMoveLeft(int control,GameObject[][] ObjectsPosition)
        {
            int distance = GetPosX() - (ObjectsPosition[0][0].GetPosX() + ProgramParameters.OneAreaWidth);

            if (control != 0 &&
                (GetPosX() - GetSpeed() + ProgramParameters.OneAreaWidth < ObjectsPosition[0][2].GetPosX()
                || GetPosX() - GetSpeed() + ProgramParameters.OneAreaWidth < ObjectsPosition[0][3].GetPosX()))
            {
                MoveCharacter(-1, 0, 3);
            }
            else if (distance > GetSpeed())
            {
                MoveCharacter(-GetSpeed(), 0, 6);
            }
            else if (distance > 0)
            {
                MoveCharacter(-distance, 0, 6);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(1, false, ObjectsPosition);
            }
        }
        protected void CheckMoveRight(int control, GameObject[][] ObjectsPosition)
        {
            int distance = ObjectsPosition[0][1].GetPosX() - (GetPosX() + ProgramParameters.OneAreaWidth);

            if (control != 0 &&
                (GetPosX() + GetSpeed() - ProgramParameters.OneAreaWidth > ObjectsPosition[0][2].GetPosX()
                || GetPosX() + GetSpeed() - ProgramParameters.OneAreaWidth > ObjectsPosition[0][3].GetPosX()))
            {
                MoveCharacter(1, 0, 3);
            }
            else if (distance > GetSpeed())
            {
                MoveCharacter(GetSpeed(), 0, 9);
            }
            else if (distance > 0)
            {
                MoveCharacter(distance, 0, 9);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(2, false, ObjectsPosition);
            }
        }

        private void MoveInAnotherDirection(int closerObject, bool UpOrDownTrue, GameObject[][] ObjectsPosition)
        {
            if (UpOrDownTrue)
            {
                int position = GetPosX() - ObjectsPosition[0][closerObject-1].GetPosX();
                if ((position < -ProgramParameters.OneAreaWidth / 3 || (this is AI && position < -ProgramParameters.OneAreaWidth)) && ObjectsPosition[closerObject][0].GetPosX() + ProgramParameters.OneAreaWidth < ObjectsPosition[0][closerObject - 1].GetPosX())
                {
                    CheckMoveLeft(1, ObjectsPosition);
                }
                else if ((position > ProgramParameters.OneAreaWidth / 3 || (this is AI && position > ProgramParameters.OneAreaWidth)) && ObjectsPosition[closerObject][1].GetPosX() - ProgramParameters.OneAreaWidth > ObjectsPosition[0][closerObject - 1].GetPosX())
                {
                    CheckMoveRight(1, ObjectsPosition);
                }
            }
            else
            {
                int position = GetPosY() - ObjectsPosition[0][closerObject - 1].GetPosY();
                if ((position < -ProgramParameters.OneAreaHeight / 3 ||( this is AI && position < -ProgramParameters.OneAreaHeight)) && ObjectsPosition[closerObject][2].GetPosY() + ProgramParameters.OneAreaHeight < ObjectsPosition[0][closerObject - 1].GetPosY())
                {
                    CheckMoveUp(1, ObjectsPosition);
                }
                else if ((position > ProgramParameters.OneAreaHeight / 3 || (this is AI && position > ProgramParameters.OneAreaHeight)) && ObjectsPosition[closerObject][3].GetPosY() - ProgramParameters.OneAreaHeight > ObjectsPosition[0][closerObject - 1].GetPosY())
                {
                    CheckMoveDown(1, ObjectsPosition);
                }
            }
        }
    }
}

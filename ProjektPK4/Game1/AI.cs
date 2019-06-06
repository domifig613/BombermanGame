using System;
using System.Linq;

namespace ProjektPK4.game
{
    class AI : Character
    {
        private int Delay = 0;
        private int LastMove = 6; //0 stay, 1 up, 2 down, 3 left, 4 right, 5 bomb
        private int OldLastMove, OldPosX, OldPosY;
        private int Life = 3;//0 == dead
        static Random Rnd = new Random();

        public AI(int posX, int posY, int characterNumber) : base(posX, posY, characterNumber)
        {
            OldLastMove = LastMove;
            OldPosX = GetPosX();
            OldPosY = GetPosY();
        }

        public void SetLife(int lifeToLeft)
        {
            Life = lifeToLeft;
        }

        public int GetLife()
        {
            return Life;
        }
        public void GetDamage()
        {
            Life--;
            SetIndestuctible(300);
        }

        public void CheckMove(char[][] objects, GameObject[][] objectToColision)
        {
            if (Delay <= 0)
            {
                //here AI can change only lastMove variable
                SetDecision(objects);
            }
            else
            {
                Delay--;
            }

            GoAhead(objectToColision);//last move use here
            CheckAIMove();

            OldLastMove = LastMove;
            OldPosX = GetPosX();
            OldPosY = GetPosY();
        }

        private void GoAhead(GameObject[][] Objects)
        {
            switch (LastMove)
            {
                case 1:
                    CheckMoveUp(0, Objects);
                    break;
                case 2:
                    CheckMoveDown(0, Objects);
                    break;
                case 3:
                    CheckMoveLeft(0, Objects);
                    break;
                case 4:
                    CheckMoveRight(0, Objects);
                    break;
                default:
                    break;
            }

        }

        private void CheckAIMove()
        {
            if (LastMove == 6 || (OldLastMove == LastMove && LastMove != 0 && !CheckMove()))
            {
                LastMove = Rnd.Next(1, 5);
            }
        }

        private bool CheckMove()
        {
            if(OldPosX != GetPosX() || OldPosY != GetPosY())
            {
                return true;
            }
            return false;
        }

        public int GetLastMove()
        {
            return LastMove;
        }

        private void SetDecision(char[][] Objects)
        {
            for(int i=0; i<Objects.Length-1; i++)//try find this ai in objects map
            {
                for(int j=0; j<Objects[i].Length-1; j++)
                {
                    if(GetPosX() / ProgramParameters.OneAreaWidth == j
                       && GetPosY() / ProgramParameters.OneAreaHeight == i)
                    {
                        if (Objects[i][j] == 'C')
                        {
                            Objects[i][j] = '1';
                        }
                    }
                }
            }

            if (FindBomb(Objects, GetPosX() / ProgramParameters.OneAreaWidth, GetPosY() / ProgramParameters.OneAreaHeight))
                return;

            if (LastMove == 0)//if stay and any bomb in area better go
            {
                LastMove = Rnd.Next(1, 5);
                return;
            }

            if (TryPutBomb(Objects))
                return;

            if (GetPosY() % ProgramParameters.OneAreaHeight == 0 && GetPosX() % ProgramParameters.OneAreaWidth == 0)//turn in another way sometimes
            {
                if (Rnd.Next(1, 4) == 1)
                {
                    if (LastMove == 1 || LastMove == 2)
                        LastMove = Rnd.Next(3, 5);
                    else if (LastMove == 3 || LastMove == 4)
                        LastMove = Rnd.Next(1, 3);
                }
            }
            else
            {
                TryFindAnotherPlayer(Objects);
            }
        }

        private bool TryPutBomb(char [][] objects)
        {
            for(int i=0; i < objects.Length-1; i++)
            {
                for(int j=0; j<objects[i].Length-1; j++)
                {
                    if (objects[i][j] == '1' && GetPosY() % ProgramParameters.OneAreaHeight <= 10 && GetPosX() % ProgramParameters.OneAreaWidth <= 10)
                    {
                        if (objects[i + 1][j] == 'X' || objects[i - 1][j] == 'X' || objects[i][j + 1] == 'X' || objects[i][j - 1] == 'X'
                           || objects[i + 1][j] == 'C' || objects[i - 1][j] == 'C' || objects[i][j + 1] == 'C' || objects[i][j - 1] == 'C')
                        {
                            Delay = 1;
                            LastMove = 5;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool FindBomb(char[][] Objects, int x, int y)
        {

            if(Objects[y][x] == 'B')//if ai in bomb
            {
                int[] way = new int[4];//up down left right
                FindLongestWay(Objects, GetPosX() / ProgramParameters.OneAreaWidth, GetPosY() / ProgramParameters.OneAreaHeight, ref way);
                int maxValue = way.Max();
                LastMove = way.ToList().IndexOf(maxValue) + 1;
                Delay = 20;
                return true;
            }

            int range = 3;//robot is trying to find a bomb around him, range == count of block
            int lowX = x - range;
            int maxX = x + range;
            int lowY = y - range;
            int maxY = y + range;
            if (lowX < 0)
                lowX = 0;
            if (maxX > (ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth) - 1)
                maxX = (ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth) - 1;
            if (lowY < 0)
                lowY = 0;
            if (maxY > (ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight) - 1)
                maxY = (ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight) - 1;

            for (int i = lowY; i <= maxY; i++)
            {
                for (int j = lowX; j <= maxX; j++)
                {
                    if (Objects[i][j] == 'B')//if bomb in range 2x2
                    {
                        if (((y != i && y + 1 != i) || (y != i && GetPosY() % ProgramParameters.OneAreaHeight==0)) 
                            && ((x != j && x + 1 != j)||(x != j && GetPosX() % ProgramParameters.OneAreaWidth==0)))
                            //if ai is in safe position just stay a few frames
                        {
                            Delay = 20;
                            LastMove = 0;
                            return true;
                        }
                        else//try go away from bomb to safe place
                        {
                            int[] way = new int[4];//up down left right
                            FindLongestWay(Objects, GetPosX() / ProgramParameters.OneAreaWidth, GetPosY() / ProgramParameters.OneAreaHeight, ref way);
                            if ((x == j || (x + 1 == j && x % ProgramParameters.OneAreaWidth != 0)) && y > i)//go left, right or down
                            {
                                if (way[2] > way[3])
                                {
                                    LastMove = 3;
                                }
                                else if (way[2] < way[3])
                                {
                                    LastMove = 4;
                                }
                                else if (way[2] == way[3] && way[2] == 0)
                                {
                                    LastMove = 2;
                                }
                                else
                                {
                                    LastMove = Rnd.Next(3, 5);
                                }
                            }
                            else if ((x == j || (x + 1 == j && x % ProgramParameters.OneAreaWidth != 0)) && y < i)//go left, right or up
                            {
                                if (way[2] > way[3])
                                {
                                    if (LastMove != 3)
                                        LastMove = 3;
                                    else
                                        LastMove = 1;
                                }
                                else if (way[2] < way[3])
                                {
                                    if (LastMove != 4)
                                        LastMove = 4;
                                    else
                                        LastMove = 1;
                                }
                                else if (way[2] == way[3] && way[2] == 0)
                                {
                                    LastMove = 1;
                                }
                                else
                                {
                                    if (LastMove != 3 && LastMove != 4)
                                        LastMove = Rnd.Next(3, 5);
                                    else
                                        LastMove = 1;
                                }
                            }
                            else if (x > j && (y == i || (y + 1 == i && y % ProgramParameters.OneAreaHeight != 0)))//up down or right
                            {
                                if (way[0] > way[1])
                                {
                                    LastMove = 1;
                                }
                                else if (way[0] < way[1])
                                {
                                    LastMove = 2;
                                }
                                else if (way[0] == way[1] && way[0] == 0)
                                {
                                    LastMove = 4;
                                }
                                else
                                {
                                    LastMove = Rnd.Next(1, 3);
                                }
                            }
                            else if (x < j && (y == i || (y + 1 == i && y % ProgramParameters.OneAreaHeight != 0)))//up down or left
                            {
                                if (way[0] > way[1])
                                {
                                    if (LastMove == 1)
                                    {
                                        LastMove = 3;
                                    }
                                    else
                                    {
                                        LastMove = 1;
                                    }
                                }
                                else if (way[0] < way[1])
                                {
                                    if (LastMove != 2)
                                        LastMove = 2;
                                    else
                                        LastMove = 3;
                                }
                                else if (way[0] == way[1] && way[0] == 0)
                                {
                                    LastMove = 3;
                                }
                                else
                                {
                                    if (LastMove != 1 && LastMove != 2)
                                        LastMove = Rnd.Next(1, 3);
                                    else
                                        LastMove = 3;
                                }
                            }
                            Delay = 30;
                            return true;
                        }
                    }
                    else if (Objects[i][j] == 'F')//if fire in range
                    {
                        Delay = 20;
                        LastMove = 0; // stay and wait frames==delay and check again
                        return true;
                    }
                }
            }
            return false;
        }

        private void TryFindAnotherPlayer(char[][] Objects)
        {
            int pointsToAnotherCharacter = 100;
            int x = GetPosX() / ProgramParameters.OneAreaWidth;
            int y = GetPosY() / ProgramParameters.OneAreaHeight;

            int iTrackingCharacter = 0;
            int jTrackingCharacter = 0;

            if (x % ProgramParameters.OneAreaWidth > ProgramParameters.OneAreaWidth / 2)
                x++;
            if (y % ProgramParameters.OneAreaHeight > ProgramParameters.OneAreaHeight / 2)
                y++;

            for(int i=0; i<Objects.Length-1; i++)
            {
                for(int j=0; j<Objects[i].Length-1; j++)
                {
                    if(Objects[i][j] == 'C')
                    {
                        if(pointsToAnotherCharacter> Math.Abs(i-y) + Math.Abs(j-x))
                        {
                            iTrackingCharacter = i;
                            jTrackingCharacter = j;
                            pointsToAnotherCharacter = Math.Abs(i - y) + Math.Abs(j - x);
                        }
                    }
                }
            }

            if (Rnd.Next(1, 3) == 1)
                if (iTrackingCharacter > y)
                    LastMove = 2;
                else
                    LastMove = 1;
            else
                if (jTrackingCharacter > x)
                    LastMove = 4;
                else
                    LastMove = 3;

            Delay = 10;
        }

        private void FindLongestWay(char[][] Objects, int x, int y,ref int[] way)
        {
            int up = 1, down = 1, left = 1, right = 1;

            while (Objects[y - up][x] == 'E' || Objects[y - up][x] == 'P')
            {
                if (Objects[y - up - 1][x] == 'B')
                {
                    way[0] = 0;
                    break;
                }
                way[0]++;
                up++;
            }
            while (Objects[y + down][x] == 'E' || Objects[y + down][x] == 'P')
            {
                if (Objects[y + down + 1][x] == 'B')
                {
                    way[1] = 0;
                    break;
                }
                way[1]++;
                down++;      
            }
            while (Objects[y][x - left] == 'E' || Objects[y][x - left] == 'P')
            {
                if (Objects[y][x - left -1] == 'B')
                {
                    way[2] = 0;
                    break;
                }
                way[2]++;
                left++;
            }
            while (Objects[y][x + right] == 'E' || Objects[y][x + right] == 'P')
            {
                if (Objects[y][x + right + 1] == 'B')
                {
                    way[3] = 0;
                    break;
                }
                way[3]++;
                right++;
            }
        }   
    }
}

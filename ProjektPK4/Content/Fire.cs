using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPK4.Content
{
    class Fire : GameObject
    {
        int TimeToEndFire;//frame
        int MaxTimeToEndFire = 20;//frame
        int TextureNumber;

        public Fire(int power, int positionX, int positionY, int Width, int height, ref List<GameObject> fireList,
            int[] Distance, bool[] Destroyable) : base(positionX, positionY, Width, height)//use in bomb, middle fire
        {
            TimeToEndFire = MaxTimeToEndFire;
            FindGoodTextureNumberForStart(Distance, Destroyable, Width,height);

            if (Distance[0] > height || Destroyable[0] == true)
            {
                fireList.Add(new Fire(power - 1, 1, positionX, positionY - height, Width, height, Distance[0]-height, Destroyable[0], ref fireList));
            }
            if (Distance[1] > height || Destroyable[1] == true)
            {
                fireList.Add(new Fire(power - 1, 2, positionX, positionY + height, Width, height, Distance[1] - height, Destroyable[1], ref fireList));
            }
            if (Distance[2] > Width || Destroyable[2] == true)
            {
                fireList.Add(new Fire(power - 1, 3, positionX - Width, positionY, Width, height, Distance[2] - Width, Destroyable[2], ref fireList));
            }
            if (Distance[3] > Width || Destroyable[3] == true)
            {
                fireList.Add(new Fire(power - 1, 4, positionX + Width, positionY , Width, height, Distance[3] - Width, Destroyable[3], ref fireList));
            }
        }

        public Fire(int power, int FireDirection, int positionX, int positionY, int Width, int height, int distance, bool destroyable,
           ref List<GameObject> fireList) : base(positionX, positionY, Width, height)//use in first fire constructor or recursion
        {
            TimeToEndFire = MaxTimeToEndFire;
            if (power == 0 || distance <= 0 || distance == Width && destroyable == false)
            {
                switch (FireDirection)
                {
                    case 1:
                        TextureNumber = 3;
                        break;
                    case 2:
                        TextureNumber = 4;
                        break;
                    case 3:
                        TextureNumber = 5;
                        break;
                    case 4:
                        TextureNumber = 6;
                        break;
                }
            }
            else
            {
                switch (FireDirection)
                {
                    case 1:
                        positionY -= height;
                        distance -= height;
                        TextureNumber = 2;
                        break;
                    case 2:
                        positionY += height;
                        distance -= height;
                        TextureNumber = 2;
                        break;
                    case 3:
                        positionX -= Width;
                        distance -= Width;
                        TextureNumber = 1;
                        break;
                    case 4:
                        positionX += Width;
                        distance -= Width;
                        TextureNumber = 1;
                        break;
                }

                fireList.Add(new Fire(power - 1, FireDirection, positionX, positionY, Width, height, distance, destroyable, ref fireList));
            }
        }//fire direction 1 up, 2 down, 3 left, 4 right

        public Fire(int positionX, int positionY, int Width, int height) :base(positionX, positionY, Width, height)
        {
            TextureNumber = 15;
            TimeToEndFire = MaxTimeToEndFire;
        }


        public void BurnObject(ref List<GameObject> fireList, int shade)
        {
            for (int i = fireList.Count - 1; i >= 0; i--)
            {
                if (MaxTimeToEndFire-1 == TimeToEndFire && fireList[i].GetPosX() == GetPosX() && fireList[i].GetPosY() ==GetPosY() && !(fireList[i] is Fire))
                { 
                    if (fireList[i] is Box || fireList[i] is Bomb || fireList[i] is Character || fireList[i] is Powerups)
                    {
                        if(fireList[i] is Box)
                        {
                            Box box1 = (Box)fireList[i];
                            if (box1.randomDrop() <= box1.GetChanceToDrop())
                            {
                                fireList.Add(box1.TrySpawnPowerup());
                            }
                                fireList.Remove(box1);
                            
                        }
                        else if (fireList[i] is Bomb)
                        {
                            Bomb bomb1 = (Bomb)fireList[i];
                            bomb1.setTimeToZero();
                        }
                        else if (fireList[i] is Character)
                        {
                            fireList.Add(new Fire(fireList[i].GetPosX(), fireList[i].GetPosY(), fireList[i].GetRectangle().Width, fireList[i].GetRectangle().Height));
                            fireList.Remove(fireList[i]);
                        }
                        else if( fireList[i] is Powerups)
                        {
                            Powerups powerups1 = (Powerups)fireList[i];
                            if(powerups1.getIndestructible() <=0)
                            {
                                fireList.Remove(fireList[i]);
                            }
                        }
                        else
                        {
                            fireList.Remove(fireList[i]);
                        }
                    }
                 
                }
                else if (fireList[i] is Character && fireList[i].chceckColision(GetPosX(), GetRectangle().Width + GetPosX(), GetPosY(), GetRectangle().Height + GetPosY()-shade, shade))
                {
                    fireList.Add(new Fire(fireList[i].GetPosX(), fireList[i].GetPosY(), fireList[i].GetRectangle().Width, fireList[i].GetRectangle().Height));
                    fireList.Remove(fireList[i]);
                }
            }
        }

        private void FindGoodTextureNumberForStart(int[] Distance, bool[] Destroyable, int Width, int height)
        {
            List<int> posibleNumber= new List<int>();
            for(int i=0; i<15; i++)
            {
                posibleNumber.Add(i);
            }
 
            if (Distance[0] != height|| Destroyable[0] == true)//up 
            {
                int[] numberToRemove1 = { 1, 3, 5, 6, 8,  13, 14 };
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove1);
            }
            else
            {
                int[] numberToRemove1 = { 0,2, 4, 7,9,10,11,12};
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove1);
            }

            if (Distance[1] != height || Destroyable[1] == true)//down
            {
                int[] numberToRemove2 = { 1, 4, 5, 6, 7, 11, 12 };
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove2);
            }
            else
            {
                int[] numberToRemove2 = {  0,2,3,8,9,10,13,14};
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove2);
            }

            if (Distance[2] != Width || Destroyable[2] ==true)//left
            {

                int[] numberToRemove3 = { 2, 3, 4, 5, 9, 11, 13 };
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove3);
            }
            else
            {
                int[] numberToRemove3 = { 0,1,6,7,8,10,12,14 };
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove3);
            }

            if (Distance[3] != Width || Destroyable[3] ==true)//right
            {
                int[] numberToRemove4 = { 2, 3, 4, 6, 10, 12, 14 };
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove4);
            }
            else
            {
                int[] numberToRemove4 = { 0,1,5,7,8,9,11,13 };
                DeleteBadTextrueNumber(ref posibleNumber, numberToRemove4);
            }

            if (posibleNumber.Count > 0)
            {
                TextureNumber = posibleNumber[0];
            }
            else
            {
                TextureNumber = 0;
            }
        }

        private static void DeleteBadTextrueNumber(ref List<int> posibleNubmer, int[] numberToRemove)//decision true if delete remove or false if keep remove
        {
            for (int j = 0; j < numberToRemove.Length; j++)
            {
                for (int i = posibleNubmer.Count - 1; i >= 0; i--)
                {
                    if (posibleNubmer[i] == numberToRemove[j])
                    {
                        posibleNubmer.Remove(posibleNubmer[i]);
                    }  
                }
            } 
        }

       

        public int GetTextureNumber()
        {
            return TextureNumber;
        }
        
        public int GetTimeToEndFire()
        {
            return TimeToEndFire;
        }

        public void shorterTimeToEndFire()
        {
            TimeToEndFire--;
        }
    }
}

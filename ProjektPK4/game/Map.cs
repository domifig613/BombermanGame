using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ProjektPK4.game
{
    static class Map
    {
        static Random rnd = new Random();

        static private string[] TextureName;//0-end, 1-rock, 2-box, 3-better box
        static private string[] FireTextureName;
        static private string[][] PowerupsTextureName;
        static private List<Character> Player;

        static private List<GameObject> ObjectToDraw;

        static Map()
        {
            CreateOrResetMap();  
        }

        static public void setFireTextureName(string[] array)
        {
            FireTextureName= new string[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
               FireTextureName[i] = array[i];
            }
        }
       
        static public void setTextureName(string[] array)
        {
            TextureName = new string[array.Length];
            
            for(int i=0; i<array.Length; i++)
            {
                TextureName[i] = array[i];
            }
        }

        static public void setPowerupsTextureName(string[] array, int[] _lenght)
        {
            PowerupsTextureName = new string[array.Length][];

            for(int i=0; i<array.Length; i++)
            {
                PowerupsTextureName[i] = new string[_lenght[i]];

                for(int j=0; j<_lenght[i]; j++)
                {
                    PowerupsTextureName[i][j] = array[i] + j.ToString();
                }
            }
        }


        static private void CreateOrResetMap()
        {

            Player = new List<Character>();
            ObjectToDraw = new List<GameObject>();

            for (int i = 0; i < ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth; i++)//up down wall
            {
                ObjectToDraw.Add(new GameObject(ProgramParameters.OneAreaWidth * i, 0));//up wall                                                                                                                                      
                ObjectToDraw.Add(new GameObject(ProgramParameters.OneAreaWidth * i, ProgramParameters.WindowHeight - ProgramParameters.OneAreaHeight - ProgramParameters.AreaYShade));   //down wall
            }

            for (int i = 0; i < ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight - 1; i++)//left right wall
            {
                ObjectToDraw.Add(new GameObject(0, ProgramParameters.OneAreaHeight * i));//left wall;
                ObjectToDraw.Add(new GameObject(ProgramParameters.WindowWidth - ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight * i));//right wall
            }

            for (int i = 0; i < ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth / 2 - 1; i++)//middle rock
            {
                for (int j = 0; j < ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight / 2 - 1; j++)
                {
                    ObjectToDraw.Add(new GameObject(((i * 2) + 2) * ProgramParameters.OneAreaWidth, ((j * 2) + 2) * ProgramParameters.OneAreaHeight));
                }
            }

            AddBoxes();
            AddPremiumBoxes();

            Player.Add(new Player(ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space, 1));
            Player.Add(new Player(ProgramParameters.WindowWidth - 2 * ProgramParameters.OneAreaWidth, ProgramParameters.WindowHeight - (2 * ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade), Keys.W, Keys.S, Keys.A, Keys.D, Keys.C, 2));
            Player.Add(new Player(ProgramParameters.OneAreaWidth, ProgramParameters.WindowHeight - (2 * ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade), Keys.Y, Keys.H, Keys.G, Keys.J, Keys.O, 3));
            Player.Add(new Player(ProgramParameters.WindowWidth - 2 * ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaWidth, Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad0, 4));


            for (int i = 0; i < 4; i++)
            {
                ObjectToDraw.Add(Player[i]);
            }
            SortObjectToDraw();

        }

        static private void AddPremiumBoxes()
        {
            for (int i = 0; i < ProgramParameters.CountOfPremiumBox; i++)
            {
                int RandPosX, RandPosY;
                do
                {
                    RandPosX = rnd.Next((ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth) - 2);//random position x box
                    RandPosX = RandPosX * ProgramParameters.OneAreaWidth + (1 * ProgramParameters.OneAreaWidth);

                    RandPosY = rnd.Next((ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight) - 2);//random position y box
                    RandPosY = (RandPosY * ProgramParameters.OneAreaHeight + (1 * ProgramParameters.OneAreaHeight)) + 0;//+0 because window start 0
                }
                while (CheckBoxPos(RandPosX, RandPosY));

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, 90));
            }
        }

        static private void AddBoxes()
        {
            for (int i = 0; i < ProgramParameters.CountOfNormalBox; i++)
            {
                int RandPosX, RandPosY;
                do
                {
                    RandPosX = rnd.Next((ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth) - 2);//random position x box
                    RandPosX = RandPosX * ProgramParameters.OneAreaWidth + (1 * ProgramParameters.OneAreaWidth);

                    RandPosY = rnd.Next((ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight) - 2);//random position y box
                    RandPosY = (RandPosY * ProgramParameters.OneAreaHeight + (1 * ProgramParameters.OneAreaHeight)) + 0;//+0 because window start 0
                }
                while (CheckBoxPos(RandPosX, RandPosY));

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, 25));
            }
        }

        static bool CheckBoxPos(int posX, int posY)
        {
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() == posX && object1.GetPosY() == posY)
                {
                    return true;
                }
            }

            if (posX < (ProgramParameters.StartPositionSize + 1) * ProgramParameters.OneAreaWidth || posX >= ProgramParameters.WindowWidth - (ProgramParameters.StartPositionSize + 1) * ProgramParameters.OneAreaWidth)//true if box is on start positin
            {
                if (posY < (ProgramParameters.StartPositionSize + 1) * ProgramParameters.OneAreaHeight || posY >= ProgramParameters.WindowHeight - (((ProgramParameters.StartPositionSize + 1) * ProgramParameters.OneAreaHeight) + ProgramParameters.AreaYShade))
                {
                    return true;
                }
            }
            return false;
        }

        static public void DrawMap(SpriteBatch Batch)
        {
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1 is Box box1)
                {
                    if (box1.GetChanceToDrop() == 90)
                    {
                        Batch.Draw(GameTextures.GetTexture(TextureName[3]), object1.GetRectangle(), Color.White);
                    }
                    else
                    {
                        Batch.Draw(GameTextures.GetTexture(TextureName[2]), object1.GetRectangle(), Color.White);
                    }
                }
                else if (object1 is Player)
                {
                    Character character1 = (Character)object1;
                    Batch.Draw(character1.GetTexture(), object1.GetRectangle(), Color.White);
                }
                else if (object1.GetPosX() >= ProgramParameters.WindowWidth - ProgramParameters.OneAreaWidth || object1.GetPosX() < ProgramParameters.OneAreaWidth || object1.GetPosY() >= ProgramParameters.WindowHeight - (ProgramParameters.OneAreaWidth + ProgramParameters.AreaYShade) || object1.GetPosY() < ProgramParameters.OneAreaHeight)
                {
                    Batch.Draw(GameTextures.GetTexture(TextureName[0]), object1.GetRectangle(), Color.White);
                }
                else if (object1 is Bomb bomb1)
                {
                    Batch.Draw(GameTextures.GetTexture(bomb1.GetTexuteName() + bomb1.GetTextureNumber()), object1.GetRectangle(), Color.White);
                }
                else if (object1 is Fire fire1)
                {
                    Batch.Draw(GameTextures.GetTexture(FireTextureName[fire1.GetTextureNumber()]), object1.GetRectangle(), Color.White);
                }
                else if (object1 is Powerups powerups1)
                {
                    if (powerups1.GetIndestructible() <= 0)
                    {
                        Batch.Draw(PowerupsTextures[powerups1.GetTypePowerups() - 1][powerups1.GetNumberTexture()], object1.GetRectangle(), Color.White);
                    }
                }
                else
                {
                    Batch.Draw(GameTextures.GetTexture(TextureName[1]), object1.GetRectangle(), Color.White);
                }
            }
        }

        static public void SortObjectToDraw()
        {
            List<Character> character1 = new List<Character>();
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if (ObjectToDraw[i] is Character)
                {
                    character1.Add((Character)ObjectToDraw[i]);
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                }
            }
            ObjectToDraw.Sort((x, y) => x.GetPosY().CompareTo(y.GetPosY()));
        
            foreach(Character character2 in character1)
            {
                for (int i = 0; i < ObjectToDraw.Count; i++)
                {
                    if (ObjectToDraw[i].GetPosY() > character2.GetPosY())
                    {
                        ObjectToDraw.Insert(i, character2);
                        break;
                    }
                }
            }
        }

        static public void MapChanges()
        {
            List<GameObject> TemporaryListFire = new List<GameObject>(); // list for fire and powerups
            List<GameObject> TemporaryListPowerups = new List<GameObject>(); // list for fire and powerups


            ChceckTimerObjects();//change texture and time of object
            AddToTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups);//take powerups and fire from objectToDraw
            CheckBurnObject(ref TemporaryListFire, ref TemporaryListPowerups);//fire objectToDraw or powerups if colision

            PutBomb();//if player press key to put bomb
            Move();

            TryPickPowerUp(ref TemporaryListPowerups);//check player position and powerups and try pick powerups
            RemoveFromTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups); // put back powerups and fire to objectToDraw

            CheckWinCondition();
        }

        private static void CheckWinCondition()
        {
           if(Player.Count==1)
            {
                Scorebar.AddScore(Player[0].PlayerNumber);
            }
           if(Player.Count<=1)
            {
                CreateOrResetMap();
            }
        }

        static private void Move()
        {
            foreach (Player player1 in Player)
            {
                CheckMovePlayer(0, player1);// move (cant see powerups and fire)
            }
        }

        static private void TryPickPowerUp(ref List<GameObject> powerups)
        {
            foreach (Character character1 in Player)
            {
                for (int i = powerups.Count - 1; i >= 0; i--)
                {
                    if (powerups[i].ChceckColision(character1.GetPosX(), character1.GetPosY()))
                    { 
                        Powerups powerup1 = (Powerups)powerups[i];
                        powerup1.AddPower(character1);
                        powerups.Remove(powerups[i]);
                    }
                }
            }
        }

        static private void AddToTemporaryList(ref List<GameObject> fire, ref List<GameObject> powerups)
        {
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if (ObjectToDraw[i] is Fire)
                {
                    fire.Add(ObjectToDraw[i]);
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                }
                else if (ObjectToDraw[i] is Powerups)
                {
                    powerups.Add(ObjectToDraw[i]);
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                }
            }
        }
        static private void RemoveFromTemporaryList(ref List<GameObject> fire, ref List<GameObject> powerups)
        {
            for (int i = fire.Count - 1; i >= 0; i--)
            {
                ObjectToDraw.Add(fire[i]);
                fire.Remove(fire[i]);
            }
            for (int i = powerups.Count - 1; i >= 0; i--)
            {
                ObjectToDraw.Add(powerups[i]);
                powerups.Remove(powerups[i]);
            }
        }

        static private void CheckBurnObject(ref List<GameObject> fire, ref List<GameObject> powerups)
        {
            for (int i = fire.Count - 1; i >= 0; i--)
            {
                Fire fire1 = (Fire) fire[i];
                fire1.BurnObject(ref ObjectToDraw, ProgramParameters.AreaYShade);
                fire1.BurnObject(ref powerups, ProgramParameters.AreaYShade);
            }
            FindBurnedPlayers();
        }

        static private void FindBurnedPlayers()
        {
            for(int i=Player.Count-1; i>=0; i--)
            {
                Player.Remove(Player[i]);
            }
             
            for(int i=ObjectToDraw.Count-1; i>=0; i--)
            {
                if(ObjectToDraw[i] is Character character1)
                {
                    Player.Add(character1);
                }
            }

        }

        static private void ChceckTimerObjects()
        {
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if(ObjectToDraw[i] is Fire fire1)
                {
                    fire1.ShorterTimeToEndFire();
                    if(fire1.GetTimeToEndFire() <=0)
                    {
                        ObjectToDraw.Remove(ObjectToDraw[i]);
                    }
                }
                else if(ObjectToDraw[i] is Powerups powerups1)
                {
                    if (powerups1.GetIndestructible() <= 0)
                    {
                        powerups1.SetNextTexture(PowerupsTextures[powerups1.GetTypePowerups()-1].Count);
                    }
                    else
                    {
                        powerups1.IndestructibleDecrement();
                    }
                }
                else if (ObjectToDraw[i] is Bomb bomb1)
                {
                    bomb1.ShortenTheTimer();
                    if (bomb1.GetTimer() % ((int)(bomb1.GetMaxTime() / (BombTextures.Count)) + 1) == 0 && bomb1.GetMaxTime() != bomb1.GetTimer())
                    {
                        bomb1.SetNextTexture();
                    }
                    if (bomb1.CheckDestroyTimer())
                    {
                        DestroyBomb(i, bomb1);
                    }
                }
            }

            foreach (Player Player1 in Player)
            {
                Player1.ShortenTheDelay();//Player shorten time to put bomb
            }
        }

        static private void DestroyBomb(int i, Bomb bomb1)
        {
            GameObject[] objects = { FindObjectWithLowerY(bomb1.GetPosX(), bomb1.GetPosY()), FindObjectWithHigherY(bomb1.GetPosX(),
                            bomb1.GetPosY()) , FindObjectWithLowerX(bomb1.GetPosX(), bomb1.GetPosY()),FindObjectWithHigherX(bomb1.GetPosX(), bomb1.GetPosY())};

            int[] DistanceArray = { 0, 0, 0, 0 };
            DistanceArray[0] = bomb1.GetPosY() - objects[0].GetPosY();
            DistanceArray[1] = objects[1].GetPosY() - bomb1.GetPosY();
            DistanceArray[2] = bomb1.GetPosX() - objects[2].GetPosX();
            DistanceArray[3] = objects[3].GetPosX() - bomb1.GetPosX();

            bool[] Destroyable = { false, false, false, false };

            for (int j = 0; j < 4; j++)
            {
                if (objects[j] is Box || objects[j] is Character || objects[j] is Bomb || objects[j] is Powerups)
                {
                    Destroyable[j] = true;
                }
            }

            bomb1.DestroyBombCreateFire(ref ObjectToDraw, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight, DistanceArray, Destroyable);
            ObjectToDraw.RemoveAt(i);
        }

        static private void PutBomb()
        {
            foreach (Player player1 in Player)
            {
                if (Keyboard.GetState().IsKeyDown(player1.GetKey(4)) && player1.ActuallDelayBomb == 0)
                {
                    ObjectToDraw.Add(player1.PutBomb(ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight, ProgramParameters.AreaYShade));
                }
            }
        }

        //{     //try move for every object
        static private void CheckMovePlayer(int control, Player player1)
        {
                if (Keyboard.GetState().IsKeyDown(player1.GetKey(0)) && control == 0 || control == 1)//up
                {
                    CheckMoveUp(control, player1);
                }
                else if (Keyboard.GetState().IsKeyDown(player1.GetKey(1)) && control == 0 || control == 2)//down
                {
                    CheckMoveDown(control, player1);
                }
                else if (Keyboard.GetState().IsKeyDown(player1.GetKey(2)) && control == 0 || control == 3)//left
                {
                    CheckMoveLeft(control, player1);
                }
                else if (Keyboard.GetState().IsKeyDown(player1.GetKey(3)) && control == 0 || control == 4)//right
                {
                    CheckMoveRight(control, player1);
                }  
        } //1

        static private void CheckMoveUp(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY());
            int distance = Player.GetPosY() - (closerObject.GetPosY() + ProgramParameters.OneAreaHeight);

            if (control != 0 && 
               ( Player.GetPosY()-Player.getSpeed() + ProgramParameters.OneAreaHeight <= FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(2))
                || Player.GetPosY() - Player.getSpeed() + ProgramParameters.OneAreaHeight <= FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY()).GetPosY()&& Keyboard.GetState().IsKeyDown(Player.GetKey(3))))
            {
                Player.MoveCharacter(0, -1, 3);
            }
            else if (distance > Player.getSpeed())
            {
                Player.MoveCharacter(0, -Player.getSpeed(), 0);
            }
            else if (distance > 0)
            {
                Player.MoveCharacter(0, -distance, 0);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, true);
            }
        } //2
        static private void CheckMoveDown(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY());
            int distance = closerObject.GetPosY() - (Player.GetPosY() + ProgramParameters.OneAreaHeight);

            if (control != 0 &&
                (Player.GetPosY() + Player.getSpeed() - ProgramParameters.OneAreaHeight >= FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(2))
                || Player.GetPosY() + Player.getSpeed() - ProgramParameters.OneAreaHeight >= FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(3))))
            {
                Player.MoveCharacter(0, 1, 3);
            }
            else if (distance > Player.getSpeed())
            {
                Player.MoveCharacter(0, Player.getSpeed(), 3);
            }
            else if (distance > 0)
            {
                Player.MoveCharacter(0, distance, 3);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, true);
            }
        }
        static private void CheckMoveLeft(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY());
            int distance = Player.GetPosX() - (closerObject.GetPosX() + ProgramParameters.OneAreaWidth);

            if (control != 0 &&
                (Player.GetPosX() - Player.getSpeed() + ProgramParameters.OneAreaWidth < FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(0))
                || Player.GetPosX() - Player.getSpeed() + ProgramParameters.OneAreaWidth < FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(1))))
            {
                Player.MoveCharacter(-1, 0, 3);
            }
            else if (distance > Player.getSpeed())
            {
                Player.MoveCharacter(-Player.getSpeed(), 0, 6);
            }
            else if (distance > 0)
            {
                Player.MoveCharacter(-distance, 0, 6);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, false);
            }
        }
        static private void CheckMoveRight(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY());
            int distance = closerObject.GetPosX() - (Player.GetPosX() + ProgramParameters.OneAreaWidth);

            if (control != 0 &&
                (Player.GetPosX() + Player.getSpeed() - ProgramParameters.OneAreaWidth > FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(0))
                || Player.GetPosX() + Player.getSpeed() - ProgramParameters.OneAreaWidth > FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(1))))
            {
                Player.MoveCharacter(1, 0, 3);
            }
            else if (distance > Player.getSpeed())
            {
                Player.MoveCharacter(Player.getSpeed(), 0, 9);
            }
            else if (distance > 0)
            {
                Player.MoveCharacter(distance, 0, 9);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, false);
            }
        }

        static private GameObject FindObjectWithHigherX(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(ProgramParameters.WindowWidth, 0);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() >= posX + ProgramParameters.OneAreaWidth && object1.GetPosX() <= objectReturn.GetPosX() && object1.CheckColisionHeight(posY))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        } //3
        static private GameObject FindObjectWithLowerX(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() + ProgramParameters.OneAreaWidth <= posX && object1.GetPosX() >= objectReturn.GetPosX() && object1.CheckColisionHeight(posY))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }
        static private GameObject FindObjectWithHigherY(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, ProgramParameters.WindowHeight);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() >= posY + ProgramParameters.OneAreaHeight && object1.GetPosY() <= objectReturn.GetPosY() && object1.CheckColisionWidth(posX))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }
        static private GameObject FindObjectWithLowerY(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() + ProgramParameters.OneAreaHeight <= posY && object1.GetPosY() >= objectReturn.GetPosY() && object1.CheckColisionWidth(posX))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }

        static private void MoveInAnotherDirection(Player Player, GameObject closerObject, bool UpOrDownTrue)
        {
            if (UpOrDownTrue)
            {
                int position = Player.GetPosX() - closerObject.GetPosX();
                if (position < 0 && FindObjectWithLowerX(closerObject.GetPosX(), closerObject.GetPosY()).GetPosX() + ProgramParameters.OneAreaWidth < closerObject.GetPosX())
                {
                    CheckMovePlayer(3, Player);
                }
                else if (position > 0 && FindObjectWithHigherX(closerObject.GetPosX(), closerObject.GetPosY()).GetPosX() - ProgramParameters.OneAreaWidth > closerObject.GetPosX())
                {
                    CheckMovePlayer(4, Player);
                }
            }
            else
            {
                int position = Player.GetPosY() - closerObject.GetPosY();
                if (position < 0 && FindObjectWithLowerY(closerObject.GetPosX(), closerObject.GetPosY()).GetPosY() + ProgramParameters.OneAreaHeight < closerObject.GetPosY())
                {
                    CheckMovePlayer(1, Player);
                }
                else if (position > 0 && FindObjectWithHigherY(closerObject.GetPosX(), closerObject.GetPosY()).GetPosY() - ProgramParameters.OneAreaHeight > closerObject.GetPosY())
                {
                    CheckMovePlayer(2, Player);
                }
            }
        } //4
        // end try move
    }
}

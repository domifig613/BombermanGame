﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ProjektPK4.Content
{
    public static class Map
    {
        static Random rnd = new Random();

        static private Texture2D RockTexture;//Rock texture
        static private Texture2D MiddleRockTexture;//Rock2 texture
        static private Texture2D BoxTexture;//Box texture
        static private Texture2D BoxWithBetterDropTexture;//premium box texture
        static private List<Texture2D> BombTextures = new List<Texture2D>();//bomb texture
        static private List<Texture2D> FireTextures = new List<Texture2D>();
        static private List<List<Texture2D>> PowerupsTextures = new List<List<Texture2D>>();

        static private List<Player> Player = new List<Player>();

        static private List<GameObject> ObjectToDraw = new List<GameObject>();

        static Map()
        {
            for (int i = 0; i < 4; i++)//powerups type(4)
            {
                PowerupsTextures.Add(new List<Texture2D>());
            }

            for (int i = 0; i < ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth; i++)//up down wall
            {
                ObjectToDraw.Add(new GameObject(ProgramParameters.OneAreaWidth * i, 0, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade));//up wall                                                                                                                                      
                ObjectToDraw.Add(new GameObject(ProgramParameters.OneAreaWidth * i, ProgramParameters.WindowHeight - ProgramParameters.OneAreaHeight - ProgramParameters.AreaYShade, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade));   //down wall
            }

            for (int i = 0; i < ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight - 1; i++)//left right wall
            {
                ObjectToDraw.Add(new GameObject(0, ProgramParameters.OneAreaHeight * i, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade));//left wall;
                ObjectToDraw.Add(new GameObject(ProgramParameters.WindowWidth - ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight * i, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade));//right wall
            }

            for (int i = 0; i < ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth / 2 - 1; i++)//middle rock
            {
                for (int j = 0; j < ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight / 2 - 1; j++)
                {
                    ObjectToDraw.Add(new GameObject(((i * 2) + 2) * ProgramParameters.OneAreaWidth, ((j * 2) + 2) * ProgramParameters.OneAreaHeight, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade));
                }
            }

            AddBoxes();
            AddPremiumBoxes();

            Player.Add(new Player(ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade, ProgramParameters.CharacterSlowerAnimation, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.P));
            Player.Add(new Player(ProgramParameters.WindowWidth - 2*ProgramParameters.OneAreaWidth, ProgramParameters.WindowHeight - (2 * ProgramParameters.OneAreaHeight+ProgramParameters.AreaYShade), ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade, ProgramParameters.CharacterSlowerAnimation, Keys.W, Keys.S, Keys.A, Keys.D, Keys.V));
            Player.Add(new Player(ProgramParameters.OneAreaWidth, ProgramParameters.WindowHeight - (2 * ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade), ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade, ProgramParameters.CharacterSlowerAnimation, Keys.W, Keys.S, Keys.A, Keys.D, Keys.V));
            Player.Add(new Player(ProgramParameters.WindowWidth - 2 * ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade, ProgramParameters.CharacterSlowerAnimation, Keys.W, Keys.S, Keys.A, Keys.D, Keys.V));


            for(int i=0; i<4; i++)
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

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade, 90));
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

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight + ProgramParameters.AreaYShade, 25));
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

        static public void LoadTextureMap(Texture2D texture, int control)//0 rock, 1 middle rock, 2 box, rock default;
        {
            switch (control)
            {
                case 0:
                    RockTexture = texture;
                    break;
                case 1:
                    MiddleRockTexture = texture;
                    break;
                case 2:
                    BoxTexture = texture;
                    break;
                case 3:
                    BoxWithBetterDropTexture = texture;
                    break;
                case 4:
                    BombTextures.Add(texture);
                    break;
                case 5:
                    FireTextures.Add(texture);
                    break;
                case 6:
                    PowerupsTextures[0].Add(texture);
                    break;
                case 7:
                    PowerupsTextures[1].Add(texture);
                    break;
                case 8:
                    PowerupsTextures[2].Add(texture);
                    break;
                case 9:
                    PowerupsTextures[3].Add(texture);
                    break;
                default:
                    break;
            }
        }

        static public void LoadTexturePlayer(Texture2D texture, int x, int y, int CharacterNumber)
        {
            Player[CharacterNumber].SetTexture(texture, x, y);
        }//one time at start load texture

        static public void DrawMap(SpriteBatch Batch)
        {
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1 is Box box1)
                {
                    if (box1.GetChanceToDrop() == 90)
                    {
                        Batch.Draw(BoxWithBetterDropTexture, object1.GetRectangle(), Color.White);

                    }
                    else
                    {
                        Batch.Draw(BoxTexture, object1.GetRectangle(), Color.White);
                    }
                }
                else if (object1 is Player)
                {
                    Character character1 = (Character)object1;
                    Batch.Draw(character1.GetTexture(), object1.GetRectangle(), Color.White);
                }
                else if (object1.GetPosX() >= ProgramParameters.WindowWidth - ProgramParameters.OneAreaWidth || object1.GetPosX() < ProgramParameters.OneAreaWidth || object1.GetPosY() >= ProgramParameters.WindowHeight - (ProgramParameters.OneAreaWidth + ProgramParameters.AreaYShade) || object1.GetPosY() < ProgramParameters.OneAreaHeight)
                {
                    Batch.Draw(RockTexture, object1.GetRectangle(), Color.White);
                }
                else if (object1 is Bomb bomb1)
                {
                    Batch.Draw(BombTextures[bomb1.GetTextureNumber()], object1.GetRectangle(), Color.White);
                }
                else if (object1 is Fire fire1)
                {
                    Batch.Draw(FireTextures[fire1.GetTextureNumber()], object1.GetRectangle(), Color.White);
                }
                else if (object1 is Powerups powerups1)
                {
                    if (powerups1.getIndestructible() <= 0)
                    {
                        Batch.Draw(PowerupsTextures[powerups1.getTypePowerups() - 1][powerups1.getNumberTexture()], object1.GetRectangle(), Color.White);
                    }
                }
                else
                {
                    Batch.Draw(MiddleRockTexture, object1.GetRectangle(), Color.White);
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

            ShortDelayBomb();
            PutBomb();//if player press key to put bomb
            Move();

            TryPickPowerUp(ref TemporaryListPowerups);//check player position and powerups and try pick powerups
            RemoveFromTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups); // put back powerups and fire to objectToDraw
        }

        static private void Move()
        {
            foreach (Player player1 in Player)
            {
                CheckMovePlayer(0, player1);// move (cant see powerups and fire)
            }
        }

        static private void ShortDelayBomb()
        {
            foreach (Player Player1 in Player)
            {
                Player1.shortenTheDelay();//Player shorten time to put bomb
            }
        }

        static private void TryPickPowerUp(ref List<GameObject> powerups)
        {
            foreach (Character character1 in Player)
            {
                for (int i = powerups.Count - 1; i >= 0; i--)
                {
                    if (powerups[i].chceckColision(character1.GetPosX(), character1.GetRectangle().Width + character1.GetPosX(),
                        character1.GetPosY(), character1.GetRectangle().Height + character1.GetPosY() - ProgramParameters.AreaYShade, ProgramParameters.AreaYShade))
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
                    if (powerups1.getIndestructible() <= 0)
                    {
                        powerups1.setNextTexture(PowerupsTextures[powerups1.getTypePowerups()-1].Count);
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
                if (Keyboard.GetState().IsKeyDown(player1.GetKey(4)) && player1.actuallDelayBomb == 0)
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
               ( Player.GetPosY()-Player.Speed + ProgramParameters.OneAreaHeight <= FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(2))
                || Player.GetPosY() - Player.Speed + ProgramParameters.OneAreaHeight <= FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY()).GetPosY()&& Keyboard.GetState().IsKeyDown(Player.GetKey(3))))
            {
                Player.MoveCharacter(0, -1, 3);
            }
            else if (distance > Player.Speed)
            {
                Player.MoveCharacter(0, -Player.Speed, 0);
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
                (Player.GetPosY() + Player.Speed - ProgramParameters.OneAreaHeight >= FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(2))
                || Player.GetPosY() + Player.Speed - ProgramParameters.OneAreaHeight >= FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(3))))
            {
                Player.MoveCharacter(0, 1, 3);
            }
            else if (distance > Player.Speed)
            {
                Player.MoveCharacter(0, Player.Speed, 3);
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
                (Player.GetPosX() - Player.Speed + ProgramParameters.OneAreaWidth < FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(0))
                || Player.GetPosX() - Player.Speed + ProgramParameters.OneAreaWidth < FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(1))))
            {
                Player.MoveCharacter(-1, 0, 3);
            }
            else if (distance > Player.Speed)
            {
                Player.MoveCharacter(-Player.Speed, 0, 6);
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
                (Player.GetPosX() + Player.Speed - ProgramParameters.OneAreaWidth > FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(0))
                || Player.GetPosX() + Player.Speed - ProgramParameters.OneAreaWidth > FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(1))))
            {
                Player.MoveCharacter(1, 0, 3);
            }
            else if (distance > Player.Speed)
            {
                Player.MoveCharacter(Player.Speed, 0, 9);
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
            GameObject objectReturn = new GameObject(ProgramParameters.WindowWidth, 0, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() >= posX + ProgramParameters.OneAreaWidth && object1.GetPosX() <= objectReturn.GetPosX() && object1.CheckColisionHeight(posY, posY + ProgramParameters.OneAreaHeight, ProgramParameters.AreaYShade))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        } //3
        static private GameObject FindObjectWithLowerX(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() + ProgramParameters.OneAreaWidth <= posX && object1.GetPosX() >= objectReturn.GetPosX() && object1.CheckColisionHeight(posY, posY + ProgramParameters.OneAreaHeight, ProgramParameters.AreaYShade))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }
        static private GameObject FindObjectWithHigherY(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, ProgramParameters.WindowHeight, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() >= posY + ProgramParameters.OneAreaHeight && object1.GetPosY() <= objectReturn.GetPosY() && object1.CheckColisionWidth(posX, posX + ProgramParameters.OneAreaWidth))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }
        static private GameObject FindObjectWithLowerY(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() + ProgramParameters.OneAreaHeight <= posY && object1.GetPosY() >= objectReturn.GetPosY() && object1.CheckColisionWidth(posX, posX + ProgramParameters.OneAreaWidth))
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
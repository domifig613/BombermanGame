using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ProjektPK4.Content
{
    public class Map
    {
        Random rnd = new Random();

        ProgramParameters GameParam = new ProgramParameters();

        private Texture2D RockTexture;//Rock texture
        private Texture2D MiddleRockTexture;//Rock2 texture
        private Texture2D BoxTexture;//Box texture
        private Texture2D BoxWithBetterDropTexture;//premium box texture
        private List<Texture2D> BombTextures = new List<Texture2D>();//bomb texture
        private List<Texture2D> FireTextures = new List<Texture2D>();
        private List<List<Texture2D>> PowerupsTextures = new List<List<Texture2D>>();

        private List<Player> Player = new List<Player>();

        private List<GameObject> ObjectToDraw = new List<GameObject>();

        public Map()
        {
            for (int i = 0; i < 4; i++)//powerups type(4)
            {
                PowerupsTextures.Add(new List<Texture2D>());
            }

            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth; i++)//up down wall
            {
                ObjectToDraw.Add(new GameObject(GameParam.OneAreaWidth * i, 0, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade));//up wall                                                                                                                                      
                ObjectToDraw.Add(new GameObject(GameParam.OneAreaWidth * i, GameParam.WindowHeight - GameParam.OneAreaHeight - GameParam.AreaYShade, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade));   //down wall
            }

            for (int i = 0; i < GameParam.WindowHeight / GameParam.OneAreaHeight - 1; i++)//left right wall
            {
                ObjectToDraw.Add(new GameObject(0, GameParam.OneAreaHeight * i, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade));//left wall;
                ObjectToDraw.Add(new GameObject(GameParam.WindowWidth - GameParam.OneAreaWidth, GameParam.OneAreaHeight * i, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade));//right wall
            }

            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth / 2 - 1; i++)//middle rock
            {
                for (int j = 0; j < GameParam.WindowHeight / GameParam.OneAreaHeight / 2 - 1; j++)
                {
                    ObjectToDraw.Add(new GameObject(((i * 2) + 2) * GameParam.OneAreaWidth, ((j * 2) + 2) * GameParam.OneAreaHeight, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade));
                }
            }

            AddBoxes();
            AddPremiumBoxes();

            Player.Add(new Player(GameParam.OneAreaWidth, GameParam.OneAreaHeight, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, GameParam.CharacterSlowerAnimation, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.P));
            Player.Add(new Player(GameParam.WindowWidth - 2*GameParam.OneAreaWidth, GameParam.WindowHeight - (2 * GameParam.OneAreaHeight+GameParam.AreaYShade), GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, GameParam.CharacterSlowerAnimation, Keys.W, Keys.S, Keys.A, Keys.D, Keys.V));
            Player.Add(new Player(GameParam.OneAreaWidth, GameParam.WindowHeight - (2 * GameParam.OneAreaHeight + GameParam.AreaYShade), GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, GameParam.CharacterSlowerAnimation, Keys.W, Keys.S, Keys.A, Keys.D, Keys.V));
            Player.Add(new Player(GameParam.WindowWidth - 2 * GameParam.OneAreaWidth, GameParam.OneAreaWidth, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, GameParam.CharacterSlowerAnimation, Keys.W, Keys.S, Keys.A, Keys.D, Keys.V));


            for(int i=0; i<4; i++)
            {
                ObjectToDraw.Add(Player[i]);
            }
            SortObjectToDraw();
        }

        private void AddPremiumBoxes()
        {
            for (int i = 0; i < GameParam.CountOfPremiumBox; i++)
            {
                int RandPosX, RandPosY;
                do
                {
                    RandPosX = rnd.Next((GameParam.WindowWidth / GameParam.OneAreaWidth) - 2);//random position x box
                    RandPosX = RandPosX * GameParam.OneAreaWidth + (1 * GameParam.OneAreaWidth);

                    RandPosY = rnd.Next((GameParam.WindowHeight / GameParam.OneAreaHeight) - 2);//random position y box
                    RandPosY = (RandPosY * GameParam.OneAreaHeight + (1 * GameParam.OneAreaHeight)) + 0;//+0 because window start 0
                }
                while (CheckBoxPos(RandPosX, RandPosY));

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, 90));
            }
        }

        private void AddBoxes()
        {
            for (int i = 0; i < GameParam.CountOfNormalBox; i++)
            {
                int RandPosX, RandPosY;
                do
                {
                    RandPosX = rnd.Next((GameParam.WindowWidth / GameParam.OneAreaWidth) - 2);//random position x box
                    RandPosX = RandPosX * GameParam.OneAreaWidth + (1 * GameParam.OneAreaWidth);

                    RandPosY = rnd.Next((GameParam.WindowHeight / GameParam.OneAreaHeight) - 2);//random position y box
                    RandPosY = (RandPosY * GameParam.OneAreaHeight + (1 * GameParam.OneAreaHeight)) + 0;//+0 because window start 0
                }
                while (CheckBoxPos(RandPosX, RandPosY));

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, 25));
            }
        }

        bool CheckBoxPos(int posX, int posY)
        {
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() == posX && object1.GetPosY() == posY)
                {
                    return true;
                }
            }

            if (posX < (GameParam.StartPositionSize + 1) * GameParam.OneAreaWidth || posX >= GameParam.WindowWidth - (GameParam.StartPositionSize + 1) * GameParam.OneAreaWidth)//true if box is on start positin
            {
                if (posY < (GameParam.StartPositionSize + 1) * GameParam.OneAreaHeight || posY >= GameParam.WindowHeight - (((GameParam.StartPositionSize + 1) * GameParam.OneAreaHeight) + GameParam.AreaYShade))
                {
                    return true;
                }
            }
            return false;
        }

        public void LoadTextureMap(Texture2D texture, int control)//0 rock, 1 middle rock, 2 box, rock default;
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

        public void LoadTexturePlayer(Texture2D texture, int x, int y, int CharacterNumber)
        {
            Player[CharacterNumber].SetTexture(texture, x, y);
        }//one time at start load texture

        public void DrawMap(SpriteBatch Batch)
        {
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1 is Box)
                {
                    Box box1 = (Box)object1;
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
                else if (object1.GetPosX() >= GameParam.WindowWidth - GameParam.OneAreaWidth || object1.GetPosX() < GameParam.OneAreaWidth || object1.GetPosY() >= GameParam.WindowHeight - (GameParam.OneAreaWidth + GameParam.AreaYShade) || object1.GetPosY() < GameParam.OneAreaHeight)
                {
                    Batch.Draw(RockTexture, object1.GetRectangle(), Color.White);
                }
                else if (object1 is Bomb)
                {
                    Bomb bomb1 = (Bomb)object1;
                    Batch.Draw(BombTextures[bomb1.getTextureNumber()], object1.GetRectangle(), Color.White);
                }
                else if (object1 is Fire)
                {
                    Fire fire1 = (Fire)object1;
                    Batch.Draw(FireTextures[fire1.GetTextureNumber()], object1.GetRectangle(), Color.White);
                }
                else if (object1 is Powerups)
                {
                    Powerups powerups1 = (Powerups)object1;
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

        public void SortObjectToDraw()
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

        public void MapChanges()
        {
            List<GameObject> TemporaryListFire = new List<GameObject>(); // list for fire and powerups
            List<GameObject> TemporaryListPowerups = new List<GameObject>(); // list for fire and powerups


            ChceckTimerObjects();//change texture and time of object
            AddToTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups);//take powerups and fire from objectToDraw
            CheckBurnObject(ref TemporaryListFire, ref TemporaryListPowerups);//fire objectToDraw or powerups if colision

            shortDelayBomb();
            PutBomb();//if player press key to put bomb
            move();

            TryPickPowerUp(ref TemporaryListPowerups);//check player position and powerups and try pick powerups
            RemoveFromTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups); // put back powerups and fire to objectToDraw
        }

        private void move()
        {
            foreach (Player player1 in Player)
            {
                CheckMovePlayer(0, player1);// move (cant see powerups and fire)
            }
        }

        private void shortDelayBomb()
        {
            foreach (Player Player1 in Player)
            {
                Player1.shortenTheDelay();//Player shorten time to put bomb
            }
        }

        private void TryPickPowerUp(ref List<GameObject> powerups)
        {
            foreach (Character character1 in Player)
            {
                for (int i = powerups.Count - 1; i >= 0; i--)
                {
                    if (powerups[i].chceckColision(character1.GetPosX(), character1.GetRectangle().Width + character1.GetPosX(),
                        character1.GetPosY(), character1.GetRectangle().Height + character1.GetPosY() - GameParam.AreaYShade, GameParam.AreaYShade))
                    {
                        Powerups powerup1 = (Powerups)powerups[i];
                        powerup1.AddPower(character1);
                        powerups.Remove(powerups[i]);
                    }
                }
            }
        }

        private void AddToTemporaryList(ref List<GameObject> fire, ref List<GameObject> powerups)
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
        private void RemoveFromTemporaryList(ref List<GameObject> fire, ref List<GameObject> powerups)
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

        private void CheckBurnObject(ref List<GameObject> fire, ref List<GameObject> powerups)
        {
            for (int i = fire.Count - 1; i >= 0; i--)
            {
                Fire fire1 = (Fire) fire[i];
                fire1.BurnObject(ref ObjectToDraw, GameParam.AreaYShade);
                fire1.BurnObject(ref powerups, GameParam.AreaYShade);
            }
        }

        private void ChceckTimerObjects()
        {
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if(ObjectToDraw[i] is Fire)
                {
                    Fire fire1 = (Fire)ObjectToDraw[i];
                    fire1.shorterTimeToEndFire();
                    if(fire1.GetTimeToEndFire() <=0)
                    {
                        ObjectToDraw.Remove(ObjectToDraw[i]);
                    }
                }
                else if(ObjectToDraw[i] is Powerups)
                {
                    Powerups powerups1 = (Powerups)ObjectToDraw[i];
                    if (powerups1.getIndestructible() <= 0)
                    {
                        powerups1.setNextTexture(PowerupsTextures[powerups1.getTypePowerups()-1].Count);
                    }
                    else
                    {
                        powerups1.IndestructibleDecrement();
                    }
                }
                else if (ObjectToDraw[i] is Bomb)
                {
                    Bomb bomb1 = (Bomb)ObjectToDraw[i];
                    bomb1.shortenTheTimer();
                    if (bomb1.getTimer() % ((int)(bomb1.getMaxTime() / (BombTextures.Count)) + 1) == 0 && bomb1.getMaxTime() != bomb1.getTimer())
                    {
                        bomb1.SetNextTexture();
                    }
                    if (bomb1.checkDestroyTimer())
                    {
                        DestroyBomb(i, bomb1);
                    }
                }
            }
        }

        private void DestroyBomb(int i, Bomb bomb1)
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

            bomb1.DestroyBombCreateFire(ref ObjectToDraw, GameParam.OneAreaWidth, GameParam.OneAreaHeight, DistanceArray, Destroyable);
            ObjectToDraw.RemoveAt(i);
        }

        private void PutBomb()
        {
            foreach (Player player1 in Player)
            {
                if (Keyboard.GetState().IsKeyDown(player1.GetKey(4)) && player1.actuallDelayBomb == 0)
                {
                    ObjectToDraw.Add(player1.PutBomb(GameParam.OneAreaWidth, GameParam.OneAreaHeight, GameParam.AreaYShade));
                }
            }
        }

        //{     //try move for every object
        private void CheckMovePlayer(int control, Player player1)
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

        private void CheckMoveUp(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY());
            int distance = Player.GetPosY() - (closerObject.GetPosY() + GameParam.OneAreaHeight);

            if (control != 0 && 
               ( Player.GetPosY()-Player.Speed + GameParam.OneAreaHeight <= FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(2))
                || Player.GetPosY() - Player.Speed + GameParam.OneAreaHeight <= FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY()).GetPosY()&& Keyboard.GetState().IsKeyDown(Player.GetKey(3))))
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
        private void CheckMoveDown(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY());
            int distance = closerObject.GetPosY() - (Player.GetPosY() + GameParam.OneAreaHeight);

            if (control != 0 &&
                (Player.GetPosY() + Player.Speed - GameParam.OneAreaHeight >= FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(2))
                || Player.GetPosY() + Player.Speed - GameParam.OneAreaHeight >= FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY()).GetPosY() && Keyboard.GetState().IsKeyDown(Player.GetKey(3))))
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
        private void CheckMoveLeft(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY());
            int distance = Player.GetPosX() - (closerObject.GetPosX() + GameParam.OneAreaWidth);

            if (control != 0 &&
                (Player.GetPosX() - Player.Speed + GameParam.OneAreaWidth < FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(0))
                || Player.GetPosX() - Player.Speed + GameParam.OneAreaWidth < FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(1))))
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
        private void CheckMoveRight(int control, Player Player)
        {
            GameObject closerObject = FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY());
            int distance = closerObject.GetPosX() - (Player.GetPosX() + GameParam.OneAreaWidth);

            if (control != 0 &&
                (Player.GetPosX() + Player.Speed - GameParam.OneAreaWidth > FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(0))
                || Player.GetPosX() + Player.Speed - GameParam.OneAreaWidth > FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY()).GetPosX() && Keyboard.GetState().IsKeyDown(Player.GetKey(1))))
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

        private GameObject FindObjectWithHigherX(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(GameParam.WindowWidth, 0, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() >= posX + GameParam.OneAreaWidth && object1.GetPosX() <= objectReturn.GetPosX() && object1.CheckColisionHeight(posY, posY + GameParam.OneAreaHeight, GameParam.AreaYShade))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        } //3
        private GameObject FindObjectWithLowerX(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() + GameParam.OneAreaWidth <= posX && object1.GetPosX() >= objectReturn.GetPosX() && object1.CheckColisionHeight(posY, posY + GameParam.OneAreaHeight, GameParam.AreaYShade))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }
        private GameObject FindObjectWithHigherY(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, GameParam.WindowHeight, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() >= posY + GameParam.OneAreaHeight && object1.GetPosY() <= objectReturn.GetPosY() && object1.CheckColisionWidth(posX, posX + GameParam.OneAreaWidth))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }
        private GameObject FindObjectWithLowerY(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0, 1, 1);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() + GameParam.OneAreaHeight <= posY && object1.GetPosY() >= objectReturn.GetPosY() && object1.CheckColisionWidth(posX, posX + GameParam.OneAreaWidth))
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }

        private void MoveInAnotherDirection(Player Player, GameObject closerObject, bool UpOrDownTrue)
        {
            if (UpOrDownTrue)
            {
                int position = Player.GetPosX() - closerObject.GetPosX();
                if (position < 0 && FindObjectWithLowerX(closerObject.GetPosX(), closerObject.GetPosY()).GetPosX() + GameParam.OneAreaWidth < closerObject.GetPosX())
                {
                    CheckMovePlayer(3, Player);
                }
                else if (position > 0 && FindObjectWithHigherX(closerObject.GetPosX(), closerObject.GetPosY()).GetPosX() - GameParam.OneAreaWidth > closerObject.GetPosX())
                {
                    CheckMovePlayer(4, Player);
                }
            }
            else
            {
                int position = Player.GetPosY() - closerObject.GetPosY();
                if (position < 0 && FindObjectWithLowerY(closerObject.GetPosX(), closerObject.GetPosY()).GetPosY() + GameParam.OneAreaHeight < closerObject.GetPosY())
                {
                    CheckMovePlayer(1, Player);
                }
                else if (position > 0 && FindObjectWithHigherY(closerObject.GetPosX(), closerObject.GetPosY()).GetPosY() - GameParam.OneAreaHeight > closerObject.GetPosY())
                {
                    CheckMovePlayer(2, Player);
                }
            }
        } //4
        // end try move
    }
}

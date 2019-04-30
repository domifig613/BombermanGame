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
        private List<Texture2D> PowerupsTexturesBomb = new List<Texture2D>();


        private Player Player;

        private List<GameObject> ObjectToDraw = new List<GameObject>();

        public Map()
        {
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

            Player = new Player(GameParam.OneAreaWidth, GameParam.OneAreaHeight, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, GameParam.CharacterSlowerAnimation);

            ObjectToDraw.Add(Player);
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

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, 1f));
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

                ObjectToDraw.Add(new Box(RandPosX, RandPosY, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, 0.1f));
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
                    PowerupsTexturesBomb.Add(texture);
                    break;
                default:
                    break;
            }
        }

        public void LoadTexturePlayer(Texture2D texture, int x, int y)
        {
            Player.SetTexture(texture, x, y);
        }//one time at start load texture

        public void DrawMap(SpriteBatch Batch)
        {
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1 is Box)
                {
                    Box box1 = (Box)object1;
                    if (box1.GetChanceToDrop() == 1f)
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
                    Batch.Draw(Player.GetTexture(), object1.GetRectangle(), Color.White);
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
                else if(object1 is Fire)
                {
                    Fire fire1 = (Fire)object1;
                    Batch.Draw(FireTextures[fire1.GetTextureNumber()], object1.GetRectangle(), Color.White);
                }
                else if(object1 is Powerups)
                {
                    Powerups powerups1 = (Powerups)object1;
                    Batch.Draw(PowerupsTexturesBomb[powerups1.getNumberTexture()], object1.GetRectangle(), Color.White);
                }
                else
                {
                    Batch.Draw(MiddleRockTexture, object1.GetRectangle(), Color.White);
                }
            }
        }

        public void SortObjectToDraw()
        {
            Character character1=null;
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if(ObjectToDraw[i] is Character)
                {
                    character1 = (Character)ObjectToDraw[i];
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                }
            }
            ObjectToDraw.Sort((x, y) => x.GetPosY().CompareTo(y.GetPosY()));
            if (character1 != null)
            {
                for (int i = 0; i < ObjectToDraw.Count; i++)
                {
                    if (ObjectToDraw[i].GetPosY() > character1.GetPosY())
                    {
                        ObjectToDraw.Insert(i, character1);
                        break;
                    }
                }
            }
        }

        //{     //try move for every object
        private void CheckMovePlayer(int control)
        {
            if (Keyboard.GetState().IsKeyDown(Player.GetKey(0)) && control == 0 || control == 1)//up
            {
                CheckMoveUp(control);
            }
            else if (Keyboard.GetState().IsKeyDown(Player.GetKey(1)) && control == 0 || control == 2)//down
            {
                CheckMoveDown(control);
            }
            else if (Keyboard.GetState().IsKeyDown(Player.GetKey(2)) && control == 0 || control == 3)//left
            {
                CheckMoveLeft(control);
            }
            else if (Keyboard.GetState().IsKeyDown(Player.GetKey(3)) && control == 0 || control == 4)//right
            {
                CheckMoveRight(control);
            }
        } //1

        public void MapChanges()
        {
            Player.shortenTheDelay();
            PutBomb();

            CheckMovePlayer(0);

            CheckTimerBomb();
            ChceckTimeFire();
            CheckBurnObject();
        }

        private void CheckBurnObject()
        {
            List<Fire> fire = new List<Fire>();
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if (ObjectToDraw[i] is Fire)
                {
                    Fire fire1 = (Fire)ObjectToDraw[i];
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                    fire.Add(fire1);
                }
            }
            for(int i= fire.Count-1; i>=0; i--)
            {
                fire[i].BurnObject(ref ObjectToDraw, GameParam.AreaYShade);
            }
            for(int i=fire.Count-1; i>=0; i--)
            {
                ObjectToDraw.Add(fire[i]);
            }
        }

        private void ChceckTimeFire()
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
                    powerups1.setNextTexture();
                }
            }
        }

        private void PutBomb()
        {
            if (Keyboard.GetState().IsKeyDown(Player.GetKey(4)) && Player.actuallDelayBomb == 0)
            {
                ObjectToDraw.Add(Player.PutBomb(GameParam.OneAreaWidth, GameParam.OneAreaHeight, GameParam.AreaYShade));
            }            
        }

        private void CheckTimerBomb()
        {
            for(int i=ObjectToDraw.Count-1; i>=0; i--)
            { 
                if(ObjectToDraw[i] is Bomb)
                {
                    Bomb bomb1 = (Bomb)ObjectToDraw[i];
                    bomb1.shortenTheTimer();
                    if (bomb1.getTimer() % ((int)(bomb1.getMaxTime() / (BombTextures.Count))+1) == 0 && bomb1.getMaxTime() != bomb1.getTimer())
                    {
                        bomb1.SetNextTexture();
                    }
                    if (bomb1.checkDestroyTimer())
                    {
                        int[] DistanceArray= {0,0,0,0 };
                        DistanceArray[0] = bomb1.GetPosY() - FindObjectWithLowerY(bomb1.GetPosX(), bomb1.GetPosY()).GetPosY();
                        DistanceArray[1] = FindObjectWithHigherY(bomb1.GetPosX(), bomb1.GetPosY()).GetPosY()- bomb1.GetPosY();
                        DistanceArray[2] = bomb1.GetPosX() - FindObjectWithLowerX(bomb1.GetPosX(), bomb1.GetPosY()).GetPosX();
                        DistanceArray[3] = FindObjectWithHigherX(bomb1.GetPosX(), bomb1.GetPosY()).GetPosX() - bomb1.GetPosX();

                        bool[] Destroyable = {false,false,false,false};
                        if(FindObjectWithLowerY(bomb1.GetPosX(), bomb1.GetPosY()) is Box || FindObjectWithLowerY(bomb1.GetPosX(), bomb1.GetPosY()) is Character || FindObjectWithLowerY(bomb1.GetPosX(), bomb1.GetPosY()) is Bomb)
                        {
                            Destroyable[0] = true;
                        }
                        if (FindObjectWithHigherY(bomb1.GetPosX(), bomb1.GetPosY()) is Box || FindObjectWithHigherY(bomb1.GetPosX(), bomb1.GetPosY()) is Character || FindObjectWithHigherY(bomb1.GetPosX(), bomb1.GetPosY()) is Bomb)
                        {
                            Destroyable[1] = true;
                        }
                        if (FindObjectWithLowerX(bomb1.GetPosX(), bomb1.GetPosY()) is Box || FindObjectWithLowerX(bomb1.GetPosX(), bomb1.GetPosY()) is Character || FindObjectWithLowerX(bomb1.GetPosX(), bomb1.GetPosY()) is Bomb)
                        {
                            Destroyable[2] = true;
                        }
                        if (FindObjectWithHigherX(bomb1.GetPosX(), bomb1.GetPosY()) is Box || FindObjectWithHigherX(bomb1.GetPosX(), bomb1.GetPosY()) is Character || FindObjectWithHigherX(bomb1.GetPosX(), bomb1.GetPosY()) is Bomb)
                        {
                            Destroyable[3] = true;
                        }

                        bomb1.DestroyBombCreateFire(ref ObjectToDraw, GameParam.OneAreaWidth, GameParam.OneAreaHeight, DistanceArray, Destroyable);
                        ObjectToDraw.RemoveAt(i);
                    }
                }    
            }
        }
        private void CheckMoveUp(int control)
        {
            GameObject closerObject = FindObjectWithLowerY(Player.GetPosX(), Player.GetPosY());
            int distance = Player.GetPosY() - (closerObject.GetPosY() + GameParam.OneAreaHeight);

            if (distance > Player.Speed || closerObject is Fire)
            {
                Player.MoveCharacter(0, -Player.Speed, 0);
            }
            else if (distance > 0 || closerObject is Fire)
            {
                Player.MoveCharacter(0, -distance, 0);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, true);
            }
        } //2
        private void CheckMoveDown(int control)
        {
            GameObject closerObject = FindObjectWithHigherY(Player.GetPosX(), Player.GetPosY());
            int distance = closerObject.GetPosY() - (Player.GetPosY() + GameParam.OneAreaHeight);

            if (distance > Player.Speed || closerObject is Fire)
            {
                Player.MoveCharacter(0, Player.Speed, 3);
            }
            else if (distance > 0 || closerObject is Fire)
            {
                Player.MoveCharacter(0, distance, 3);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, true);
            }
        }
        private void CheckMoveLeft(int control)
        {
            GameObject closerObject = FindObjectWithLowerX(Player.GetPosX(), Player.GetPosY());
            int distance = Player.GetPosX() - (closerObject.GetPosX() + GameParam.OneAreaWidth);
            if (distance > Player.Speed || closerObject is Fire)
            {
                Player.MoveCharacter(-Player.Speed, 0, 6);
            }
            else if (distance > 0 || closerObject is Fire)
            {
                Player.MoveCharacter(-distance, 0, 6);
            }
            else if (control == 0)
            {
                MoveInAnotherDirection(Player, closerObject, false);

            }
        }
        private void CheckMoveRight(int control)
        {
            GameObject closerObject = FindObjectWithHigherX(Player.GetPosX(), Player.GetPosY());
            int distance = closerObject.GetPosX() - (Player.GetPosX() + GameParam.OneAreaWidth);
            if (distance > Player.Speed || closerObject is Fire)
            {
                Player.MoveCharacter(Player.Speed, 0, 9);
            }
            else if (distance > 0 || closerObject is Fire)
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

        private void MoveInAnotherDirection(GameObject moving, GameObject closerObject, bool UpOrDownTrue)
        {
            if (UpOrDownTrue)
            {
                int position = Player.GetPosX() - closerObject.GetPosX();
                if (position < 0 && FindObjectWithLowerX(closerObject.GetPosX(), closerObject.GetPosY()).GetPosX() + GameParam.OneAreaWidth < closerObject.GetPosX())
                {
                    CheckMovePlayer(3);
                }
                else if (position > 0 && FindObjectWithHigherX(closerObject.GetPosX(), closerObject.GetPosY()).GetPosX() - GameParam.OneAreaWidth > closerObject.GetPosX())
                {
                    CheckMovePlayer(4);
                }
            }
            else
            {
                int position = Player.GetPosY() - closerObject.GetPosY();
                if (position < 0 && FindObjectWithLowerY(closerObject.GetPosX(), closerObject.GetPosY()).GetPosY() + GameParam.OneAreaHeight < closerObject.GetPosY())
                {
                    CheckMovePlayer(1);
                }
                else if (position > 0 && FindObjectWithLowerY(closerObject.GetPosX(), closerObject.GetPosY()).GetPosY() - GameParam.OneAreaHeight < closerObject.GetPosY())
                {
                    CheckMovePlayer(2);
                }
            }
        } //4
        // end try move
    }
}

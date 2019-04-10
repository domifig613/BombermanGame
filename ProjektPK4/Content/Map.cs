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
    public class Map
    {
        Random rnd = new Random();

        ProgramParameters GameParam = new ProgramParameters();


        private Texture2D RockTexture;//Rock texture
        private Texture2D MiddleRockTexture;//Rock2 texture
        private Texture2D BoxTexture;//Box texture
        private Texture2D BoxWithBetterDropTexture;

        private GameObject[,] Up_Down_Wall;//up down walls GameObjects
        private GameObject[,] Left_Right_Wall;//left right walls GameObjects
        private GameObject[,] MiddleRock; // GameObject in middle

        private Box[,] Boxes;
        private Box[] PremiumBoxes;

        private Player Player;

        private List<GameObject> ObjectToDraw = new List<GameObject>();

        public Map()
        {
          
            Up_Down_Wall = new GameObject[GameParam.WindowWidth/GameParam.OneAreaWidth, 2];
            Left_Right_Wall = new GameObject[GameParam.WindowHeight/ GameParam.OneAreaHeight - 2, 2];
            MiddleRock = new GameObject[GameParam.WindowWidth / GameParam.OneAreaWidth / 2-1, GameParam.WindowHeight / GameParam.OneAreaHeight / 2-1];

            Boxes = new Box[GameParam.WindowWidth / GameParam.OneAreaWidth / 4*3, GameParam.WindowHeight / GameParam.OneAreaHeight / 4*3];
            PremiumBoxes = new Box[15];

            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth; i++) // create up and down wall
            {
                Up_Down_Wall[i, 0] = new GameObject(GameParam.OneAreaWidth * i, 0, GameParam.OneAreaWidth, GameParam.OneAreaHeight+GameParam.AreaYShade); //0 because window start x=0 y=0 (up wall)
                Up_Down_Wall[i, 1] = new GameObject(GameParam.OneAreaWidth * i, GameParam.WindowHeight-GameParam.OneAreaHeight-GameParam.AreaYShade, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade); //down wall
            }

            for (int i = 1; i < GameParam.WindowHeight / GameParam.OneAreaHeight - 1; i++) //create left and right wall
            {
                Left_Right_Wall[i-1, 0] = new GameObject(0, GameParam.OneAreaHeight * i, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade);//left wall
                Left_Right_Wall[i-1, 1] = new GameObject(GameParam.WindowWidth-GameParam.OneAreaWidth, GameParam.OneAreaHeight * i, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade);//right wall
            }

            for(int i=0; i< GameParam.WindowWidth / GameParam.OneAreaWidth / 2 - 1; i++)
            {
                for(int j=0; j< GameParam.WindowHeight / GameParam.OneAreaHeight / 2 - 1; j++)
                {
                    MiddleRock[i, j] = new GameObject(((i*2)+2)* GameParam.OneAreaWidth, ((j*2)+2)* GameParam.OneAreaHeight, GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade);
                }
            }

            for(int i=0; i< GameParam.WindowWidth / GameParam.OneAreaWidth / 4*3; i++)
            {
                for(int j=0; j< GameParam.WindowHeight / GameParam.OneAreaHeight / 4*3; j++)
                {
                    int RandPosX, RandPosY;
                    do
                    {
                        RandPosX = rnd.Next((GameParam.WindowWidth / GameParam.OneAreaWidth)-2);//random position x box
                        RandPosX = RandPosX * GameParam.OneAreaWidth +( 1* GameParam.OneAreaWidth);

                        RandPosY = rnd.Next((GameParam.WindowHeight / GameParam.OneAreaHeight) - 2);//random position y box
                        RandPosY = (RandPosY * GameParam.OneAreaHeight + (1* GameParam.OneAreaHeight))+0;//+0 because window start 0
                    }
                    while (CheckBoxPos(RandPosX, RandPosY));

                    Boxes[i, j] = new Box(RandPosX, RandPosY,  GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, 0.1f);
                }
            }


            for(int i=0; i<PremiumBoxes.Length; i++)
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

                PremiumBoxes[i] = new Box(RandPosX, RandPosY,GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, 1f);
            }

            Player = new Player(GameParam.OneAreaWidth, GameParam.OneAreaHeight,GameParam.OneAreaWidth, GameParam.OneAreaHeight + GameParam.AreaYShade, GameParam.CharacterSlowerAnimation);

            AddObjectToDrawAndSort();
        }

        bool CheckBoxPos(int posX, int posY)
        {
            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth / 4*3; i++)
            {
                for (int j = 0; j < GameParam.WindowHeight / GameParam.OneAreaHeight / 4*3; j++)
                {
                    if ((Boxes[i, j] != null && Boxes[i, j].GetPosX() == posX && Boxes[i, j].GetPosY() == posY))
                    {
                        return true;
                    }
                }
            }
            for(int i=0; i<PremiumBoxes.Length; i++)
            {
                if ((PremiumBoxes[i] != null && PremiumBoxes[i].GetPosX() == posX && PremiumBoxes[i].GetPosY() == posY))
                {
                    return true;
                }

            }
            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth / 2 - 1; i++)
            {
                for (int j = 0; j < GameParam.WindowHeight / GameParam.OneAreaHeight / 2 - 1; j++)
                {
                    if(MiddleRock[i,j].GetPosX() == posX && MiddleRock[i,j].GetPosY() == posY)
                    {
                        return true;
                    }
                }
            }

            if(posX < (GameParam.StartPositionSize + 1) *GameParam.OneAreaWidth || posX >= GameParam.WindowWidth - (GameParam.StartPositionSize + 1) * GameParam.OneAreaWidth)//true if box is on start positin
            {
                if (posY < (GameParam.StartPositionSize + 1) * GameParam.OneAreaHeight || posY >= GameParam.WindowHeight - (((GameParam.StartPositionSize + 1) * GameParam.OneAreaHeight)+GameParam.AreaYShade))
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
            foreach(GameObject object1 in ObjectToDraw)
            {
                if(object1.GetType() == typeof(Box))
                {
                    var AnotherBox = CastToType<Box>(object1);
                    if (AnotherBox.GetChanceToDrop() == 1f)
                    {
                        Batch.Draw(BoxWithBetterDropTexture, object1.GetRectangle(), Color.White);

                    }
                    else
                    {
                        Batch.Draw(BoxTexture, object1.GetRectangle(), Color.White);
                    }
                }
                else if (object1.GetType() == typeof(Player))
                {
                    Batch.Draw(Player.GetTexture(), Player.GetRectangle(), Color.White);
                }
                else if (object1.GetPosX() >= GameParam.WindowWidth-GameParam.OneAreaWidth || object1.GetPosX() < GameParam.OneAreaWidth || object1.GetPosY() >= GameParam.WindowHeight - (GameParam.OneAreaWidth + GameParam.AreaYShade)|| object1.GetPosY() < GameParam.OneAreaHeight)
                {
                    Batch.Draw(RockTexture, object1.GetRectangle(), Color.White);
                }
                else
                {
                    Batch.Draw(MiddleRockTexture, object1.GetRectangle(), Color.White);
                }
            }
        }

        public T CastToType<T>(object objType) where T : class
        {
            var cast = objType as T;

            if (cast == null)
                throw new InvalidCastException();

            return cast;
        }

        public void AddObjectToDrawAndSort()
        {
            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth; i++)
            {//draw up wall
                ObjectToDraw.Add(Up_Down_Wall[i,0]);
                ObjectToDraw.Add(Up_Down_Wall[i, 1]);

            }

            for (int i = 0; i < GameParam.WindowHeight / GameParam.OneAreaHeight - 2; i++)
            {//draw left right wall
                ObjectToDraw.Add(Left_Right_Wall[i, 0]);
                ObjectToDraw.Add(Left_Right_Wall[i, 1]);
            }

            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth / 2 - 1; i++)
            {
                for (int j = 0; j < GameParam.WindowHeight / GameParam.OneAreaHeight / 2 - 1; j++)
                {
                    ObjectToDraw.Add(MiddleRock[i, j]);
                }
            }

            for (int i = 0; i < GameParam.WindowWidth / GameParam.OneAreaWidth / 4 * 3; i++)
            {
                for (int j = 0; j < GameParam.WindowHeight / GameParam.OneAreaHeight / 4 * 3; j++)
                {
                    ObjectToDraw.Add(Boxes[i, j]);
                }
            }

            for (int i = 0; i < PremiumBoxes.Length; i++)
            {
                ObjectToDraw.Add(PremiumBoxes[i]);
            }

            ObjectToDraw.Add(Player);
            SortObjectToDraw();
        }

        public void SortObjectToDraw()
        {
            ObjectToDraw.Sort((x, y) => x.GetPosY().CompareTo(y.GetPosY()));
        }

        public void CheckMovePlayer(int control)
        {
            if (Keyboard.GetState().IsKeyDown(Player.GetKey(0)) || control == 1)
            {
                int distance = Player.GetPosY() - FindYofObjectWithLowerY(Player.GetPosY());

                if (distance > Player.Speed)
                {
                    Player.MoveCharacter(0, -Player.Speed, 0);
                }
                else if(distance>0)
                {
                    Player.MoveBody(0, -distance);
                }
                else if (control == 0)
                {
                    int position = Player.GetPosX() % GameParam.OneAreaWidth;
                    if(position < GameParam.OneAreaWidth/3 && position > 0)
                    {
                        CheckMovePlayer(3);
                    }
                    else if (position > GameParam.OneAreaWidth  - (2 * GameParam.OneAreaWidth / 3))
                    {
                        CheckMovePlayer(4);
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Player.GetKey(1)) || control == 2)
            {
                int distance = FindYofObjectWithHigherY(Player.GetPosY()) - (Player.GetPosY()+GameParam.OneAreaHeight);

                if (distance > Player.Speed)
                {
                    Player.MoveCharacter(0, Player.Speed, 3);
                }
                else if (distance > 0)
                {
                    Player.MoveBody(0, distance);
                }
                else if (control == 0)
                {
                    int position = Player.GetPosX() % GameParam.OneAreaWidth;
                    if (position < GameParam.OneAreaWidth / 3 && position > 0)
                    {
                        CheckMovePlayer(3);
                    }
                    else if(position > GameParam.OneAreaWidth - (2 * GameParam.OneAreaWidth / 3))
                    {
                        CheckMovePlayer(4);
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Player.GetKey(2)) || control == 3)
            {
                int distance = Player.GetPosX() - FindXofObjectWithLowerX(Player.GetPosX());
                if (distance > Player.Speed)
                {
                    Player.MoveCharacter(-Player.Speed, 0, 6);
                }
                else if (distance > 0)
                {
                    Player.MoveBody(-distance, 0);
                }
                else if (control == 0)
                {
                    int position = Player.GetPosY() % GameParam.OneAreaHeight;
                    if (position < GameParam.OneAreaHeight / 3 && position > 0)
                    {
                        CheckMovePlayer(1);
                    }
                    else if (position > GameParam.OneAreaHeight - (2 * GameParam.OneAreaHeight / 3))
                    {
                        CheckMovePlayer(2);
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Player.GetKey(3)) || control == 4)
            {
                int distance = FindXofObjectWithHigerX(Player.GetPosX()) - (Player.GetPosX()+GameParam.OneAreaWidth);
                if (distance > Player.Speed)
                {
                    Player.MoveCharacter(Player.Speed, 0, 9);
                }
                else if (distance > 0)
                {
                    Player.MoveBody(distance, 0);
                }
                else if(control == 0)
                {
                    int position = Player.GetPosY() % GameParam.OneAreaHeight;
                    if (position < GameParam.OneAreaHeight / 3 && position > 0)
                    {
                        CheckMovePlayer(1);
                    }
                    else if (position > GameParam.OneAreaHeight - ( 2* GameParam.OneAreaHeight / 3))
                    {
                        CheckMovePlayer(2);
                    }
                }
            }
        }

        public int FindXofObjectWithHigerX(int posX)
        {
            int NextX = GameParam.WindowWidth;
            foreach(GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() >= posX+GameParam.OneAreaWidth && object1.GetPosX() <= NextX && object1.CheckColisionHeight(Player.GetPosY(), Player.GetPosY() + GameParam.OneAreaHeight, GameParam.AreaYShade))
                {
                    NextX = object1.GetPosX();
                }
            }
            return NextX;
        }
        public int FindXofObjectWithLowerX(int posX)
        {
            int NextX = 0;
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() < posX && object1.GetPosX() >= NextX && object1.CheckColisionHeight(Player.GetPosY(), Player.GetPosY() + GameParam.OneAreaHeight, GameParam.AreaYShade))
                {
                    NextX = object1.GetPosX()+GameParam.OneAreaWidth;
                }
            }
            return NextX;
        }
        public int FindYofObjectWithHigherY(int posY)
        {
            int NextY = GameParam.WindowHeight;
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() >= posY+GameParam.OneAreaHeight && object1.GetPosY() <= NextY && object1.CheckColisionWidth(Player.GetPosX(), Player.GetPosX() + GameParam.OneAreaWidth))
                {
                    NextY = object1.GetPosY();
                }
            }
            return NextY;
        }
        public int FindYofObjectWithLowerY(int posY)
        {
            int NextY=0;
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() < posY && object1.GetPosY() >= NextY && object1.CheckColisionWidth(Player.GetPosX(), Player.GetPosX() + GameParam.OneAreaWidth))
                {
                    NextY = object1.GetPosY()+GameParam.OneAreaHeight;
                }
            }
            return NextY;
        }
    }
}

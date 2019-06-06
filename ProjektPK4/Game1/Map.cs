using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace ProjektPK4.game
{
    static class Map
    {
        static Random rnd = new Random();

        static private Texture2D RockTexture;//Rock texture
        static private Texture2D MiddleRockTexture;//Rock2 texture
        static private Texture2D BoxTexture;//Box texture
        static private Texture2D BoxWithBetterDropTexture;//premium box texture
        static private List<Texture2D> BombTextures;//bomb texture
        static private List<Texture2D> FireTextures;
        static private List<List<Texture2D>> PowerupsTextures;
        static private List<List<List<Texture2D>>> CharacterTextures;
        static private List<Character> Player;
        static private List<GameObject> ObjectToDraw;
        static private SoundEffect PowerupsPick;
        static private SoundEffect BombDestroy;
        static private int DelayBeforeRestartMap = -10;
        readonly static private int[] CharacterType = new int[4];
        static private int FramesToApocalypse = 3600;
        static private bool IsPlayerInGame = true;

        static Map()
        {
            Player = new List<Character>();
            ObjectToDraw = new List<GameObject>();
            BombTextures = new List<Texture2D>();
            FireTextures = new List<Texture2D>();
            PowerupsTextures = new List<List<Texture2D>>();
            CharacterTextures = new List<List<List<Texture2D>>>();

            TexturesPowerupsAndPlayersAdd();

        }

        static public void LoadSoundEffect(SoundEffect powerup, SoundEffect bomb)
        {
            PowerupsPick = powerup;
            BombDestroy = bomb;
        }

        static public void SetCharacterType(int[] _characterType)
        {
          
            for(int i=0; i<_characterType.Length; i++)
            {
                CharacterType[i] = _characterType[i];
            }
            CreateOrResetMap();
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

            Keys[][] PlayersKeys = new Keys[4][];
            PlayersKeys[0] = new Keys[] { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.M};
            PlayersKeys[1] = new Keys[] { Keys.W, Keys.S, Keys.A, Keys.D, Keys.C };
            PlayersKeys[2] = new Keys[] { Keys.U, Keys.J, Keys.H, Keys.K, Keys.O };
            PlayersKeys[3] = new Keys[] { Keys.NumPad8, Keys.NumPad5, Keys.NumPad4, Keys.NumPad6, Keys.NumPad0 };

            int[][] PlayersPosition = new int[4][];
            PlayersPosition[0] = new int[] { ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight };
            PlayersPosition[1] = new int[] {ProgramParameters.WindowWidth-2* ProgramParameters.OneAreaWidth,ProgramParameters.WindowHeight -2* ProgramParameters.OneAreaHeight - ProgramParameters.AreaYShade };
            PlayersPosition[2] = new int[] { ProgramParameters.OneAreaWidth, ProgramParameters.WindowHeight - 2 * ProgramParameters.OneAreaHeight-ProgramParameters.AreaYShade };
            PlayersPosition[3] = new int[] { ProgramParameters.WindowWidth - 2 * ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight };

            for (int i = 0; i < 4; i++)
            {
                if (CharacterType[i] == 1)
                {
                    Player.Add(new Player(PlayersPosition[i][0], PlayersPosition[i][1], PlayersKeys[i], i+1));
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            Player[Player.Count - 1].SetTexture(CharacterTextures[i][j][k], j, k);
                        }
                    }
                }
                else if (CharacterType[i] == 2)
                {
                    Player.Add(new AI(PlayersPosition[i][0], PlayersPosition[i][1], i+1));
                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            Player[Player.Count-1].SetTexture(CharacterTextures[i][j][k], j, k);
                        }
                    }
                }
            }

            for (int i = 0; i < Player.Count; i++)
            {
                ObjectToDraw.Add(Player[i]);
            }
            SortObjectToDraw();

            FramesToApocalypse = 3600;
            IsPlayerInGame = true;
        }

        public static void ApocalypseCondition(int framesToNextBomb)
        {
            if (FramesToApocalypse <= 0)
            {
                FramesToApocalypse = framesToNextBomb;
                int x =rnd.Next(1, ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth);
                int y = rnd.Next(1, ProgramParameters.WindowHeight / ProgramParameters.OneAreaHeight);
                x *= ProgramParameters.OneAreaWidth;
                y *= ProgramParameters.OneAreaHeight;
                bool freeSpace = true;

                foreach(GameObject go1 in ObjectToDraw)
                {
                    if (go1.GetPosX() == x && go1.GetPosY() == y)
                        freeSpace = false;
                }

                if(freeSpace)
                    ObjectToDraw.Add(new Bomb(x, y, 5));
            }
            else
            {
                FramesToApocalypse--;
            }
            
        }

        private static void TexturesPowerupsAndPlayersAdd()
        {
            for (int i = 0; i < 4; i++)//powerups type(4)
            {
                PowerupsTextures.Add(new List<Texture2D>());
            }
            
            for(int i=0; i<4; i++)
            {
                CharacterTextures.Add(new List<List<Texture2D>>());

                for (int j = 0; j < 3; j++)
                {
                    CharacterTextures[i].Add(new List<Texture2D>());
                }
            }
       
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
                case 10:
                    Character.SetGhostTexture(texture);
                    break;
                default:
                    break;
            }
        }

        static public void LoadTexturePlayer(Texture2D texture, int x, int y, int CharacterNumber)
        {
            CharacterTextures[CharacterNumber][x].Add(texture);
        }//one time at start load texture

        static private void AddTextureToPlayer()
        {
            for(int i=0;i<4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Player[i].SetTexture(CharacterTextures[i][j][k], j, k);
                    }
                }
            }
        }

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
                else if (object1 is Character character1)
                {
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
                    if (powerups1.GetIndestructible() <= 0)
                    {
                        Batch.Draw(PowerupsTextures[powerups1.GetTypePowerups() - 1][powerups1.GetNumberTexture()], object1.GetRectangle(), Color.White);
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
            List<Fire> TemporaryListFire = new List<Fire>(); // list for fire and powerups
            List<Powerups> TemporaryListPowerups = new List<Powerups>(); // list for fire and powerups


            ChceckTimerObjects();//change texture and time of object
            AddToTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups);//take powerups and fire from objectToDraw
            CheckBurnObject(ref TemporaryListFire, ref TemporaryListPowerups);//fire objectToDraw or powerups if colision

            PutBomb();//if player press key to put bomb
            Move(ref TemporaryListFire, ref TemporaryListPowerups);

            TryPickPowerUp(ref TemporaryListPowerups);//check player position and powerups and try pick powerups
            RemoveFromTemporaryList(ref TemporaryListFire, ref TemporaryListPowerups); // put back powerups and fire to objectToDraw

            CheckWinCondition();

            CheckAnyPlayerInGame();
        }

        private static void CheckAnyPlayerInGame()//if every player die, every ai have only one life
        {
            if (IsPlayerInGame)
            {
                foreach (Character c1 in Player)
                {
                    if (c1 is Player)
                    {
                        return;
                    }
                }
                foreach (AI ai1 in Player)
                {
                    ai1.SetLife(1);
                }
                IsPlayerInGame = false;
            }
        }

        public static void DelayScreen()
        {
            if(DelayBeforeRestartMap <= 120)
            {
                DelayBeforeRestartMap--;
            }
        }

        private static void CheckWinCondition()
        {
           if(Player.Count==1)
            {
                if(DelayBeforeRestartMap <= -10)
                {
                    DelayBeforeRestartMap = 120;
                }
                else if (DelayBeforeRestartMap <= 0)
                {
                    Scorebar.AddScore(Player[0].PlayerNumber - 1);
                    CreateOrResetMap();
                }
            }
           else if(Player.Count<=1)
            {
                if (DelayBeforeRestartMap <= -10)
                    DelayBeforeRestartMap = 120;
                else if (DelayBeforeRestartMap <= 0)
                    CreateOrResetMap();
            }
        }

        static private void Move(ref List<Fire> fire, ref List<Powerups> powerups)
        {
            foreach (Character player1 in Player)
            {
                GameObject[][] arrayOfCloserObjects;

                if (player1 is Player p1)
                {
                    arrayOfCloserObjects = new GameObject[5][];
                    FillCloserObjectForPlayer(ref arrayOfCloserObjects, player1.GetPosX(), player1.GetPosY());
                    p1.CheckMove(arrayOfCloserObjects);
                }
                else
                {
                    AI ai1 = (AI)player1;
                    char[][] arrayOfCloserObjects1;
                    arrayOfCloserObjects1 = new char[ProgramParameters.WindowHeight/ProgramParameters.OneAreaHeight][];
                    FillCloserObjectForAI(ref arrayOfCloserObjects1, player1.GetPosX(), player1.GetPosY(), ref fire, ref powerups);
                    GameObject[][] array2 = new GameObject[5][];
                    FillCloserObjectForPlayer(ref array2, player1.GetPosX(), player1.GetPosY());
                    ai1.CheckMove(arrayOfCloserObjects1, array2);
                }

            }
        }

        static private void FillCloserObjectForPlayer(ref GameObject[][] array, int posX, int posY )
        {
            for (int i = 0; i < 5; i++)
            {
                array[i] = new GameObject[4];

                for (int j = 0; j < 4; j++)
                {
                        int x, y;
                        Func<int,int, GameObject> getCloser = null;

                        if (i==0)
                        {
                            x = posX;
                            y = posY;
                        }
                        else
                        {
                            x = array[0][i - 1].GetPosX();
                            y = array[0][i-1].GetPosY();
                        }

                        switch (j)
                        {
                            case 0:
                                getCloser = FindObjectWithLowerX;
                                break;
                            case 1:
                                getCloser = FindObjectWithHigherX;
                                break;
                            case 2:
                                getCloser = FindObjectWithLowerY;
                                break;
                            case 3:
                                getCloser = FindObjectWithHigherY;
                                break;
                        }
             
                            array[i][j] = getCloser(x, y);     
                }
            }
        }

        static private void FillCloserObjectForAI(ref char[][] array, int posX, int posY,
            ref List<Fire> fire, ref List<Powerups> powerups)
        {
           for(int i=0; i<array.Length; i++)
           {
                array[i] = new char[ProgramParameters.WindowWidth / ProgramParameters.OneAreaWidth];

                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = 'W';

                    foreach(Fire object1 in fire)
                    {
                        if(object1.GetPosX() / ProgramParameters.OneAreaWidth == j
                             && object1.GetPosY() / ProgramParameters.OneAreaHeight == i)
                        {
                            array[i][j] = 'F';
                        }
                    }
                    if (array[i][j] == 'W')
                    {
                        foreach (GameObject object1 in ObjectToDraw)
                        {
                            if (object1.GetPosX() / ProgramParameters.OneAreaWidth == j
                             && object1.GetPosY() / ProgramParameters.OneAreaHeight == i)
                            {
                                if (object1 is Box)
                                {
                                    array[i][j] = 'X';
                                }
                                else if (object1 is Bomb)
                                {
                                    array[i][j] = 'B';
                                }
                                else if (object1 is Character)
                                {
                                    array[i][j] = 'C';
                                }
                                else if (object1 is GameObject)
                                {
                                    array[i][j] = 'R';
                                }
                                break;
                            }
                            else
                            {
                                array[i][j] = 'E';
                            }
                        }
                    }
                    if(array[i][j]=='E')
                    {
                        foreach (GameObject object1 in powerups)
                        {
                            if (object1.GetPosX() / ProgramParameters.OneAreaWidth == j
                           && object1.GetPosY() / ProgramParameters.OneAreaHeight == i)
                            {
                                array[i][j] = 'P';
                            }
                        }
                    }
                }
           }
        }

        static private void TryPickPowerUp(ref List<Powerups> powerups)
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
                        if (ProgramParameters.Get_MusicEnable())
                        {
                            PowerupsPick.Play(0.5f, 0f, 0f);
                        }

                    }
                }
            }
        }

        static private void AddToTemporaryList(ref List<Fire> fire, ref List<Powerups> powerups)
        {
            for (int i = ObjectToDraw.Count - 1; i >= 0; i--)
            {
                if (ObjectToDraw[i] is Fire fire1)
                {
                    fire.Add(fire1);
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                }
                else if (ObjectToDraw[i] is Powerups powerups1)
                {
                    powerups.Add(powerups1);
                    ObjectToDraw.Remove(ObjectToDraw[i]);
                }
            }
        }
        static private void RemoveFromTemporaryList(ref List<Fire> fire, ref List<Powerups> powerups)
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

        static private void CheckBurnObject(ref List<Fire> fire, ref List<Powerups> powerups)
        {
            for (int i = fire.Count - 1; i >= 0; i--)
            {
                List<GameObject> objectsTofire = new List<GameObject>();

                for(int j=ObjectToDraw.Count-1; j>=0; j--)
                {
                    if(((ObjectToDraw[j] is Character && ObjectToDraw[j].ChceckColision(fire[i].GetPosX(), fire[i].GetPosY())) 
                      || ObjectToDraw[j].GetPosX() == fire[i].GetPosX() && ObjectToDraw[j].GetPosY() == fire[i].GetPosY()))
                    {
                        objectsTofire.Add(ObjectToDraw[j]);
                        ObjectToDraw.Remove(ObjectToDraw[j]);
                    }
                }
                fire[i].BurnObject(ref objectsTofire);

                for (int j = objectsTofire.Count - 1; j >= 0; j--)
                {
                    ObjectToDraw.Add(objectsTofire[j]);
                    objectsTofire.Remove(objectsTofire[j]);
                }

                for (int j = powerups.Count - 1; j >= 0; j--)
                {
                    if (powerups[j].GetPosX() == fire[i].GetPosX() && powerups[j].GetPosY() == fire[i].GetPosY())
                    {
                        objectsTofire.Add(powerups[j]);
                        powerups.Remove(powerups[j]);
                    }
                }

                fire[i].BurnObject(ref objectsTofire);

                for (int j = objectsTofire.Count - 1; j >= 0; j--)
                {
                    powerups.Add((Powerups)objectsTofire[j]);
                    objectsTofire.Remove(objectsTofire[j]);
                }

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
                        if (ProgramParameters.Get_MusicEnable())
                        {
                            BombDestroy.Play(0.5f, 0f, 0f);
                        }
                    }
                }
            }

            foreach (Character Character1 in Player)
            {
                Character1.ShortenTheDelay();//Player shorten time to put bomb
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
            foreach (Character player1 in Player)
            {
                if (player1 is Player p1)
                {
                    if (Keyboard.GetState().IsKeyDown(p1.GetKey(4)) && p1.ChceckBombDelay())
                    {
                        ObjectToDraw.Add(p1.PutBomb(ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight, ProgramParameters.AreaYShade));
                    }
                }
                else if (player1 is AI ai1)
                {
                    if (ai1.GetLastMove() == 5 && ai1.ChceckBombDelay())
                    {
                        ObjectToDraw.Add(ai1.PutBomb(ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight, ProgramParameters.AreaYShade));
                    }
                }
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
        } 
        static private GameObject FindObjectWithHigherXFixed(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(ProgramParameters.WindowWidth, 0);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() >= posX + ProgramParameters.OneAreaWidth && object1.GetPosX() <= objectReturn.GetPosX() 
                    && object1.GetPosY() < posY && object1.GetPosY()+ProgramParameters.OneAreaHeight > posY)
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        } 

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
        static private GameObject FindObjectWithLowerXFixed(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosX() + ProgramParameters.OneAreaWidth <= posX && object1.GetPosX() >= objectReturn.GetPosX() 
                    && object1.GetPosY() < posY && object1.GetPosY() + ProgramParameters.OneAreaHeight > posY)
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
        static private GameObject FindObjectWithHigherYFixed(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, ProgramParameters.WindowHeight);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() >= posY + ProgramParameters.OneAreaHeight && object1.GetPosY() <= objectReturn.GetPosY()
                    && object1.GetPosX() < posY && object1.GetPosX() + ProgramParameters.OneAreaWidth > posX)
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
        static private GameObject FindObjectWithLowerYFixed(int posX, int posY)
        {
            GameObject objectReturn = new GameObject(0, 0);
            foreach (GameObject object1 in ObjectToDraw)
            {
                if (object1.GetPosY() + ProgramParameters.OneAreaHeight <= posY && object1.GetPosY() >= objectReturn.GetPosY()
                    && object1.GetPosX() < posY && object1.GetPosX() + ProgramParameters.OneAreaWidth > posX)
                {
                    objectReturn = object1;
                }
            }
            return objectReturn;
        }

    }
}

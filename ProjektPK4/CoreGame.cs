using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjektPK4.game;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;

namespace ProjektPK4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CoreGame : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;//help draw textures

        int GameState = 0; //0-start screen, 1-chose players, 2-game, 3-end screen
        int OldGameState=-1;
        Menu StartMenu;
        Menu EndGameMenu;
        SelectMenu SelectCharactersMenu;

        readonly Button[] SoundButtons = new Button[2];

        
        MouseState MouseState;
        MouseState OldMouseState;

        Song MenuSong;
        Song GameSong;

        public CoreGame()
        {
            this.Window.Title = "Pirate Bomberman 0.9";

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //frame rate 60

            Graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content"; //where we find content

            Graphics.PreferredBackBufferHeight = ProgramParameters.WindowHeight;
            Graphics.PreferredBackBufferWidth =  ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth;

            this.IsMouseVisible = true;

            OldMouseState = Mouse.GetState();

            MediaPlayer.IsRepeating = true;

            SoundButtons[0] = new Button(2, ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth - 50, 20, 30, 30);
            SoundButtons[1] = new Button(1, ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth - 50, 20, 30, 30);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() //load texture, before start game
        {
            LoadMusics();

            LoadTextures();
        }

        private void LoadMusics()
        {
            MenuSong = Content.Load<Song>("sound\\menu");
            GameSong = Content.Load<Song>("sound\\game");

            Map.LoadSoundEffect(Content.Load<SoundEffect>("sound\\powerup"), Content.Load<SoundEffect>("sound\\explosiv"));
        }

        private void LoadTextures()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTexturesToMap();
            LoadTexturesToScoreBar();
            LoadMenuTextures();

            StartMenu.LoadButtonSound(Content.Load<SoundEffect>("sound\\button"));
            SelectCharactersMenu.LoadButtonSound(Content.Load<SoundEffect>("sound\\button"));
            EndGameMenu.LoadButtonSound(Content.Load<SoundEffect>("sound\\button"));
        }

        private void LoadMenuTextures()
        {
            StartMenu = new Menu(Content.Load<Texture2D>("Sprites\\Menu1Back"), Content.Load<Texture2D>("Sprites\\PirateBomberman"));
            StartMenu.LoadTextureToButtons(Content.Load<Texture2D>("Sprites\\StartButton"), Content.Load<Texture2D>("Sprites\\QuitButton"));

            EndGameMenu = new Menu(Content.Load<Texture2D>("Sprites\\Menu1Back"), Content.Load<Texture2D>("Sprites\\GameOver"));
            EndGameMenu.LoadTextureToButtons(Content.Load<Texture2D>("Sprites\\StartButton"), Content.Load<Texture2D>("Sprites\\QuitButton"));

            List<Texture2D> charactersTextureList = new List<Texture2D>
            {
                Content.Load<Texture2D>("Sprites\\Pirate1Down1"),
                Content.Load<Texture2D>("Sprites\\Pirate2Down1"),
                Content.Load<Texture2D>("Sprites\\Pirate3Down1"),
                Content.Load<Texture2D>("Sprites\\Pirate4Down1")
            };

            Texture2D[] characterTypeTexures = new Texture2D[3];
            characterTypeTexures[0] = Content.Load<Texture2D>("Sprites\\Player");
            characterTypeTexures[1] = Content.Load<Texture2D>("Sprites\\Computer");
            characterTypeTexures[2] = Content.Load<Texture2D>("Sprites\\Disable");

            Texture2D[] KeysTextures = new Texture2D[4];
            KeysTextures[0] = Content.Load<Texture2D>("Sprites\\Keys1");
            KeysTextures[1] = Content.Load<Texture2D>("Sprites\\Keys2");
            KeysTextures[2] = Content.Load<Texture2D>("Sprites\\Keys3");
            KeysTextures[3] = Content.Load<Texture2D>("Sprites\\Keys4");


            SelectCharactersMenu = new SelectMenu(new Texture2D(GraphicsDevice, 1, 1), Content.Load<Texture2D>("Sprites\\SelectCharacters"), 
                new Texture2D(GraphicsDevice, 1, 1),charactersTextureList, Content.Load<Texture2D>("Sprites\\RightClickButton"), 
                Content.Load<Texture2D>("Sprites\\LeftClickButton"), characterTypeTexures, KeysTextures);

            SelectCharactersMenu.LoadTextureToButtons(Content.Load<Texture2D>("Sprites\\PlayButton"), Content.Load<Texture2D>("Sprites\\QuitButton"));

            SoundButtons[0].SetTexture(Content.Load<Texture2D>("Sprites\\MusicOn"));
            SoundButtons[1].SetTexture(Content.Load<Texture2D>("Sprites\\MusicOff"));
        }

        private void LoadTexturesToScoreBar()
        {
            Texture2D[] characterHead = new Texture2D[4];
            for(int i=1; i<=4; i++)
            {
                string name = "Sprites\\headScore" + i;
                characterHead[i-1] = Content.Load<Texture2D>(name);
            }

            Scorebar.LoadScoreboardTextures(characterHead,Content.Load<Texture2D>("Sprites\\chest"), Content.Load<Texture2D>("Sprites\\chestBack"), new Texture2D(GraphicsDevice, 1, 1));
        }

        private void LoadTexturesToMap()
        {
            

            string[] textureNameArray = { "End1", "Rock1", "Box1", "Box2", "Bomb1", "Bomb2",
                "Bomb3", "Bomb4" ,"Bomb5","Bomb6","Bomb7","Bomb8","Bomb9","Bomb10","Bomb11",
                "Bomb12","Bomb13","fireNormalStart","fireMiddleLeftRight","fireMiddleUpDown",
                "fireEndUp","fireEndDown","fireEndLeft","fireEndRight","fireStartDown1",
                "fireStartUp1","fireStartLeft1","fireStartRight1","fireStartDownLeft2",
                "fireStartDownRight2","fireStartUpLeft2","fireStartUpRight2", "DeadFire"};

            string name = "Sprites\\" + textureNameArray[0];
            Map.LoadTextureMap(Content.Load<Texture2D>(name), 0);
            name = "Sprites\\" + textureNameArray[1];
            Map.LoadTextureMap(Content.Load<Texture2D>(name), 1);
            name = "Sprites\\" + textureNameArray[2];
            Map.LoadTextureMap(Content.Load<Texture2D>(name), 2);
            name = "Sprites\\" + textureNameArray[3];
            Map.LoadTextureMap(Content.Load<Texture2D>(name), 3);
            
            for(int i=0; i<13; i++)
            {
                name = "Sprites\\" + textureNameArray[i+4];
                Map.LoadTextureMap(Content.Load<Texture2D>(name), 4);
            }

            for(int i=0; i<16; i++)
            {
                name = "Sprites\\" + textureNameArray[i + 17];
                Map.LoadTextureMap(Content.Load<Texture2D>(name), 5);
            }

            for (int i = 1; i <= 18; i++)
            {
                name = "Sprites\\" + "PowerupsBomb" + i.ToString();
                Map.LoadTextureMap(Content.Load<Texture2D>(name), 6);
            }

            for (int i = 1; i <= 16; i++)
            {
                name = "Sprites\\" + "PowerupsLighting" + i.ToString();
                Map.LoadTextureMap(Content.Load<Texture2D>(name), 7);
            }

            for (int i = 1; i <= 17; i++)
            {
                name = "Sprites\\" + "PowerupsPotion" + i.ToString();
                Map.LoadTextureMap(Content.Load<Texture2D>(name), 8);
            }

            for (int i = 1; i <= 15; i++)
            {
                name = "Sprites\\" + "PowerupsStar" + i.ToString();
                Map.LoadTextureMap(Content.Load<Texture2D>(name), 9);
            }

            string[] array = { "Up", "Down", "Left", "Right" };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string control = "Sprites\\Pirate" + 5 + array[j] + (i + 1);
                    Map.LoadTextureMap(Content.Load<Texture2D>(control), 10);
                }
            }


            LoadCharacterTexture();

        }

        private void LoadCharacterTexture()
        {
            string[] array = { "Up", "Down", "Left", "Right" };
            for (int k = 1; k <= 4; k++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        string control = "Sprites\\Pirate"+ k + array[j] + (i + 1);
                        Map.LoadTexturePlayer(Content.Load<Texture2D>(control), i, j, k-1);                
                    }
                }
               
                  
            }
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState = Mouse.GetState();

            UpdateGameState();
            PrepareGame();
            MusicControl();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || GameState == -1)//close game
            {
                Exit();
            }

            OldMouseState = MouseState;//update state
            OldGameState = GameState;

            base.Update(gameTime);
        }

        private void MusicControl()
        {
            if (!ProgramParameters.Get_MusicEnable())
            {
                MediaPlayer.Stop();
            }
            else if (GameState == 0 && OldGameState == -1 || GameState != 2 && OldGameState == 2 || MediaPlayer.State == MediaState.Stopped && GameState != 2)//launch game or end screen with game
            {
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.Play(MenuSong);

            }
            else if (GameState == 2 && OldGameState != 2 || MediaPlayer.State == MediaState.Stopped && GameState == 2)
            {
                MediaPlayer.Volume = 0.7f;
                MediaPlayer.Play(GameSong);
            }

            SoundButtonUpdate();
        }

        private void SoundButtonUpdate()
        {
            if (MouseState.LeftButton == ButtonState.Pressed && OldMouseState.LeftButton == ButtonState.Released)
            {
                if (ProgramParameters.Get_MusicEnable())
                {
                    ProgramParameters.MusicSwitch(Convert.ToBoolean(SoundButtons[0].CheckMoveInButtonPosition(MouseState.X, MouseState.Y, Convert.ToInt16(ProgramParameters.Get_MusicEnable()))));
                }
                else
                {
                    ProgramParameters.MusicSwitch(Convert.ToBoolean(SoundButtons[1].CheckMoveInButtonPosition(MouseState.X, MouseState.Y, Convert.ToInt16(ProgramParameters.Get_MusicEnable()))));
                }
            }

            if (ProgramParameters.Get_MusicEnable())
            {
                if (!SoundButtons[0].GetOnMoveState() && SoundButtons[0].CheckMoveInButtonPositionX(MouseState.X) && SoundButtons[0].CheckMoveInButtonPositionY(MouseState.Y))
                {
                    SoundButtons[0].SetOnMoveState(true);
                }
                else if (SoundButtons[0].GetOnMoveState())
                {
                    SoundButtons[0].SetOnMoveState(false);
                }
            }
            else
            {
                if (!SoundButtons[1].GetOnMoveState() && SoundButtons[1].CheckMoveInButtonPositionX(MouseState.X) && SoundButtons[1].CheckMoveInButtonPositionY(MouseState.Y))
                {
                    SoundButtons[1].SetOnMoveState(true);
                }
                else if (SoundButtons[1].GetOnMoveState())
                {
                    SoundButtons[1].SetOnMoveState(false);
                }
            }
        }

        private void PrepareGame()
        {
            if (GameState == 2 && OldGameState == 1)
            {
                Map.SetCharacterType(SelectCharactersMenu.GetCharacterPlay());
                Scorebar.SetCharacter(SelectCharactersMenu.GetCharacterPlay());
            }
        }

        private void UpdateGameState()
        {
            if (GameState == 2)
            {
                Map.MapChanges();
                Map.SortObjectToDraw();

                if (Scorebar.CheckWin())
                {
                    GameState++;
                }
            }
            else if (GameState == 0)
            {
                GameState = StartMenu.CheckKeysPressed(MouseState, OldMouseState, GameState);
            }
            else if (GameState == 1)
            {
                GameState = SelectCharactersMenu.CheckKeysPressed(MouseState, OldMouseState, GameState);
            }
            else if (GameState == 3)
            {
                GameState = EndGameMenu.CheckKeysPressed(MouseState, OldMouseState, GameState);
            }
        }

        protected override void Draw(GameTime gameTime) //draw
        {
            GraphicsDevice.Clear(Color.BurlyWood);

            SpriteBatch.Begin();
            // draw here

            switch (GameState)
            {
                case 0:
                    {
                        StartMenu.DrawMenu(SpriteBatch);
                        break;
                    }
                case 1:
                    {
                        SelectCharactersMenu.DrawMenu(SpriteBatch);
                        break;
                    }
                case 2:
                    {
                        Scorebar.DrawScorebar(SpriteBatch);
                        Map.DrawMap(SpriteBatch);
                        break;
                    }
                case 3:
                    {
                        EndGameMenu.DrawMenu(SpriteBatch);
                        break;
                    }
            }

            DrawSoundButton();

            SpriteBatch.End();

            base.Draw(gameTime);

            Map.ApocalypseCondition(60);//if map time so long put random bomb in map every 20 frames
            Map.DelayScreen();
        }

        private void DrawSoundButton()
        {
            if (ProgramParameters.Get_MusicEnable())
            {
                SoundButtons[0].DrawButton(SpriteBatch);
            }
            else
            {
                SoundButtons[1].DrawButton(SpriteBatch);
            }
        }
    }
}

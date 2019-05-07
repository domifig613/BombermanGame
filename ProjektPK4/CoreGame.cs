using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjektPK4.game;
using System;

namespace ProjektPK4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CoreGame : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;//help draw textures


        public CoreGame()
        {
           TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //frame rate 60

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; //where we find content

            graphics.PreferredBackBufferHeight = ProgramParameters.WindowHeight;
            graphics.PreferredBackBufferWidth =  ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent() //load texture, before start game
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTexturesToMap();
            LoadTexturesToScoreBar();
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

            LoadCharacterTexture();
            Map.AddTextureToPlayer();
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
            Map.MapChanges();
            Map.SortObjectToDraw();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))//close game
            {
                Exit();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) //draw
        {
            GraphicsDevice.Clear(Color.BurlyWood);

            spriteBatch.Begin();
            // draw here
            Scorebar.DrawScorebar(spriteBatch);
            Map.DrawMap(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

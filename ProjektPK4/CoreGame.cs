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
        int gameState = 0;//0-start, 1-chose characters, 2- game, 3-end screen

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

            LoadMapAndCharactersTextures();
            LoadTexturesToScoreBar();
        }

        private void LoadTexturesToScoreBar()
        {
            Texture2D[] characterHead = new Texture2D[4];
            for(int i=1; i<=4; i++)
            {
                string name = "Sprites\\headScore" + i;
                characterHead[i-1] = Content.Load<Texture2D>(name);
                GameTextures.AddTexture(characterHead[i - 1]);
            }

            GameTextures.AddTexture(Content.Load<Texture2D>("Sprites\\chest"));
            GameTextures.AddTexture(Content.Load<Texture2D>("Sprites\\chestBack"));
            GameTextures.AddTexture(new Texture2D(GraphicsDevice, 1, 1));

        }

        private void LoadMapAndCharactersTextures()
        {
            string[] textureNameArray = { "End1", "Rock1", "Box1", "Box2", "Bomb1", "Bomb2",
                "Bomb3", "Bomb4" ,"Bomb5","Bomb6","Bomb7","Bomb8","Bomb9","Bomb10","Bomb11",
                "Bomb12","Bomb13","fireNormalStart","fireMiddleLeftRight","fireMiddleUpDown",
                "fireEndUp","fireEndDown","fireEndLeft","fireEndRight","fireStartDown1",
                "fireStartUp1","fireStartLeft1","fireStartRight1","fireStartDownLeft2",
                "fireStartDownRight2","fireStartUpLeft2","fireStartUpRight2", "DeadFire"};

            string name;

            for (int i=0; i<textureNameArray.Length; i++)
            {
                name = "Sprites\\";
                GameTextures.AddTexture(Content.Load<Texture2D>(name + textureNameArray[i]));
            }


            string[] arrayFire = new string[16];

            int h = 0;
            for(int i=textureNameArray.Length-16; i<textureNameArray.Length; i++,h++)
            {
                arrayFire[h] = textureNameArray[i];
            }

            Map.setFireTextureName(arrayFire);

            string[] mapArray = { "End1", "Rock1", "Box1", "Box2" };

            Map.setTextureName(mapArray);

            string[] powerupsName = { "PowerupsBomb", "PowerupsLighting", "PowerupsPotion", "PowerupsStar" };

            int[] _lenghtPowerupsTexture = { 18, 16, 17, 15 };

            Map.setPowerupsTextureName(powerupsName, _lenghtPowerupsTexture);




            string[] array = { "Up", "Down", "Left", "Right" };

            for (int k = 1; k <= 4; k++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        string control = "Sprites\\Pirate" + k + array[j] + (i + 1);
                        GameTextures.AddTexture(Content.Load<Texture2D>(control));
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

            spriteBatch.Begin();
            // draw here


            if(gameState == 0)
            {

            }
            else if (gameState == 2)
            {
                GraphicsDevice.Clear(Color.BurlyWood);
                Scorebar.DrawScorebar(spriteBatch);
                Map.DrawMap(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

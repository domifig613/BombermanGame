using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ProjektPK4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CoreGame : Game
    {
        ProgramParameters GameParameters = new ProgramParameters(); 

        Content.Map map;
        GraphicsDeviceManager graphics;
  
        SpriteBatch spriteBatch;//help draw textures


        public CoreGame() //konstrukotr
        {
           TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d); //frame rate 60

            graphics = new GraphicsDeviceManager(this); //tworzymy nowa aplikacje graphicDeviceManager
            Content.RootDirectory = "Content"; //folder gdzie szukamy tresci gry

            graphics.PreferredBackBufferHeight = GameParameters.WindowHeight;
            graphics.PreferredBackBufferWidth =  GameParameters.WindowWidth;

            map = new Content.Map();
        }

   
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() //po konstruktorze ale przed glowna petla gry, ladowanie tresci niezwiazane z grafika
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() //ladowanie tresci gry TYLKO RAZ NA POCZATKU
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            LoadTexturesToMap();
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
            map.LoadTextureMap(Content.Load<Texture2D>(name), 0);
            name = "Sprites\\" + textureNameArray[1];
            map.LoadTextureMap(Content.Load<Texture2D>(name), 1);
            name = "Sprites\\" + textureNameArray[2];
            map.LoadTextureMap(Content.Load<Texture2D>(name), 2);
            name = "Sprites\\" + textureNameArray[3];
            map.LoadTextureMap(Content.Load<Texture2D>(name), 3);
            
            for(int i=0; i<13; i++)
            {
                name = "Sprites\\" + textureNameArray[i+4];
                map.LoadTextureMap(Content.Load<Texture2D>(name), 4);
            }

            for(int i=0; i<16; i++)
            {
                name = "Sprites\\" + textureNameArray[i + 17];
                map.LoadTextureMap(Content.Load<Texture2D>(name), 5);
            }

            for (int i = 1; i <= 18; i++)
            {
                name = "Sprites\\" + "PowerupsBomb" + i.ToString();
                map.LoadTextureMap(Content.Load<Texture2D>(name), 6);
            }

            for (int i = 1; i <= 16; i++)
            {
                name = "Sprites\\" + "PowerupsLighting" + i.ToString();
                map.LoadTextureMap(Content.Load<Texture2D>(name), 7);
            }

            for (int i = 1; i <= 17; i++)
            {
                name = "Sprites\\" + "PowerupsPotion" + i.ToString();
                map.LoadTextureMap(Content.Load<Texture2D>(name), 8);
            }

            for (int i = 1; i <= 15; i++)
            {
                name = "Sprites\\" + "PowerupsStar" + i.ToString();
                map.LoadTextureMap(Content.Load<Texture2D>(name), 9);
            }

            


            LoadCharacterTexture();
        }

        private void LoadCharacterTexture()
        {
            string[] array = { "Up", "Down", "Left", "Right" };
            for (int k = 1; k <= 2; k++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        string control = "Sprites\\Pirate"+ k + array[j] + (i + 1);
                        map.LoadTexturePlayer(Content.Load<Texture2D>(control), i, j, k-1);
                    }
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) //kolizja gromadzenie danych wejsciowych dzwiek, wiecej niz rysowanie
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) //rysowanie, wiele razy na sekunde
        {

            map.MapChanges();
            map.SortObjectToDraw();

            GraphicsDevice.Clear(Color.BurlyWood); //kolor tla

            spriteBatch.Begin();

            // TODO: Add your drawing code here
            map.DrawMap(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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


            map.LoadTextureMap(Content.Load<Texture2D>("End1"), 0);
            map.LoadTextureMap(Content.Load<Texture2D>("Rock1"), 1);
            map.LoadTextureMap(Content.Load<Texture2D>("Box1"), 2);
            map.LoadTextureMap(Content.Load<Texture2D>("Box2"), 3);
          
           // map.CreateRectangleInMap();

            LoadCharacterTexture();

        }

        private void LoadCharacterTexture()
        {
            string[] array = { "Up", "Down", "Left", "Right" };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    string control = "Pirate1" + array[j] + (i + 1);
                    map.LoadTexturePlayer(Content.Load<Texture2D>(control), i, j);
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

            map.CheckMovePlayer(0);
            map.SortObjectToDraw();

            GraphicsDevice.Clear(Color.Wheat); //kolor tla

            spriteBatch.Begin();

            // TODO: Add your drawing code here
            map.DrawMap(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

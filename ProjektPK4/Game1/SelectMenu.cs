using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ProjektPK4.game
{
    class SelectMenu : Menu
    {
        private List<Rectangle> CharactersBody = new List<Rectangle>();
        private List<Rectangle> KeysBody = new List<Rectangle>();
        private List<Texture2D> CharactersTextures = new List<Texture2D>();
        readonly private Button[][] RightLeftClickButtonsBody = new Button[4][];
        readonly private Rectangle[][] CharacterTypeBody = new Rectangle[4][];
        readonly private Texture2D[] KeysTextures = new Texture2D[4];
        readonly private Texture2D[] CharacterTypeNameTexures = new Texture2D[3];
        private Texture2D EmptyTexture;
        private List<Rectangle> CharacterBackgroundBody = new List<Rectangle>();
        readonly private Color CharacterBackgroundColor= Color.Aqua;
        readonly private int[] CharacterType = new int[4];//1=player, 2=computer, 3=disable


        public SelectMenu(Texture2D backgroundTexture, Texture2D captionTexture, Texture2D empty, List<Texture2D> charactersTexture,
                          Texture2D rightClickButton, Texture2D leftClickButton, Texture2D[] characterTypeNameTextures, Texture2D[] keysTextures) 
                          : base(backgroundTexture, captionTexture, 50)
        {
            EmptyTexture = empty;
            EmptyTexture.SetData(new Color[] { new Color(30,0,0,50)});

            for (int i = 0; i < 4; i++)
            {
                CharacterBackgroundBody.Add(new Rectangle(ProgramParameters.WindowWidth / 14 + (ProgramParameters.WindowWidth * i  * 2 / 7),
                ProgramParameters.WindowHeight * 4 / 14, ProgramParameters.WindowWidth * 7 / 28, ProgramParameters.WindowHeight / 2));

                CharactersBody.Add(new Rectangle(CharacterBackgroundBody[i].X + CharacterBackgroundBody[i].Width * 3 / 8,
                    CharacterBackgroundBody[i].Y + CharacterBackgroundBody[i].Height / 3, CharacterBackgroundBody[i].Width / 4, CharacterBackgroundBody[i].Height / 4));

                RightLeftClickButtonsBody[i] = new Button[2];
                RightLeftClickButtonsBody[i][0] = new Button(1, CharacterBackgroundBody[i].X + CharacterBackgroundBody[i].Width * 5 / 6,
                    CharacterBackgroundBody[i].Y + CharacterBackgroundBody[i].Height / 12, CharacterBackgroundBody[i].Width / 8, CharacterBackgroundBody[i].Width / 8);
                RightLeftClickButtonsBody[i][1] = new Button(2, CharacterBackgroundBody[i].X +  (CharacterBackgroundBody[i].Width / 16),
                   CharacterBackgroundBody[i].Y + CharacterBackgroundBody[i].Height / 12, CharacterBackgroundBody[i].Width / 8, CharacterBackgroundBody[i].Width / 8);

                RightLeftClickButtonsBody[i][0].SetTexture(rightClickButton);
                RightLeftClickButtonsBody[i][1].SetTexture(leftClickButton);

                CharacterTypeBody[i] = new Rectangle[3];
                for (int j = 0; j < 3; j++)
                {
                    CharacterTypeBody[i][j] = new Rectangle((CharacterBackgroundBody[i].X+ CharacterBackgroundBody[i].Width/2) - (358 / 6),
                   CharacterBackgroundBody[i].Y + CharacterBackgroundBody[i].Height / 11, 358/3 , 50/3);
                }

                KeysBody.Add(new Rectangle(CharacterBackgroundBody[i].X + (CharacterBackgroundBody[i].Width / 4),
                CharacterBackgroundBody[i].Y + CharacterBackgroundBody[i].Height * 11 / 16, CharacterBackgroundBody[i].Width - CharacterBackgroundBody[i].Width * 15 / 32, CharacterBackgroundBody[i].Height / 3));

                KeysTextures[i] = keysTextures[i];

                CharacterType[i] = 1;
            }

            foreach (Texture2D textures1 in charactersTexture)
            {
                CharactersTextures.Add(textures1);
            }

            for (int i = 0; i < 3; i++)
            {
                CharacterTypeNameTexures[i] = characterTypeNameTextures[i];
            }
        }

        new public int CheckKeysPressed(MouseState now, MouseState old, int gameState)
        {
            foreach (Button button1 in Buttons)
            {
                int PlayerCount = 0;
                for(int i=0; i<4; i++)
                {
                    if(CharacterType[i]!=3)
                    {
                        PlayerCount++;
                    }
                }

                if (button1.GetButtonType() != 1 || PlayerCount >= 2)
                {
                    if (now.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released)
                    {
                        gameState = button1.CheckMoveInButtonPosition(now.X, now.Y, gameState);
                    }
                    if (button1.CheckMoveInButtonPositionX(now.X) && button1.CheckMoveInButtonPositionY(now.Y))
                    {
                        if (!button1.CheckMoveInButtonPositionX(old.X) || !button1.CheckMoveInButtonPositionY(old.Y))
                        {
                            button1.PlaySoundEffect();
                        }
                        button1.SetOnMoveState(true);
                    }
                    else
                    {
                        button1.SetOnMoveState(false);
                    }
                }
            }

            for (int i=0; i<4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (now.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released)
                    {
                        CharacterType[i] = RightLeftClickButtonsBody[i][j].CheckMoveInButtonPosition(now.X, now.Y, CharacterType[i]);

                        if (CharacterType[i]<1)
                        {
                            CharacterType[i] = 3;
                        }
                    }
                    if (RightLeftClickButtonsBody[i][j].CheckMoveInButtonPositionX(now.X) && RightLeftClickButtonsBody[i][j].CheckMoveInButtonPositionY(now.Y))
                    {
                        if (!RightLeftClickButtonsBody[i][j].CheckMoveInButtonPositionX(old.X) || !RightLeftClickButtonsBody[i][j].CheckMoveInButtonPositionY(old.Y))
                        {
                            RightLeftClickButtonsBody[i][j].PlaySoundEffect();
                        }
                        RightLeftClickButtonsBody[i][j].SetOnMoveState(true);
                    }
                    else
                    {
                        RightLeftClickButtonsBody[i][j].SetOnMoveState(false);
                    }
                }
            }

            return gameState;
        }

        new public void LoadButtonSound(SoundEffect sound)
        {
            base.LoadButtonSound(sound);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    RightLeftClickButtonsBody[i][j].LoadSoundButton(sound);
                }
            }
        }

        new public void DrawMenu(SpriteBatch Batch)
        {
            base.DrawMenu(Batch);

            foreach (Rectangle rectangle in CharacterBackgroundBody)
            {
                Batch.Draw(EmptyTexture, rectangle, Color.Aqua);
            }

            for(int i=0; i<4; i++)
            {
                if (CharacterType[i] == 1 || CharacterType[i] == 2)
                {
                    Batch.Draw(CharactersTextures[i], CharactersBody[i], Color.White);
                }

                if(CharacterType[i]==1)
                {
                    Batch.Draw(KeysTextures[i], KeysBody[i], Color.White);
                }

                for (int j = 0; j < 2; j++)
                {
                    RightLeftClickButtonsBody[i][j].DrawButton(Batch);
                }

                Batch.Draw(CharacterTypeNameTexures[CharacterType[i]-1], CharacterTypeBody[i][CharacterType[i]-1], Color.White);
            }

        }

        public int[] GetCharacterPlay()
        {
            return CharacterType;
        }
    }
}

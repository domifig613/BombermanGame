using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ProjektPK4.game
{
    class Menu
    {
        protected List<Button> Buttons = new List<Button>();
        private Texture2D BackgroundTexture;
        private Rectangle Background;
        readonly Texture2D CaptionTexture;
        protected Rectangle Caption;

        public Menu(Texture2D backgroundTexture, Texture2D captionTexture)
        {
            Caption = new Rectangle((ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth - 840) / 2, 120, 840, 60);
            Background = new Rectangle(0, 0, ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth, ProgramParameters.WindowHeight);

            BackgroundTexture = backgroundTexture;
            CaptionTexture = captionTexture;


            Buttons.Add(new Button(1, (ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth) / 2 - 60, ProgramParameters.WindowHeight / 2 - 40, 120, 40));
            Buttons.Add(new Button(0, (ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth) / 2 - 50, ProgramParameters.WindowHeight / 2 + 20, 100, 33));

            if (this.BackgroundTexture.Width == 1 && this.BackgroundTexture.Height == 1)
            {
                this.BackgroundTexture.SetData(new Color[] { Color.BurlyWood });
            }
        }

        public void LoadTextureToButtons(Texture2D start, Texture2D exit)
        {
            Buttons[0].SetTexture(start);
            Buttons[1].SetTexture(exit);
        }

        protected Menu(Texture2D backgroundTexture, Texture2D captionTexture, int fixCaptionPosAndHeightPlus)
        {
            Caption = new Rectangle((ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth - 840 - fixCaptionPosAndHeightPlus) / 2
                , 120-fixCaptionPosAndHeightPlus ,840+fixCaptionPosAndHeightPlus ,60);
            Background = new Rectangle(0, 0, ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth, ProgramParameters.WindowHeight);

            BackgroundTexture = backgroundTexture;
            CaptionTexture = captionTexture;

            Buttons.Add(new Button(1, (ProgramParameters.WindowWidth + ProgramParameters.ScoreBarWidth) - 140, ProgramParameters.WindowHeight - 80, 120, 40));
            Buttons.Add(new Button(2, 20, ProgramParameters.WindowHeight - 80, 120, 40));

            if (BackgroundTexture.Width == 1 && this.BackgroundTexture.Height == 1)
            {
                BackgroundTexture.SetData(new Color[] { Color.BurlyWood });
            }
        }

        public void DrawMenu(SpriteBatch Batch)
        {
            Batch.Draw(BackgroundTexture, Background, Color.White);
            Batch.Draw(CaptionTexture, Caption, Color.White);
            foreach(Button button1 in Buttons)
            {
                button1.DrawButton(Batch);
            }
        }

        public void LoadButtonSound(SoundEffect sound)
        {
            foreach(Button button1 in Buttons)
            {
                button1.LoadSoundButton(sound);
            }
        }

        public int CheckKeysPressed(MouseState now, MouseState old, int gameState)
        {
            foreach(Button button1 in Buttons)
            {
                if (now.LeftButton == ButtonState.Pressed && old.LeftButton == ButtonState.Released)
                {
                   gameState = button1.CheckMoveInButtonPosition(now.X, now.Y, gameState);
                }
                if(button1.CheckMoveInButtonPositionX(now.X) && button1.CheckMoveInButtonPositionY(now.Y))
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
            return gameState;
        }
    }
}

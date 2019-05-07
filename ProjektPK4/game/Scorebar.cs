using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektPK4.game
{
    static class Scorebar
    {
        static private Rectangle Background;
        static private Texture2D EmptyBackground;
        static private Texture2D[] Chest;//0- emptym, 1- scoreChest
        static private Texture2D[] CheadTextures;
        static private Rectangle[] CharacterRectangles;
        static private Rectangle[,] ChestRectangles;
        static private bool[] CharacterPlay;
        static private int[] playersWin;

        static Scorebar()
        {
            Background = new Rectangle(ProgramParameters.WindowWidth, 0, ProgramParameters.ScoreBarWidth, ProgramParameters.WindowHeight);
            CharacterRectangles = new Rectangle[4];
            ChestRectangles = new Rectangle[4, 3];
            playersWin = new int[4];
  
            for (int i = 0; i < 4; i++)
            {
                Rectangle rectangle = new Rectangle(ProgramParameters.WindowWidth+(ProgramParameters.ScoreBarWidth/2)-(ProgramParameters.OneAreaWidth/2), 
                 ProgramParameters.WindowHeight /5+ (i*ProgramParameters.WindowHeight/5) - ProgramParameters.OneAreaHeight, ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight);

                CharacterRectangles[i] = rectangle;
                for (int j = 0; j < 3; j++)
                {
                    rectangle = new Rectangle(ProgramParameters.WindowWidth + (ProgramParameters.ScoreBarWidth / 2) - (ProgramParameters.OneAreaWidth*2) + (j*ProgramParameters.OneAreaWidth*3/2),
                        ProgramParameters.WindowHeight / 5 + (i * ProgramParameters.WindowHeight / 5) + ProgramParameters.OneAreaHeight/2,
                        ProgramParameters.OneAreaWidth, ProgramParameters.OneAreaHeight);
                    ChestRectangles[i, j] = rectangle;
                }
            }

            CharacterPlay = new bool[4];
            CheadTextures = new Texture2D[4];
            Chest = new Texture2D[2];
        }

        static void SetCharacter(bool[] _characterPlay)
        {
            for (int i = 0; i < 4; i++)
            {
                CharacterPlay[i] = _characterPlay[i];
            }
        }

        static public void LoadScoreboardTextures(Texture2D[] character, Texture2D chest, Texture2D emptyChest, Texture2D EmptyTexture)
        {
            takeCharacterHeadAndChestTextures(character, chest, emptyChest);
            TakeEmptyTexture(EmptyTexture);
        }

        static public void takeCharacterHeadAndChestTextures(Texture2D[] character, Texture2D chest, Texture2D emptyChest)
        {
            for(int i=0;i<4; i++)
            {
                CheadTextures[i] = character[i];
            }
            Chest[0] = emptyChest;
            Chest[1] = chest;
        }

        static public void TakeEmptyTexture(Texture2D texture)
        {
            texture.SetData(new Color[] { Color.Gray});
            EmptyBackground = texture;
        }

        static public void DrawScorebar(SpriteBatch Batch)
        {
           Batch.Draw(EmptyBackground, Background, Color.Gray);
            for(int i=0; i<4; i++)
            {
              
                //if(CharacterPlay[i]==true)
                //{
                Batch.Draw(CheadTextures[i], CharacterRectangles[i], Color.White);
                for(int j=0; j<3; j++)
                {
                    
                    if (playersWin[i] > j)
                    {
                        Batch.Draw(Chest[1], ChestRectangles[i, j], Color.White);
                    }
                    else
                    {
                        Batch.Draw(Chest[0], ChestRectangles[i, j], Color.White);
                    }
                }
               // }
            }
        }

        static public void AddScore(int playerNumber)
        {
            playersWin[playerNumber]++;
        }
    }
}

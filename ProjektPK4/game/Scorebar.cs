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

        static Scorebar()
        {
            Background = new Rectangle(ProgramParameters.WindowWidth, 0, ProgramParameters.ScoreBarWidth, ProgramParameters.WindowHeight);
        }

        static public void TakeEmptyTexture(Texture2D texture)
        {
            texture.SetData(new Color[] { Color.Gray});
            EmptyBackground = texture;
        }

        static public void DrawScorebar(SpriteBatch Batch)
        {
            Batch.Draw(EmptyBackground, Background, Color.Gray);
        }
    }
}

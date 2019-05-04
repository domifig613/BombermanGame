using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPK4
{
   static class ProgramParameters
    {
        static public int WindowWidth { get; } = 760;
        static public int WindowHeight { get; } = 620;

        static public int OneAreaWidth { get; } = 40;
        static public int OneAreaHeight { get; } = 40;
        static public int AreaYShade { get; } = 20;
        static public int StartPositionSize { get; } = 3; //how much space free from box example 3 = 3x3 free space
        static public int CharacterSlowerAnimation { get; } = 12; //higer = slower animation
        static public int CountOfNormalBox { get; } = 90;
        static public int CountOfPremiumBox { get; } = 10;

    }
}

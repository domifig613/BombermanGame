using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPK4
{
    class ProgramParameters
    {
        public int WindowWidth { get; } = 760;
        public int WindowHeight { get; } = 620;

        public int OneAreaWidth { get; } = 40;
        public int OneAreaHeight { get; } = 40;
        public int AreaYShade { get; } = 20;
        public int StartPositionSize { get; } = 3; //how much space free from box example 3 = 3x3 free space
        public int CharacterSlowerAnimation { get; } = 12; //higer = slower animation
        public int CountOfNormalBox { get; } = 30;
        public int CountOfPremiumBox { get; } = 20;

    }
}

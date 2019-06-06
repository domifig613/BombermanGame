using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektPK4
{
   static class ProgramParameters
    {
        readonly static public int WindowWidth  = 760;
        readonly static public int WindowHeight  = 620;
        readonly static public int ScoreBarWidth = 200;

        readonly static public int OneAreaWidth  = 40;
        readonly static public int OneAreaHeight  = 40;
        readonly static public int AreaYShade  = 20;
        readonly static public int StartPositionSize  = 3; //how much space free from box example 3 = 3x3 free space
        readonly static public int CharacterSlowerAnimation  = 6; //higer = slower animation
        readonly static public int CountOfNormalBox = 90;
        readonly static public int CountOfPremiumBox  = 10;
        private static bool MusicEnable = true;

        public static bool Get_MusicEnable()
        {
            return MusicEnable;
        }

        static public void MusicSwitch(bool enable)
        {
            MusicEnable = enable;
        }
    }
}

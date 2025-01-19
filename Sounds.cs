using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetThemYap
{
    internal class Sounds
    {
        public static SoundID CLSpeak1 { get; private set; }
        public static SoundID CLSpeak2 { get; private set; }
        public static SoundID CLSpeak3 { get; private set; }

        internal static void Initialize()
        {
            CLSpeak1 = new SoundID("CL_Speak1", true);
            CLSpeak2 = new SoundID("CL_Speak2", true);
            CLSpeak3 = new SoundID("CL_Speak3", true);
        }



    }
}

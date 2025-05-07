using System;
using HunterExpansion.CustomOracle;

namespace LetThemYap
{
    internal class NSHYap
    {
        //MODDED Choose randomly between 2 of No Significant Harassment's lines then play them
        public static void playNSHAudio(HUD.DialogBox self, Random rnd, Oracle.OracleID oracleId)
        {
            if(oracleId == NSHOracleRegistry.NSHOracle)
            {
                switch (rnd.Next(0, 2))
                {
                    case 0:
                        self.hud.PlaySound(NSHOracleSoundID.NSH_AI_Break_1);
                        break;
                    case 1:
                        self.hud.PlaySound(NSHOracleSoundID.NSH_AI_Break_2);
                        break;

                }
            }


        }


    }
}

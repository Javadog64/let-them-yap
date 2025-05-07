using System;
using HunterExpansion.CustomOracle;
using MoreSlugcats;

namespace LetThemYap
{
    internal class NSHYap
    {
        //MODDED Choose randomly between 2 of No Significant Harassment's lines then play them
        public static void playNSHAudio(HUD.DialogBox self, Random rnd, Oracle.OracleID oracleId, string text, SlugcatStats.Name slugName, string region)
        {
            if(oracleId == NSHOracleRegistry.NSHOracle || (ModManager.MSC && slugName == MoreSlugcatsEnums.SlugcatStatsName.Saint && region == "HR"))
            {
                switch (rnd.Next(0, 4))
                {
                    case 0:
                        self.hud.PlaySound(NSHOracleSoundID.NSH_AI_Break_1);
                        break;
                    case 1:
                        self.hud.PlaySound(NSHOracleSoundID.NSH_AI_Break_2);
                        break;
                    case 2:
                        self.hud.PlaySound(NSHOracleSoundID.NSH_AI_Break_3);
                        break;
                    case 3:
                        self.hud.PlaySound(NSHOracleSoundID.NSH_AI_Break_4);
                        break;
                }
            }


        }


    }
}

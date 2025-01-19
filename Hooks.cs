using MoreSlugcats;
using JetBrains.Annotations;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetThemYap
{
    internal class Hooks
    {
        
        //Choose randomly between 5 of Pebble's lines then play them
        private static void playPebblesAudio(HUD.DialogBox self, Random rnd)
        {
            switch (rnd.Next(0, 5))
            {
                case 0:
                    self.hud.PlaySound(SoundID.SS_AI_Talk_1);
                    break;
                case 1:
                    self.hud.PlaySound(SoundID.SS_AI_Talk_2);
                    break;
                case 2:
                    self.hud.PlaySound(SoundID.SS_AI_Talk_3);
                    break;
                case 3:
                    self.hud.PlaySound(SoundID.SS_AI_Talk_4);
                    break;
                case 4:
                    self.hud.PlaySound(SoundID.SS_AI_Talk_5);
                    break;

            }
        }
        
        //Choose randomly between 5 of Moon's lines then play them
        private static void playMoonAudio(HUD.DialogBox self, Random rnd)
        {
            switch (rnd.Next(0, 5))
            {
                case 0:
                    self.hud.PlaySound(SoundID.SL_AI_Talk_1);
                    break;
                case 1:
                    self.hud.PlaySound(SoundID.SL_AI_Talk_2);
                    break;
                case 2:
                    self.hud.PlaySound(SoundID.SL_AI_Talk_3);
                    break;
                case 3:
                    self.hud.PlaySound(SoundID.SL_AI_Talk_4);
                    break;
                case 4:
                    self.hud.PlaySound(SoundID.SL_AI_Talk_5);
                    break;

            }
        }

        public static void Apply()
        {
            //set variables
            Oracle.OracleID oracleID = null;
            string textSaying = "";
            string region = "";
            bool isEchoHere = false;
            SlugcatStats.Name slugName = null;

            On.HUD.DialogBox.InitNextMessage += (orig, self) =>
            {
                //create random variable
                Random rnd = new Random();

                //Is the text not 3 dots and an echo is not here?
                if (textSaying != "..." && textSaying != ". . ." && textSaying != " . . . " && !isEchoHere)
                {
                    //Are we Saint, in Rubicon and is More Slugcats on?
                    if(ModManager.MSC && (region == "HR" && slugName == MoreSlugcatsEnums.SlugcatStatsName.Saint))
                    {
                        //Is moon talking when both of them are ascended or is it just moon?
                        if (textSaying.StartsWith("BSM:") || (ModManager.MSC && oracleID == MoreSlugcatsEnums.OracleID.DM  && !textSaying.StartsWith("FP:")))
                        {
                            playMoonAudio(self, rnd);
                        }
                        //Is pebbles talking when both of them are ascended or is it just pebbles?
                        else if (textSaying.StartsWith("FP:") || oracleID == Oracle.OracleID.SS)
                        {
                            playPebblesAudio(self, rnd);
                        }
                    }
                    //Is it shoreline moon thats talking or is it Spearmaster moon?
                    else if (oracleID == Oracle.OracleID.SL || (ModManager.MSC && oracleID == MoreSlugcatsEnums.OracleID.DM))
                    {
                        playMoonAudio(self, rnd);
                    }
                    //Is Five Pebbles talking?
                    else if (oracleID == Oracle.OracleID.SS)
                    {
                        playPebblesAudio(self, rnd);
                    }
                    //Is More Slugcats on and is it Saint Pebbles?
                    else if (ModManager.MSC && oracleID == MoreSlugcatsEnums.OracleID.CL)
                    {
                        switch (rnd.Next(0, 3))
                        {
                            case 0:
                                self.hud.PlaySound(Sounds.CLSpeak1);
                                break;
                            case 1:
                                self.hud.PlaySound(Sounds.CLSpeak2);
                                break;
                            case 2:
                                self.hud.PlaySound(Sounds.CLSpeak3);
                                break;
                        }



                    }
                }
                
                
                orig(self);
            };

            //Here to get the ID, region, and slugcatname
            On.Oracle.InitiateGraphicsModule += (orig, self) =>
            {
                oracleID = self.ID;
                region = self.room.world.name;
                slugName = self.room.world.game.StoryCharacter;
                orig(self);
            };

            On.Ghost.ThightSprite += (orig, self, someNumber) =>
            {
                isEchoHere = self.room.ViewedByAnyCamera(self.pos, 100f);
                return orig(self, someNumber);
            };


            //Here to get the current text
            On.HUD.DialogBox.NewMessage_string_float_float_int  += (orig, self, text, xOrien, yPos, extra) =>
            {
                textSaying = text;
                orig(self,text,xOrien,yPos, extra);
            };


            
        }

        


    }
}

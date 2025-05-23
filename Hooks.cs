﻿using MoreSlugcats;
using JetBrains.Annotations;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Utils;
using Watcher;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using BepInEx;
using BepInEx.Logging;

namespace LetThemYap
{
    internal class Hooks
    {
        //public static ManualLogSource Logger;


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
        private static void playMoonAudio(HUD.DialogBox self, Random rnd, bool isProtest)
        {

            if (!isProtest)
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
            } else
            {
                switch (rnd.Next(0, 5))
                {
                    case 0:
                        self.hud.PlaySound(SoundID.SL_AI_Protest_1);
                        break;
                    case 1:
                        self.hud.PlaySound(SoundID.SL_AI_Protest_2);
                        break;
                    case 2:
                        self.hud.PlaySound(SoundID.SL_AI_Protest_3);
                        break;
                    case 3:
                        self.hud.PlaySound(SoundID.SL_AI_Protest_4);
                        break;
                    case 4:
                        self.hud.PlaySound(SoundID.SL_AI_Protest_5);
                        break;

                }
            }
            
           
        }






        public static void Apply()
        {
            //set variables
            Oracle.OracleID oracleID = null;
            string textSaying = "";
            string region = "";
            bool isEchoHere = false;
            SLOracleBehaviorHasMark moon = null;   
            SlugcatStats.Name slugName = null;
            bool moonAngry = false;
            Ghost echo = null;
            HUD.Map.MapType test = HUD.Map.MapType.WarpLinks;



            On.HUD.DialogBox.InitNextMessage += (orig, self) =>
            {
                //create random variable
                Random rnd = new Random();

                //Is the text not 3 dots and an echo is not here?
                if (textSaying != "..." && textSaying != ". . ." && textSaying != " . . . " && textSaying !=".  .  .")
                {
                    if (isEchoHere && echo is not SpinningTop)
                    {
                        self.hud.PlaySound(Sounds.EchoYapSounds);
                    }
                    //Are we Saint, in Rubicon and is More Slugcats on?
                    else if (ModManager.MSC && (region == "HR" && slugName == MoreSlugcatsEnums.SlugcatStatsName.Saint))
                    {
                        //Is moon talking when both of them are ascended or is it just moon?
                        if (textSaying.StartsWith("BSM:") || (ModManager.MSC && oracleID == MoreSlugcatsEnums.OracleID.DM && !textSaying.StartsWith("FP:")))
                        {
                            playMoonAudio(self, rnd, false);
                        }
                        //Is pebbles talking when both of them are ascended or is it just pebbles?
                        else if (textSaying.StartsWith("FP:") || oracleID == Oracle.OracleID.SS)
                        {
                            playPebblesAudio(self, rnd);
                        }
                        //MODDED
                        //Is NSH talking?
                        else if (textSaying.StartsWith("NSH:") && ModManager.ActiveMods.Any(mod => mod.id == "Quaeledy.hunterexpansion"))
                        {
                            NSHYap.playNSHAudio(self, rnd, oracleID, textSaying, slugName, region);
                        }
                    }
                    //Is it shoreline moon?
                    else if (oracleID == Oracle.OracleID.SL)
                    {
                        if (moon.playerHoldingNeuronNoConvo || moon.pauseReason == SLOracleBehaviorHasMark.PauseReason.GrabNeuron)
                        {
                            moonAngry = true;
                        }
                        else
                        {
                            moonAngry = false;
                        }

                        playMoonAudio(self, rnd, moonAngry);
                    }
                    // Is it Spearmaster moon?
                    else if (ModManager.MSC && oracleID == MoreSlugcatsEnums.OracleID.DM)
                    {
                        playMoonAudio(self, rnd, false);
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
                    //MODDED Is a Custom Iterator talking?
                    //Is Chasing Wind talking?
                    else if (ModManager.ActiveMods.Any(mod => mod.id == "myr.chasing_wind"))
                    {
                        ChasingWindYap.playChasingWindAudio(self, rnd, oracleID);
                    }
                    //Is Chasing Wind talking?
                    else if (ModManager.ActiveMods.Any(mod => mod.id == "Quaeledy.hunterexpansion"))
                    {
                        NSHYap.playNSHAudio(self, rnd, oracleID, textSaying, slugName, region);
                    }
                }
                
                
                orig(self);
            };

            //Here to get the ID, region, and slugcatname
            On.Oracle.ctor += (orig, self, abstractPhysObj, room) =>
            {
                orig(self, abstractPhysObj, room);
                oracleID = self.ID;
                region = self.room.world.name;
                slugName = self.room.world.game.StoryCharacter;
            };

            On.GhostConversation.ctor += (orig, self, id, ghost, diaBox) =>
            {
                isEchoHere = true;
                echo = ghost;
                orig(self, id, ghost, diaBox);
            };

            On.RainWorldGame.ctor += (orig, self, manager) =>
            {
                isEchoHere = false;
                orig(self, manager);
            };

            //Here to get the current text
            On.HUD.DialogBox.NewMessage_string_float_float_int  += (orig, self, text, xOrien, yPos, extra) =>
            {
                textSaying = text;
                orig(self,text,xOrien,yPos, extra);
            };

            On.SLOracleBehaviorHasMark.Update += (orig, self, wh) =>
            {
                moon = self;
                orig(self, wh);
            };

            IL.Watcher.SpinningTop.SpinningTopConversation.Update += Watcher_SpinningTop_SpinningTopConversation_Update;


        }

        private static void Watcher_SpinningTop_SpinningTopConversation_Update(ILContext il)
        {
            try
            {
                ILCursor c = new ILCursor(il);
                c.GotoNext(
                    MoveType.After,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdfld<Watcher.SpinningTop.SpinningTopConversation>(nameof(Watcher.SpinningTop.SpinningTopConversation.timeSinceLastSound)),
                    x => x.MatchLdcI4(120)
                    );
                c.MoveAfterLabels();
                c.Index--;
                c.Next.Operand = 1;
                //c.Emit(OpCodes.Nop);
                //UnityEngine.Debug.Log(il);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }
        }


    }
}

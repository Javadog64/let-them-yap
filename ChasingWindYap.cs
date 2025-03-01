using System;
using CWStuff;

namespace LetThemYap
{
    internal class ChasingWindYap
    {
        //MODDED Choose randomly between 2 of Chasing Wind's lines then play them
        public static void playChasingWindAudio(HUD.DialogBox self, Random rnd)
        {
            switch (rnd.Next(0, 2))
            {
                case 0:
                    self.hud.PlaySound(NewSoundID.CW_AI_Talk_1);
                    break;
                case 1:
                    self.hud.PlaySound(NewSoundID.CW_AI_Talk_2);
                    break;

            }
        }


    }
}

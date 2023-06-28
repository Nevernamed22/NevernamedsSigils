using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
   public static class NevernamedsTribes
    {
        public static Tribe Arachnid;
        public static Tribe Crustacean;
        public static Tribe Rodent;

        public static void InitTribes()
        {
            Arachnid = TribeManager.Add("nevernamed.inscryption.sigils", "Arachnid", Plugin.arachnophobiaMode.Value ? Tools.LoadTex("NevernamedsSigils/Resources/Other/webtribe.png") : Tools.LoadTex("NevernamedsSigils/Resources/Other/arachnid_tribe.png"), true, Plugin.arachnophobiaMode.Value ? Tools.LoadTex("NevernamedsSigils/Resources/Other/webcardback.png") : Tools.LoadTex("NevernamedsSigils/Resources/Other/arachnid_cardback.png"));
            Crustacean = TribeManager.Add("nevernamed.inscryption.sigils", "Crustacean", Tools.LoadTex("NevernamedsSigils/Resources/Other/crustacean_tribe.png"), true, Tools.LoadTex("NevernamedsSigils/Resources/Other/crustacean_cardback.png"));
            Rodent = TribeManager.Add("nevernamed.inscryption.sigils", "Rodent", Tools.LoadTex("NevernamedsSigils/Resources/Other/rodent_tribe.png"), true, Tools.LoadTex("NevernamedsSigils/Resources/Other/rodent_cardback.png"));
        }
    }
}

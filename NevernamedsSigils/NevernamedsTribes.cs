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
            Arachnid = TribeManager.Add("nevernamed.inscryption.sigils", "Arachnid", Tools.LoadTex("NevernamedsSigils/Resources/Other/arachnid_tribe.png"), true, Tools.LoadTex("NevernamedsSigils/Resources/Other/arachnid_cardback.png"));
            Crustacean = TribeManager.Add("nevernamed.inscryption.sigils", "Crustacean", Tools.LoadTex("NevernamedsSigils/Resources/Other/crustacean_tribe.png"), true, Tools.LoadTex("NevernamedsSigils/Resources/Other/crustacean_cardback.png"));
            Rodent = TribeManager.Add("nevernamed.inscryption.sigils", "Rodent", Tools.LoadTex("NevernamedsSigils/Resources/Other/rodent_tribe.png"), true, Tools.LoadTex("NevernamedsSigils/Resources/Other/rodent_cardback.png"));

            CardManager.BaseGameCards.CardByName("Amalgam").tribes.AddRange(new List<Tribe>() { Arachnid, Crustacean, Rodent });
            CardManager.BaseGameCards.CardByName("Hydra").tribes.AddRange(new List<Tribe>() { Arachnid, Crustacean, Rodent });

            CardManager.BaseGameCards.CardByName("PackRat").tribes.Add(Rodent);
            CardManager.BaseGameCards.CardByName("Porcupine").tribes.Add(Rodent);
            CardManager.BaseGameCards.CardByName("Beaver").tribes.Add(Rodent);
            CardManager.BaseGameCards.CardByName("FieldMouse").tribes.Add(Rodent);
            CardManager.BaseGameCards.CardByName("RatKing").tribes.Add(Rodent);
        }
    }
}

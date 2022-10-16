﻿using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
   public static class NevernamedsTribes
    {
        public static Tribe Arachnid;
        public static void InitTribes()
        {
            Arachnid = TribeManager.Add("nevernamed.inscryption.sigils", "Arachnid", Tools.LoadTex("NevernamedsSigils/Resources/Other/arachnid_tribe.png"), true, Tools.LoadTex("NevernamedsSigils/Resources/Other/arachnid_cardback.png"));
        }
    }
}

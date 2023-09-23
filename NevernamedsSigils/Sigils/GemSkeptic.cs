using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class GemSkeptic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gem Skeptic", "While [creature] is on the board, its owner may not play any mox gems.",
                      typeof(GemSkeptic),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gemskeptic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/gemskeptic_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
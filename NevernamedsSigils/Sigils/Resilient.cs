using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Resilient : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Resilient", "All damage taken by [creature] is reduced to 1.",
                      typeof(Resilient),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook},
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/resilient.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/resilient_pixel.png"));

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

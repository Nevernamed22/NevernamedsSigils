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
    public class Abstain : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Abstain", "[creature] will not attack unless forced to attack by another effect.",
                      typeof(Abstain),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook},
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/abstain.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/abstain_pixel.png"));

            Abstain.ability = newSigil.ability;
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
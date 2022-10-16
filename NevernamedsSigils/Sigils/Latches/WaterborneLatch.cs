using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class WaterborneLatch : Latch
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Waterborne Latch", "When [creature] perishes, its owner chooses a creature to gain the waterborne sigil.",
                      typeof(WaterborneLatch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Latches/waterbornelatch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Latches/waterbornelatch_pixel.png"));

            WaterborneLatch.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override Ability LatchAbility
        {
            get
            {
                return Ability.Submerge;
            }
        }
    }
}
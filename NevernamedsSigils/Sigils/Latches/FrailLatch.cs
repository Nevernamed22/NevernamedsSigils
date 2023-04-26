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
    public class FrailLatch : Latch
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Frail Latch", "When [creature] perishes, its owner chooses a creature to gain the frail sigil.",
                      typeof(FrailLatch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Latches/fraillatch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Latches/fraillatch_pixel.png"));

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
        public override Ability LatchAbility
        {
            get
            {
                return Frail.ability;
            }
        }
    }
}
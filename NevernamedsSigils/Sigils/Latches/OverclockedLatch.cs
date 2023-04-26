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
    public class OverclockedLatch : Latch
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Overclocked Latch", "When [creature] perishes, its owner chooses a creature to gain 1 power, and the overclocked sigil.",
                      typeof(OverclockedLatch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Latches/overclockedlatch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Latches/overclockedlatch_pixel.png"));

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
        public override void OnSuccessfullyLatched(PlayableCard target)
        {
            if (target)
            {
                CardModificationInfo cardModificationInfo = new CardModificationInfo(1, 0);
                cardModificationInfo.fromOverclock = true;
                target.AddTemporaryMod(cardModificationInfo);
            }
            base.OnSuccessfullyLatched(target);
        }
        public override Ability LatchAbility
        {
            get
            {
                return Ability.PermaDeath;
            }
        }
    }
}
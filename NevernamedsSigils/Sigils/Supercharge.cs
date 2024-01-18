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
    public class Supercharge : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Supercharge", "When [creature] is played, it refreshes its owner's energy to the current maximum.",
                      typeof(Supercharge),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/supercharge.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/supercharge_pixel.png"),
                      triggerText: "[creature] restores its owner's energy.");

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override bool RespondsToResolveOnBoard()
        {
            return !base.Card.OpponentCard;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<ResourcesManager>.Instance.RefreshEnergy();
            yield return base.LearnAbility(0f);
            yield break;
        }

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
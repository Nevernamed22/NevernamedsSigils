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
    public class UnstableGems : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Unstable Gems", "While [creature] is on the board, all friendly creatures which cost a mox gem will gain the brittle sigil when played.",
                      typeof(UnstableGems),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/unstablegems.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/unstablegems_pixel.png"),
                      triggerText: "[creature] renders the gem cost creature unstable!"
                      );

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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard != base.Card && otherCard.OpponentCard == base.Card.OpponentCard && otherCard.Info && otherCard.Info.gemsCost != null && otherCard.Info.gemsCost.Count > 0;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return base.PreSuccessfulTriggerSequence();
            otherCard.AddTemporaryMod(new CardModificationInfo(Ability.Brittle));
            otherCard.RenderCard();
            yield break;
        }

    }
}
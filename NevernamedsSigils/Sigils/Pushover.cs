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
    public class Pushover : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Pushover", "If [creature] is opposed by an enemy creature, the card will perish.",
                      typeof(Pushover),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/pushover.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/pushover_pixel.png"));

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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return base.Card.OnBoard;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return DoOpposedCheck();
            yield break;
        }
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            yield return DoOpposedCheck();
            yield break;
        }
        private IEnumerator DoOpposedCheck()
        {
            if (base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card != null)
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(1f);
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.Card.Die(false, null, true);
            }
            yield break;
        }
    }
}
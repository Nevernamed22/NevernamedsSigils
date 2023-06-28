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
    public class Waterbird : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Waterbird", "If [creature] strikes a slot containing a submerged creature, that creature will re-emerge and [creature] will attack it instead of attacking directly.",
                      typeof(Waterbird),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/waterbird.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/waterbird_pixel.png"));

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
        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return slot.Card != null && attacker == base.Card && slot.Card.FaceDown;
        }
        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            slot.Card.AddTemporaryMod(new CardModificationInfo() { singletonId = "waterbirdEmerged", RemoveOnUpkeep = true });
            slot.Card.SetFaceDown(false, false);
            slot.Card.UpdateFaceUpOnBoardEffects();
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}

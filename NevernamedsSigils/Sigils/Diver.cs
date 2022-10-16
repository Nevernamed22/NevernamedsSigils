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
    public class Diver : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Diver", "[creature] can strike the opposing card even while it is submerged.",
                      typeof(Diver),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/diver.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/diver_pixel.png"));

            Diver.ability = newSigil.ability;
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
            return slot.Card != null && attacker == base.Card && slot.Card.FaceDown && (!base.Card.HasAbility(Ability.Flying) || slot.Card.HasAbility(Ability.Reach));
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            slot.Card.SetFaceDown(false, false);
            slot.Card.UpdateFaceUpOnBoardEffects();
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}

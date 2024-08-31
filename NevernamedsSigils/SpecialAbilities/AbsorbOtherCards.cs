using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

using InscryptionAPI.Card;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class AbsorbOtherCards : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "AbsorbOtherCards", typeof(AbsorbOtherCards)).Id;
        }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            if (base.PlayableCard.OnBoard && otherCard && otherCard.OnBoard && otherCard != base.PlayableCard && otherCard.OpponentCard == base.PlayableCard.OpponentCard && !otherCard.Info.CardIsInSideDeck()) return true;
            else return false;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            if (otherCard == null) { yield break; }
            base.PlayableCard.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
            CardModificationInfo assimilation = new CardModificationInfo(otherCard.Attack, otherCard.Health);
            foreach (Ability ab in otherCard.GetAllAbilities())
            {
                AbilityInfo info = AbilitiesUtil.GetInfo(ab);
                if (info && info.powerLevel >= 0)
                {
                    assimilation.abilities.Add(ab);
                }
            }
            otherCard.UnassignFromSlot();
            bool impactFrameReached = false;
            Tween.Position(otherCard.transform, base.PlayableCard.transform.position + new Vector3(0, 0.1f, 0.5f), 0.3f, 0f, Tween.EaseOut, Tween.LoopType.None, null, delegate ()
            {
                impactFrameReached = true;
            }, true);
            yield return new WaitUntil(() => impactFrameReached);
            yield return new WaitForSeconds(0.1f);
            otherCard.Anim.PlayDeathAnimation(true);
            yield return new WaitForSeconds(1f);
            UnityEngine.Object.Destroy(otherCard.gameObject);
            base.PlayableCard.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
            base.PlayableCard.AddTemporaryMod(assimilation);
            base.PlayableCard.RenderCard();        
            yield break;
        }
    }
}

using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

using InscryptionAPI.Card;

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
            CardModificationInfo assimilation = new CardModificationInfo(otherCard.Attack, otherCard.Health);

            foreach (Ability ab in otherCard.GetAllAbilities())
            {
                assimilation.abilities.Add(ab);
            }
            base.PlayableCard.Info.tribes.AddRange(otherCard.Info.tribes);

            base.PlayableCard.AddTemporaryMod(assimilation);
            otherCard.UnassignFromSlot();
            UnityEngine.Object.Destroy(otherCard.gameObject);

            base.Card.Anim.StrongNegationEffect();
            base.PlayableCard.RenderCard();
            yield break;
        }
    }
}

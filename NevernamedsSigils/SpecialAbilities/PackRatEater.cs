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
    public class PackRatEater : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "PackRatEater", typeof(PackRatEater)).Id;
        }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            if (base.PlayableCard.OnBoard && otherCard && otherCard.OnBoard && otherCard != base.PlayableCard && otherCard.OpponentCard == base.PlayableCard.OpponentCard && !otherCard.Info.CardIsInSideDeck() && otherCard.Info.name == "PackRat") return true;
            else return false;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            if (otherCard != null && !otherCard.Dead) { yield return EatCard(otherCard); }
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return Singleton<BoardManager>.Instance.GetSlots(!base.PlayableCard.OpponentCard).Exists(x => x.Card != null && x.Card.Info.name == "PackRat");
        }
        public override IEnumerator OnResolveOnBoard()
        {
            List<CardSlot> rats = new List<CardSlot>();
            rats.AddRange(Singleton<BoardManager>.Instance.GetSlots(!base.PlayableCard.OpponentCard).FindAll(x => x.Card != null && x.Card.Info.name == "PackRat"));
            foreach(CardSlot slot in rats)
            {
                if (base.PlayableCard && !base.PlayableCard.Dead && slot && slot.Card != null && slot.Card.Info.name == "PackRat") { yield return EatCard(slot.Card); }
            }
            yield break;
        }

        private IEnumerator EatCard(PlayableCard otherCard)
        {
            base.PlayableCard.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);

            int absorbedAttk = otherCard.Attack;
            int absorbedHP = otherCard.Health;
            List<Ability> otherCardAbilities = new List<Ability>();
            foreach (Ability ab in otherCard.GetAllAbilities())
            {
                if (!base.PlayableCard.HasAbility(ab)) otherCardAbilities.Add(ab);
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
            RunState.Run.playerDeck.RemoveCard(otherCard.Info);

            yield return new WaitForSeconds(1f);
            UnityEngine.Object.Destroy(otherCard.gameObject);
            base.PlayableCard.Anim.PlayTransformAnimation();

            yield return new WaitForSeconds(0.15f);

            CardModificationInfo newMod = new CardModificationInfo(absorbedAttk, absorbedHP);
            newMod.abilities = otherCardAbilities;
            RunState.Run.playerDeck.ModifyCard(base.PlayableCard.Info, newMod);

            foreach(Ability abil in newMod.abilities)
            {
                base.PlayableCard.TriggerHandler.AddAbility(abil);
            }
            base.PlayableCard.OnStatsChanged();

            base.PlayableCard.RenderCard();
        }
    }
}
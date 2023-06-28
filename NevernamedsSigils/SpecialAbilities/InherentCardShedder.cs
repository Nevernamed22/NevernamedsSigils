using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class InherentCardShedder : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "InherentCardShedder", typeof(InherentCardShedder)).Id;
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.PlayableCard.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.PlayableCard.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.PlayableCard.Slot, false);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            yield return this.DoStrafe(toLeft, toRight);
            yield break;
        }
        protected virtual IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
            bool flag = toLeft != null && toLeft.Card == null;
            bool flag2 = toRight != null && toRight.Card == null;
            if (this.movingLeft && !flag)
            {
                this.movingLeft = false;
            }
            if (!this.movingLeft && !flag2)
            {
                this.movingLeft = true;
            }
            CardSlot destination = this.movingLeft ? toLeft : toRight;
            bool destinationValid = this.movingLeft ? flag : flag2;
            yield return this.MoveToSlot(destination, destinationValid);
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null && destinationValid)
            {
                CardSlot oldSlot = base.PlayableCard.Slot;
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.PlayableCard, destination, 0.1f, null, true);
                yield return this.PostSuccessfulMoveSequence(oldSlot);
                yield return new WaitForSeconds(0.25f);
                oldSlot = null;
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            string cardIdentifier = "SigilNevernamed ShadowedCreature";
            if (base.Card.Info.GetExtendedProperty("InherentCardShedderLeaveBehind") != null) { cardIdentifier = base.Card.Info.GetExtendedProperty("InherentCardShedderLeaveBehind"); }

            yield return new WaitForSeconds(0.1f);
            if (oldSlot && oldSlot.Card == null)
            {
                CardInfo segment = CardLoader.GetCardByName(cardIdentifier);
                segment.mods.Add(base.PlayableCard.CondenseMods(new List<Ability>() {  }));
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
            }
            yield break;
        }
        protected bool movingLeft;
    }
}

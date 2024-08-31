using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Dripping : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Dripping", "At the end of the owner's turn, [creature] will move in the direction inscribed in the sigil and leave part of it's own body in it's wake.",
                      typeof(Dripping),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/dripping.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/dripping_pixel.png"));

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

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
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
            if (destination != null && destinationValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null && destinationValid)
            {
                CardSlot oldSlot = base.Card.Slot;
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
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
            string cardIdentifier = "SigilNevernamed LooseFlesh";
            switch (Tools.GetActAsInt())
            {
                case 3:
                    cardIdentifier = "SigilNevernamed Components";
                    break;
                case 4:
                    cardIdentifier = "SigilNevernamed LooseFleshGrimora";
                    break;
                default:
                    cardIdentifier = "SigilNevernamed LooseFlesh";
                    break;
            }
            if (base.Card.Info.GetExtendedProperty("DrippingLeaveBehind") != null) { cardIdentifier = base.Card.Info.GetExtendedProperty("DrippingLeaveBehind"); }

            yield return new WaitForSeconds(0.1f);
            if (oldSlot && oldSlot.Card == null)
            {
                CardInfo segment = CardLoader.GetCardByName(cardIdentifier);
                segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Dripping.ability }));
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
            }
            yield break;
        }
        protected bool movingLeft;

    }
}

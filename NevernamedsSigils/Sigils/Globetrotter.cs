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
    public class Globetrotter : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Globetrotter", "At the end of the owner's turn, [creature] will move in the direction inscribed on the sigil. If obstructed, it will move to the other side of the board and begin to circle back.",
                      typeof(Globetrotter),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/globetrotter.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/globetrotter_pixel.png"));

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
        private bool randomisedSigil;
        public override bool RespondsToDrawn()
        {
            return !randomisedSigil;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return !randomisedSigil;
        }
        public override IEnumerator OnDrawn()
        {
            randomisedSigil = true;
            if (UnityEngine.Random.value <= 0.5f) { Flip(); }
            yield break;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            randomisedSigil = true;
            if (UnityEngine.Random.value <= 0.5f) { Flip(); }
            yield break;
        }
        public void Flip()
        {
            this.movingLeft = true;
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, true);
            base.Card.RenderInfo.flippedPortrait = base.Card.Info.flipPortraitForStrafe;
            base.Card.RenderCard();
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd && !base.Card.HasAbility(Stalwart.ability);
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            CardSlot opposing = base.Card.OpposingSlot();
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            yield return this.DoStrafe(toLeft, toRight, opposing);
            yield break;
        }
        protected virtual IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight, CardSlot opposing)
        {
            //Valid means exists and is empty
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            bool opposingValid = opposing != null && opposing.Card == null;

            bool switchSides = false;
            if (movingLeft && !toLeftValid)
            {
                movingLeft = false;
              if (opposingValid)  switchSides = true;
            }
            if (!movingLeft && !toRightValid)
            {
                if (opposingValid) switchSides = true;
                movingLeft = true;
            }

                CardSlot destination;
            if (switchSides) { destination = opposing; }
            else { destination = movingLeft ? toLeft : toRight; }
            bool destinationValid = this.movingLeft ? toLeftValid : toRightValid;

            yield return this.MoveToSlot(destination, destinationValid, switchSides);

            if (destination != null && destinationValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid, bool swappedSides)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if ((destination != null && destinationValid || swappedSides))
            {
                CardSlot oldSlot = base.Card.Slot;
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                if (swappedSides) { base.Card.SetIsOpponentCard(!destination.IsPlayerSlot); }
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
            if (base.Card.Info.GetExtendedProperty("GlobetrotterLeaveBehind") != null)
            {
                yield return new WaitForSeconds(0.1f);
                if (oldSlot && oldSlot.Card == null)
                {
                    CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("GlobetrotterLeaveBehind"));
                    segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Globetrotter.ability }));
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
                }
            }
            yield break;
        }
        protected bool movingLeft;
    }
}
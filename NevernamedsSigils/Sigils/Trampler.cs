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
    public class Trampler : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trampler", "At the end of the owner's turn, [creature] will move in the direction inscribed on the sigil. Cards in the way will be killed, and grant +1 attack power to the sigil bearer.",
                      typeof(Trampler),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/trampler.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/trampler_pixel.png"));

            Trampler.ability = newSigil.ability;
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
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd && !base.Card.HasAbility(Stalwart.ability);
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
            bool toLeftValid = toLeft != null;
            bool toRightValid = toRight != null;

            if (this.movingLeft && !toLeftValid)
            {
                this.movingLeft = false;
            }
            if (!this.movingLeft && !toRightValid)
            {
                this.movingLeft = true;
            }

            CardSlot destination = this.movingLeft ? toLeft : toRight;
            bool destinationValid = this.movingLeft ? toLeftValid : toRightValid;

            yield return this.MoveToSlot(destination, destinationValid);
            bool flag3 = destination != null && destinationValid;
            if (flag3)
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
            if (destination != null && destinationValid && base.Card != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (base.Card != null && destination.Card != null && destination.Card != base.Card)
                    {
                        if (destination.Card.FaceDown)
                        {
                            destination.Card.SetFaceDown(false, true);
                        }
                        yield return destination.Card.Die(false, base.Card, false);
                        base.Card.temporaryMods.Add(new CardModificationInfo(1, 0));
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                if (destination.Card == null && base.Card != null)
                {
                    CardSlot oldSlot = base.Card.Slot;
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                    yield return this.PostSuccessfulMoveSequence(oldSlot);
                    yield return new WaitForSeconds(0.25f);
                    oldSlot = null;
                }
                else
                {
                    if (destination.Card != base.Card && base.Card != null)
                    {
                        base.Card.Anim.StrongNegationEffect();
                        yield return new WaitForSeconds(0.15f);
                    }
                }
            }
            else if (base.Card != null)
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            if (base.Card.Info.GetExtendedProperty("TramplerLeaveBehind") != null)
            {
                yield return new WaitForSeconds(0.1f);
                if (oldSlot && oldSlot.Card == null)
                {
                    CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("TramplerLeaveBehind"));
                    segment.mods.Add(base.Card.CondenseMods(new List<Ability>() {Trampler.ability }));
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
                }
            }
            yield break;
        }
        protected bool movingLeft;
    }
}
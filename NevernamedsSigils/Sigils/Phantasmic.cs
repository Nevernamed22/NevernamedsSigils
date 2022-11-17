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
    public class Phantasmic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Phantasmic", "At the end of the turn, [creature] will move in the direction inscribed in the sigil, phasing past obstacles and wrapping around to the other side of the board if movement is impossible.",
                      typeof(Phantasmic),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular},
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/phantasmic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/phantasmic_pixel.png"));

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
        public override bool RespondsToDrawn() { return !hasRandomised; }
        public override bool RespondsToResolveOnBoard() { return !hasRandomised; }
        public override IEnumerator OnDrawn() { RandomiseIcon(); yield break; }
        public override IEnumerator OnResolveOnBoard() { RandomiseIcon(); yield break; }
        private void RandomiseIcon()
        {
            movingLeft = UnityEngine.Random.value <= 0.5f;
            hasRandomised = true;
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
        }
        private bool movingLeft;
        private bool hasRandomised;
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return (playerTurnEnd == base.Card.IsPlayerCard() && !base.Card.HasAbility(Stalwart.ability));
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard));
            for (int i = availableSlots.Count - 1; i >= 0; i--) { if (availableSlots[i].Card != null) availableSlots.RemoveAt(i); }

            int baseIndex = base.Card.slot.Index;

            if (availableSlots.Count > 0)
            {
                List<CardSlot> greaterIndexSlots = availableSlots.FindAll((CardSlot slot) => slot.Index > baseIndex);
                List<CardSlot> lesserIndexSlots = availableSlots.FindAll((CardSlot slot) => slot.Index < baseIndex);

                CardSlot finalTarget = null;
                if (movingLeft)
                {
                    if (lesserIndexSlots.Count > 0) finalTarget = lesserIndexSlots[lesserIndexSlots.Count - 1];
                    else if (greaterIndexSlots.Count > 0) finalTarget = greaterIndexSlots[greaterIndexSlots.Count - 1];
                }
                else
                {
                    if (greaterIndexSlots.Count > 0) finalTarget = greaterIndexSlots[0];
                    else if (lesserIndexSlots.Count > 0) finalTarget = lesserIndexSlots[0];
                }

                if (finalTarget != null)
                {
                    if (Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, movingLeft) == finalTarget)
                    {
                        CardSlot oldSlot = base.Card.Slot;
                        yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, finalTarget, 0.1f, null, true);
                        yield return this.PostSuccessfulMoveSequence(oldSlot);
                    }
                    else
                    {
                        CardSlot oldSlot = base.Card.Slot;
                        if (base.Card.Anim is PaperCardAnimationController)
                        {
                            ((PaperCardAnimationController)base.Card.Anim).Play("death", 0f);
                        }
                        yield return new WaitForSeconds(0.5f);
                        yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, finalTarget, 0f, null, true);
                        base.Card.Anim.LightNegationEffect();
                        yield return this.PostSuccessfulMoveSequence(oldSlot);
                    }
                }

                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.3f);
            }
            yield break;
        }
        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            if (base.Card.Info.GetExtendedProperty("PhantasmicLeaveBehind") != null)
            {
                yield return new WaitForSeconds(0.1f);
                if (oldSlot && oldSlot.Card == null)
                {
                    CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("PhantasmicLeaveBehind"));
                    segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Phantasmic.ability }));
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
                }
            }
            yield break;
        }
    }
}

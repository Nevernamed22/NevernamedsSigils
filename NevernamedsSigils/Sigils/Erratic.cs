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
    public class Erratic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Erratic", "At the end of the owner's turn, [creature] will move in the direction inscribed on the sigil will not move if the target slot is obstructed. Direction has a 50/50 chance to change after moving.",
                      typeof(Erratic),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/erratic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/erratic_pixel.png"));

            Erratic.ability = newSigil.ability;
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
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            yield return this.DoStrafe();
            yield break;
        }
        protected virtual IEnumerator DoStrafe()
        {
            CardSlot direction = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, IsMovingLeft);
            if (direction != null && direction.Card == null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return this.MoveToSlot(direction);
                yield return base.LearnAbility(0f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
            }

            if (UnityEngine.Random.value <= 0.5f) { IsMovingLeft = true; }
            else { IsMovingLeft = false; }

            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, IsMovingLeft);
            base.Card.RenderCard();
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination)
        {
            base.Card.RenderInfo.flippedPortrait = (IsMovingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();

            CardSlot oldSlot = base.Card.Slot;
            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
            yield return this.PostSuccessfulMoveSequence(oldSlot);
            yield return new WaitForSeconds(0.25f);

            yield break;
        }
        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            if (base.Card.Info.GetExtendedProperty("ErraticLeaveBehind") != null)
            {
                yield return new WaitForSeconds(0.1f);
                if (oldSlot && oldSlot.Card == null)
                {
                    CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("ErraticLeaveBehind"));
                    segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Erratic.ability }));
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
                }
            }
            yield break;
        }
        private bool IsMovingLeft;
    }
}
using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class RunningStrike : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Running Strike", "After attacking, [creature] will move in the direction inscribed on the sigil, striking the space opposing it's destination as it does so.",
                      typeof(RunningStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/runningstrike.png"),
                      pixelTex: null);

            RunningStrike.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
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
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            if (this.movingLeft && !toLeftValid) this.movingLeft = false;
            if (!this.movingLeft && !toRightValid) this.movingLeft = true;

            CardSlot destination = null;
            if (movingLeft)
            {
                if (toLeftValid) destination = toLeft;
                else if (toRightValid) { destination = toRight; movingLeft = false; }
            }
            else
            {
                if (toRightValid) destination = toRight;
                else if (toLeftValid) { destination = toLeft; movingLeft = true; }
            }
            if (destination != null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (destination.opposingSlot != null && destination.opposingSlot != null)
                {
                    yield return attackSlot(destination.opposingSlot);
                    yield return new WaitForSeconds(0.8f);
                }
                if (!base.Card.HasAbility(Stalwart.ability)) yield return this.MoveToSlot(destination);
                yield return base.LearnAbility(0f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
        public IEnumerator attackSlot(CardSlot target)
        {
            if (base.Card && base.Card.slot && target)
            {
                FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot, new List<CardSlot>() { target });
            }
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null)
            {
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
        protected bool movingLeft;
    }
}


using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class FakeCombatHandler
    {
        public static void DoSimpleFakeCombat(int damage, CardSlot attacker, CardSlot target)
        {
            if (BoardManager.Instance) BoardManager.Instance.StartCoroutine(HandleSimpleFakeCombat(damage, attacker, target));
        }
        private static IEnumerator HandleSimpleFakeCombat(int damage, CardSlot attacker, CardSlot target)
        {
            bool impactFrameReached = false;
            attacker.Card.Anim.PlayAttackAnimation(false, target, delegate ()
            {
                impactFrameReached = true;
            });
            yield return new WaitUntil(() => impactFrameReached);
            yield return target.Card.TakeDamage(damage, attacker.Card);
            yield break;
        }
        public static void DoFakeCombat(CardSlot attackingSlot, List<CardSlot> overrideTargets = null)
        {
            FakeCombatThing fakecombat = new FakeCombatThing();
            if (attackingSlot && attackingSlot.Card) fakecombat.StartFakeCombat(attackingSlot.Card, overrideTargets);
        }
        public class FakeCombatThing
        {
            public void StartFakeCombat(PlayableCard attacker, List<CardSlot> overrideTargets = null)
            {
                attacker.StartCoroutine(this.FakeCombat(!attacker.OpponentCard, null, attacker.slot, overrideTargets));
            }
            public int DamageDealtThisPhase { get; private set; }
            public IEnumerator FakeCombat(bool playerIsAttacker, SpecialBattleSequencer specialSequencer, CardSlot attacker, List<CardSlot> overrideTargets = null)
            {
                this.DamageDealtThisPhase = 0;
                yield return this.SlotAttackSequence(attacker, overrideTargets);

                if (this.DamageDealtThisPhase > 0 && !isCombatPhase)
                {
                    yield return new WaitForSeconds(0.4f);
                    int excessDamage = 0;
                    if (playerIsAttacker)
                    {
                        excessDamage = Singleton<LifeManager>.Instance.Balance + this.DamageDealtThisPhase - 5;
                        excessDamage = Mathf.Max(0, excessDamage);
                    }

                    int damage = this.DamageDealtThisPhase - excessDamage;
                    if (this.DamageDealtThisPhase >= 666) AchievementManager.Unlock(Achievement.PART2_SPECIAL2);

                    if (specialSequencer == null || !specialSequencer.PreventDamageAddedToScales)
                    {
                        yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, !playerIsAttacker, 0f, null, 0f);
                    }
                    if (specialSequencer != null)
                    {
                        yield return specialSequencer.DamageAddedToScale(damage + excessDamage, playerIsAttacker);
                    }

                    bool giveCurrency = (!(specialSequencer != null) || !specialSequencer.PreventDamageAddedToScales) && excessDamage > 0 && Singleton<TurnManager>.Instance.Opponent.NumLives == 1 && Singleton<TurnManager>.Instance.Opponent.GiveCurrencyOnDefeat;
                    if (giveCurrency)
                    {
                        yield return Singleton<TurnManager>.Instance.Opponent.TryRevokeSurrender();
                        RunState.Run.currency += excessDamage;
                    }
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    this.DamageDealtThisPhase = 0;
                }
                yield break;
            }
            public IEnumerator SlotAttackSlot(CardSlot attackingSlot, CardSlot opposingSlot, float waitAfter = 0f)
            {
                yield return Singleton<GlobalTriggerHandler>.Instance.TriggerCardsOnBoard(Trigger.SlotTargetedForAttack, false, new object[]
                {
                opposingSlot,
                attackingSlot.Card
                });
                yield return new WaitForSeconds(0.025f);
                if (attackingSlot.Card != null)
                {
                    bool doingAttackAnimation = attackingSlot.Card.Anim.DoingAttackAnimation;
                    if (doingAttackAnimation)
                    {
                        yield return new WaitUntil(() => !attackingSlot.Card.Anim.DoingAttackAnimation);
                        yield return new WaitForSeconds(0.25f);
                    }

                    if (opposingSlot.Card != null && attackingSlot.Card.AttackIsBlocked(opposingSlot))
                    {
                        ProgressionData.SetAbilityLearned(Ability.PreventAttack);
                        yield return Singleton<CombatPhaseManager>.Instance.ShowCardBlocked(attackingSlot.Card);
                    }
                    else
                    {
                        if (attackingSlot.Card.CanAttackDirectly(opposingSlot))
                        {
                            this.DamageDealtThisPhase += attackingSlot.Card.Attack;
                            yield return Singleton<CombatPhaseManager>.Instance.VisualizeCardAttackingDirectly(attackingSlot, opposingSlot, 0);
                            bool flag4 = attackingSlot.Card.TriggerHandler.RespondsToTrigger(Trigger.DealDamageDirectly, new object[]
                            {
                            attackingSlot.Card.Attack
                            });
                            if (flag4)
                            {
                                yield return attackingSlot.Card.TriggerHandler.OnTrigger(Trigger.DealDamageDirectly, new object[]
                                {
                                attackingSlot.Card.Attack
                                });
                            }
                        }
                        else
                        {
                            float heightOffset = (opposingSlot.Card == null) ? 0f : opposingSlot.Card.SlotHeightOffset;
                            if (heightOffset > 0f)
                            {
                                Tween.Position(attackingSlot.Card.transform, attackingSlot.Card.transform.position + Vector3.up * heightOffset, 0.05f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                            }
                            attackingSlot.Card.Anim.PlayAttackAnimation(attackingSlot.Card.IsFlyingAttackingReach(), opposingSlot, null);
                            yield return new WaitForSeconds(0.07f);
                            attackingSlot.Card.Anim.SetAnimationPaused(true);
                            PlayableCard attackingCard = attackingSlot.Card;
                            yield return Singleton<GlobalTriggerHandler>.Instance.TriggerCardsOnBoard(Trigger.CardGettingAttacked, false, new object[]
                            {
                            opposingSlot.Card
                            });
                            if (attackingCard != null && attackingCard.Slot != null)
                            {
                                attackingSlot = attackingCard.Slot;
                                if (attackingSlot.Card.IsFlyingAttackingReach())
                                {
                                    opposingSlot.Card.Anim.PlayJumpAnimation();
                                    yield return new WaitForSeconds(0.3f);
                                    attackingSlot.Card.Anim.PlayAttackInAirAnimation();
                                }
                                attackingSlot.Card.Anim.SetAnimationPaused(false);
                                yield return new WaitForSeconds(0.05f);
                                int overkillDamage = attackingSlot.Card.Attack - opposingSlot.Card.Health;
                                yield return opposingSlot.Card.TakeDamage(attackingSlot.Card.Attack, attackingSlot.Card);
                                yield return this.DealOverkillDamage(overkillDamage, attackingSlot, opposingSlot);
                                if (attackingSlot.Card != null && heightOffset > 0f)
                                {
                                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(attackingSlot.Card, attackingSlot.Card.Slot, 0.1f, null, false);
                                }
                            }
                            attackingCard = null;
                        }
                    }
                    yield return new WaitForSeconds(waitAfter);
                }
                yield break;
            }
            private IEnumerator SlotAttackSequence(CardSlot slot, List<CardSlot> overrideTargets = null)
            {
                List<CardSlot> opposingSlots = slot.Card.GetOpposingSlots();
                if (overrideTargets != null) opposingSlots = overrideTargets;
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);

                foreach (CardSlot opposingSlot in opposingSlots)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                    yield return this.SlotAttackSlot(slot, opposingSlot, (opposingSlots.Count > 1) ? 0.1f : 0f);
                }
                Singleton<CombatPhaseManager>.Instance.VisualizeClearSniperAbility();
                yield break;
            }
            protected virtual IEnumerator DealOverkillDamage(int damage, CardSlot attackingSlot, CardSlot opposingSlot)
            {
                if (attackingSlot.Card != null && attackingSlot.IsPlayerSlot && damage > 0)
                {
                    PlayableCard queuedCard = Singleton<BoardManager>.Instance.GetCardQueuedForSlot(opposingSlot);
                    if (queuedCard != null)
                    {
                        yield return new WaitForSeconds(0.1f);
                        Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.QueueView, false, false);
                        yield return new WaitForSeconds(0.3f);

                        if (queuedCard.HasAbility(Ability.PreventAttack))
                        {
                            yield return Singleton<CombatPhaseManager>.Instance.ShowCardBlocked(attackingSlot.Card);
                        }
                        else
                        {
                            yield return Singleton<CombatPhaseManager>.Instance.PreOverkillDamage(queuedCard);
                            yield return queuedCard.TakeDamage(damage, attackingSlot.Card);
                            yield return Singleton<CombatPhaseManager>.Instance.PostOverkillDamage(queuedCard);
                        }
                    }
                    queuedCard = null;
                }
                yield break;
            }

            public static Ability ability;
            public static bool isCombatPhase;
        }

    }
}

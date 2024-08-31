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
        public static IEnumerator DoSimpleFakeCombat(int damage, CardSlot attacker, CardSlot target)
        {
            yield return HandleSimpleFakeCombat(damage, attacker, target);
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
        public class FakeCombatThing
        {
            public int DamageDealtThisPhase { get; private set; }
            public IEnumerator FakeCombat(bool playerIsAttacker, SpecialBattleSequencer specialSequencer, CardSlot attacker, List<CardSlot> overrideTargets = null, int overrideDMG = -1)
            {
                this.DamageDealtThisPhase = 0;
                yield return this.SlotAttackSequence(attacker, overrideTargets, overrideDMG);

                if (this.DamageDealtThisPhase > 0 && !isCombatPhase)
                {
                    yield return new WaitForSeconds(0.4f);
                    yield return this.VisualizeDamageMovingToScales(playerIsAttacker);
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
             private IEnumerator VisualizeDamageMovingToScales(bool playerIsAttacker)
            {
                if (Singleton<LifeManager>.Instance.Scales3D != null)
                {
                    Vector3 scalesPos = playerIsAttacker ? Singleton<LifeManager>.Instance.Scales3D.OpponentSide : Singleton<LifeManager>.Instance.Scales3D.PlayerSide;
                    foreach (Transform transform in this.damageWeights)
                    {
                        Tween.Position(transform, scalesPos, 0.2f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                        UnityEngine.Object.Destroy(transform.gameObject, 0.2f);
                        yield return new WaitForSeconds(0.02f);
                    }
                    scalesPos = default(Vector3);
                }
                else
                {
                    foreach (Transform transform2 in this.damageWeights)
                    {
                        Part1Scales.DissolveWeight(transform2.gameObject);
                        yield return new WaitForSeconds(0.02f);
                    }
                }
                yield return new WaitForSeconds(0.25f);
                yield break;
            }
            public IEnumerator SpecialVisualiseAttacking(CardSlot attackingSlot, CardSlot targetSlot, int damage)
            {
                if (Singleton<CombatPhaseManager>.Instance)
                {
                    if (Singleton<CombatPhaseManager>.Instance is CombatPhaseManager3D)
                    {
                        List<Transform> newWeights = new List<Transform>();
                        for (int i = 0; i < Mathf.Min(20, damage); i++)
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((Singleton<CombatPhaseManager>.Instance as CombatPhaseManager3D).weightPrefab);
                            Vector3 vector = new Vector3(0f, 0f, attackingSlot.IsPlayerSlot ? 0.75f : -0.75f);
                            gameObject.transform.position = targetSlot.transform.position + vector + new Vector3((float)i * 0.1f, 0f, (float)i * 0.1f);
                            gameObject.transform.eulerAngles = UnityEngine.Random.insideUnitSphere;
                            newWeights.Add(gameObject.transform);
                        }
                        this.damageWeights.AddRange(newWeights);
                        attackingSlot.Card.Anim.PlayAttackAnimation(true, targetSlot, delegate ()
                        {
                            if (Singleton<TableVisualEffectsManager>.Instance != null)
                            {
                                Singleton<TableVisualEffectsManager>.Instance.ThumpTable(0.075f * (float)Mathf.Min(10, damage));
                            }
                            foreach (Transform transform in newWeights)
                            {
                                if (transform != null)
                                {
                                    transform.gameObject.SetActive(true);
                                    transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 4f, (ForceMode)2);
                                }
                            }
                        });
                    }
                    else
                    {
                        attackingSlot.Card.Anim.PlayAttackAnimation(true, targetSlot, delegate ()
                        {
                        });
                    }
                }                
                yield break;
            }
            public IEnumerator SlotAttackSlot(CardSlot attackingSlot, CardSlot opposingSlot, float waitAfter = 0f, int overrideDMG = -1)
            {
                yield return Singleton<GlobalTriggerHandler>.Instance.TriggerCardsOnBoard(Trigger.SlotTargetedForAttack, false, new object[] { opposingSlot, attackingSlot.Card });
                yield return new WaitForSeconds(0.025f);

                if (attackingSlot.Card != null)
                {
                    if (attackingSlot.Card.Anim.DoingAttackAnimation)
                    {
                        yield return new WaitUntil(() => !attackingSlot.Card.Anim.DoingAttackAnimation);
                        yield return new WaitForSeconds(0.25f);
                    }

                    //Repulsive Block
                    if (opposingSlot.Card != null && attackingSlot.Card.AttackIsBlocked(opposingSlot)) { ProgressionData.SetAbilityLearned(Ability.PreventAttack); yield return Singleton<CombatPhaseManager>.Instance.ShowCardBlocked(attackingSlot.Card); }
                    else
                    {
                        //Direct
                        if (attackingSlot.Card.CanAttackDirectly(opposingSlot))
                        {
                            this.DamageDealtThisPhase += overrideDMG > -1 ? overrideDMG : attackingSlot.Card.Attack;
                            yield return this.SpecialVisualiseAttacking(attackingSlot, opposingSlot, overrideDMG > -1 ? overrideDMG : attackingSlot.Card.Attack);

                            if (attackingSlot.Card.TriggerHandler.RespondsToTrigger(Trigger.DealDamageDirectly, new object[] { overrideDMG > -1 ? overrideDMG : attackingSlot.Card.Attack }))
                            {
                                yield return attackingSlot.Card.TriggerHandler.OnTrigger(Trigger.DealDamageDirectly, new object[] { overrideDMG > -1 ? overrideDMG : attackingSlot.Card.Attack });
                            }
                        }
                        else //Blocked
                        {
                            float heightOffset = (opposingSlot.Card == null) ? 0f : opposingSlot.Card.SlotHeightOffset;
                            if (heightOffset > 0f) { Tween.Position(attackingSlot.Card.transform, attackingSlot.Card.transform.position + Vector3.up * heightOffset, 0.05f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true); }

                            attackingSlot.Card.Anim.PlayAttackAnimation(attackingSlot.Card.IsFlyingAttackingReach(), opposingSlot, null);
                            yield return new WaitForSeconds(0.07f);
                            attackingSlot.Card.Anim.SetAnimationPaused(true);
                            PlayableCard attackingCard = attackingSlot.Card;
                            yield return Singleton<GlobalTriggerHandler>.Instance.TriggerCardsOnBoard(Trigger.CardGettingAttacked, false, new object[] { opposingSlot.Card });

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
                                int overkillDamage = overrideDMG > -1 ? overrideDMG - opposingSlot.Card.Health : attackingSlot.Card.Attack - opposingSlot.Card.Health;

                                yield return opposingSlot.Card.TakeDamage(overrideDMG > -1 ? overrideDMG : attackingSlot.Card.Attack, attackingSlot.Card);
                                yield return this.DealOverkillDamage(overkillDamage, attackingSlot, opposingSlot);

                                if (attackingSlot.Card != null && heightOffset > 0f) { yield return Singleton<BoardManager>.Instance.AssignCardToSlot(attackingSlot.Card, attackingSlot.Card.Slot, 0.1f, null, false); }
                            }
                            attackingCard = null;
                        }
                    }
                    yield return new WaitForSeconds(waitAfter);

                    //Recreated Patches
                    if (attackingSlot.Card != null &&  attackingSlot.Card.HasAbility(SplashDamage.ability))
                    {
                        CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true);
                        CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false);

                        if (toLeft && toLeft.Card != null) { yield return toLeft.Card.TakeDamage(1, attackingSlot.Card); }
                        if (toRight && toRight.Card != null) { yield return toRight.Card.TakeDamage(1, attackingSlot.Card); }
                    }
                    if (opposingSlot.Card != null && opposingSlot.Card.FaceDown && opposingSlot.Card.HasAbility(SubaquaticSpines.ability) && attackingSlot.Card != null &&  !attackingSlot.Card.AttackIsBlocked(opposingSlot) && (!attackingSlot.Card.HasAbility(Ability.Flying) || opposingSlot.Card.HasAbility(Ability.Reach)))
                    {
                        yield return new WaitForSeconds(0.55f);
                        yield return attackingSlot.Card.TakeDamage(1, opposingSlot.Card);
                    }
                }
                yield break;
            }
            private IEnumerator SlotAttackSequence(CardSlot slot, List<CardSlot> overrideTargets = null, int overrideDMG = -1)
            {
                List<CardSlot> opposingSlots = slot.Card.GetOpposingSlots();
                if (overrideTargets != null) opposingSlots = overrideTargets;
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);

                foreach (CardSlot opposingSlot in opposingSlots)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                    yield return this.SlotAttackSlot(slot, opposingSlot, (opposingSlots.Count > 1) ? 0.1f : 0f, overrideDMG);
                }
                Singleton<CombatPhaseManager>.Instance.VisualizeClearSniperAbility();
                yield break;
            }
            protected virtual IEnumerator DealOverkillDamage(int damage, CardSlot attackingSlot, CardSlot opposingSlot)
            {
                if (attackingSlot && attackingSlot.Card && attackingSlot.Card.HasAbility(Mauler.ability) & damage > 0)
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, !attackingSlot.Card.slot.IsPlayerSlot, 0.1f, null, 0f, true);
                }
                else if (attackingSlot.Card != null && attackingSlot.IsPlayerSlot && damage > 0)
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

            private List<Transform> damageWeights = new List<Transform>();
            public static Ability ability;
            public static bool isCombatPhase;
        }

    }
}

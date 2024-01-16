using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx;
using System.Collections;
using InscryptionAPI.Card;
using GBC;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(PlayableCard), "CanAttackDirectly", MethodType.Normal)]
    public class CanAttackDirectlyPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref CardSlot opposingSlot, ref bool __result, PlayableCard __instance)
        {
            if (opposingSlot.Card)
            {
                if (opposingSlot.Card.HasAbility(Immaterial.ability)) { __result = true; }
                else if (opposingSlot.Card.HasAbility(Giant.ability) || (opposingSlot.Card.TemporaryMods.Exists(x => x.singletonId == "waterbirdEmerged") && __instance.HasAbility(Waterbird.ability))) { __result = false; }
            }
        }
    }
    [HarmonyPatch(typeof(PlayableCard), "AttackIsBlocked", MethodType.Normal)]
    public class AttackIsBlockedPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref CardSlot opposingSlot, ref bool __result, PlayableCard __instance)
        {
            //Repulsive when powered
            if (opposingSlot.Card && opposingSlot.Card.HasAbility(RepulsiveWhenPowered.ability) && Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(opposingSlot))
            {
                __result = true;
            }
            //Web
            if (__instance.Info.tribes.Contains(Tribe.Insect) && opposingSlot.Card && opposingSlot.Card.HasAbility(WebSigil.ability))
            {
                __result = true;
            }
            else if (opposingSlot.Card && opposingSlot.Card.Info.GetExtendedProperty("InherentRepulsive") != null) __result = true;
        }
    }
    [HarmonyPatch(typeof(CombatPhaseManager), "SlotAttackSlot", 0)]
    public class SlotAttackSlotPrefix
    {
        [HarmonyPrefix]
        public static bool SlotAttackSlot(ref CardSlot attackingSlot, ref CardSlot opposingSlot, float waitAfter = 0f)
        {
            if (attackingSlot.Card != null)
            {
                if (attackingSlot.Card.gameObject.GetComponent<TangledEffect>() && !attackingSlot.Card.gameObject.GetComponent<TangledEffect>().primed)
                {
                    attackingSlot.Card.gameObject.GetComponent<TangledEffect>().primed = true;
                }

                    //Abstain
                    if (attackingSlot.Card.HasAbility(Abstain.ability) || (attackingSlot.Card.GetComponent<Docile>() && attackingSlot.Card.GetComponent<Docile>().turnsUntilNextAttack != 0) || (attackingSlot.Card.GetComponent<VivaLaRevolution>() && attackingSlot.Card.GetComponent<VivaLaRevolution>().wasOpponent != attackingSlot.Card.OpponentCard))
                {
                    attackingSlot.Card.Anim.StrongNegationEffect();
                    return false;
                }

                //Cute
                Cute.AffectedByCute guilt = attackingSlot.Card.GetComponent<Cute.AffectedByCute>();
                if (guilt)
                {
                    if (Singleton<TurnManager>.Instance.TurnNumber == (guilt.turnInflicted + 1))
                    {
                        attackingSlot.Card.Anim.StrongNegationEffect();
                        return false;
                    }
                    else if (Singleton<TurnManager>.Instance.TurnNumber > (guilt.turnInflicted + 1))
                    {
                        UnityEngine.Object.Destroy(guilt);
                    }
                }


                PlayableCard card = attackingSlot.Card;
                if (!attackingSlot.Card.HasAbility(Ability.AllStrike))
                {

                    if (attackingSlot.Card.HasAbility(TrophyHunter.ability) || Singleton<BoardManager>.Instance.AllSlots.Find(x => (x.IsPlayerSlot == card.IsPlayerCard()) && x.Card && x.Card.HasAbility(Telepathic.ability)))
                    {
                        PlayableCard target = Tools.GetStrongestCardOnBoard(!card.slot.IsPlayerSlot);
                        if (target && target.slot) opposingSlot = target.slot;
                    }
                    else if (attackingSlot.Card.HasAbility(Bully.ability))
                    {
                        PlayableCard target = Tools.GetStrongestCardOnBoard(!card.slot.IsPlayerSlot, true);
                        if (target && target.slot) opposingSlot = target.slot;
                    }
                    else if (attackingSlot.Card.HasAbility(HomeRun.ability) && attackingSlot.Card.GetComponent<HomeRun>())
                    {
                        if (attackingSlot.Card.GetComponent<HomeRun>().home && attackingSlot.Card.GetComponent<HomeRun>().home.opposingSlot) opposingSlot = attackingSlot.Card.GetComponent<HomeRun>().home.opposingSlot;
                    }
                    else if (attackingSlot.Card.HasAbility(UnfocusedStrike.ability))
                    {
                        List<CardSlot> viableslots = new List<CardSlot>();
                        if (card.slot.IsPlayerSlot) viableslots = Singleton<BoardManager>.Instance.opponentSlots;
                        else viableslots = Singleton<BoardManager>.Instance.playerSlots;
                        CardSlot cardSlot = Tools.SeededRandomElement(viableslots);
                        opposingSlot = cardSlot;
                    }
                    if (card.HasAbility(CrookedStrikeLeft.ability) && !card.HasAbility(CrookedStrikeRight.ability))
                    {
                        if (Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true) != null) { opposingSlot = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true); }
                    }
                    else if (card.HasAbility(CrookedStrikeRight.ability) && !card.HasAbility(CrookedStrikeLeft.ability))
                    {
                        if (Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false) != null) { opposingSlot = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false); }
                    }
                    if (card.GetComponent<WaveringStrike>())
                    {
                        if (Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, card.GetComponent<WaveringStrike>().isLeft) != null) { opposingSlot = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, card.GetComponent<WaveringStrike>().isLeft); }
                    }

                    if (opposingSlot && card.HasAbility(DiagonalStrike.ability))
                    {
                        List<CardSlot> slots = Singleton<BoardManager>.Instance.GetAdjacentSlots(opposingSlot);
                        if (!slots.Exists(x => x.Card == null) || !slots.Exists(x => x.Card != null))
                        {
                            opposingSlot = Tools.SeededRandomElement(slots);
                        }
                        else
                        {
                            opposingSlot = slots.Find(x => x.Card != null);
                        }
                    }
                }

                List<CardSlot> opponentSlotsWithCards = (attackingSlot.Card.IsPlayerCard() ? Singleton<BoardManager>.Instance.opponentSlots : Singleton<BoardManager>.Instance.playerSlots).FindAll(x => x.Card != null);
               if (!attackingSlot.Card.HasAbility(Diver.ability)) { opponentSlotsWithCards.RemoveAll(x => x.Card.FaceDown); }          
                if (opponentSlotsWithCards != null && opponentSlotsWithCards.Count > 0)
                {
                    if (attackingSlot.Card.HasAbility(SweepingStrikeLeft.ability))
                    {
                        if (attackingSlot.Card.HasAbility(SweepingStrikeRight.ability))
                        {
                            opposingSlot = UnityEngine.Random.value <= 0.5 ? opposingSlot = opponentSlotsWithCards[0] : opponentSlotsWithCards[opponentSlotsWithCards.Count - 1];
                        }
                        else { opposingSlot = opponentSlotsWithCards[0]; }
                    }
                    else if (attackingSlot.Card.HasAbility(SweepingStrikeRight.ability)) { opposingSlot = opponentSlotsWithCards[opponentSlotsWithCards.Count - 1]; }
                }

                if (opposingSlot && (opposingSlot.Card == null || !opposingSlot.Card.HasAbility(TemptingTarget.ability)))
                {
                    CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true);
                    CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false);
                    if (toLeft != null && toLeft.Card != null && toLeft.Card.HasAbility(TemptingTarget.ability))
                    {
                        opposingSlot = toLeft;
                    }
                    else if (toRight != null && toRight.Card != null && toRight.Card.HasAbility(TemptingTarget.ability))
                    {
                        opposingSlot = toRight;
                    }
                }
                if (opposingSlot != null && card.CanAttackDirectly(opposingSlot))
                {
                    if (Singleton<BoardManager>.Instance.GetSlots(card.OpponentCard).Exists(x => x.Card != null && x.Card.HasAbility(Giant.ability)))
                    {
                        opposingSlot = Singleton<BoardManager>.Instance.GetSlots(card.OpponentCard).Find(x => x.Card != null && x.Card.HasAbility(Giant.ability));
                    }
                }
            }
            return true;
        }


    }

    [HarmonyPatch(typeof(CombatPhaseManager), "SlotAttackSlot", MethodType.Normal)]
    public class SlotAttackSlotPostfix
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, CardSlot attackingSlot, CardSlot opposingSlot, float waitAfter = 0f)
        {
            yield return enumerator;
            if (attackingSlot.Card != null)
            {
                int splashDamageAMT = 0;
                if (attackingSlot.Card.HasAbility(SplashDamage.ability)) { splashDamageAMT++; }
                if ((attackingSlot.Card.HasAbility(SplashDamageWhenPowered.ability) && Singleton<ConduitCircuitManager>.Instance != null && Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(attackingSlot))) { splashDamageAMT++; }
                if (splashDamageAMT > 0)
                {
                    CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true);
                    CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false);

                    if (toLeft && toLeft.Card != null) { yield return toLeft.Card.TakeDamage(splashDamageAMT, attackingSlot.Card); }
                    if (toRight && toRight.Card != null) { yield return toRight.Card.TakeDamage(splashDamageAMT, attackingSlot.Card); }
                }
                if (attackingSlot.Card.HasAbility(Piercing.ability))
                {
                    PlayableCard queuedCard = Singleton<BoardManager>.Instance.GetCardQueuedForSlot(opposingSlot);
                    if (queuedCard != null && !queuedCard.Dead)
                    {
                        yield return new WaitForSeconds(0.1f);
                        Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.QueueView, false, false);
                        if (Tools.GetActAsInt() == 2 && !Singleton<GlobalTriggerHandler>.Instance.AbilitiesTriggeredThisTurn.Contains(Piercing.ability))
                        {
                            yield return Singleton<TextBox>.Instance.ShowUntilInput($"{attackingSlot.Card.Info.DisplayedNameLocalized} pierces through and deals 1 damage to {queuedCard.Info.displayedNameLocId}!",
                                (TextBox.Style)attackingSlot.Card.Info.temple,
                                null,
                                TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                        }
                        yield return new WaitForSeconds(0.3f);
                        yield return queuedCard.TakeDamage(1, attackingSlot.Card);
                        Singleton<GlobalTriggerHandler>.Instance.AbilitiesTriggeredThisTurn.Add(Piercing.ability);
                    }
                }
                if (opposingSlot.Card != null && opposingSlot.Card.FaceDown && opposingSlot.Card.HasAbility(SubaquaticSpines.ability) && !attackingSlot.Card.AttackIsBlocked(opposingSlot) && (!attackingSlot.Card.HasAbility(Ability.Flying) || opposingSlot.Card.HasAbility(Ability.Reach)))
                {
                    yield return new WaitForSeconds(0.55f);
                    yield return attackingSlot.Card.TakeDamage(1, opposingSlot.Card);
                }
            }
            yield break;
        }
    }
    [HarmonyPatch(typeof(PlayableCard), "TakeDamage", MethodType.Normal)]
    public class TakeDamagePatch
    {
        [HarmonyPrefix]
        public static void Prefix(out int __state, PlayableCard __instance, ref int damage, ref PlayableCard attacker)
        {
            __state = damage;
            if (__instance)
            {
                if (__instance.HasShield() && __instance.GetComponent<Healshield>()) { __instance.GetComponent<Healshield>().tookDamageThisTurn = true; }
                if (__instance.HasAbility(Resilient.ability) && damage > 1) { __state = 1; damage = 1; }
                if (__instance.HasAbility(Soak.ability) && damage > 0) { __state--; damage--; }
                if (__instance.HasAbility(Sturdy.ability) && damage > 0) { __state--; damage--; }
                if (attacker != null && attacker.HasAbility(Wimpy.ability) && damage > 0) { __state--; damage--; }
                if (__instance.HasAbility(Bastion.ability) && damage > 1)
                {
                    float damToFloat = damage;
                    int final = Mathf.CeilToInt(damToFloat * 0.5f);
                    __state = final;
                    damage = final;
                }
            }
        }
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, int __state, PlayableCard __instance, int damage, PlayableCard attacker)
        {
            yield return enumerator;
            if (__state <= 0 && __instance.NotDead())
            {
                __instance.Anim.PlayHitAnimation();
                if (__instance.TriggerHandler.RespondsToTrigger(Trigger.TakeDamage, new object[] { attacker }))
                {
                    yield return __instance.TriggerHandler.OnTrigger(Trigger.TakeDamage, new object[] { attacker });
                }

                if (attacker != null)
                {
                    if (attacker.TriggerHandler.RespondsToTrigger(Trigger.DealDamage, new object[] { damage, __instance }))
                    {
                        yield return attacker.TriggerHandler.OnTrigger(Trigger.DealDamage, new object[] { damage, __instance });
                    }
                    yield return Singleton<GlobalTriggerHandler>.Instance.TriggerCardsOnBoard(Trigger.OtherCardDealtDamage, false, new object[] { attacker, attacker.Attack, __instance });
                }
            }
            yield break;
        }
    }

    [HarmonyPatch(typeof(CombatPhaseManager), "DealOverkillDamage", MethodType.Normal)]
    public class OverkillDamagePatch
    {
        [HarmonyPostfix]
        public static IEnumerator PostFix(IEnumerator enumerator, int damage, CardSlot attackingSlot, CardSlot opposingSlot)
        {
            bool skipOverkill = false;
            if (attackingSlot && attackingSlot.Card)
            {
                if (attackingSlot.Card.HasAbility(Mauler.ability))
                {
                    skipOverkill = true;
                    yield return new WaitForSeconds(0.1f);
                    yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, !attackingSlot.Card.slot.IsPlayerSlot, 0.1f, null, 0f, true);
                }
                if (attackingSlot.Card.HasAbility(SweepingStrikeLeft.ability) && opposingSlot != null)
                {
                    CardSlot rightslot = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false);
                    if (rightslot && rightslot.Card && !rightslot.Card.Dead && (!rightslot.Card.FaceDown || attackingSlot.Card.HasAbility(Diver.ability)))
                    {
                        yield return DealHorizontalOverkill(opposingSlot, attackingSlot, false, damage);
                        skipOverkill = true;
                    }
                }
                if (attackingSlot.Card.HasAbility(SweepingStrikeRight.ability) && opposingSlot != null)
                {
                    CardSlot leftslot = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true);
                    if (leftslot && leftslot.Card && !leftslot.Card.Dead && (!leftslot.Card.FaceDown || attackingSlot.Card.HasAbility(Diver.ability)))
                    {
                        yield return DealHorizontalOverkill(opposingSlot, attackingSlot, true, damage);
                        skipOverkill = true;
                    }
                }
            }
            if (!skipOverkill) { yield return enumerator; }
            yield break;
        }

        public static IEnumerator DealHorizontalOverkill(CardSlot damagedSlot, CardSlot attackingSlot, bool left, int amountOfOverkill)
        {
            CardSlot horizSlot = Singleton<BoardManager>.Instance.GetAdjacent(damagedSlot, left);
            if (horizSlot && horizSlot.Card && !horizSlot.Card.Dead && (!horizSlot.Card.FaceDown || attackingSlot.Card.HasAbility(Diver.ability)))
            {
                bool wasFaceDown = false;
                if (horizSlot.Card.FaceDown)
                {
                    wasFaceDown = true;
                    horizSlot.Card.SetFaceDown(false, false);
                    horizSlot.Card.UpdateFaceUpOnBoardEffects();
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(0.1f);
                int leftovers = amountOfOverkill - horizSlot.Card.Health;
                yield return horizSlot.Card.TakeDamage(amountOfOverkill, attackingSlot.Card);

                if (wasFaceDown && horizSlot.Card != null && !horizSlot.Card.Dead)
                {
                    yield return new WaitForSeconds(0.1f);
                    horizSlot.Card.SetFaceDown(true, false);
                    horizSlot.Card.UpdateFaceUpOnBoardEffects();
                }

                if (leftovers > 0)
                {
                    yield return DealHorizontalOverkill(horizSlot, attackingSlot, left, leftovers);
                }           
            }
            yield break;
        }
    }
}
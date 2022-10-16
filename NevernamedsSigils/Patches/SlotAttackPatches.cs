using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BepInEx;
using System.Collections;

namespace NevernamedsSigils
{
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
            else if (opposingSlot.Card && opposingSlot.Card.Info.traits.Contains(NevernamedsTraits.InherentRepulsive)) __result = true;
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

                //Abstain
                if (attackingSlot.Card.HasAbility(Abstain.ability))
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


                if (!attackingSlot.Card.HasAbility(Ability.AllStrike))
                {
                    PlayableCard card = attackingSlot.Card;
                    if (attackingSlot.Card.HasAbility(TrophyHunter.ability))
                    {
                        PlayableCard target = Tools.GetStrongestCardOnBoard(!card.slot.IsPlayerSlot);
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
                        CardSlot cardSlot = Tools.RandomElement(viableslots);
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
                }


                if (opposingSlot)
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
            if (attackingSlot.Card != null && opposingSlot.Card != null && opposingSlot.Card.FaceDown && opposingSlot.Card.HasAbility(SubaquaticSpines.ability) && !attackingSlot.Card.AttackIsBlocked(opposingSlot) && (!attackingSlot.Card.HasAbility(Ability.Flying) || opposingSlot.Card.HasAbility(Ability.Reach)))
            {
                yield return enumerator;
                yield return new WaitForSeconds(0.55f);
                yield return attackingSlot.Card.TakeDamage(1, opposingSlot.Card);
            }
            else
            {
                yield return enumerator;

            }
            yield break;
        }
    }
}
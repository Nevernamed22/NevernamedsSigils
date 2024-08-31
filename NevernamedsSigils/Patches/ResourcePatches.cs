using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(ResourcesManager), nameof(ResourcesManager.AddBones))]
    public class BonesPatch
    {
        [HarmonyPostfix]
        static void PreventBones(ref IEnumerator __result, ref CardSlot slot)
        {
            if (slot?.Card?.Info?.GetExtendedProperty("PreventBones") == null)
            {
                return;
            }

            IEnumerator GetEmptyEnumerator()
            {
                yield break;
            }
            __result = GetEmptyEnumerator();
        }
    }
    [HarmonyPatch(typeof(BoardManager), "GetValueOfSacrifices")]
    public class GetSacValuePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardSlot> sacrifices, ref int __result)
        {
            foreach (CardSlot cardSlot in sacrifices)
            {
                if (cardSlot != null && cardSlot.Card)
                {

                    if (cardSlot.Card.HasAbility(ExceptionalSacrifice.ability)) __result += 5;
                    if (cardSlot.Card.HasAbility(TriflingSacrifice.ability)) __result += 1;
                    if (cardSlot.Card.HasAbility(Bloated.ability) && (cardSlot.Card.Health - 1) > 0) __result += (cardSlot.Card.Health - 1);

                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.Sacrifice))]
    public class SacrificePatch
    {
        [HarmonyPrefix]
        public static void CardSacrificed(PlayableCard __instance)
        {
            if (Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard != null && __instance != null)
            {
                PlayableCard saccer = Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard;
                if (saccer.GetComponent<SinEater>() != null)
                {
                    saccer.GetComponent<SinEater>().sigilsOnSacced += __instance.GetAllAbilities().Count;
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayableCard))]
    public class CanBeSacrificed
    {
        [HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.CanBeSacrificed), MethodType.Getter)]
        public static void CanBeSacrificedPatch(ref PlayableCard __instance, ref bool __result)
        {
            if (__instance && !__instance.FaceDown && !__result)
            {
                List<Ability> overrideAbilities = new List<Ability>()
                {
                    TriflingSacrifice.ability,
                    ExceptionalSacrifice.ability,
                    Bloated.ability
                };
                if (__instance.HasAnyOfAbilities(overrideAbilities.ToArray())) __result = true;
                if (Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard != null)
                {
                    PlayableCard saccer = Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard;
                    if (saccer.HasAbility(BloodFromStone.ability)) __result = true;
                }
                if (Tools.GetNumberOfSigilOnBoard(true, BloodMagic.ability) > 0 && __instance.HasTrait(Trait.Gem)) { __result = true; }
                if (__instance.Info.GetExtendedProperty("CardAlwaysSacrificeable") != null) { __result = true; }
            }

        }
    }
    [HarmonyPatch(typeof(PlayableCard), "CanPlay")]
    public class CanPlayPatch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayableCard __instance, ref bool __result)
        {
            if (__instance && !__result)
            {
                if (__instance.HasAbility(BloodFromStone.ability))
                {
                    __result = __instance.BloodCost() <= Singleton<BoardManager>.Instance.GetValueOfSacrifices(Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x.Card != null))
                         && __instance.BonesCost() <= Singleton<ResourcesManager>.Instance.PlayerBones
                         && __instance.EnergyCost <= Singleton<ResourcesManager>.Instance.PlayerEnergy
                         && __instance.GemsCostRequirementMet()
                         && Singleton<BoardManager>.Instance.SacrificesCreateRoomForCard(__instance, Singleton<BoardManager>.Instance.PlayerSlotsCopy);
                }            
            }
            if (__instance?.Info?.GetExtendedProperty("PreventPlay") != null || (__instance.Info.HasAbility(Malware.ability) && (((LifeManager.GOAL_BALANCE * 2) - Singleton<LifeManager>.Instance.DamageUntilPlayerWin) > 3)))
            {
                __result = false;
            }
            if (__instance && __instance.Info)
            {
                if (__instance.Info.HasAbility(MoxMax.ability) && __instance.GemsCost() != null && __instance.GemsCost().Count > 0)
                {
                    bool satisfied = true;
                    foreach (GemType gem in __instance.GemsCost())
                    {
                        if (Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && (x.Card.HasAbility(gemCostToAbility[gem]) || x.Card.HasAbility(Ability.GainGemTriple))).Count < 2) { satisfied = false; }
                    }
                    if (!satisfied) { __result = false; }
                }
                if (__instance.HasTrait(Trait.Gem))
                {
                    if (Singleton<BoardManager>.Instance.playerSlots.Exists(x => x.Card != null && x.Card.HasAbility(GemSkeptic.ability))) { __result = false; }
                }
            }
        }
        public static Dictionary<GemType, Ability> gemCostToAbility = new Dictionary<GemType, Ability>()
        {
            { GemType.Blue, Ability.GainGemBlue },
            { GemType.Orange, Ability.GainGemOrange },
            { GemType.Green, Ability.GainGemGreen },
        };
    }

    [HarmonyPatch(typeof(Deck), "CardCanBePlayedByTurn2WithHand")]
    public class CanBeplayedTurn2Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Deck __instance, CardInfo card, List<CardInfo> hand, ref bool __result)
        {
            if (__result)
            {
                if (card)
                {
                    if (card.GetExtendedProperty("PreventPlay") != null || card.HasAbility(MoxMax.ability) || card.HasAbility(InstantEffect.ability)) { __result = false; }
                }
            }
        }
    }

    //Runtime Cost Modifications
    [HarmonyPatch(typeof(CardExtensions), nameof(CardExtensions.BloodCost))]
    internal static class CardExtensions_BloodCost
    {
        public static void Postfix(PlayableCard card, ref int __result)
        {
            int modification = 0;
            if (card && Singleton<BoardManager>.Instance) { }
            {
                if (card.InHand || !card.OpponentCard) //Friendly
                {

                    modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                    modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Exsanguination.ability)).Count;
                    if (card != null && card.Info != null && card.HasAbility(Ability.Flying)) { modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
                }
                else //Unfriendly
                {
                    modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                    modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Exsanguination.ability)).Count;
                    if (card != null && card.Info != null && card.HasAbility(Ability.Flying)) { modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
                }
            }

            __result += modification;
            __result = Math.Max(__result, 0);
        }
    }

    [HarmonyPatch(typeof(CardExtensions), nameof(CardExtensions.BonesCost))]
    internal static class CardExtensions_BonesCost
    {
        public static void Postfix(PlayableCard card, ref int __result)
        {
            int modification = 0;
            if (card.InHand || !card.OpponentCard) //Friendly
            {
                modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                if (card != null && card.Info != null && card.HasAbility(Ability.Flying)) { modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
            }
            else //Unfriendly
            {
                modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                if (card != null && card.Info != null && card.HasAbility(Ability.Flying)) { modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
            }

            __result += modification;
            __result = Math.Max(__result, 0);
        }
    }

    [HarmonyPatch(typeof(CardExtensions), nameof(CardExtensions.GemsCost))]
    internal static class CardExtensions_GemsCost
    {
        public static void Postfix(PlayableCard card, ref List<GemType> __result)
        {
            List<GemType> gemsModified = new List<GemType>();
            gemsModified.AddRange(__result);

            int gemsToRemove = 0;
            if (card.InHand || !card.OpponentCard) //Friendly
            {
                gemsToRemove += Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                gemsToRemove += Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(PerfectForm.ability)).Count;
                gemsToRemove += Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(ImmaculateForm.ability)).Count * 2;
                if (card != null && card.Info != null && card.HasAbility(Ability.Flying)) { gemsToRemove += Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
            }
            else //Unfriendly
            {
                gemsToRemove += Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                gemsToRemove += Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(PerfectForm.ability)).Count;
                gemsToRemove += Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(ImmaculateForm.ability)).Count * 2;
                if (card != null && card.Info != null && card.HasAbility(Ability.Flying)) { gemsToRemove += Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
            }

            for (int i = 0; i < gemsToRemove; i++)
            {
                if (gemsModified.Count > 0) { gemsModified.RemoveAt(0); }
                else { break; }
            }
            __result = gemsModified;
        }
    }

    [HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.EnergyCost), MethodType.Getter)]
    internal static class CardExtensions_EnergyCost
    {
        public static void Postfix(ref int __result, PlayableCard __instance)
        {
            int modification = 0;
            if (__instance.InHand || !__instance.OpponentCard) //Friendly
            {
                modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                if (__instance != null && __instance.Info != null && __instance.HasAbility(Ability.Flying)) { modification -= Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
            }
            else //Unfriendly
            {
                modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(TestSigil.ability)).Count;
                if (__instance != null && __instance.Info != null && __instance.HasAbility(Ability.Flying)) { modification -= Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x.Card != null && x.Card.Info != null && x.Card.temporaryMods != null && x.Card.HasAbility(Freefall.ability)).Count; }
            }

            __result += modification;
            __result = Math.Max(__result, 0);
        }
    }
}

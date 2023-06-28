using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(PlayableCard), "GetPassiveAttackBuffs")]
    public class PassiveAttackBuffs
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, ref PlayableCard __instance)
        {
            if (__instance.OnBoard && __instance.slot)
            {
                    CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(__instance.slot, true);
                CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(__instance.slot, false);

                if (__instance.HasTrait(Trait.Pelt) || __instance.Info.name == "BeastNevernamed SelkieSkin")
                {
                    List<CardSlot> viableslots = new List<CardSlot>();
                    if (__instance.slot.IsPlayerSlot) viableslots = Singleton<BoardManager>.Instance.playerSlots;
                    else viableslots = Singleton<BoardManager>.Instance.opponentSlots;
                    int toAdd = 0;
                    foreach (CardSlot slot in viableslots)
                    {
                        if (slot && slot.Card && slot.Card.HasAbility(SkinAnimator.ability)) toAdd += slot.Card.Info.Attack;
                    }
                    if (toAdd > 0) __result += toAdd;
                }
                if (__instance.HasAbility(Lonesome.ability))
                {
                    if (toLeft != null && toLeft.Card == null) __result += 1;
                    if (toRight != null && toRight.Card == null) __result += 1;
                }
                if (__instance.Info.tribes.Contains(Tribe.Hooved))
                {
                    List<CardSlot> viableslots = new List<CardSlot>();
                    if (__instance.slot.IsPlayerSlot) viableslots = Singleton<BoardManager>.Instance.playerSlots;
                    else viableslots = Singleton<BoardManager>.Instance.opponentSlots;
                    foreach (CardSlot slot in viableslots)
                    {
                        if (slot && slot.Card && slot.Card != __instance && slot.Card.HasAbility(DeusHoof.ability)) __result += 1;
                    }
                }
                if (__instance.HasAbility(DogGone.ability) == true && __instance?.slot?.opposingSlot?.Card?.IsOfTribe(Tribe.Canine) == true) { __result += 2; }
                if (__instance.HasAbility(Snakebite.ability) == true && __instance?.slot?.opposingSlot?.Card?.IsOfTribe(Tribe.Reptile) == true) { __result += 2; }
                if (__instance.HasAbility(DeerlyDeparted.ability) == true && __instance?.slot?.opposingSlot?.Card?.IsOfTribe(Tribe.Hooved) == true) { __result += 2; }
                if (__instance.HasAbility(FowlPlay.ability) == true && __instance?.slot?.opposingSlot?.Card?.IsOfTribe(Tribe.Bird) == true) { __result += 2; }
                if (__instance.HasAbility(Insectivore.ability) == true && __instance?.slot?.opposingSlot?.Card?.IsOfTribe(Tribe.Insect) == true) { __result += 2; }
                if (__instance.HasAbility(Crusher.ability) == true && __instance?.slot?.opposingSlot?.Card?.HasTrait(Trait.Terrain) == true) { __result += 2; }
                if (__instance.HasAbility(Eager.ability) == true && __instance.GetComponent<Eager>() != null && __instance.GetComponent<Eager>().livedTurns < 1) { __result += 2; }
                if (__instance.HasAbility(Spurred.ability) == true && __instance.Info != null && __instance.GetComponent<Spurred>() != null && __instance?.slot?.opposingSlot?.Card != null) { __result += __instance.GetComponent<Spurred>().BuffAmount; }
                if (__instance.slot.Index == 0 || __instance.slot.Index == (__instance.OpponentCard ? BoardManager.Instance.GetSlots(true).Count - 1 : BoardManager.Instance.GetSlots(false).Count - 1))
                {
                    __result += Tools.GetNumberOfSigilOnBoard(!__instance.OpponentCard, EspritDeCorp.ability);
                }
                if (__instance.Info.gemsCost.Contains(GemType.Orange)) { __result += Tools.GetNumberOfSigilOnBoard(!__instance.OpponentCard, OrangeInspiration.ability); }
                CardSlot toRight2 = Singleton<BoardManager>.Instance.GetAdjacent(__instance.Slot, false); ;
                if (toRight2 && toRight2.Card && toRight2.Card.HasAbility(UnbalancedLeadership.ability)) { __result += 2; }

                if (__instance.HasAbility(Siphon.ability) && __instance.GetComponent<Siphon>()) { __result += __instance.GetComponent<Siphon>().siphonedDamamge; }
                    if (toRight && toRight.Card != null && toRight.Card.HasAbility(Siphon.ability))
                {
                    if (toRight.Card.GetComponent<Siphon>()) { toRight.Card.GetComponent<Siphon>().siphonedDamamge = __instance.Info.Attack + __instance.GetAttackModifications() + __result; }
                    __result = __instance.Info.Attack * -1;
                }


                if (__instance.HasAbility(Claw.ability))
                {
                    if (__instance.OnBoard)
                    {
                        bool foundClawed = false;
                        if (toLeft && toLeft.Card && toLeft.Card.HasAbility(Clawed.ability)) foundClawed = true;
                        if (toRight && toRight.Card && toRight.Card.HasAbility(Clawed.ability)) foundClawed = true;
                        if (!foundClawed)
                        {
                            __result = __instance.Info.Attack * -1;
                        }
                    }
                }
                if (__instance.OnBoard && __instance.HasAbility(EyeForBattle.ability) && __instance.Slot.opposingSlot && __instance.Slot.opposingSlot.Card == null) { __result = __instance.Info.Attack * -1; }
            }
        }
    }
    [HarmonyPatch(typeof(PlayableCard), "GetPassiveHealthBuffs")]
    public class HealthBuffs
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, ref PlayableCard __instance)
        {
            if (__instance.OnBoard && __instance.slot)
            {
                if (__instance.Info.tribes.Contains(Tribe.Hooved))
                {
                    List<CardSlot> viableslots = new List<CardSlot>();
                    if (__instance.slot.IsPlayerSlot) viableslots = Singleton<BoardManager>.Instance.playerSlots;
                    else viableslots = Singleton<BoardManager>.Instance.opponentSlots;
                    foreach (CardSlot slot in viableslots)
                    {
                        if (slot && slot.Card && slot.Card.HasAbility(DeusHoof.ability)) __result += 1;
                    }
                }
                if (__instance.Info.gemsCost != null && __instance.Info.gemsCost.Contains(GemType.Green))
                {
                    __result += (Singleton<BoardManager>.Instance.GetSlots(!__instance.OpponentCard).FindAll(x => x.Card != null && !x.Card.Dead && x.Card.HasAbility(GreenInspiration.ability)).Count * 2);
                }
            }
        }
    }
}

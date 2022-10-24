using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NevernamedsSigils
{
    [HarmonyPatch]
    public class OpposingSlotsPatches
    {
        [HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetOpposingSlots))]
        public static void GetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
        {
            bool isTri = __instance.HasTriStrike();
            bool isBif = __instance.HasAbility(Ability.SplitStrike);
            bool isDouble = __instance.HasAbility(Ability.DoubleStrike);
            bool isSniper = __instance.HasAbility(Ability.Sniper);
            

            List<CardSlot> alteredOpposings = new List<CardSlot>();

            
            if (alteredOpposings.Count > 0) __result.AddRange(alteredOpposings);

            if (__result.Count > 0 && __instance.Info.GetExtendedProperty("AllStrikesDoubled") != null)
            {
                List<CardSlot> slots = new List<CardSlot>();
                slots.AddRange(__result);
                __result.AddRange(slots);
            }
        }
    }
}
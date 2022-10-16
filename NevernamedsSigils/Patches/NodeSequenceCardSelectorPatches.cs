using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForHost")] //sacstone host
    public class SacStoneHost_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardInfo> __result)
        {
            var list = __result;

            list.RemoveAll((CardInfo x) => x.traits.Contains(NevernamedsTraits.BannedSigilTransferTarget) );

            __result = list;
        }
    }


    [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForSacrifice")] //sacstone sacrifice
    public class SacStoneSacrifice_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardInfo> __result)
        {
            var list = __result;

            list.RemoveAll((CardInfo x) => x.traits.Contains(NevernamedsTraits.BannedSigilTransferVictim));

            __result = list;
        }
    }


    [HarmonyPatch(typeof(CardStatBoostSequencer), "GetValidCards")] //campfire
    public class CampfireBoost_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardInfo> __result)
        {
            var list = __result;

            list.RemoveAll((CardInfo x) => x.traits.Contains(NevernamedsTraits.BannedFromCampfire));

            __result = list;
        }
    }
}

using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(CardMergeSequencer), "GetValidCardsForHost")] //sacstone host
    public class SacStoneHost_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardInfo> __result)
        {
            var list = __result;

            list.RemoveAll((CardInfo x) => x.GetExtendedProperty("BannedSigilTransferTarget") != null);

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

            list.RemoveAll((CardInfo x) => x.GetExtendedProperty("BannedSigilTransferVictim") != null);

            __result = list;
        }
    }


    [HarmonyPatch(typeof(CardStatBoostSequencer), "GetValidCards")] //campfire
    public class CampfireBoost_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardInfo> __result, bool forAttackMod)
        {
            var list = __result;

            list.RemoveAll((CardInfo x) => x.GetExtendedProperty("BannedFromCampfire") != null);
            if (forAttackMod) { list.RemoveAll((CardInfo x) => x.GetExtendedProperty("BannedFromCampfireDamage") != null); }
            else { list.RemoveAll((CardInfo x) => x.GetExtendedProperty("BannedFromCampfireHealth") != null); }

            __result = list;
        }
    }


    [HarmonyPatch(typeof(CardRemoveSequencer), "GetValidCards")] //campfire
    public class BoneLordPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardInfo> __result)
        {
            var list = __result;

            list.RemoveAll((CardInfo x) => x.GetExtendedProperty("BannedFromBoneLord") != null);

            __result = list;
        }
    }
}

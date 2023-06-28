using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InscryptionAPI.Card;
using GBC;

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

    [HarmonyPatch(typeof(MycologistsDialogueNPC), "Start")]
    public class GBCMycosPatch
    {
        [HarmonyPrefix]
        public static void MycosPrestart(MycologistsDialogueNPC __instance)
        {
            __instance.requiredCards.Clear();
            __instance.fusedCards.Clear();
            if (overrideDictionary.Count <= 0)
            {
                List<CardInfo> fusables = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.GetExtendedProperty("GBCMycologistFusedVersion") != null);
                List<CardInfo> fuseds = new List<CardInfo>();
                foreach (CardInfo inf in fusables) { fuseds.Add(CardLoader.GetCardByName(inf.GetExtendedProperty("GBCMycologistFusedVersion"))); }
                for (int i = 0; i < fusables.Count; i++) { overrideDictionary.Add(fusables[i], fuseds[i]); }

                overrideDictionary.Add(CardLoader.GetCardByName("BlueMage"), CardLoader.GetCardByName("BlueMage_Fused"));
                overrideDictionary.Add(CardLoader.GetCardByName("FieldMouse"), CardLoader.GetCardByName("FieldMouse_Fused"));
                overrideDictionary.Add(CardLoader.GetCardByName("Gravedigger"), CardLoader.GetCardByName("Gravedigger_Fused"));
                overrideDictionary.Add(CardLoader.GetCardByName("SentryBot"), CardLoader.GetCardByName("SentryBot_Fused"));

                Dictionary<CardInfo, CardInfo> reordered = ReorderDictionary(overrideDictionary);
                overrideDictionary = reordered;
            }           
            for(int i = 0; i < overrideDictionary.Count; i++)
            {
                __instance.requiredCards.Add(overrideDictionary.ElementAt(i).Key);
                __instance.fusedCards.Add(overrideDictionary.ElementAt(i).Value);
            }
        }
        public static Dictionary<CardInfo, CardInfo> overrideDictionary = new Dictionary<CardInfo, CardInfo>();
        public static Dictionary<CardInfo, CardInfo> ReorderDictionary(Dictionary<CardInfo, CardInfo> toReorder)
        {        
            Dictionary<CardInfo, CardInfo> toReturn = new Dictionary<CardInfo, CardInfo>();
            while(toReorder.Count > 0)
            {
                int indexToRemove = UnityEngine.Random.Range(0, toReorder.Count);
                KeyValuePair<CardInfo, CardInfo> keyval = toReorder.ElementAt(indexToRemove);
                toReturn.Add(keyval.Key, keyval.Value);
                toReorder.Remove(keyval.Key);
            }
            return toReturn;
        }
    }
}

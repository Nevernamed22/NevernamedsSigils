using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(SteelTrap), "OnDie", MethodType.Normal)]
    public class SteelTrapPatches
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, SteelTrap __instance, bool wasSacrifice, PlayableCard killer)
        {
            bool runCode = __instance.Card.Slot.opposingSlot.Card != null;
            yield return enumerator;
            if (runCode && Tools.GetActAsInt() == 2)
            {
                yield return new WaitForSeconds(0.5f);
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("SigilNevernamed WolfPelt"), null, 0.25f, null);
            }
            yield break;
        }
    }

    [HarmonyPatch(typeof(SubmergeSquid), "OnResurface", 0)]
    public class SubmergeSquidPrefix
    {
        [HarmonyPrefix]
        public static bool Resurface(SubmergeSquid __instance)
        {
            if (__instance && __instance.Card)
            {
                List<string> tentacles = new List<string>() { "SquidBell", "SquidCards", "SquidMirror" };
                if (SaveManager.SaveFile.IsPart1)
                {
                    List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.GetExtendedProperty("ValidSquidTentacleAct1") != null);
                    foreach(CardInfo c in cards) { tentacles.Add(c.name); }
                }
                else if (SaveManager.SaveFile.IsPart2)
                {
                    List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.GetExtendedProperty("ValidSquidTentacleAct2") != null);
                    foreach (CardInfo c in cards) { tentacles.Add(c.name); }
                }
                CardInfo cardByName = CardLoader.GetCardByName(tentacles[UnityEngine.Random.Range(0, tentacles.Count)]);
                cardByName.Mods.AddRange(__instance.GetNonDefaultModsFromSelf(new Ability[]
                {
                __instance.Ability
                }));
                __instance.Card.SetInfo(cardByName);
                return false;
            }
            return true;
        }
    }
}

using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GBC;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(HammerItem), "GetValidTargets")] 
    public class HammerItemPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardSlot> __result)
        {
            var list = __result;

            list.RemoveAll((CardSlot x) => x.Card != null && x.Card.HasAbility(Unhammerable.ability));

            __result = list;
        }
    }
    [HarmonyPatch(typeof(HammerButton), "GetValidTargets")]
    public class HammerButtonPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardSlot> __result)
        {
            var list = __result;

            list.RemoveAll((CardSlot x) => x.Card != null && x.Card.HasAbility(Unhammerable.ability));

            __result = list;
        }
    }
}
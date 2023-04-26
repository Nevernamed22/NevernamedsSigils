using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    
    
    [HarmonyPatch(typeof(CardInfo))]
    public class BloodCostModifications
    {
        [HarmonyPostfix, HarmonyPatch(nameof(CardInfo.BloodCost), MethodType.Getter)]
        public static void BloodCostModificationsPatch(ref CardInfo __instance, ref int __result)
        {
            if (__instance && __instance.Gemified)
            {

            }

        }
    }
}

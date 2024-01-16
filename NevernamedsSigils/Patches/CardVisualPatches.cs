using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(PixelCardAbilityIcons), nameof(PixelCardAbilityIcons.DisplayAbilities), typeof(List<Ability>), typeof(PlayableCard))]
    public class PixelDisplayCardsPatch
    {
        [HarmonyPrefix]
        public static void PixelDisplayCards(ref List<Ability> abilities, PlayableCard card)
        {
            if (card != null)
            {
                for (int i = abilities.Count - 1; i >= 0; i--)
                {
                    if (card.Status.hiddenAbilities.Contains(abilities[i]))
                    {
                        abilities.RemoveAt(i);
                    }
                }
                foreach (CardModificationInfo info in card.temporaryMods.FindAll((x) => x.abilities.Count > 0))
                {
                    foreach (Ability ab in info.abilities)
                    {
                        if (!card.Status.hiddenAbilities.Contains(ab)) abilities.Add(ab);
                    }
                }


            }
        }
    }

   /* [HarmonyPatch(typeof(PixelCardDisplayer), nameof(PixelCardDisplayer.DisplayInfo))]
    public class PixelCardDisplayInfoPatch
    {
        [HarmonyPrefix]
        public static void PixelDisplayInfo(PixelCardDisplayer __instance, CardRenderInfo renderInfo, PlayableCard playableCard)
        {
            
            if (renderInfo != null && renderInfo.baseInfo != null && renderInfo.baseInfo.mods.Find((CardModificationInfo x) => x.gemify == true) != null)
            {
                __instance.gameObject.AddComponent<PixelGemificationBorder>();
            }
            else if (__instance.gameObject.GetComponent<PixelGemificationBorder>() != null)
            {
                UnityEngine.Object.Destroy(__instance.gameObject.GetComponent<PixelGemificationBorder>());
            }
            
            
        }
    }*/
    

    [HarmonyPatch(typeof(BoardManager3D), "OffsetCardNearBoard", 0)]
    public class OffsetSacrificeDemanderPatch
    {
        [HarmonyPrefix]
        public static bool OffsetPatch(BoardManager3D __instance, bool offsetCard)
        {
            if (__instance.CurrentSacrificeDemandingCard && __instance.CurrentSacrificeDemandingCard.OnBoard) { return false; }
            else return true;
        }
    }
    /*  [HarmonyPatch(typeof(AbilityIconInteractable), "LoadIcon", 0)]
      public class LoadIconPatch
      {
          [HarmonyPostfix]
          public static void SlotAttackSlot(CardInfo info, AbilityInfo ability, bool opponentCard, Texture __result)
          {
              if (ability.ability == Doomed.ability && info.GetExtendedProperty("CustomDoomedDuration") != null)
              {
                  //Debug.Log("got thing");
                  int num;
                  bool succeed = int.TryParse(info.GetExtendedProperty("CustomDoomedDuration"), out num);
                  if (succeed && Doomed.countDownIcons.ContainsKey(num)) { __result = Doomed.countDownIcons[num]; Debug.Log("parsed and was in dictionary"); }
              }
          }
      }*/

    [HarmonyPatch(typeof(CardInfo), "GetGBCDescriptionLocalized")]
    public class GBCDescriptionPatch
    {
        [HarmonyPostfix]
        public static void Postfix(List<Ability> allAbilities, CardInfo __instance, ref string __result)
        {
            if (__instance && __instance.GetExtendedProperty("InherentRepulsive") != null)
            {
                string altered = $"If a creature would attack {__instance.DisplayedNameEnglish}, it does not." + " \n" + __result;
                __result = altered;
            }
            if (__instance && __instance.HasTrait(Trait.Pelt))
            {
                string altered = $"{__instance.DisplayedNameEnglish} is a pelt." + " \n" + __result;
                __result = altered;
            }
            if (__instance && __instance.GetExtendedProperty("PreventBones") != null)
            {
                string altered = "DOES NOT YIELD A BONE." + " \n" + __result;
                __result = altered;
            }
            if (__instance && __instance.GetExtendedProperty("CardAlwaysSacrificeable") != null)
            {
                string altered = __result.Replace(Localization.Translate("CAN'T BE SACRIFICED."), "");
                __result = altered;
            }
        }
    }
}

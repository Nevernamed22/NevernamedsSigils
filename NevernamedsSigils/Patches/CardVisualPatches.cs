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
        public static void PixelDisplayCards(List<Ability> abilities, PlayableCard card)
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
            }

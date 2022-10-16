using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(ResourcesManager), "AddBones")]
    public class AddBonesPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(int amount, CardSlot slot)
        {
            if (slot != null && slot.Card != null && slot.Card.HasTrait(NevernamedsTraits.NoBones)) return false;
            else
            {


                return true;
            }
        }
    }
    [HarmonyPatch(typeof(BoardManager), "GetValueOfSacrifices")]
    public class GetSacValuePatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref List<CardSlot> sacrifices, ref int __result)
        {
            foreach (CardSlot cardSlot in sacrifices)
            {
                if (cardSlot != null && cardSlot.Card)
                {
                    if (cardSlot.Card.HasAbility(ExceptionalSacrifice.ability)) __result += 5;
                    if (cardSlot.Card.HasAbility(TriflingSacrifice.ability)) __result += 1;
                }
            }
        }
    }
    [HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.Sacrifice))]
    public class SacrificePatch
    {
        [HarmonyPrefix]
        public static void CardSacrificed(PlayableCard __instance)
        {
            if (Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard != null && __instance != null)
            {
                PlayableCard saccer = Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard;
                if (saccer.GetComponent<SinEater>() != null)
                {
                    saccer.GetComponent<SinEater>().sigilsOnSacced += __instance.GetAllAbilities().Count;
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayableCard))]
    public class CanBeSacrificed
    {
        [HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.CanBeSacrificed), MethodType.Getter)]
        public static void CanBeSacrificedPatch(ref PlayableCard __instance, ref bool __result)
        {
            if (__instance && !__instance.FaceDown && !__result)
            {
                if (__instance.HasAbility(ExceptionalSacrifice.ability)) __result = true;
                else if (__instance.HasAbility(TriflingSacrifice.ability)) __result = true;
                if (Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard != null)
                {
                    PlayableCard saccer = Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard;
                    if (saccer.HasAbility(BloodFromStone.ability)) __result = true;
                }
            }

        }
    }
    [HarmonyPatch(typeof(PlayableCard), "CanPlay")]
    public class CanPlayPatch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayableCard __instance, ref bool __result)
        {
            if (__instance && !__result && __instance.HasAbility(BloodFromStone.ability))
            {
                __result = __instance.Info.BloodCost <= Singleton<BoardManager>.Instance.GetValueOfSacrifices(Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x.Card != null))
                         && __instance.Info.BonesCost <= Singleton<ResourcesManager>.Instance.PlayerBones
                         && __instance.EnergyCost <= Singleton<ResourcesManager>.Instance.PlayerEnergy
                         && __instance.GemsCostRequirementMet()
                         && Singleton<BoardManager>.Instance.SacrificesCreateRoomForCard(__instance, Singleton<BoardManager>.Instance.PlayerSlotsCopy);
            }
        }
    }
}

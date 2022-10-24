using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    public static class ExtendedSigilSpawns
    {
        public static void SetCustomDams(this CardInfo card, string damID, string leshyBlockedDialogue = "Blocked on both sides...")
        {
            card.SetExtendedProperty("CustomDamDefinition", damID);
            card.SetExtendedProperty("CustomDamDefinitionDialogue", leshyBlockedDialogue);
        }
        public static void SetFactoryConduitSpawns(this CardInfo card, string cardToSpawn)
        {
            card.SetExtendedProperty("CustomFactoryConduitDefinition", cardToSpawn);
        }
        public static void SetCustomRabbitHoleSpawn(this CardInfo card, string cardToGive)
        {
            card.SetExtendedProperty("CustomRabbitHoleDefinition", cardToGive);
        }
        public static void SetCustomAntSpawnerSpawn(this CardInfo card, string cardToGive)
        {
            card.SetExtendedProperty("CustomAntSpawnerDefinition", cardToGive);
        }
        public static void SetCustomSquirrelShedderSpawn(this CardInfo card, string cardToSpawn)
        {
            card.SetExtendedProperty("CustomSquirrelShedderDefinition", cardToSpawn);
        }
        public static void SetCustomSkeletonCrewSpawn(this CardInfo card, string cardToSpawn)
        {
            card.SetExtendedProperty("CustomSkeletonCrewDefinition", cardToSpawn);
        }
        public static void SetCustomBeesWithinSpawn(this CardInfo card, string cardToGive)
        {
            card.SetExtendedProperty("CustomBeesWithinDefinition", cardToGive);
        }
        public static void SetCustomBells(this CardInfo card, string bellID, string leshyBlockedDialogue = "Blocked on both sides...")
        {
            card.SetExtendedProperty("CustomBellDefinition", bellID);
            card.SetExtendedProperty("CustomBellDefinitionDialogue", leshyBlockedDialogue);
        }
    }

    //Customise Factory Conduit
    [HarmonyPatch(typeof(ConduitFactory), "GetSpawnCardId")]
    public class FactoryConduitCustomisationPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(ConduitFactory), nameof(ConduitFactory.GetSpawnCardId))]
        public static void GetFactoryCardPatch(ConduitFactory __instance, ref string __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomFactoryConduitDefinition") != null))
            {
                __result = __instance.Card.Info.GetExtendedProperty("CustomFactoryConduitDefinition");
            }
        }
    }

    //Customise Dams
    [HarmonyPatch(typeof(CreateDams))]
    public class DamBuilderCustomisationPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(CreateDams.SpawnedCardId), MethodType.Getter)]
        public static bool DamGetPrefix(ref CreateDams __instance, ref string __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomDamDefinition") != null))
            {
                __result = __instance.Card.Info.GetExtendedProperty("CustomDamDefinition");
                return false;
            }
            return true;
        }
        [HarmonyPrefix, HarmonyPatch(nameof(CreateDams.CannotSpawnDialogue), MethodType.Getter)]
        public static bool DamBlockedDialoguePrefix(ref CreateDams __instance, ref string __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomDamDefinitionDialogue") != null))
            {
                __result = __instance.Card.Info.GetExtendedProperty("CustomDamDefinitionDialogue");
                return false;
            }
            return true;
        }
    }

    //Customise Rabbit Hole
    [HarmonyPatch(typeof(DrawRabbits))]
    public class RabbitHoleCustomisationPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(DrawRabbits.CardToDraw), MethodType.Getter)]
        public static bool RabbitGetPrefix(ref DrawRabbits __instance, ref CardInfo __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomRabbitHoleDefinition") != null))
            {
                __result = CardLoader.GetCardByName(__instance.Card.Info.GetExtendedProperty("CustomRabbitHoleDefinition"));
                __result.Mods.AddRange(__instance.GetNonDefaultModsFromSelf(new Ability[]
                {
                    __instance.Ability
                }));
                return false;
            }
            return true;
        }
    }

    //Customise Ant Spawner
    [HarmonyPatch(typeof(DrawAnt))]
    public class AntSpawnerCustomisationPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(DrawAnt.CardToDraw), MethodType.Getter)]
        public static bool GetAntPrefix(ref DrawAnt __instance, ref CardInfo __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomAntSpawnerDefinition") != null))
            {
                __result = CardLoader.GetCardByName(__instance.Card.Info.GetExtendedProperty("CustomAntSpawnerDefinition"));
                return false;
            }
            return true;
        }
    }

    //Customise Bellist
    [HarmonyPatch(typeof(CreateBells))]
    public class BellistCustomisationPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(CreateBells.SpawnedCardId), MethodType.Getter)]
        public static bool BellGetPrefix(ref CreateBells __instance, ref string __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomBellDefinition") != null))
            {
                __result = __instance.Card.Info.GetExtendedProperty("CustomBellDefinition");
                return false;
            }
            return true;
        }
        [HarmonyPrefix, HarmonyPatch(nameof(CreateBells.CannotSpawnDialogue), MethodType.Getter)]
        public static bool BellBlockedDialoguePrefix(ref CreateBells __instance, ref string __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomBellDefinitionDialogue") != null))
            {
                __result = __instance.Card.Info.GetExtendedProperty("CustomBellDefinitionDialogue");
                return false;
            }
            return true;
        }
    }

    //Customise Squirrel Shedder
    [HarmonyPatch(typeof(SquirrelStrafe), "PostSuccessfulMoveSequence", 0)]
    public class SquirrelShedderCustomisationPatch
    {
        [HarmonyPostfix]
        public static IEnumerator PostSuccessfulMoveSequence(IEnumerator res, SquirrelStrafe __instance, CardSlot cardSlot)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomSquirrelShedderDefinition") != null))
            {
                if (cardSlot.Card == null)
                {
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(__instance.Card.Info.GetExtendedProperty("CustomSquirrelShedderDefinition")), cardSlot, 0.1f, true);
                }
            }
            else
            {
                yield return res;
            }
            yield break;
        }
    }

    //Customise Skeleton Crew
    [HarmonyPatch(typeof(SkeletonStrafe), "PostSuccessfulMoveSequence", 0)]
    public class SkeletonStrafeCustomisationPatch
    {
        [HarmonyPostfix]
        public static IEnumerator PostSuccessfulMoveSequence(IEnumerator res, SkeletonStrafe __instance, CardSlot cardSlot)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomSkeletonCrewDefinition") != null))
            {
                if (cardSlot.Card == null)
                {
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(__instance.Card.Info.GetExtendedProperty("CustomSkeletonCrewDefinition")), cardSlot, 0.1f, true);
                }
            }
            else
            {
                yield return res;
            }
            yield break;
        }
    }

    //Customise Bees Within
    [HarmonyPatch(typeof(BeesOnHit))]
    public class BeesWithinCustomisationPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(BeesOnHit.CardToDraw), MethodType.Getter)]
        public static bool RabbitGetPrefix(ref BeesOnHit __instance, ref CardInfo __result)
        {
            if (__instance && __instance.Card && __instance.Card.Info && (__instance.Card.Info.GetExtendedProperty("CustomBeesWithinDefinition") != null))
            {
                __result = CardLoader.GetCardByName(__instance.Card.Info.GetExtendedProperty("CustomBeesWithinDefinition"));
                __result.Mods.AddRange(__instance.GetNonDefaultModsFromSelf(new Ability[]
                {
                    __instance.Ability
                }));
                return false;
            }
            return true;
        }
    }
}

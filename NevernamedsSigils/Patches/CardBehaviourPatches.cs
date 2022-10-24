using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(BoardManager), "AssignCardToSlot", MethodType.Normal)]
    public class CardAssignPrefix
    {
        [HarmonyPrefix]
        public static void AssignCardToSlotPrefix(out List<PlayableCard> __state, PlayableCard card, CardSlot slot, float transitionDuration = 0.1f, Action tweenCompleteCallback = null, bool resolveTriggers = true)
        {
            __state = new List<PlayableCard>();
            foreach (CardSlot p in Singleton<BoardManager>.Instance.AllSlots)
            {
                if (p && p.Card && p.Card.HasAbility(Stalwart.ability) && !p.Card.GetComponent<Stalwart>().beingMoved) __state.Add(p.Card);
            }
            //Debug.Log($"Move prefix triggered by card '{card.Info.displayedName}'");
        }

        [HarmonyPostfix]
        public static IEnumerator AssignCardToSlotPostfix(IEnumerator enumerator, List<PlayableCard> __state, PlayableCard card, CardSlot slot, float transitionDuration = 0.1f, Action tweenCompleteCallback = null, bool resolveTriggers = true)
        {
            yield return enumerator;

            foreach (PlayableCard target in __state)
            {
                if (target && target.GetComponent<Stalwart>() && target.GetComponent<Stalwart>().home != null && !target.GetComponent<Stalwart>().beingMoved) { yield return target.GetComponent<Stalwart>().ReturnToHome(); }
            }

            yield break;
        }
    }

    [HarmonyPatch(typeof(PickAxeSlam), "StrikeCardSlot", MethodType.Normal)]
    public class ProspectorPostfix
    {
        [HarmonyPostfix]
        public static IEnumerator ProspectorSlamPatch(IEnumerator enumerator, CardSlot slot)
        {
            bool cardHadCopier = slot && slot.Card && slot.Card.HasAbility(Copier.ability);

            yield return enumerator;

            if (slot && slot.Card && slot.Card.Info.name == "GoldNugget" && cardHadCopier)
            {
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("SigilNevernamed CopiedProspecter"), 0.25f);
            }

            yield break;
        }
    }
    [HarmonyPatch(typeof(PirateSkullBattleSequencer), "FireCannonsSequence", MethodType.Normal)]
    public class RoyalCannonPostfix
    {
        [HarmonyPrefix]
        public static void FireCannonsSequencePrefix(out List<PlayableCard> __state, PirateSkullBattleSequencer __instance)
        {
            __state = new List<PlayableCard>();
           // Debug.Log("Prefire Cannons");
            foreach (CardSlot p in __instance.cannonTargetSlots)
            {
                if (p && p.Card != null && p.Card.HasAbility(Copier.ability)) 
                { __state.Add(p.Card);
                    //Debug.Log($"Found Copier card in slot {p.Index}.");

                }
            }        
        }

        [HarmonyPostfix]
        public static IEnumerator FireCannonsSequencePostfix(IEnumerator enumerator, List<PlayableCard> __state, PirateSkullBattleSequencer __instance)
        {
            yield return enumerator;

            //Debug.Log("Postfire Cannons");
            foreach (PlayableCard target in __state)
            {
            //Debug.Log("Checking card state.");
               if (target == null || !target.NotDead() || !target.OnBoard)
                {
            //Debug.Log("Card was null!");
                    if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                    {
                        yield return new WaitForSeconds(0.2f);
                        Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                        yield return new WaitForSeconds(0.2f);
                    }
                    yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("SigilNevernamed CopiedRoyal"), 0.25f);
                }
            }

            yield break;
        }
    }

}

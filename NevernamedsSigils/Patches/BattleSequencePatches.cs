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
    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.StartGame), typeof(EncounterData))]
    public class BattleStart
    {
        [HarmonyPrefix]
        public static void BattleStartPatch(TurnManager __instance)
        {
            //if (CardSelectionHandler.instance != null) { UnityEngine.Object.Destroy(CardSelectionHandler.instance.gameObject); }
            //GameObject inst = new GameObject();
            //CardSelectionHandler resinst = inst.AddComponent<CardSelectionHandler>();
            //CardSelectionHandler.instance = resinst;

        }
    }


    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.CleanupPhase))]
    public class BattleEnd
    {
        [HarmonyPostfix]
        public static IEnumerator BattleEndPatch(IEnumerator enumerator, TurnManager __instance)
        {
            //if (CardSelectionHandler.instance != null) { UnityEngine.Object.Destroy(CardSelectionHandler.instance.gameObject); CardSelectionHandler.instance = null; }
            yield return enumerator;
            yield break;
        }
    }
    [HarmonyPatch(typeof(CardDrawPiles3D), "DrawOpeningHand", MethodType.Normal)]
    public class OpeningHandDraw
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, CardDrawPiles3D __instance, List<CardInfo> fixedHand)
        {
            Deck deck = __instance.Deck;
            for (int i = deck.cards.Count - 1; i >= 0; i--)
            {
                List<CardSlot> FreePlayerSlots = Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card == null);
                if (FreePlayerSlots.Count > 0)
                {
                    if (deck.cards[i].HasAbility(Quickdraw.ability))
                    {
                        __instance.pile.Draw();
                        CardInfo info = deck.Draw(deck.cards[i]);
                        PlayableCard playable = CardSpawner.SpawnPlayableCard(info);
                        yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(playable, Tools.SeededRandomElement<CardSlot>(FreePlayerSlots, Tools.GetRandomSeed()), 0.1f, null, true);
                    }
                }
            }
            yield return enumerator;
            yield break;
        }
    }
    [HarmonyPatch(typeof(PixelCardDrawPiles), "DrawOpeningHand", MethodType.Normal)]
    public class OpeningHandDrawPixel
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, PixelCardDrawPiles __instance, List<CardInfo> fixedHand)
        {
            Deck deck = __instance.Deck;
            for (int i = deck.cards.Count - 1; i >= 0; i--)
            {
                List<CardSlot> FreePlayerSlots = Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card == null);
                if (FreePlayerSlots.Count > 0)
                {
                    if (deck.cards[i].HasAbility(Quickdraw.ability))
                    {
                        CardInfo info = deck.Draw(deck.cards[i]);
                        PlayableCard playable = CardSpawner.SpawnPlayableCard(info);
                        yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(playable, Tools.SeededRandomElement<CardSlot>(FreePlayerSlots, Tools.GetRandomSeed()), 0.1f, null, true);
                    }
                }
            }
            yield return enumerator;
            yield break;
        }
    }
    [HarmonyPatch(typeof(Opponent), nameof(Opponent.TryOfferSurrender))]
    public class TryOfferSurrender
    {
        [HarmonyPostfix]
        public static IEnumerator SurrenderTry(IEnumerator enumerator, Opponent __instance)
        {
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(true).FindAll(x => x.Card && x.Card.HasAbility(HighPowered.ability)))
            {
                if (slot.Card.GetComponent<HighPowered>()) yield return slot.Card.GetComponent<HighPowered>().SpendEnergy();
            }
            yield return enumerator;
            yield break;
        }
    }
}

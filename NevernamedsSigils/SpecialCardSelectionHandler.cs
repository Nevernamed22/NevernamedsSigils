using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    class SpecialCardSelectionHandler
    {
        public static IEnumerator DoSpecialCardSelectionReturn(Action<CardInfo> cardToReturn, List<CardInfo> cards, bool destroySelected)
        {
            CardInfo selectedCard = null;
            yield return HandleInternalSelec(delegate (CardInfo c)
            {
                selectedCard = c;
            }, cards, destroySelected);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            cardToReturn(selectedCard);
            yield break;

        }

        public static IEnumerator DoSpecialCardSelectionDraw(List<CardInfo> cards, bool destroySelected)
        {
            CardInfo selectedCard = null;
            yield return HandleInternalSelec(delegate (CardInfo c)
            {
                selectedCard = c;
            }, cards, destroySelected);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(Tools.TrueClone(selectedCard), 0.25f);
        }
        private static IEnumerator HandleInternalSelec(Action<CardInfo> cardSelectedCallback, List<CardInfo> cards, bool destroySelected)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, false, true);
            SelectableCard selectedCard = null;

            DoingSpecialCardSelection = true;
            curSpecialCardSelectionCount = (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile.cards.Count;
            yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(cards, null, delegate (SelectableCard x)
            {
                selectedCard = x;
            }, null, true);
            Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
            if (destroySelected) UnityEngine.Object.Destroy(selectedCard.gameObject, 0.1f);
            cardSelectedCallback(selectedCard.Info);
            yield break;
        }

        [HarmonyPatch(typeof(SelectableCardArray), "WaitForClearCardPile")]
        public class ClearCardPilePatch
        {
            [HarmonyPostfix]
            public static IEnumerator ClearCardPile(IEnumerator enumerator, CardPile pile, SelectableCardArray __instance)
            {
                CardPile actPile = (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile;
                if (DoingSpecialCardSelection && actPile != null)
                {
                    yield return new WaitUntil(() => !actPile.DoingCardOperation);
                    __instance.StartCoroutine(actPile.DestroyCards(0.5f));
                }
                yield return enumerator;
                yield break;
            }
        }

        [HarmonyPatch(typeof(SelectableCardArray), "CleanUpCards")]
        public class ReAddCardPilePatch
        {
            [HarmonyPostfix]
            public static IEnumerator ReAddCardPile(IEnumerator enumerator, SelectableCardArray __instance)
            {
                yield return enumerator;
                if (DoingSpecialCardSelection && (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile != null)
                {
                    __instance.StartCoroutine((Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile.SpawnCards(curSpecialCardSelectionCount, 1f)); 
                }
                yield break;
            }
        }
        public static bool DoingSpecialCardSelection;
        public static int curSpecialCardSelectionCount;
    }
}

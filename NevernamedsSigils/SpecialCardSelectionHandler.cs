using DiskCardGame;
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
        public static IEnumerator DoSpecialCardSelectionReturn(Action<CardInfo> cardToReturn, List<CardInfo> cards, bool destroySelected, bool useCardPile)
        {
            CardInfo selectedCard = null;
            yield return HandleInternalSelec(delegate (CardInfo c)
            {
                selectedCard = c;
            }, cards, destroySelected, useCardPile);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            cardToReturn(selectedCard);
            yield break;

        }

        public static IEnumerator DoSpecialCardSelectionDraw(List<CardInfo> cards, bool destroySelected, bool useCardPile)
        {
            CardInfo selectedCard = null;
            yield return HandleInternalSelec(delegate (CardInfo c)
            {
                selectedCard = c;
            }, cards, destroySelected, useCardPile);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(Tools.TrueClone(selectedCard), 0.25f);
        }
        private static IEnumerator HandleInternalSelec(Action<CardInfo> cardSelectedCallback, List<CardInfo> cards, bool destroySelected, bool useCardPile)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, false, true);
            SelectableCard selectedCard = null;
            CardPile pile = null;
            if (useCardPile) pile = (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile;
            yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(cards, pile, delegate (SelectableCard x)
            {
                selectedCard = x;
            }, null, true);
            Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
            if (destroySelected) UnityEngine.Object.Destroy(selectedCard.gameObject, 0.1f);
            cardSelectedCallback(selectedCard.Info);
            yield break;
        }
    }
}

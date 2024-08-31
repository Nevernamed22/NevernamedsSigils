using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionCommunityPatch.PixelTutor;
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
        public static IEnumerator ChoosePixelCard(Action<CardInfo> cardToReturn, List<CardInfo> cards)
        {
            PixelPlayableCard selectedCard = null;
            yield return PixelBoardManager.Instance.GetComponent<PixelPlayableCardArray>().SelectPixelCardFrom(cards, delegate (PixelPlayableCard x)
            {
                selectedCard = x;
            });

            Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn);
            UnityEngine.Object.Destroy(selectedCard.gameObject, 0.1f);

            cardToReturn(selectedCard.Info);
        }

        public static IEnumerator DoSpecialCardSelectionReturn(Action<CardInfo> cardToReturn, List<CardInfo> cards, bool despawnPile = true)
        {
            CardInfo selectedCard = null;
            yield return HandleInternalSelec(delegate (CardInfo c)
            {
                selectedCard = c;
            }, cards, despawnPile);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            cardToReturn(selectedCard);
            yield break;

        }

        public static IEnumerator DoSpecialCardSelectionDraw(List<CardInfo> cards, bool despawnPile = true)
        {
            CardInfo selectedCard = null;
            yield return HandleInternalSelec(delegate (CardInfo c)
            {
                selectedCard = c;
            }, cards, despawnPile);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);

            CardInfo inf = CardLoader.GetCardByName(selectedCard.name);
            foreach (CardModificationInfo cardModificationInfo in selectedCard.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo item = (CardModificationInfo)cardModificationInfo.Clone();
                inf.Mods.Add(item);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(inf, 0.25f);
        }
        private static IEnumerator HandleInternalSelec(Action<CardInfo> cardSelectedCallback, List<CardInfo> cards, bool despawnPile = true)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, false, true);
            SelectableCard selectedCard = null;

            CardPile pile = Singleton<CardDrawPiles>.Instance is CardDrawPiles3D ? (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile : null;
            int numCards = 0;
            if (pile != null && despawnPile)
            {
                numCards = pile.NumCards;
                yield return new WaitUntil(() => !pile.DoingCardOperation);
                Singleton<BoardManager>.Instance.CardSelector.StartCoroutine(pile.DestroyCards(0.5f));
            }

            yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(cards, null, delegate (SelectableCard x)
            {
                selectedCard = x;
            }, null, true);
            Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
            UnityEngine.Object.Destroy(selectedCard.gameObject, 0.1f);

            if (pile != null && despawnPile)
            {
                Singleton<BoardManager>.Instance.CardSelector.StartCoroutine(pile.SpawnCards(numCards, 1f));
            }

            cardSelectedCallback(selectedCard.Info);
            yield break;
        }

        /*
        [HarmonyPatch(typeof(SelectableCardArray), "WaitForClearCardPile")]
        public class ClearCardPilePatch
        {
            [HarmonyPostfix]
            public static IEnumerator ClearCardPile(IEnumerator enumerator, CardPile pile, SelectableCardArray __instance)
            {
                if (Singleton<CardDrawPiles>.Instance && Singleton<CardDrawPiles>.Instance is CardDrawPiles3D)
                {
                    CardPile actPile = (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile;
                    if (DoingSpecialCardSelection && actPile != null)
                    {
                        yield return new WaitUntil(() => !actPile.DoingCardOperation);
                        __instance.StartCoroutine(actPile.DestroyCards(0.5f));
                    }
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
        public static int curSpecialCardSelectionCount;*/
    }
}

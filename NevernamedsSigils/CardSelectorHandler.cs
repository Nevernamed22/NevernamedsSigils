/*using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class CardSelectionHandler : ManagedBehaviour
    {
        public static CardSelectionHandler instance;
        public IEnumerator DisplayUntilCancelled(List<CardInfo> cards, CardPile pile, Func<bool> cancelCondition, Action cardsPlacedCallback = null, Action<SelectableCard> cardSelectedCallback = null)
        {
            InitializeGamepadGrid();
            yield return SpawnAndPlaceCards(cards, pile, GetNumRows(cards.Count), true, true);
            if (cardsPlacedCallback != null) { cardsPlacedCallback(); }

            displayedCards.ForEach(delegate (SelectableCard x) { x.DefaultCursorType = true; });

            SetCardsEnabled(true);
            displayUntilCancelledSelectionCallback = cardSelectedCallback;
            yield return new WaitUntil(() => cancelCondition());
            displayUntilCancelledSelectionCallback = null;
            SetCardsEnabled(false);
            yield return CleanUpCards();
            if (pile != null)
            {
                StartCoroutine(pile.SpawnCards(cards.Count, 1f));
            }
            yield break;
        }
        public IEnumerator SelectCardFrom(List<CardInfo> cards, CardPile pile, Action<SelectableCard> cardSelectedCallback, Func<bool> cancelCondition = null, bool forPositiveEffect = true)
        {
            InitializeGamepadGrid();
            yield return SpawnAndPlaceCards(cards, pile, this.GetNumRows(cards.Count), false, forPositiveEffect);
            yield return new WaitForSeconds(0.15f);
            SetCardsEnabled(true);
            selectedCard = null;
            yield return new WaitUntil(() => selectedCard != null || (cancelCondition != null && cancelCondition()));
            SetCardsEnabled(false);
            if (selectedCard != null)
            {
                displayedCards.Remove(selectedCard);
                selectedCard.SetLocalPosition(Vector3.zero, 30f, false);
            }
            yield return CleanUpCards();
            if (selectedCard != null)
            {
                cards.Remove(selectedCard.Info);
                if (cardSelectedCallback != null)
                {
                    cardSelectedCallback(selectedCard);
                }
            }
            if (pile != null)
            {
                base.StartCoroutine(pile.SpawnCards(cards.Count, 1f));
            }
            yield break;
        }
        protected Vector4 GetCardPositionAndRotation(int cardIndex, int numCards, int numRows, int cardsPerRow)
        {
            int num = Mathf.Min(numRows - 1, Mathf.FloorToInt((float)cardIndex / (float)cardsPerRow));
            int num2 = (num == numRows - 1) ? (numCards - num * cardsPerRow) : cardsPerRow;
            float num3 = this.arrayWidth / (float)num2;
            float num4 = this.leftAnchor + num3 * 0.5f;
            float num5 = num4 + num3 * (float)(cardIndex % num2);
            float num6 = Mathf.Abs(num5 - num4) / this.arrayWidth;
            num6 = num6 * 2f - 1f;
            float w = num6 * -11f * this.curveFactor;
            float num7 = this.rowSpacing * (float)num;
            float num8 = -(this.rowSpacing * (float)(numRows - 1) * 0.5f);
            float num9 = 0.25f * -Mathf.Abs(num6) * this.curveFactor;
            float z = num8 + num7 + num9;
            return new Vector4(num5, this.GetCardYPos(cardIndex, numCards - 1), z, w);
        }
        protected IEnumerator WaitForClearCardPile(CardPile pile)
        {
            if (pile != null)
            {
                yield return new WaitUntil(() => !pile.DoingCardOperation);
                base.StartCoroutine(pile.DestroyCards(0.5f));
            }
            yield break;
        }
        protected virtual IEnumerator CleanUpCards()
        {
            List<SelectableCard> list = new List<SelectableCard>(this.displayedCards);
            foreach (SelectableCard selectableCard in list)
            {
                if (selectableCard != null)
                {
                    Tween.Position(selectableCard.transform, selectableCard.transform.position + this.offscreenPositionOffset, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                    selectableCard.Anim.PlayQuickRiffleSound();
                    UnityEngine.Object.Destroy(selectableCard.gameObject, 0.1f);
                    yield return new WaitForSeconds(CardPile.GetPauseBetweenCardTime(this.displayedCards.Count) * this.tweenDurationModifier * 0.5f);
                }
            }
            List<SelectableCard>.Enumerator enumerator = default(List<SelectableCard>.Enumerator);
            yield break;
            yield break;
        }

        // Token: 0x06001AE4 RID: 6884 RVA: 0x000591FB File Offset: 0x000573FB
        protected virtual IEnumerator SpawnAndPlaceCards(List<CardInfo> cards, CardPile pile, int numRows, bool isDeckReview = false, bool forPositiveEffect = true)
        {
            this.displayedCards.ForEach(delegate (SelectableCard x)
            {
                if (x != null)
                {
                    UnityEngine.Object.Destroy(x.gameObject);
                }
            });
            this.displayedCards.Clear();
            yield return this.WaitForClearCardPile(pile);
            int cardsPerRow = cards.Count / numRows;
            int num;
            for (int i = 0; i < cards.Count; i = num + 1)
            {
                Vector4 cardPositionAndRotation = this.GetCardPositionAndRotation(i, cards.Count, numRows, cardsPerRow);
                SelectableCard card = this.CreateAndPlaceCard(cards[i], cardPositionAndRotation, cardPositionAndRotation.w, cardsPerRow >= this.minCardsForTilt - 1);
                yield return this.TriggerSpecialBehaviours(card, isDeckReview, forPositiveEffect);
                yield return new WaitForSeconds(CardPile.GetPauseBetweenCardTime(cards.Count) * this.tweenDurationModifier);
                num = i;
            }
            yield break;
        }

        // Token: 0x06001AE5 RID: 6885 RVA: 0x00059230 File Offset: 0x00057430
        protected void TweenInCard(Transform cardTransform, Vector3 cardPos, float zRot, bool tiltCard = false)
        {
            cardTransform.localPosition = cardPos;
            if (tiltCard && this.cardsTilt > 0f)
            {
                cardTransform.eulerAngles = new Vector3(90f + this.cardsTilt, 90f, 90f);
            }
            else
            {
                cardTransform.eulerAngles = new Vector3(90f, 0f, zRot);
            }
            Vector3 position = cardTransform.position;
            Vector3 position2 = position + this.offscreenPositionOffset;
            cardTransform.position = position2;
            Tween.Position(cardTransform, position, 0.15f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
        }

        // Token: 0x06001AE6 RID: 6886 RVA: 0x000592C4 File Offset: 0x000574C4
        private void InitializeGamepadGrid()
        {
            if (gamepadGrid == null)
            {
                gamepadGrid = gameObject.AddComponent<GamepadGridControl>();
                gamepadGrid.Rows = new List<GamepadGridControl.Row>();
            }
            gamepadGrid.Rows.Clear();
            for (int i = 0; i < maxRows; i++)
            {
                gamepadGrid.Rows.Add(new GamepadGridControl.Row(Array.Empty<InteractableBase>()));
            }
            gamepadGrid.enabled = false;
            gamepadGrid.enabled = true;
        }

        // Token: 0x06001AE7 RID: 6887 RVA: 0x00059354 File Offset: 0x00057554
        private SelectableCard CreateAndPlaceCard(CardInfo info, Vector3 cardPos, float zRot, bool tiltCard)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.selectableCardPrefab);
            gameObject.transform.SetParent(base.transform);
            SelectableCard component = gameObject.GetComponent<SelectableCard>();
            component.Initialize(info, new Action<SelectableCard>(this.OnCardSelected), null, false, new Action<SelectableCard>(this.OnCardInspected));
            component.SetEnabled(false);
            this.gamepadGrid.Rows[0].interactables.Add(component);
            this.displayedCards.Add(component);
            this.TweenInCard(component.transform, cardPos, zRot, tiltCard);
            component.Anim.PlayQuickRiffleSound();
            return component;
        }

        // Token: 0x06001AE8 RID: 6888 RVA: 0x000593EF File Offset: 0x000575EF
        private IEnumerator TriggerSpecialBehaviours(SelectableCard card, bool isDeckReview, bool forPositiveEffect)
        {
            foreach (SpecialCardBehaviour specialCardBehaviour in card.GetComponents<SpecialCardBehaviour>())
            {
                if (isDeckReview)
                {
                    specialCardBehaviour.OnShownInDeckReview();
                }
                else
                {
                    yield return specialCardBehaviour.OnShownForCardSelect(forPositiveEffect);
                }
            }
            SpecialCardBehaviour[] array = null;
            yield break;
        }

        // Token: 0x06001AE9 RID: 6889 RVA: 0x0005940C File Offset: 0x0005760C
        private int GetNumRows(int numCards)
        {
            return Mathf.Min(numCards / (this.maxCardsPerRow + 1) + 1, this.maxRows);
        }

        // Token: 0x06001AEA RID: 6890 RVA: 0x00059428 File Offset: 0x00057628
        private void SetCardsEnabled(bool enabled)
        {
            foreach (SelectableCard selectableCard in this.displayedCards)
            {
                selectableCard.SetEnabled(enabled);
            }
        }

        // Token: 0x06001AEB RID: 6891 RVA: 0x0005947C File Offset: 0x0005767C
        private void OnCardSelected(SelectableCard card)
        {
            this.selectedCard = card;
            Action<SelectableCard> action = this.displayUntilCancelledSelectionCallback;
            if (action == null)
            {
                return;
            }
            action(card);
        }

        // Token: 0x06001AEC RID: 6892 RVA: 0x00059498 File Offset: 0x00057698
        private void OnCardInspected(SelectableCard card)
        {
            card.Anim.PlayRiffleSound();
            int inspectedIndex = this.displayedCards.IndexOf(card);
            foreach (SelectableCard selectableCard in this.displayedCards)
            {
                int cardIndex = this.displayedCards.IndexOf(selectableCard);
                selectableCard.transform.localPosition = new Vector3(selectableCard.transform.localPosition.x, this.GetCardYPos(cardIndex, inspectedIndex), selectableCard.transform.localPosition.z);
                selectableCard.SetLocalPosition(Vector3.zero, 0f, true);
            }
            card.SetLocalPosition(this.inspectedCardOffset, 20f, false);
        }

        // Token: 0x06001AED RID: 6893 RVA: 0x00059564 File Offset: 0x00057764
        private float GetCardYPos(int cardIndex, int inspectedIndex)
        {
            int num = Mathf.Abs(cardIndex - inspectedIndex);
            return 0.01f - 0.00015f * (float)num;
        }

        // Token: 0x040011E3 RID: 4579
        protected SelectableCard selectedCard;

        // Token: 0x040011E4 RID: 4580
        private List<SelectableCard> displayedCards = new List<SelectableCard>();

        // Token: 0x040011E5 RID: 4581
        [SerializeField]
        private GameObject selectableCardPrefab;

        // Token: 0x040011E6 RID: 4582
        [Header("Rows")]
        [SerializeField]
        private int maxRows = 1;

        // Token: 0x040011E7 RID: 4583
        [SerializeField]
        private int maxCardsPerRow = 10;

        // Token: 0x040011E8 RID: 4584
        [SerializeField]
        private float rowSpacing = 2f;

        // Token: 0x040011E9 RID: 4585
        [Header("Horizontal")]
        [SerializeField]
        private float arrayWidth = 7f;

        // Token: 0x040011EA RID: 4586
        [SerializeField]
        private float leftAnchor = -3.25f;

        // Token: 0x040011EB RID: 4587
        [SerializeField]
        private float curveFactor = 1f;

        // Token: 0x040011EC RID: 4588
        [SerializeField]
        private int minCardsForTilt = 6;

        // Token: 0x040011ED RID: 4589
        [SerializeField]
        private float cardsTilt;

        // Token: 0x040011EE RID: 4590
        [Header("Movement")]
        [SerializeField]
        private float tweenDurationModifier = 0.5f;

        // Token: 0x040011EF RID: 4591
        [SerializeField]
        private Vector3 inspectedCardOffset = new Vector3(0f, 0.05f, -0.2f);

        // Token: 0x040011F0 RID: 4592
        [SerializeField]
        protected Vector3 offscreenPositionOffset = new Vector3(2f, 2f, -4f);

        // Token: 0x040011F1 RID: 4593
        private GamepadGridControl gamepadGrid;

        // Token: 0x040011F2 RID: 4594
        private Action<SelectableCard> displayUntilCancelledSelectionCallback;
    }
}*/

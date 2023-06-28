using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Act2SpawnLice : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Act2SpawnLice", typeof(Act2SpawnLice)).Id;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return GetOrderedAvailableSlots().Count > 0 && (Singleton<PlayerHand>.Instance.CardsInHand.Exists((PlayableCard x) => this.IsLice(x.Info)) || Singleton<CardDrawPiles>.Instance.Deck.Cards.Exists((CardInfo x) => this.IsLice(x)));
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return new WaitForSeconds(0.4f);
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.25f);
            List<CardSlot> availableSlots = this.GetOrderedAvailableSlots();
            foreach (CardInfo cardInfo in Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll((CardInfo x) => this.IsLice(x)))
            {
                if (availableSlots.Count > 0)
                {
                    Singleton<CardDrawPiles>.Instance.Deck.Draw(cardInfo);
                    if (Tools.GetActAsInt() != 2) (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile.Draw();
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardInfo, availableSlots[0], 0.1f, true);
                    availableSlots.RemoveAt(0);
                }
            }
            foreach (PlayableCard card in Singleton<PlayerHand>.Instance.CardsInHand.FindAll((PlayableCard x) => this.IsLice(x.Info)))
            {
                if (availableSlots.Count > 0)
                {
                    yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(card, availableSlots[0]);
                    availableSlots.RemoveAt(0);
                }
            }
            yield break;
        }
        private List<CardSlot> GetOrderedAvailableSlots()
        {
            List<CardSlot> list = new List<CardSlot>();
            foreach (CardSlot cardSlot in Singleton<BoardManager>.Instance.GetAdjacentSlots(base.PlayableCard.Slot))
            {
                if (cardSlot.Card == null) { list.Add(cardSlot); }
            }
            foreach (CardSlot cardSlot2 in Singleton<BoardManager>.Instance.GetSlots(true))
            {
                if (!list.Contains(cardSlot2) && cardSlot2.Card == null) { list.Add(cardSlot2); }
            }
            return list;
        }
        private bool IsLice(CardInfo card)
        {
            return card.HasTrait(Trait.Lice);
        }
    }
}

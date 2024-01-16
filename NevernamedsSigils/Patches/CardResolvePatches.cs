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
    [HarmonyPatch(typeof(BoardManager), "ResolveCardOnBoard", MethodType.Normal)]
    public class ResolveCardPatch
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, BoardManager __instance, PlayableCard card, CardSlot slot, float tweenLength = 0.1f, Action landOnBoardCallback = null, bool resolveTriggers = true)
        {
            if (card != null && Singleton<TurnManager>.Instance != null && !Singleton<TurnManager>.Instance.IsSetupPhase && !card.GetComponent<PlayedFromHand>() && !card.OpponentCard)
            {
                if ((Singleton<PlayerHand>.Instance != null && Singleton<PlayerHand>.Instance.CardsInHand.Exists((PlayableCard x) => x.HasAbility(Unspeakable.ability)) || (Singleton<CardDrawPiles>.Instance && Singleton<CardDrawPiles>.Instance.Deck.Cards.Exists((CardInfo x) => x.HasAbility(Unspeakable.ability)))))
                {
                    List<CardInfo> Options = new List<CardInfo>() { card.Info };

                    List<PlayableCard> handOptions = Singleton<PlayerHand>.Instance.CardsInHand.FindAll((PlayableCard x) => x.HasAbility(Unspeakable.ability));
                    if (handOptions.Count > 0) { foreach (PlayableCard cardInHand in handOptions) { Options.Add(cardInHand.Info); } }

                    Options.AddRange(Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll((CardInfo x) => x.HasAbility(Unspeakable.ability)));

                    if (Options.Count > 1)
                    {
                        CardInfo selectedCard = null;
                        if (Tools.GetActAsInt() == 2)
                        {
                            yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                            {
                                selectedCard = c;
                            }, Options);
                        }
                        else
                        {
                            yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                            {
                                selectedCard = c;
                            }, Options);
                        }

                        if (selectedCard != card.Info)
                        {
                            for (int i = card.temporaryMods.Count - 1; i >= 0; i--)
                            {
                                card.RemoveTemporaryMod(card.temporaryMods[i]);
                            }
                            card.SetInfo(selectedCard);
                            bool deletedYet = true;
                            for (int i = Singleton<PlayerHand>.Instance.CardsInHand.Count - 1; i >= 0; i--)
                            {
                                PlayableCard handCard = Singleton<PlayerHand>.Instance.CardsInHand[i];
                                if (handCard.Info == selectedCard)
                                {
                                    foreach (CardModificationInfo tempMod in handCard.temporaryMods)
                                    {
                                        CardModificationInfo clonedmod = tempMod.Clone() as CardModificationInfo;
                                        card.AddTemporaryMod(clonedmod);
                                    }
                                    handCard.SetInteractionEnabled(false);
                                    handCard.Anim.PlayDeathAnimation(true);
                                    UnityEngine.Object.Destroy(handCard.gameObject, 1f);
                                    Singleton<PlayerHand>.Instance.RemoveCardFromHand(handCard);
                                    deletedYet = true;
                                }
                            }
                            if (!deletedYet && Singleton<CardDrawPiles>.Instance.Deck.Cards.Exists((CardInfo x) => x == selectedCard))
                            {
                                Singleton<CardDrawPiles>.Instance.Deck.Draw(selectedCard);
                                (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile.Draw();
                                deletedYet = true;
                            }
                        }
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
                    }
                }
            }

            yield return enumerator;
            if (card != null)
            {
                if (card.OpponentCard)
                {
                    if (card.HasAbility(Ability.Flying) && card.slot)
                    {
                        List<CardSlot> toCheck = BoardManager.Instance.GetSlots(false).FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) != null && Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x).HasAbility(Wingrider.ability));
                        if (toCheck != null && toCheck.Count > 0)
                        {
                            foreach (CardSlot validQueued in toCheck)
                            {
                                if (validQueued && Singleton<BoardManager>.Instance.GetCardQueuedForSlot(validQueued) != null)
                                {
                                    PlayableCard queued = Singleton<BoardManager>.Instance.GetCardQueuedForSlot(validQueued);
                                    if (queued && queued.GetComponent<Wingrider>())
                                    {
                                        yield return queued.GetComponent<Wingrider>().OnOtherCardResolveOpponentQueue(card);
                                    }
                                }
                            }
                        }
                    }
                }
                yield break;
            }
        }
    }
    [HarmonyPatch(typeof(BoardManager), "QueueCardForSlot")]
    public class QueueCardForSlotPatch
    {
        [HarmonyPostfix]
        public static void Postfix(PlayableCard card, CardSlot slot, float tweenLength = 0.1f, bool doTween = true, bool setStartPosition = true)
        {
            List<CardSlot> slots = Singleton<BoardManager>.Instance.opponentSlots;
            foreach (CardSlot opslot in slots)
            {
                if (opslot != null && opslot.Card != null && opslot.Card.GetComponent<Divination>())
                {
                    opslot.Card.GetComponent<Divination>().OnOpponentCardQueued(card);
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerHand), "PlayCardOnSlot")]
    public class PlayCardOnSlotPatch
    {
        [HarmonyPrefix]
        public static void Prefix(PlayableCard card, CardSlot slot, PlayerHand __instance)
        {
            if (__instance.CardsInHand.Contains(card))
            {
                card.gameObject.AddComponent<PlayedFromHand>();
            }
        }
    }
    public class PlayedFromHand : MonoBehaviour { }
}

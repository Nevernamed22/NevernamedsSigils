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
            foreach(CardSlot opslot in slots)
            {
                if (opslot != null && opslot.Card != null && opslot.Card.GetComponent<Divination>())
                {
                    opslot.Card.GetComponent<Divination>().OnOpponentCardQueued(card);
                }
            }
        }
    }
}

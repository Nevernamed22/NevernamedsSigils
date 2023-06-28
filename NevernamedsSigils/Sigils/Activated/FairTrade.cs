using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class FairTrade : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Fair Trade", "Pay 1 pelt to remove an opponent's creature from the board, and add it to your hand.",
                      typeof(FairTrade),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/fairtrade.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/fairtrade_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool CanActivate()
        {
            return Singleton<PlayerHand>.Instance.CardsInHand.Exists((PlayableCard x) => x.Info.HasTrait(Trait.Pelt)) && (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => x.Card != null && !x.Card.HasTrait(Trait.Uncuttable) || Singleton<TurnManager>.Instance.Opponent.Queue.Exists(y => y != null && !y.HasTrait(Trait.Uncuttable))));
        }
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return TakeCardSequence();
            yield return base.LearnAbility(0.1f);

            yield break;
        }
        public IEnumerator TakeCardSequence()
        {
            purchased = false;

            //Remove Pelt
            PlayableCard pelt = Singleton<PlayerHand>.Instance.CardsInHand.Find((PlayableCard x) => x.Info.HasTrait(Trait.Pelt));
            Singleton<PlayerHand>.Instance.RemoveCardFromHand(pelt);
            pelt.SetEnabled(false);
            pelt.Anim.SetTrigger("fly_off");
            Tween.Position(pelt.transform, pelt.transform.position + new Vector3(0f, 3f, 5f), 0.4f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, delegate { UnityEngine.Object.Destroy(pelt.gameObject); });

            (Singleton<BoardManager>.Instance as BoardManager3D).Bell.enabled = false;
            (Singleton<BoardManager>.Instance as BoardManager3D).Bell.SetEnabled(false);
            foreach (CardSlot cardSlot in Singleton<BoardManager>.Instance.OpponentSlotsCopy)
            {
                if (cardSlot.Card != null)
                {
                    cardSlot.Card.RenderInfo.hiddenCost = false;
                    cardSlot.Card.RenderCard();
                }
            }
            foreach (PlayableCard playableCard in Singleton<TurnManager>.Instance.Opponent.Queue)
            {
                playableCard.RenderInfo.hiddenCost = false;
                playableCard.RenderCard();
            }
            Singleton<ViewManager>.Instance.SwitchToView(View.OpponentQueue, false, true);
            Singleton<ItemsManager>.Instance.SetSlotsInteractable(false);
            Singleton<PlayerHand>.Instance.PlayingLocked = true;
            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.TraderCardsForPeltsPhase, false);


            foreach (CardSlot slot2 in Singleton<BoardManager>.Instance.OpponentSlotsCopy)
            {
                if (slot2.Card != null)
                {
                    CardSlot cardSlot = slot2;
                    cardSlot.CursorSelectStarted = (Action<MainInputInteractable>)Delegate.Combine(cardSlot.CursorSelectStarted, (Action<MainInputInteractable>)delegate
                    {
                        OnTradableSelected(slot2, slot2.Card);
                    });
                    slot2.HighlightCursorType = CursorType.Pickup;
                }
            }
            foreach (PlayableCard card in Singleton<TurnManager>.Instance.Opponent.Queue)
            {
                HighlightedInteractable slot = Singleton<BoardManager>.Instance.OpponentQueueSlots[Singleton<BoardManager>.Instance.OpponentSlotsCopy.IndexOf(card.QueuedSlot)];
                HighlightedInteractable highlightedInteractable = slot;
                highlightedInteractable.CursorSelectStarted = (Action<MainInputInteractable>)Delegate.Combine(highlightedInteractable.CursorSelectStarted, (Action<MainInputInteractable>)delegate
                {
                    OnTradableSelected(slot, card);
                });
                slot.HighlightCursorType = CursorType.Pickup;
            }

            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            yield return new WaitWhile(() => !purchased );

            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
            foreach (CardSlot item3 in Singleton<BoardManager>.Instance.OpponentSlotsCopy)
            {
                item3.ClearDelegates();
                item3.HighlightCursorType = CursorType.Default;
            }
            foreach (HighlightedInteractable opponentQueueSlot in Singleton<BoardManager>.Instance.OpponentQueueSlots)
            {
                opponentQueueSlot.ClearDelegates();
                opponentQueueSlot.HighlightCursorType = CursorType.Default;
            }

            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            Singleton<ItemsManager>.Instance.SetSlotsInteractable(true);
            Singleton<PlayerHand>.Instance.PlayingLocked = false;
            (Singleton<BoardManager>.Instance as BoardManager3D).Bell.enabled = true;
            (Singleton<BoardManager>.Instance as BoardManager3D).Bell.SetEnabled(enabled: true);
            Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardGameDefault, false);

            //Remove Cost from enemy cards
            foreach (CardSlot item4 in Singleton<BoardManager>.Instance.OpponentSlotsCopy)
            {
                if (item4.Card != null)
                {
                    item4.Card.RenderInfo.hiddenCost = true;
                    item4.Card.RenderCard();
                }
            }
            foreach (PlayableCard item5 in Singleton<TurnManager>.Instance.Opponent.Queue)
            {
                if (item5 != null)
                {
                    item5.RenderInfo.hiddenCost = true;
                    item5.RenderCard();
                }
            }

        }
        public bool purchased = false;
        private void OnTradableSelected(HighlightedInteractable slot, PlayableCard card)
        {
            if (!purchased && !card.HasTrait(Trait.Uncuttable))
            {
                AscensionStatsData.TryIncrementStat(AscensionStat.Type.PeltsTraded);
                card.UnassignFromSlot();
                Tween.Position(card.transform,card.transform.position + new Vector3(0f, 0.25f, -5f), 0.3f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, delegate
                {
                    Destroy(card.gameObject);
                });
                ((MonoBehaviour)this).StartCoroutine(Singleton<PlayerHand>.Instance.AddCardToHand(CardSpawner.SpawnPlayableCard(card.Info), new Vector3(0f, 0.5f, -3f), 0f));
                slot.ClearDelegates();
                slot.HighlightCursorType = CursorType.Default;
                purchased = true;
            }
        }
    }
}
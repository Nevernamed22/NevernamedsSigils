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
    public class BloodActivatedAbility : ActivatedAbilityBehaviour
    {
        public override Ability Ability { get { return Ability.None; } }
        public virtual int BloodRequired()
        {
            return 1;
        }       
        public override bool CanActivate()
        {
            return Singleton<BoardManager>.Instance.GetValueOfSacrifices(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll((CardSlot x) => x.Card && x.Card != base.Card)) >= BloodRequired();
        }
        public override IEnumerator Activate()
        {
            yield return ChooseSacrificesSequence();                      
            yield break;
        }        
        private IEnumerator ChooseSacrificesSequence()
        {
			BoardManager board = Singleton<BoardManager>.Instance;
			Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;			
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Sacrifice);
			board.cancelledPlacementWithInput = false;
			board.currentValidSlots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll((CardSlot x) => x.Card && x.Card != base.Card);
			board.currentSacrificeDemandingCard = base.Card;
			board.CancelledSacrifice = false;
			board.LastSacrificesInfo.Clear();
			board.SetQueueSlotsEnabled(false);

			foreach (CardSlot cardSlot in board.AllSlots)
			{
				if (!cardSlot.IsPlayerSlot || cardSlot.Card == null)
				{
					cardSlot.SetEnabled(false);
					cardSlot.ShowState(HighlightedInteractable.State.NonInteractable, false, 0.15f);
				}
				if (cardSlot.IsPlayerSlot && cardSlot.Card != null && cardSlot.Card.CanBeSacrificed && cardSlot.Card != base.Card)
				{
					cardSlot.Card.Anim.SetShaking(true);
				}
			}

			yield return board.SetSacrificeMarkersShown(BloodRequired());

			while (board.GetValueOfSacrifices(board.currentSacrifices) < BloodRequired() && !board.cancelledPlacementWithInput)
			{
				board.SetSacrificeMarkersValue(board.currentSacrifices.Count);
				yield return new WaitForEndOfFrame();
			}


			foreach (CardSlot cardSlot2 in board.AllSlots)
			{
				cardSlot2.SetEnabled(false);
				if (cardSlot2.IsPlayerSlot && cardSlot2.Card != null) { cardSlot2.Card.Anim.SetShaking(false); }
			}

			foreach (CardSlot cardSlot3 in board.currentSacrifices) { board.LastSacrificesInfo.Add(cardSlot3.Card.Info); }

			if (board.cancelledPlacementWithInput)
			{
				board.HideSacrificeMarkers();
				foreach (CardSlot cardSlot4 in board.GetSlots(true))
				{
					if (cardSlot4.Card != null)
					{
						cardSlot4.Card.Anim.SetSacrificeHoverMarkerShown(false);
						if (board.currentSacrifices.Contains(cardSlot4))
						{
							cardSlot4.Card.Anim.SetMarkedForSacrifice(false);
						}
					}
				}
				Singleton<ViewManager>.Instance.SwitchToView(board.defaultView, false, false);
				Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
				board.CancelledSacrifice = true;
			}
			else
			{
				board.SetSacrificeMarkersValue(board.GetValueOfSacrifices(board.currentSacrifices));
				yield return new WaitForSeconds(0.2f);
				board.HideSacrificeMarkers();
				foreach (CardSlot cardSlot5 in board.currentSacrifices)
				{
					if (cardSlot5.Card != null && !cardSlot5.Card.Dead)
					{
						int sacrificesMadeThisTurn = board.SacrificesMadeThisTurn;
						board.SacrificesMadeThisTurn = sacrificesMadeThisTurn + 1;
						yield return OnIndividualCardPreSacrifice(cardSlot5.Card);
						yield return cardSlot5.Card.Sacrifice();
						Singleton<ViewManager>.Instance.SwitchToView(board.BoardView, false, false);
					}
				}
				yield return base.PreSuccessfulTriggerSequence();
				yield return OnBloodAbilityPostAllSacrifices();
				yield return base.LearnAbility(0f);
			}
			board.SetQueueSlotsEnabled(true);
			foreach (CardSlot cardSlot6 in board.AllSlots)
			{
				cardSlot6.SetEnabled(true);
				cardSlot6.ShowState(HighlightedInteractable.State.Interactable, false, 0.15f);
			}
			board.currentSacrificeDemandingCard = null;
			board.currentSacrifices.Clear();
			Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
			yield break;
		}
		public virtual IEnumerator OnIndividualCardPreSacrifice(PlayableCard sacrifice)
        {
			yield break;
        }
		public virtual IEnumerator OnBloodAbilityPostAllSacrifices()
        {
			yield break;
        }
    }
}
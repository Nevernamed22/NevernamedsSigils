using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Snapshot : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Snapshot", "When [creature] perishes, the entire board reverts to the state it was in when [creature] was placed.",
                      typeof(Snapshot),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/snapshot.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/snapshot_pixel.png"));

            Snapshot.ability = newSigil.ability;
        }

        public BoardState boardState;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            this.boardState = BoardState.GenerateFromCurrentBoard();
            yield break;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return new WaitForSeconds(0.1f);
            if (boardState != null && base.Card.slot != null)
            {
                yield return RevertToBoardSnapshot(boardState);
            }
            yield break;
        }
        private IEnumerator RevertToBoardSnapshot(BoardState snapshot)
        {
            if (Tools.GetActAsInt() == 1)
            {
                AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX, 1f, 0f, null, null, null, null, false);
            Tools.ClearBoard(new List<CardSlot>() { base.Card.slot }, true, true);
            }
            else
            {
            Tools.ClearBoard(new List<CardSlot>() { base.Card.slot }, true, false);
            }
            yield return new WaitForSeconds(0.2f);
            yield return ApplySlotStates(snapshot.playerSlots, Singleton<BoardManager>.Instance.PlayerSlotsCopy);
            yield return ApplySlotStates(snapshot.opponentSlots, Singleton<BoardManager>.Instance.OpponentSlotsCopy);
            yield break;
        }

        private IEnumerator ApplySlotStates(List<BoardState.SlotState> slotStates, List<CardSlot> actualSlots)
        {
            for (int i = 0; i < slotStates.Count; i++)
            {
                if (slotStates[i].card != null && !slotStates[i].card.HasAbility(Snapshot.ability))
                {
                    yield return ApplySlotState(slotStates[i], actualSlots[i]);
                    yield return new WaitForSeconds(0.07f);
                }
            }
            yield break;
        }
        private IEnumerator ApplySlotState(BoardState.SlotState slotState, CardSlot slot)
        {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(slotState.card.info, slot, 0f, false);
                PlayableCard card = slot.Card;
                card.TemporaryMods = new List<CardModificationInfo>(slotState.card.temporaryMods);
                card.Status = new PlayableCardStatus(slotState.card.status);
                card.OnStatsChanged();
                Singleton<ResourcesManager>.Instance.ForceGemsUpdate();
            yield break;
        }
    }
}

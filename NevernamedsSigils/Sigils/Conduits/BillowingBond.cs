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
    public class BillowingBond : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Billowing Bond", "Creatures within a bond completed by [creature] gain the power of flight at the start of each turn.",
                      typeof(BillowingBond),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/billowingbond.png"),
                      pixelTex: null,
                      isConduit: true);

            BillowingBond.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            bool successfulTriggerShown = false;
            List<CardSlot> slots = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy;
            foreach (CardSlot slot in slots)
            {
                if (Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(slot).Contains(base.Card))
                {
                    if (slot.Card != null && !slot.Card.HasAbility(Ability.Flying))
                    {
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.05f);
                        if (!successfulTriggerShown)
                        {
                            yield return base.PreSuccessfulTriggerSequence();
                        }
                        successfulTriggerShown = true;

                        CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Flying);
                        cardModificationInfo.singletonId = "billowing_bond";
                        cardModificationInfo.RemoveOnUpkeep = true;
                        slot.Card.Status.hiddenAbilities.Add(Ability.Flying);
                        slot.Card.AddTemporaryMod(cardModificationInfo);
                        Vector3 position = slot.Card.transform.position;
                        Tween.Position(slot.Card.transform, position + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                        Tween.Position(slot.Card.transform, position, 1f, 0.1f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
            yield break;
        }

    }
}

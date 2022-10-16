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
    public class TenderBond : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Tender Bond", "Creatures within a bond completed by [creature] heal to full health at the start of the owner's turn.",
                      typeof(TenderBond),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/tenderbond.png"),
                      pixelTex: null,
                      isConduit: true);

            TenderBond.ability = newSigil.ability;
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
                    if (slot.Card != null && slot.Card.Status.damageTaken > 0)
                    {
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.05f);
                        if (!successfulTriggerShown)
                        {
                            yield return base.PreSuccessfulTriggerSequence();
                        }
                        successfulTriggerShown = true;
                        slot.Card.Status.damageTaken = 0;
                        slot.Card.Anim.LightNegationEffect();
                        slot.Card.UpdateStatsText();
                    }
                }               
            }
            yield return new WaitForSeconds(0.2f);
            yield break;
        }

    }
}

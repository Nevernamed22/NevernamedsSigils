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
    public class FriendshipBond : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Friendship Bond", "Empty spaces within a bond completed by [creature] spawn Squirrels at the end of the owner's turn.",
                      typeof(FriendshipBond),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/friendshipbond.png"),
                      pixelTex: null,
                      isConduit: true);

            FriendshipBond.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            bool successfulTriggerShown = false;
            List<CardSlot> slots = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy;
            foreach (CardSlot slot in slots)
            {
                if (Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(slot).Contains(base.Card))
                {
                    if (slot.Card == null)
                    {
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.05f);
                        if (!successfulTriggerShown)
                        {
                            yield return base.PreSuccessfulTriggerSequence();
                        }
                        successfulTriggerShown = true;
                        yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("Squirrel"), slot, 0.1f, true);
                    }
                    else if (slot.Card.Info.name == "Squirrel")
                    {
                        if (!successfulTriggerShown)
                        {
                            yield return base.PreSuccessfulTriggerSequence();
                        }
                        successfulTriggerShown = true;
                        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                        yield return new WaitForSeconds(0.05f);
                        slot.Card.Anim.LightNegationEffect();
                        slot.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                    }

                }
            }
            yield return new WaitForSeconds(0.2f);
            yield break;
        }

    }
}

using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;

namespace NevernamedsSigils
{
    public class StimConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Stim Conduit", "At the end of its owners turn, creatures within a circuit completed by [creature] gain 1 Health.",
                      typeof(StimConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/stimconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/stimconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] increases the health of every creature in its circuit!");

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            List<CardSlot> affectedSlots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card));

            if (affectedSlots.Count > 0)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();

                foreach (CardSlot slot in affectedSlots)
                {
                    if (slot.Card != null && !slot.Card.Dead)
                    {
                        slot.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                    }
                }
                yield return base.PreSuccessfulTriggerSequence();

                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }

            yield break;
        }

    }
}

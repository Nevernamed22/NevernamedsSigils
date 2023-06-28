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
    public class NanoConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Nano Conduit", "At the end of its owners turn, creatures within a circuit completed by [creature] gain the Armored sigil.",
                      typeof(NanoConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/nanoconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/nanoconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] shields all creatures in its circuit!");

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

                yield return base.PreSuccessfulTriggerSequence();
                foreach (CardSlot slot in affectedSlots)
                {
                    if (slot.Card != null && !slot.Card.Dead)
                    {
                        if (slot.Card.HasAbility(Ability.DeathShield))
                        {
                            slot.Card.ResetShield();
                        }
                        else
                        {
                            CardModificationInfo mod = new CardModificationInfo(Ability.DeathShield);
                            if (Tools.GetActAsInt() == 3) { slot.Card.Status.hiddenAbilities.Add(Ability.DeathShield); }
                            slot.Card.AddTemporaryMod(mod);
                        }
                        slot.Card.RenderCard();
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }

            yield break;
        }

    }
}

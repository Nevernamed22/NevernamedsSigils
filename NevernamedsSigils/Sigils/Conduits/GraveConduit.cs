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
    public class GraveConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Grave Conduit", "Completes a circuit. At the end of the owner's turn, [creature] will generate one bone for each creature contained within the circuit it completes.",
                      typeof(GraveConduit),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/graveconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/graveconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] grants a bone for every creature in its circuit.");

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
            int num = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => x.Card != null && Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card)).Count;

            if (num > 0)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();

                if (base.Card.OpponentCard)
                {
                    if (OpponentResourceManager.instance != null)
                    {
                        OpponentResourceManager.instance.AddOpponentBones(base.Card.slot, num);
                    }
                }
                else
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    yield return Singleton<ResourcesManager>.Instance.AddBones(num, base.Card.Slot);
                }
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield break;
        }

    }
}

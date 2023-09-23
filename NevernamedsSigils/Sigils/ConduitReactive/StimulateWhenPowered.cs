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
    public class StimulateWhenPowered : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Stimulate When Powered", "While [creature] is inside a completed circuit, it gains 2 health at the start of its owners turn.",
                      typeof(StimulateWhenPowered),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/stimulatewhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/stimulatewhenpowered_pixel.png"),
                      isConduitCell: true,
                      triggerText: "[creature] stimulates its health!");

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
            return base.Card.OnBoard && base.Card.OpponentCard != playerUpkeep && Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            base.Card.AddTemporaryMod(new CardModificationInfo(0,2));
            yield break;
        }    
    }
}

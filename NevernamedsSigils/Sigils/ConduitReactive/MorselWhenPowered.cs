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
    public class MorselWhenPowered : AbilityBehaviour
    {
        new public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Morsel When Powered", "When [creature] is sacrificed while inside a completed circuit, ⁣it adds its stat values to the card it was sacrificed for.",
                      typeof(MorselWhenPowered),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/morselwhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/morselwhenpowered_pixel.png"),
                      isConduitCell: true);

            ability = newSigil.ability;
        }
        new public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToSacrifice()
        {
            return Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override IEnumerator OnSacrifice()
        {
            yield return base.PreSuccessfulTriggerSequence();
            CardModificationInfo mod = new CardModificationInfo(base.Card.Attack, base.Card.Health);
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.AddTemporaryMod(mod);
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}
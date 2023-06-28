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
    public class PoweredQuills : AbilityBehaviour
    {
        new public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Powered Quills", "When [creature] is struck while inside a completed circuit, the striker is dealt 2 damage.",
                      typeof(PoweredQuills),
                      categories: new List<AbilityMetaCategory> {  AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Rulebook},
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/poweredquills.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/poweredquills_pixel.png"),
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
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Health > 0 && Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            yield return source.TakeDamage(2, base.Card);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}
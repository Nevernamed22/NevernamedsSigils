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
    public class EnlargeCustom : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Enlarge", "Pay 2 Bones to increase the Power and Health of [creature] by 1.",
                      typeof(EnlargeCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/enlarge.png"),
                      pixelTex: null,
                      isActivated: true);

            EnlargeCustom.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        protected override int BonesCost
        {
            get
            {
                return 2;
            }
        }
        
        public override IEnumerator Activate()
        {           
                yield return base.PreSuccessfulTriggerSequence();
            base.Card.AddTemporaryMod(new CardModificationInfo(1, 1));
            base.Card.Anim.LightNegationEffect();
                yield return base.LearnAbility(0f);
            
            yield break;
        }
       
    }
}

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
    public class Bonefed : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bonefed", "Pay 2 bones to heal [creature] for up to 3 health.",
                      typeof(Bonefed),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/bonefed.png"),
                      pixelTex: null,
                      isActivated: true);

            Bonefed.ability = newSigil.ability;
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
        public override bool CanActivate()
        {
            return base.Card.Status.damageTaken > 0;
        }
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            base.Card.Status.damageTaken -= 3;
            base.Card.Status.damageTaken = Mathf.Max(0, base.Card.Status.damageTaken);
            base.Card.Anim.LightNegationEffect();
            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}
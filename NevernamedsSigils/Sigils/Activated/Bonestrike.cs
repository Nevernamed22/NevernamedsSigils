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
    public class Bonestrike : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bone Strike", "Pay 2 bones to deal 1 damage to the creature opposing [creature].",
                      typeof(Bonestrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/bonestrike.png"),
                      pixelTex: null,
                      isActivated: true);

            Bonestrike.ability = newSigil.ability;
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
            return base.Card.Slot.opposingSlot.Card != null;
        }
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);

            bool impactFrameReached = false;
            base.Card.Anim.PlayAttackAnimation(false, base.Card.Slot.opposingSlot, delegate ()
            {
                impactFrameReached = true;
            });
            yield return new WaitUntil(() => impactFrameReached);
            yield return base.Card.Slot.opposingSlot.Card.TakeDamage(1, base.Card);
            yield return new WaitForSeconds(0.25f);

            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}
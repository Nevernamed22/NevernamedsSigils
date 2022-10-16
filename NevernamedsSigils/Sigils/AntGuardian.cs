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
    public class AntGuardian : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ant Guardian", "[creature] will deal damage equal to it's attack power to any card which strikes a friendly ant.",
                      typeof(AntGuardian),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/antguardian.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/antguardian_pixel.png"));

            AntGuardian.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return (target.OpponentCard == base.Card.OpponentCard) && (target.HasTrait(Trait.Ant) && (attacker != null));
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {

            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.25f);

            if (attacker != null && !attacker.Dead)
            {
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.5f);

                if (base.Card.Anim != null)
                {
                    base.Card.Anim.PlayAttackAnimation(false, attacker.Slot);
                }
                yield return attacker.TakeDamage(base.Card.Attack, base.Card);
                yield return new WaitForSeconds(0.5f);
            }

            yield return base.LearnAbility(0.5f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}
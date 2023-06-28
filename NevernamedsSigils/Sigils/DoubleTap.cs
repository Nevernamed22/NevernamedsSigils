using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class DoubleTap : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Double Tap", "When a friendly creature strikes an opponent creature, [creature] will also attack that opponent creature.",
                      typeof(DoubleTap),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doubletap.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/doubletap_pixel.png"),
                      triggerText: "[creature] aids its comrades in battle!");

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

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return attacker != null && attacker.OpponentCard == base.Card.OpponentCard && attacker != base.Card;
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            if (target.Health > 0 && !target.Dead)
            {
                bool midCombat = false;
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.25f);
                if (target != null && !target.Dead)
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    if (base.Card.Anim.Anim.speed == 0f)
                    {
                        midCombat = true;
                        base.Card.Anim.Anim.speed = 1f;
                        yield return new WaitUntil(() => !base.Card.Anim.DoingAttackAnimation);
                    }
                    else
                    {
                        base.Card.Anim.LightNegationEffect();
                        if (base.Card.Anim is DiskCardAnimationController)
                        {
                            (base.Card.Anim as DiskCardAnimationController).SetWeaponMesh(DiskCardWeapon.Turret);
                            (base.Card.Anim as DiskCardAnimationController).AimWeaponAnim(target.Slot.transform.position);
                            (base.Card.Anim as DiskCardAnimationController).ShowWeaponAnim();
                        }
                        yield return new WaitForSeconds(0.5f);
                        bool impactFrameReached = false;
                        base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), target.Slot, delegate ()
                        {
                            impactFrameReached = true;
                        });
                        yield return new WaitUntil(() => impactFrameReached);
                    }
                    yield return target.TakeDamage(base.Card.Attack, base.Card);
                }
                yield return base.LearnAbility(0.5f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                if (midCombat && !target.Dead)
                {
                    base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), target.Slot, null);
                    yield return new WaitForSeconds(0.07f);
                    base.Card.Anim.SetAnimationPaused(paused: true);
                }
            }
        }
    }
}
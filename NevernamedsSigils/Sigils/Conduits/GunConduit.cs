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
    public class GunConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gun Conduit", "When a creature moves into the slot opposing a card in a circuit completed by [creature], the moved creature is dealt 1 damage.",
                      typeof(GunConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/gunconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/gunconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] takes a free shot!");

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
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            List<CardSlot> affectedSlots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card));
            return !base.Card.Dead && !otherCard.Dead && otherCard.Slot && otherCard.slot.opposingSlot != null && affectedSlots.Contains(otherCard.slot.opposingSlot);
        }

        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            if (otherCard != this.lastShotCard || Singleton<TurnManager>.Instance.TurnNumber != this.lastShotTurn)
            {
                this.lastShotCard = otherCard;
                this.lastShotTurn = Singleton<TurnManager>.Instance.TurnNumber;
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.25f);
                bool midCombat = false;
                if (otherCard != null && !otherCard.Dead)
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
                            (base.Card.Anim as DiskCardAnimationController).AimWeaponAnim(otherCard.Slot.transform.position);
                            (base.Card.Anim as DiskCardAnimationController).ShowWeaponAnim();
                        }
                        yield return new WaitForSeconds(0.5f);
                        bool impactFrameReached = false;
                        base.Card.Anim.PlayAttackAnimation(false, otherCard.slot, delegate ()
                        {
                            impactFrameReached = true;
                        });
                        yield return new WaitUntil(() => impactFrameReached);
                    }
                    yield return otherCard.TakeDamage(1, base.Card);
                }
                yield return base.LearnAbility(0.5f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                if (midCombat && !otherCard.Dead)
                {
                    base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), otherCard.Slot, null);
                    yield return new WaitForSeconds(0.07f);
                    base.Card.Anim.SetAnimationPaused(paused: true);
                }
            }
            yield break;
        }
        private int lastShotTurn = -1;
        private PlayableCard lastShotCard;

    }
}

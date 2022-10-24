using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Lurk : Submerge, IActivateWhenFacedown
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Lurk", "Submerges during the opponent's turn. When a creature moves into the space opposing [creature], they are dealt 1 damage.",
                      typeof(Lurk),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/lurk.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/lurk_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public bool ShouldTriggerWhenFaceDown(Trigger trigger, object[] objects)
        {
            if (trigger == Trigger.OtherCardAssignedToSlot) return true;
            return false;
        }
        public bool ShouldTriggerCustomWhenFaceDown(Type customTrigger)
        {
            return false;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot;
        }

        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            if (otherCard != this.lastShotCard || Singleton<TurnManager>.Instance.TurnNumber != this.lastShotTurn)
            {
                bool setBack = false;
                if (base.Card.FaceDown)
                {
                base.Card.SetFaceDown(false, false);
                base.Card.UpdateFaceUpOnBoardEffects();
                    setBack = true;
                }
                this.lastShotCard = otherCard;
                this.lastShotTurn = Singleton<TurnManager>.Instance.TurnNumber;
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.25f);
                if (otherCard != null && !otherCard.Dead)
                {
                    yield return base.PreSuccessfulTriggerSequence();
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
                    yield return otherCard.TakeDamage(1, base.Card);
                }
                yield return base.LearnAbility(0.5f);
                if (setBack)
                {
                    base.Card.SetFaceDown(true, false);
                    base.Card.UpdateFaceUpOnBoardEffects();
                }
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield break;
        }
        private int lastShotTurn = -1;
        private PlayableCard lastShotCard;

    }
}
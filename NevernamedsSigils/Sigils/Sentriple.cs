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
    public class Sentriple : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sentriple", "When a creature moves into the space opposing [creature], they, as well as any creatures to their left or right, are dealt 1 damage.",
                      typeof(Sentriple),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/sentriple.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/sentriple_pixel.png"));

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
        private bool RespondsToTrigger(PlayableCard otherCard) { return !base.Card.Dead && !otherCard.Dead && otherCard.Slot == base.Card.Slot.opposingSlot; }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard) { return this.RespondsToTrigger(otherCard); }
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard) { return this.RespondsToTrigger(otherCard); }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard) { yield return this.FireAtOpposingSlot(otherCard); yield break; }
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard) { yield return this.FireAtOpposingSlot(otherCard); yield break; }
        private void Awake() { if (base.Card.Anim is DiskCardAnimationController) { (base.Card.Anim as DiskCardAnimationController).SetWeaponMesh(DiskCardWeapon.Turret); } }
        public override int Priority { get { return -1; } }
        private int lastShotTurn = -1;
        private PlayableCard lastShotCard;
        private IEnumerator FireAtOpposingSlot(PlayableCard otherCard)
        {
            // Debug.Log("fire triggered");
            if (otherCard != this.lastShotCard || Singleton<TurnManager>.Instance.TurnNumber != this.lastShotTurn)
            {
                //Debug.Log("not last shot or not last turn");
                this.lastShotCard = otherCard;
                this.lastShotTurn = Singleton<TurnManager>.Instance.TurnNumber;
                List<PlayableCard> targets = new List<PlayableCard>() { };
                if (otherCard && otherCard.slot && Singleton<BoardManager>.Instance.GetAdjacent(otherCard.slot, true) && Singleton<BoardManager>.Instance.GetAdjacent(otherCard.slot, true).Card)
                {
                    targets.Add(Singleton<BoardManager>.Instance.GetAdjacent(otherCard.slot, true).Card);
                }
                targets.Add(otherCard);
                if (otherCard && otherCard.slot && Singleton<BoardManager>.Instance.GetAdjacent(otherCard.slot, false) && Singleton<BoardManager>.Instance.GetAdjacent(otherCard.slot, false).Card)
                {
                    targets.Add(Singleton<BoardManager>.Instance.GetAdjacent(otherCard.slot, false).Card);
                }

                bool midCombat = false;
                // Debug.Log("target count " + targets.Count);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                for (int i = 0; i < targets.Count; i++)
                {
                    if (base.Card != null && !base.Card.Dead && targets[i] != null && !targets[i].Dead)
                    {
                        //Debug.Log("Target " + i);
                        PlayableCard indivTarget = targets[i];
                        yield return new WaitForSeconds(0.25f);
                        if (indivTarget != null && !indivTarget.Dead)
                        {
                            //Debug.Log("Target not dead");
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
                                    (base.Card.Anim as DiskCardAnimationController).AimWeaponAnim(indivTarget.Slot.transform.position);
                                    (base.Card.Anim as DiskCardAnimationController).ShowWeaponAnim();
                                }
                                yield return new WaitForSeconds(0.5f);
                                bool impactFrameReached = false;
                                base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), indivTarget.Slot, delegate ()
                                {
                                    impactFrameReached = true;
                                });
                                yield return new WaitUntil(() => impactFrameReached);
                            }
                                yield return indivTarget.TakeDamage(1, base.Card);
                            }
                        }
                    }
                    yield return base.LearnAbility(0.5f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    if (midCombat && !otherCard.Dead)
                    {
                        base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), otherCard.Slot, null);
                        yield return new WaitForSeconds(0.07f);
                        base.Card.Anim.SetAnimationPaused(paused: true);
                    }
                    yield break;
                }
            }
        }
    }
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
    public class Mirrific : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Mirrific", "When [creature] perishes, its owner chooses a creature to gain the Mirror rorriM ability.",
                      typeof(Mirrific),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/mirrific.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/mirrific_pixel.png")
                      );

            ability = newSigil.ability;
        }
        public static Ability ability;
        public static GameObject target;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return Singleton<BoardManager>.Instance.AllSlots.Exists(x => x.Card && x.Card != base.Card && x.Card.Info.SpecialStatIcon == SpecialStatIcon.None && !x.Card.HasTrait(Trait.Giant));
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (target == null) { target = Tools.GetActAsInt() == 3 ? Tools.act3holotarget : Tools.act1holotarget; }
            if (base.Card.OpponentCard)
            {
                List<CardSlot> candidates = Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.Card != null && x.Card != base.Card && x.Card.Info.SpecialStatIcon == SpecialStatIcon.None && !x.Card.HasTrait(Trait.Giant));
                List<CardSlot> enemyCandidates = candidates.FindAll(x => x.Card != null && x.Card.OpponentCard);
                List<CardSlot> playerCard = candidates.FindAll(x => x.Card != null && !x.Card.OpponentCard);
                //Debug.Log("1");
                PlayableCard bestCandidate = null;
                foreach (CardSlot playerSlot in playerCard)
                {
                   // Debug.Log("2");

                    if (bestCandidate == null || (bestCandidate != null && playerSlot.Card && playerSlot.Card.Attack > bestCandidate.Attack))
                    {
                        bestCandidate = playerSlot.Card;
                        if (bestCandidate != null && bestCandidate.slot.opposingSlot.Card != null && bestCandidate.slot.opposingSlot.Card.Health > bestCandidate.Attack)
                        {
                            bestCandidate = bestCandidate.slot.opposingSlot.Card;
                        }
                    }
                }
                if (bestCandidate == null)
                {
                    //Debug.Log("3");
                    foreach (CardSlot enemySlot in enemyCandidates)
                    {
                        if (bestCandidate == null || enemySlot.Card.Attack < bestCandidate.Attack)
                        {
                            bestCandidate = enemySlot.Card;
                        }
                    }
                }

                //Debug.Log("4");

                if (bestCandidate != null)
                {
                    if (Tools.GetActAsInt() != 2)
                    {
                        if (instanceTarget != null)
                        {
                            GameObject inst = instanceTarget;
                            Tween.LocalScale(inst.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                            {
                                UnityEngine.Object.Destroy(inst);
                            }, true);
                        }
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, bestCandidate.slot.transform);
                        gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                        gameObject.transform.localRotation = Quaternion.identity;
                        instanceTarget = gameObject;
                        yield return new WaitForSeconds(0.5f);
                        Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                        {
                            UnityEngine.Object.Destroy(instanceTarget);
                        }, true);
                    }

                    int attk = bestCandidate.Info.Attack;
                    CardInfo newInfo = bestCandidate.Info.Clone() as CardInfo;
                    CardModificationInfo cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.specialAbilities = new List<SpecialTriggeredAbility>() { SpecialTriggeredAbility.Mirror };
                    cardModificationInfo.statIcon = SpecialStatIcon.Mirror;
                    cardModificationInfo.attackAdjustment = attk * -1;
                    newInfo.mods.Add(cardModificationInfo);

                    bestCandidate.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    bestCandidate.SetInfo(newInfo);
                    bestCandidate.RenderCard();
                }
            }
            else
            {
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.AllSlots;
                List<CardSlot> opponentSlotsCopy2 = Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.Card != null && x.Card != base.Card && x.Card.Info.SpecialStatIcon == SpecialStatIcon.None && !x.Card.HasTrait(Trait.Giant));

                yield return instance.ChooseTarget(opponentSlotsCopy, opponentSlotsCopy2, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                if (instanceTarget != null)
                {
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);
                }
                if (recentlySelected != null && recentlySelected.Card != null)
                {
                    int attk = recentlySelected.Card.Info.Attack;
                    CardInfo newInfo = recentlySelected.Card.Info.Clone() as CardInfo;
                    CardModificationInfo cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.specialAbilities = new List<SpecialTriggeredAbility>() { SpecialTriggeredAbility.Mirror };
                    cardModificationInfo.statIcon = SpecialStatIcon.Mirror;
                    cardModificationInfo.attackAdjustment = attk * -1;
                    newInfo.mods.Add(cardModificationInfo);

                    recentlySelected.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    recentlySelected.Card.SetInfo(newInfo);
                    recentlySelected.Card.RenderCard();
                }

                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield break;
        }
        private CardSlot recentlySelected;
        private void CardSelected(CardSlot slot)
        {
            recentlySelected = slot;
        }
        private void InvalidTargetSelected(CardSlot slot)
        {
            //base.Card.Anim.StrongNegationEffect();
        }
        private void CursotEnteredSlot(CardSlot slot)
        {
            if (Tools.GetActAsInt() != 2)
            {
                if (instanceTarget != null)
                {
                    GameObject inst = instanceTarget;
                    Tween.LocalScale(inst.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(inst);
                    }, true);
                }
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, slot.transform);
                gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                gameObject.transform.localRotation = Quaternion.identity;
                instanceTarget = gameObject;
            }
        }
        private GameObject instanceTarget;
    }
}

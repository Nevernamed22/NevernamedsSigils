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
    public class TrajectileQuills : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trajectile Quills", "When [creature] is struck, an opponent creature of the owner's choosing is dealt 1 damage.",
                      typeof(TrajectileQuills),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/trajectilequills.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/trajectilequills_pixel.png")
                      );

            ability = newSigil.ability;
            target = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon");
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
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (target == null) { target = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon"); }
            if (base.Card.OpponentCard)
            {
                PlayableCard strongest = Tools.GetStrongestCardOnBoard(true);
                if (strongest != null)
                {
                    if (Tools.GetActAsInt() == 1)
                    {
                        if (instanceTarget != null)
                        {
                            GameObject inst = instanceTarget;
                            Tween.LocalScale(inst.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                            {
                                UnityEngine.Object.Destroy(inst);
                            }, true);
                        }
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, strongest.slot.transform);
                        gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                        gameObject.transform.localRotation = Quaternion.identity;
                        instanceTarget = gameObject;
                        yield return new WaitForSeconds(0.5f);
                        Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                        {
                            UnityEngine.Object.Destroy(instanceTarget);
                        }, true);
                    }
                    yield return strongest.TakeDamage(1, base.Card);
                }
            }
            else
            {
                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
                List<CardSlot> opponentSlotsCopy2 = Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll((CardSlot x) => x.Card != null && !x.Card.Dead);
                if (opponentSlotsCopy2.Count > 0)
                {
                    Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

                    yield return instance.ChooseTarget(opponentSlotsCopy, opponentSlotsCopy2, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                    if (instanceTarget != null && Tools.GetActAsInt() == 1)
                    {
                        Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                        {
                            UnityEngine.Object.Destroy(instanceTarget);
                        }, true);
                    }
                    if (recentlySelected != null && recentlySelected.Card != null)
                    {
                        yield return recentlySelected.Card.TakeDamage(1, base.Card);
                    }

                    Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                else
                {
                    base.Card.Anim.StrongNegationEffect();
                }
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
            if (Tools.GetActAsInt() == 1)
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

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
    public class Ranger : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ranger", "When [creature] is played, it's owner may choose an opponent creature. That creature will take 10 damage.",
                      typeof(Ranger),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ranger.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ranger_pixel.png")
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
        public override bool RespondsToResolveOnBoard()
        {
            return Singleton<BoardManager>.Instance.GetSlots(base.Card.OpponentCard).Exists(x => x.Card != null);
        }
        public override IEnumerator OnResolveOnBoard()
        {
            target = Tools.GetActAsInt()== 3 ?  Tools.act3holotarget : Tools.act1holotarget; 
            if (base.Card.OpponentCard)
            {
                PlayableCard strongest = Tools.GetStrongestCardOnBoard(true);
                CardSlot slot = strongest != null ? strongest.Slot : null;
                if (slot != null)
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
                    yield return new WaitForSeconds(0.5f);
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);

                    if (slot.Card != null) { yield return slot.Card.TakeDamage(10, null); }
                }
            }
            else
            {
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
                List<CardSlot> opponentSlotsCopy2 = Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => x.Card != null);

                yield return instance.ChooseTarget(opponentSlotsCopy, opponentSlotsCopy2, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                if (instanceTarget != null && Tools.GetActAsInt() != 2)
                {
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);
                }
                if (recentlySelected != null)
                {
                    yield return recentlySelected.Card.TakeDamage(10, null);

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
                if (slot.Card != null)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, slot.transform);
                    gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                    gameObject.transform.localRotation = Quaternion.identity;
                    instanceTarget = gameObject;
                }
            }
        }
        private GameObject instanceTarget;
    }
}

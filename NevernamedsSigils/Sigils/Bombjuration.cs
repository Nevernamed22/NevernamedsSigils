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
    public class Bombjuration : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bombjuration", "When [creature] is played, it's owner may choose an empty slot on either side of the board. A magic bomb is created in the chosen slot.",
                      typeof(Bombjuration),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bombjuration.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bombjuration_pixel.png")
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
            return Singleton<BoardManager>.Instance.AllSlots.Exists(x => x.Card == null);
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (target == null) { target = Tools.GetActAsInt() == 3 ? Tools.act3holotarget : Tools.act1holotarget; }
            if (base.Card.OpponentCard)
            {
                PlayableCard strongest = Tools.GetStrongestCardOnBoard(true);
                CardSlot slot = strongest != null ? strongest.Slot : Tools.SeededRandomElement(Singleton<BoardManager>.Instance.playerSlots);
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
                    yield return new WaitForSeconds(0.5f);
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);
                }
                List<CardSlot> allEmptyslots = Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x != null && x.Card == null);
                allEmptyslots.AddRange(Singleton<BoardManager>.Instance.opponentSlots.FindAll(x => x != null && x.Card == null));
                CardSlot final = Tools.SeededRandomElement(allEmptyslots);
                List<CardSlot> emptyAdjacents = Singleton<BoardManager>.Instance.GetAdjacentSlots(slot).FindAll(x => x != null && x.Card == null);
                if (emptyAdjacents.Count > 0) { final = Tools.SeededRandomElement(emptyAdjacents); }
                else if (slot.opposingSlot != null && slot.opposingSlot.Card == null) { final = slot.opposingSlot; }

                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("SigilNevernamed MagicBomb"), final, 0.1f, true);

            }
            else
            {
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.AllSlotsCopy;
                List<CardSlot> opponentSlotsCopy2 = Singleton<BoardManager>.Instance.AllSlotsCopy.FindAll(x => x.Card == null);

                yield return instance.ChooseTarget(opponentSlotsCopy, opponentSlotsCopy2, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                if (instanceTarget != null)
                {
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);
                }
                if (recentlySelected != null)
                {
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("SigilNevernamed MagicBomb"), recentlySelected, 0.1f, true);
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
                if (slot.Card == null)
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

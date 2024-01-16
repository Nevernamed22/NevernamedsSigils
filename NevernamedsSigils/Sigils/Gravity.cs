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
    public class Gravity : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gravity", "When [creature] is played, it's owner may choose any opponent creature, and move it to any open space on their side of the board.",
                      typeof(Gravity),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gravity.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/gravity_pixel.png")
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
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card.OpponentCard)
            {
                PlayableCard toSteal = Tools.GetStrongestCardOnBoard(true, false, Stalwart.ability);
                if (toSteal != null && Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => x.Card == null))
                {
                    CardSlot target = Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => x.Card == null));
                    if (toSteal.FaceDown)
                    {
                        toSteal.SetFaceDown(false, false);
                        toSteal.UpdateFaceUpOnBoardEffects();
                    }
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(toSteal, target, 0.1f, null, false);
                    toSteal.SetIsOpponentCard(true);
                }
            }
            else
            {
                if (!target) { target = Tools.GetActAsInt() == 3 ? Tools.act3holotarget : Tools.act1holotarget; }
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => x.Card != null && !x.Card.HasAbility(Stalwart.ability)) && Singleton<BoardManager>.Instance.PlayerSlotsCopy.Exists(x => x.Card == null))
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);

                    BoardManager instance = Singleton<BoardManager>.Instance;
                    List<CardSlot> allslots = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
                    List<CardSlot> validslots = Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => x.Card != null && !x.Card.HasAbility(Stalwart.ability));

                    yield return instance.ChooseTarget(allslots, validslots, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                    if (instanceTarget != null)
                    {
                        Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                        {
                            UnityEngine.Object.Destroy(instanceTarget);
                        }, true);
                    }
                    if (recentlySelected != null)
                    {
                        PlayableCard SelectedCard = recentlySelected.Card;

                        Vector3 a = SelectedCard.Slot.IsPlayerSlot ? Vector3.forward : Vector3.back;
                        a *= 0.5f;
                        Tween.Position(SelectedCard.transform, SelectedCard.transform.position + a * 2f + Vector3.up * 0.25f, 0.15f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                        if (SelectedCard.FaceDown)
                        {
                            SelectedCard.SetFaceDown(false, false);
                            SelectedCard.UpdateFaceUpOnBoardEffects();
                        }

                        List<CardSlot> allslots2 = Singleton<BoardManager>.Instance.playerSlots;
                        List<CardSlot> validslots2 = Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card == null);

                        yield return instance.ChooseTarget(allslots2, validslots2, CardSelected, InvalidTargetSelected, CursotEnteredSlot2, () => false, CursorType.Target);
                        if (instanceTarget != null)
                        {
                            Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                            {
                                UnityEngine.Object.Destroy(instanceTarget);
                            }, true);
                        }

                        if (recentlySelected != null)
                        {

                            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(SelectedCard, recentlySelected, 0.1f, null, false);
                            SelectedCard.SetIsOpponentCard(!recentlySelected.IsPlayerSlot);
                        }
                    }

                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.25f);
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
                if (slot.Card != null && !slot.Card.HasAbility(Stalwart.ability))
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, slot.transform);
                    gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                    gameObject.transform.localRotation = Quaternion.identity;
                    instanceTarget = gameObject;
                }
            }
        }
        private void CursotEnteredSlot2(CardSlot slot)
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

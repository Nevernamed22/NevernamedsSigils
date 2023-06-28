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
    public class PunchingBag : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Punching Bag", "Once per turn, choose any creature. That creature will strike this card for 1 damage. Afterwards, this card regains 1 health.",
                      typeof(PunchingBag),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/punchingbag.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/punchingbag_pixel.png"),
                      isActivated: true);

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
        public static GameObject target;
        bool hasActivatedThisTurn = false;
        public override bool CanActivate()
        {
            return !hasActivatedThisTurn && Singleton<BoardManager>.Instance.AllSlotsCopy.FindAll(x => x.Card != null && x.Card != base.Card).Count > 0;
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            hasActivatedThisTurn = false;
            yield break;
        }
        public override IEnumerator Activate()
        {
            hasActivatedThisTurn = true;
            if (target == null) { target = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon"); }

            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

            BoardManager instance = Singleton<BoardManager>.Instance;
            List<CardSlot> playerSlots = Singleton<BoardManager>.Instance.AllSlotsCopy;
            List<CardSlot> playerSlotsWithCards = Singleton<BoardManager>.Instance.AllSlotsCopy.FindAll((CardSlot x) => x.Card != null && x.Card != base.Card);

            yield return instance.ChooseTarget(playerSlots, playerSlotsWithCards, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

            if (instanceTarget != null && Tools.GetActAsInt() != 2)
            {
                Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate () { UnityEngine.Object.Destroy(instanceTarget); }, true);
            }
            if (recentlySelected != null)
            {
                bool impactFrameReached = false;

                bool wasFaceDown = false;
                if (recentlySelected.Card.FaceDown)
                {
                    recentlySelected.Card.SetFaceDown(false, false);
                    recentlySelected.Card.UpdateFaceUpOnBoardEffects();
                    yield return new WaitForSeconds(0.25f);
                    wasFaceDown = true;
                }

                recentlySelected.Card.Anim.PlayAttackAnimation(false, base.Card.Slot, delegate ()
                {
                    impactFrameReached = true;
                });
                yield return new WaitUntil(() => impactFrameReached);
                yield return base.Card.TakeDamage(1, recentlySelected.Card);

                if (wasFaceDown)
                {
                    recentlySelected.Card.SetFaceDown(true, false);
                    recentlySelected.Card.UpdateFaceUpOnBoardEffects();
                }

                yield return new WaitForSeconds(0.25f);
                if (base.Card.Health > 0)
                {
                    base.Card.Anim.NegationEffect(false);
                    base.Card.HealDamage(1);
                }
            }
            Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

            yield break;
        }
        private CardSlot recentlySelected;
        private void CardSelected(CardSlot slot) { recentlySelected = slot; }
        private void InvalidTargetSelected(CardSlot slot) { }
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
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
    public class Deadbeat : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Deadbeat", "Choose a creature on the same side of the board. That creature will perish.",
                      typeof(Deadbeat),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/deadbeat.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/deadbeat_pixel.png"),
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

        public override IEnumerator Activate()
        {
            if (target == null) { target = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon"); }

            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
            Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Hammer);
            BoardManager instance = Singleton<BoardManager>.Instance;
            List<CardSlot> playerSlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
            List<CardSlot> playerSlotsWithCards = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) => x.Card != null);

            yield return instance.ChooseTarget(playerSlots, playerSlotsWithCards, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

            if (instanceTarget != null && Tools.GetActAsInt() == 1)
            {
                Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate () { UnityEngine.Object.Destroy(instanceTarget); }, true);
            }
            if (recentlySelected != null)
            {
                yield return recentlySelected.Card.Die(false, base.Card);  
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
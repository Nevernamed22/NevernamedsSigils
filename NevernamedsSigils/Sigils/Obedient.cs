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
    public class Obedient : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Obedient", "At the end of the owner's turn, [creature] will move to an empty space on the same side of the board, chosen by it's owner.",
                      typeof(Obedient),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/obedient.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/obedient_pixel.png")
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
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd && !base.Card.HasAbility(Stalwart.ability) && !base.Card.OpponentCard;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            if (!Singleton<BoardManager>.Instance.PlayerSlotsCopy.Exists(x => x.Card == null))
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.25f);
                base.Card.Anim.StrongNegationEffect();
            }
            else
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.25f);

                Vector3 a = base.Card.Slot.IsPlayerSlot ? Vector3.forward : Vector3.back;
                a *= 0.5f;
                Tween.Position(base.Card.transform, base.Card.transform.position + a * 2f + Vector3.up * 0.25f, 0.15f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);

                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> allslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
                List<CardSlot> validslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll(x => x.Card == null || x.Card == base.Card);

                yield return instance.ChooseTarget(allslots, validslots, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                if (instanceTarget != null && Tools.GetActAsInt() == 1)
                {
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);
                }
                if (recentlySelected != null)
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, recentlySelected, 0.1f, null, true);
                }

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
                if (slot.Card == null || slot.Card == base.Card)
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

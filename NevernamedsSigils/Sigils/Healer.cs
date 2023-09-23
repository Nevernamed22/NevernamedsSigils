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
    public class Healer : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Healer", "At the end of the owner's turn, [creature]s owner may choose a creature they control. That creature gains 1 health.",
                      typeof(Healer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/healer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/healer_pixel.png")
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
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OnBoard && base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            if (target == null) { target = Tools.GetActAsInt() == 3 ? Tools.act3holotarget : Tools.act1holotarget; }

            if (base.Card.OpponentCard)
            {
                PlayableCard chosen = null;
                int curMax = int.MaxValue;
                foreach(CardSlot slot in Singleton<BoardManager>.Instance.OpponentSlotsCopy)
                {
                    if (slot.Card != null && slot.Card.Health < curMax && 
                        !slot.Card.HasAbility(Ability.Brittle) && 
                        !slot.Card.HasAbility(Doomed.ability) &&
                        !slot.Card.HasAbility(Frail.ability)) { chosen = slot.Card; }
                }
                if (chosen != null)
                {
                    base.Card.Anim.LightNegationEffect();
                    chosen.AddTemporaryMod(new CardModificationInfo(0, 1));
                }
            }
            else
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.25f);

                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> allslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
                List<CardSlot> validslots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll(x => x.Card != null && !x.Card.Dead);

                yield return instance.ChooseTarget(allslots, validslots, CardSelected, InvalidTargetSelected, CursotEnteredSlot, () => false, CursorType.Target);

                if (instanceTarget != null)
                {
                    Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(instanceTarget);
                    }, true);
                }
                if (recentlySelected != null && recentlySelected.Card != null)
                {
                    base.Card.Anim.LightNegationEffect();
                    recentlySelected.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                }

                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }           
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

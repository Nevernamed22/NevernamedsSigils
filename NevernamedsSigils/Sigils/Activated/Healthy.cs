using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace NevernamedsSigils
{
    public class Healthy : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Healthy", "Sacrifice [creature] to increase the health of a creature of your choosing by 4.",
                      typeof(Healthy),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/healthy.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/healthy_pixel.png"),
                      isActivated: true);

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep && base.Card.OnBoard && base.Card.OpponentCard && base.Card.Health <= 2;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            PlayableCard stronk = Tools.GetStrongestCardOnBoard(false, false, Ability.None, true, new List<PlayableCard>() { base.Card });
            if (stronk != null && stronk != base.Card && stronk.slot != null)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, stronk.slot.transform);
                gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                gameObject.transform.localRotation = Quaternion.identity;
                instanceTarget = gameObject;
                yield return new WaitForSeconds(0.5f);
                Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                {
                    UnityEngine.Object.Destroy(instanceTarget);
                }, true);

                stronk.AddTemporaryMod(new CardModificationInfo(0, 4));
                stronk.OnStatsChanged();
                stronk.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.25f);
                yield return base.Card.Die(true, null, true);
            }
            yield break;
        }
        public override IEnumerator Activate()
        {
            if (!Singleton<BoardManager>.Instance.AllSlots.Exists(x => x.Card != null)) { yield break; }

            target = Tools.GetActAsInt() == 3 ? Tools.act3holotarget : Tools.act1holotarget;

            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

            BoardManager instance = Singleton<BoardManager>.Instance;
            List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.AllSlots;
            List<CardSlot> opponentSlotsCopy2 = Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.Card != null && x.Card != base.Card);

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
                recentlySelected.Card.AddTemporaryMod(new CardModificationInfo(0, 4));
                recentlySelected.Card.OnStatsChanged();
                recentlySelected.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.25f);
                yield return base.Card.Die(true, null, true);
            }

            Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

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

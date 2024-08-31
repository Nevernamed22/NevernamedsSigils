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
    public class Bewilder : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bewilder", "When [creature] is played, it's owner may choose any creature. Swap that creatures power and health.",
                      typeof(Bewilder),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bewilder.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bewilder_pixel.png")
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
            return Singleton<BoardManager>.Instance.AllSlots.Exists(x => x.Card != null && !x.Card.HasTrait(Trait.Giant));
        }
        public override IEnumerator OnResolveOnBoard()
        {
            target = Tools.GetActAsInt() == 3 ? Tools.act3holotarget : Tools.act1holotarget;
            if (base.Card.OpponentCard)
            {
                CardSlot targetslot = null;
                foreach (CardSlot playerslot in Singleton<BoardManager>.Instance.GetSlots(true).FindAll(x => x.Card != null && !x.Card.HasTrait(Trait.Giant)))
                {
                    if (playerslot.Card && playerslot.Card.Health < playerslot.Card.Attack && (targetslot == null || playerslot.Card.Attack > targetslot.Card.Attack)) { targetslot = playerslot; }
                }
                foreach (CardSlot enemyslot in Singleton<BoardManager>.Instance.GetSlots(false).FindAll(x => x.Card != null && !x.Card.HasTrait(Trait.Giant)))
                {
                    if (enemyslot.Card != null)
                    {
                        bool unblocked = enemyslot.opposingSlot.Card == null || (enemyslot.opposingSlot.Card != null &&   enemyslot.Card.HasAbility(Ability.Flying) && !enemyslot.opposingSlot.Card.HasAbility(Ability.Reach));
                        if (unblocked)
                        {
                            int damageUntilPlayerLoss = (LifeManager.GOAL_BALANCE * 2) - Singleton<LifeManager>.Instance.DamageUntilPlayerWin;
                            if (enemyslot.Card.Health >= damageUntilPlayerLoss && enemyslot.Card.Attack > 0) { targetslot = enemyslot; }
                        }
                    }
                }
                if (targetslot == null) { targetslot = Tools.RandomElement(Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.Card != null)); }

                if (instanceTarget != null)
                {
                    GameObject inst = instanceTarget;
                    Tween.LocalScale(inst.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                    {
                        UnityEngine.Object.Destroy(inst);
                    }, true);
                }
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(target, targetslot.transform);
                gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
                gameObject.transform.localRotation = Quaternion.identity;
                instanceTarget = gameObject;
                yield return new WaitForSeconds(0.5f);
                Tween.LocalScale(instanceTarget.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                {
                    UnityEngine.Object.Destroy(instanceTarget);
                }, true);

                targetslot.Card.AddTemporaryMod(new CardModificationInfo(-targetslot.Card.Attack + targetslot.Card.Health, -targetslot.Card.Health + targetslot.Card.Attack));
                targetslot.Card.OnStatsChanged();
                targetslot.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.25f);
                if (targetslot.Card.Health <= 0) { yield return targetslot.Card.Die(false, null, true); }
            }
            else
            {
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

                BoardManager instance = Singleton<BoardManager>.Instance;
                List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.AllSlots;
                List<CardSlot> opponentSlotsCopy2 = Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.Card != null && !x.Card.HasTrait(Trait.Giant));

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
                    recentlySelected.Card.AddTemporaryMod(new CardModificationInfo(-recentlySelected.Card.Attack + recentlySelected.Card.Health, -recentlySelected.Card.Health + recentlySelected.Card.Attack));
                    recentlySelected.Card.OnStatsChanged();
                    recentlySelected.Card.Anim.StrongNegationEffect();
                    yield return new WaitForSeconds(0.25f);
                    if (recentlySelected.Card.Health <= 0) { yield return recentlySelected.Card.Die(false, null, true); }

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

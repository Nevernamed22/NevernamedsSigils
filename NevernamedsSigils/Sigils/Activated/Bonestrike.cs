using APIPlugin;
using DiskCardGame;
using OpponentBones;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Bonestrike : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bone Strike", "Pay 2 bones to deal 1 damage to the creature opposing [creature].",
                      typeof(Bonestrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/bonestrike.png"),
                      pixelTex: null,
                      isActivated: true);

            Bonestrike.ability = newSigil.ability;
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        protected override int BonesCost
        {
            get
            {
                return 2;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard && !playerUpkeep && OpponentResourceManager.instance && OpponentResourceManager.instance.OpponentBones >= 2;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            int numPossibleActivations = Mathf.FloorToInt((float)OpponentResourceManager.instance.OpponentBones / 2f);
            if (base.Card.slot != null && base.Card.slot.opposingSlot.Card && numPossibleActivations > 0)
            {
                bool canKill = numPossibleActivations >= base.Card.slot.opposingSlot.Card.Health;
                PlayableCard opponent = base.Card.slot.opposingSlot.Card;
                bool shootToKill = false;

                int damageUntilPlayerLoss = (LifeManager.GOAL_BALANCE * 2) - Singleton<LifeManager>.Instance.DamageUntilPlayerWin;

                if (opponent.Attack >= base.Card.Health && canKill) { shootToKill = true; }
                if (base.Card.Attack >= damageUntilPlayerLoss && canKill) { shootToKill = true; }

                if (shootToKill)
                {
                    yield return DoOpponentLoopStrikes(base.Card.slot.opposingSlot.Card.Health);
                }
                else if (SeededRandom.Value(Tools.GetRandomSeed()) <= 0.3f && OpponentResourceManager.instance.OpponentBones >= 2)
                {
                    yield return DoOpponentLoopStrikes(1);
                }

            }
            yield break;
        }
        public IEnumerator DoOpponentLoopStrikes(int timesToTrigger)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            for (int i = 0; i < timesToTrigger; i++)
            {
                if (base.Card.slot.opposingSlot.Card != null && !base.Card.slot.opposingSlot.Card.Dead && !base.Card.Dead)
                {
                    yield return OpponentResourceManager.instance.RemoveOpponentBones(2);
                    bool impactFrameReached = false;
                    base.Card.Anim.PlayAttackAnimation(false, base.Card.Slot.opposingSlot, delegate ()
                    {
                        impactFrameReached = true;
                    });
                    yield return new WaitUntil(() => impactFrameReached);
                    yield return base.Card.Slot.opposingSlot.Card.TakeDamage(1, base.Card);
                    yield return new WaitForSeconds(0.25f);
                }
            }
            yield return base.LearnAbility(0.1f);
            yield break;
        }
        public override bool CanActivate()
        {
            return base.Card.Slot.opposingSlot.Card != null;
        }
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

            bool impactFrameReached = false;
            base.Card.Anim.PlayAttackAnimation(false, base.Card.Slot.opposingSlot, delegate ()
            {
                impactFrameReached = true;
            });
            yield return new WaitUntil(() => impactFrameReached);
            yield return base.Card.Slot.opposingSlot.Card.TakeDamage(1, base.Card);
            yield return new WaitForSeconds(0.25f);

            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}
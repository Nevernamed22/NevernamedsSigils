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
    public class SpidersWeb : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Spiders Web", "When any opponent card moves, with the exception of entering the board for the first time, [creature] will strike them for one damage.",
                      typeof(SpidersWeb),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/spidersweb.png"),
                      pixelTex: null);

            SpidersWeb.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        List<PlayableCard> playedCards;
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            playedCards = new List<PlayableCard>();
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.opponentSlots) if (slot && slot.Card != null) playedCards.Add(slot.Card);
            yield break;
        }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return !base.Card.Dead && !otherCard.Dead && otherCard.OpponentCard != base.Card.OpponentCard;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            playedCards.Add(otherCard);
            yield break;
        }
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return !base.Card.Dead && !otherCard.Dead && otherCard.OpponentCard != base.Card.OpponentCard && playedCards.Contains(otherCard) && !otherCard.HasAbility(Stalwart.ability);
        }
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            yield return this.FireAtOpposingSlot(otherCard);
            yield break;
        }
        private IEnumerator FireAtOpposingSlot(PlayableCard otherCard)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.25f);
            if (otherCard != null && !otherCard.Dead)
            {
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.5f);
                bool flag3 = base.Card.Anim != null;
                if (flag3)
                {
                    base.Card.Anim.PlayAttackAnimation(false, otherCard.Slot);
                }
                yield return otherCard.TakeDamage(1, base.Card);
                yield return new WaitForSeconds(0.5f);

            }
            yield return base.LearnAbility(0.5f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}
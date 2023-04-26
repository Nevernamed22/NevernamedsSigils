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
    public class Collector : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Collector", "At the end of the owner's turn, [creature] will move in the direction inscribed on the sigil. Creatures in the way will be returned to the owner's hand.",
                      typeof(Collector),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/collector.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/collector_pixel.png"));

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


        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            yield return this.DoStrafe(toLeft, toRight);
            yield break;
        }
        private IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
        {
            bool flag = toLeft != null;
            bool flag2 = toRight != null;
            if (this.movingLeft && !flag) { this.movingLeft = false; }
            if (!this.movingLeft && !flag2) { this.movingLeft = true; }

            CardSlot cardSlot = this.movingLeft ? toLeft : toRight;
            PlayableCard swappedCard = cardSlot.Card;
            if (swappedCard != null)
            {
                CardInfo toDraw = swappedCard.Info;
                List<CardModificationInfo> tempMods = new List<CardModificationInfo>();
                tempMods.AddRange(swappedCard.temporaryMods);
                int damageTaken = swappedCard.Status.damageTaken;

                if (Tools.GetActAsInt() == 2)
                {
                    swappedCard.UnassignFromSlot();
                    Tween.Position(swappedCard.transform, swappedCard.transform.position + new Vector3(0, -1f, 0), 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                    yield return new WaitForSeconds(0.1f);
                    UnityEngine.Object.Destroy(swappedCard.gameObject);
                }
                else { swappedCard.ExitBoard(0.25f, Vector3.zero); }

                Singleton<ResourcesManager>.Instance.ForceGemsUpdate();

                yield return new WaitForSeconds(0.2f);
                View prev = Singleton<ViewManager>.Instance.CurrentView;
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }

                PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toDraw);
                foreach (CardModificationInfo mod in tempMods) { playableCard.AddTemporaryMod(mod); }
                playableCard.Status.damageTaken = damageTaken;

                yield return Singleton<PlayerHand>.Instance.AddCardToHand(playableCard, Singleton<CardSpawner>.Instance.spawnedPositionOffset, 0.25f);
                yield return new WaitForSeconds(0.4f);
                Singleton<ViewManager>.Instance.SwitchToView(prev, false, false);
            }
            yield return base.PreSuccessfulTriggerSequence();
            yield return MoveToSlot(cardSlot, true);
            yield return base.LearnAbility(0f);
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();
            if (destination != null && destinationValid)
            {
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
        protected bool movingLeft;
    }
}
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Divination : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Divination", "The next card drawn while [creature] is alive and on the board will be duplicated.",
                      typeof(Divination),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/divination.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/divination_pixel.png"));

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
        public override int Priority => 10;
        public override CardInfo CardToDraw
        {
            get
            {
                return currenttarget;
            }
        }
        public override List<CardModificationInfo> CardToDrawTempMods
        {
            get
            {
                return currentDrawInf;
            }
        }
        public CardInfo currenttarget = null;
        public List<CardModificationInfo> currentDrawInf = new List<CardModificationInfo>();
        public override bool RespondsToOtherCardDrawn(PlayableCard card)
        {
            return base.Card.OnBoard && !base.Card.OpponentCard && !expended;
        }
        public void OnOpponentCardQueued(PlayableCard card)
        {
            if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
            {
                PlayableCard playableCard = CardSpawner.SpawnPlayableCard(card.Info);
                playableCard.SetIsOpponentCard(true);
                Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                    Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
            }
        }
        public override IEnumerator OnOtherCardDrawn(PlayableCard card)
        {
            currenttarget = null;
            currentDrawInf.Clear();

            currenttarget = card.Info;
            currentDrawInf.AddRange(card.temporaryMods);

            expended = true;
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return base.CreateDrawnCard();
            yield return base.LearnAbility(0.5f);

            base.Card.Status.hiddenAbilities.Add(this.Ability);
            base.Card.RenderCard();
            yield break;
        }

        public bool expended = false;
    }
}
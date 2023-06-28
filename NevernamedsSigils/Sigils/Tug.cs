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
    public class Tug : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Tug", "When [creature] is played, it will draw the queued opposing creature into the main opposing space, if possible.",
                      typeof(Tug),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair1 },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/tug.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/tug_pixel.png"));

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
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.slot && base.Card.slot.opposingSlot && !base.Card.slot.opposingSlot.Card;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card.OpponentCard)
            {
                int index = base.Card.slot.Index;
                if (Singleton<PlayerHand>.Instance.CardsInHand != null && Singleton<PlayerHand>.Instance.CardsInHand.Count > base.Card.slot.Index)
                {
                    yield return new WaitForSeconds(0.5f);
                    yield return base.PreSuccessfulTriggerSequence();

                    yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(Singleton<PlayerHand>.Instance.CardsInHand[index], base.Card.slot.opposingSlot);

                    yield return base.LearnAbility(0.5f);
                }
            }
            else if (Singleton<BoardManager>.Instance.GetCardQueuedForSlot(base.Card.slot.opposingSlot))
            {
                yield return PlayQueuedCard(Singleton<BoardManager>.Instance.GetCardQueuedForSlot(base.Card.slot.opposingSlot));
            }
            yield break;
        }
        public static IEnumerator PlayQueuedCard(PlayableCard queuedCard)
        {
            Opponent self = TurnManager.Instance?.Opponent;
            if (self != null && queuedCard != null && !queuedCard.OnBoard && queuedCard.QueuedSlot != null && !queuedCard.HasAbility(Stalwart.ability))
            {
                if (!self.QueuedCardIsBlocked(queuedCard))
                {
                    CardSlot queuedSlot = queuedCard.QueuedSlot;
                    queuedCard.QueuedSlot = null;
                    if (queuedCard != null)
                    {
                        queuedCard.OnPlayedFromOpponentQueue();
                    }
                    yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(queuedCard, queuedSlot, 0.1f, null, true);
                    self.Queue.Remove(queuedCard);
                }
            }
            yield break;
        }
    }
}
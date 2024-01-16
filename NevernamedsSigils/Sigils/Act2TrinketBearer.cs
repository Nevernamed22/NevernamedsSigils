using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Act2TrinketBearer : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trinket Bearer", "When [creature] is played, it grants it's owner a random item.",
                      typeof(Act2TrinketBearer),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/act2trinketbearer.png"),
                      triggerText: "[creature] pulls out a random item."
                      );

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
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.GetExtendedProperty("Act2TrinketBearerItemCard") != null);
                    int index = SeededRandom.Range(0, cards.Count, base.GetRandomSeed());
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(cards[index]);

                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }
            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.CreateDrawnCard();
            }
            yield break;
        }
        public override CardInfo CardToDraw
        {
            get
            {
                List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.GetExtendedProperty("Act2TrinketBearerItemCard") != null);
                int index = SeededRandom.Range(0, cards.Count, base.GetRandomSeed());
                return cards[index];
            }
        }
    }
}
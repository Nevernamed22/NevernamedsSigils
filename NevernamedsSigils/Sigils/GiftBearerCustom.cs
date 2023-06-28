using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class GiftBearerCustom : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gift Bearer", "When [creature] perishes, a random creature is created in your hand.",
                      typeof(GiftBearerCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/giftbearer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/giftbearer_pixel.png"));

            GiftBearerCustom.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public override CardInfo CardToDraw
        {
            get
            {

                CardInfo result = CardLoader.GetCardByName("SigilNevernamed Guts");

                if (base.Card.Info.GetExtendedProperty("GiftBearerCustomPoolIdentifier") != null)
                {
                    List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.GetExtendedProperty(base.Card.Info.GetExtendedProperty("GiftBearerCustomPoolIdentifier")) != null);
                    if (cards != null && cards.Count > 0)
                    {
                        int index = SeededRandom.Range(0, cards.Count, base.GetRandomSeed());
                        CardInfo gift = cards[index].Clone() as CardInfo;
                        gift.Mods.Add(base.Card.CondenseMods(new List<Ability>() { GiftBearerCustom.ability }));
                        result = gift;
                    }
                }
                else
                {
                    bool isRare = false;
                    if (base.Card.Info.GetExtendedProperty("CustomGiftBearerSpawnsRare") != null) isRare = true;
                    CardInfo gift = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), isRare, Tribe.None, false).Clone() as CardInfo;
                    gift.Mods.Add(base.Card.CondenseMods(new List<Ability>() { GiftBearerCustom.ability, GiftWhenPoweredCustom.ability }));
                    result = gift;
                }
                return result;
            }
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                        PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardToDraw);
                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.RandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }

            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.CreateDrawnCard();
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
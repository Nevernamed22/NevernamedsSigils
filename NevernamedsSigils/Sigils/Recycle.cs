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
    public class Recycle : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Recycle", "While [creature] is on the board, when any friendly creature perishes by combat a special card will be created in the owners hand. The card created depends on the temple of the friendly card which perishesd.",
                      typeof(Recycle),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/recycle.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/recycle_pixel.png"));

            ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            List<string> given = new List<string>() 
            {
                "SigilNevernamed BoneEffigy",
                "SigilNevernamed DesecratedMox",
                "SigilNevernamed ExcisedPowerCell",
                "SigilNevernamed RawMeat",
            };
            return base.Card.OnBoard && (card.OpponentCard == base.Card.OpponentCard) && fromCombat && card != base.Card && !given.Contains(card.Info.name);
        }
        public override CardInfo CardToDraw
        {
            get
            {
                switch (templeOfMostRecent)
                {
                    case CardTemple.Undead:
                        return CardLoader.GetCardByName("SigilNevernamed BoneEffigy");
                    case CardTemple.Wizard:
                        return CardLoader.GetCardByName("SigilNevernamed DesecratedMox");
                    case CardTemple.Tech:
                        return CardLoader.GetCardByName("SigilNevernamed ExcisedPowerCell");
                    default:
                        return CardLoader.GetCardByName("SigilNevernamed RawMeat");
                }
            }
        }
        public CardTemple templeOfMostRecent;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            templeOfMostRecent = card.Info.temple;
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardToDraw);
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
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
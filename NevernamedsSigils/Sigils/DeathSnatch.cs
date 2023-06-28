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
    public class DeathSnatch : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Death Snatch", "When [creature] perishes, draw a card from your deck.",
                      typeof(DeathSnatch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair1 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/deathsnatch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/deathsnatch_pixel.png"));

            DeathSnatch.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return Singleton<CardDrawPiles>.Instance.Deck.CardsInDeck > 0;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    CardInfo toQueue = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), false);
                    if (toQueue != null)
                    {
                        PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toQueue);
                        playableCard.SetIsOpponentCard(true);
                        Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                        Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                            Tools.RandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                        Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                    }
                }

            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(null, null);
                if (Tools.GetActAsInt() != 2) Singleton<CardDrawPiles3D>.Instance.Pile.Draw();
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
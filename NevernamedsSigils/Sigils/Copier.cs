using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Copier : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Copier", "When [creature] perishes, a copy of it's murderer is created in your hand.",
                      typeof(Copier),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair3, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/copier.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/copier_pixel.png"));

            Copier.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override CardInfo CardToDraw
        {
            get
            {
                return storedKiller.Info;
            }
        }
        public override List<CardModificationInfo> CardToDrawTempMods
        {
            get
            {
                return null;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        private PlayableCard storedKiller;
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (killer != null && !wasSacrifice && !killer.Info.traits.Contains(Trait.Uncuttable))
            {
                if (base.Card.OpponentCard)
                {
                    if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                    {
                        PlayableCard playableCard = CardSpawner.SpawnPlayableCard(killer.Info);
                        playableCard.SetIsOpponentCard(true);
                        Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                        Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                            Tools.RandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                        Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                    }

                }
                else
                {
                    storedKiller = killer;
                    yield return base.PreSuccessfulTriggerSequence();
                    yield return base.CreateDrawnCard();
                    yield return base.LearnAbility(0.5f);
                }
            }
            yield break;
        }
    }
}

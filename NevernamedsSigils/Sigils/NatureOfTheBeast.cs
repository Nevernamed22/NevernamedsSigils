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
    public class NatureOfTheBeast : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Nature Of The Beast", "When [creature] perishes, a completely random creature is created in the owner's hand.",
                      typeof(NatureOfTheBeast),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/natureofthebeast.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/natureofthebeast_pixel.png"));

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
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
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
                        Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }

            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.CreateDrawnCard();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        public override CardInfo CardToDraw
        {
            get
            {

                CardInfo deathcard = CustomDeathcardPortrait.CompletelyRandomAnimalDeathcard((Tools.GetActAsInt() == 3 ? 0 : 2) + Singleton<TurnManager>.Instance.TurnNumber);
                deathcard.mods.Add(base.Card.CondenseMods(new List<Ability>() { NatureOfTheBeast.ability }, true));
                return deathcard;
            }
        }
    }
}
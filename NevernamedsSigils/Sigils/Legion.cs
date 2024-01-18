using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Legion : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Legion", "When [creature] is struck, it creates a number of cards in the owner's hand equal to its remaining HP, then perishes.",
                      typeof(Legion),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/legion.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/legion_pixel.png"));

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
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return base.Card && base.Card.NotDead();
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            mods = base.Card.CondenseMods(new List<Ability>() { Legion.ability }, true);
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < base.Card.Health; i++)
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
                    yield return base.CreateDrawnCard();
                }
            }
            yield return base.Card.Die(false);

            yield return base.LearnAbility(0f);
        }
        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo guts = (base.Card.Info.GetExtendedProperty("LegionCardOverride") != null) ? CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("LegionCardOverride")) : CardLoader.GetCardByName("SigilNevernamed SpiritBeast");
                if (mods != null) guts.Mods.Add(mods);
                return guts;
            }
        }
        private CardModificationInfo mods;
    }
}

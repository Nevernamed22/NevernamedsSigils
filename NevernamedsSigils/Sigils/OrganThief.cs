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
    public class OrganThief : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Organ Thief", "When [creature] kills another creature, its remains are created in your hand.",
                      typeof(OrganThief),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.Part2Modular, Plugin.GrimoraModChair1 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/organthief.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/organthief_pixel.png"));

            OrganThief.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card;
        }
        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo guts = null;
                if ((base.Card.Info.GetExtendedProperty("OrganThiefGutOverride") != null))
                {
                    guts = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("OrganThiefGutOverride"));
                }
                else
                {
                    switch (Tools.GetActAsInt())
                    {
                        case 3:
                            guts = CardLoader.GetCardByName("SigilNevernamed Components");
                            break;
                        case 4:
                            guts = CardLoader.GetCardByName("SigilNevernamed GutsGrimora");
                            break;
                        default:
                            guts = CardLoader.GetCardByName("SigilNevernamed Guts");
                            break;
                    }
                }
                if (lastKilled != null) guts.Mods.Add(lastKilled);
                return guts;
            }
        }
        private CardModificationInfo lastKilled;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            lastKilled = card.CondenseMods();
            yield return new WaitForSeconds(0.3f);

            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardToDraw);
                    playableCard.SetIsOpponentCard(true);
                    if (lastKilled != null) playableCard.AddTemporaryMod(lastKilled);

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


            if (!base.Card.Dead)
            {
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

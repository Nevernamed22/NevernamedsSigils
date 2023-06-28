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
    public class GutSpewer : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gut Spewer", "When [creature] is played, its innards are created in your hand.",
                      typeof(GutSpewer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair1, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gutspewer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/gutspewer_pixel.png"));

            GutSpewer.ability = newSigil.ability;
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
                CardInfo guts = null;
                if ((base.Card.Info.GetExtendedProperty("GutSpewerGutOverride") != null))
                {
                    guts = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("GutSpewerGutOverride"));
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
                if (base.Card != null)
                {
                    List<Ability> abilities = base.Card.Info.Abilities;
                    foreach (CardModificationInfo cardModificationInfo in base.Card.TemporaryMods)
                    {
                        abilities.AddRange(cardModificationInfo.abilities);
                    }
                    abilities.RemoveAll((Ability x) => x == GutSpewer.ability || x == ExplodingCorpseCustom.ability);

                    if (abilities.Count > 0)
                    {
                        CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
                        cardModificationInfo2.fromCardMerge = true;
                        cardModificationInfo2.abilities = abilities;
                        guts.Mods.Add(cardModificationInfo2);
                    }
                }
                return guts;
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
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

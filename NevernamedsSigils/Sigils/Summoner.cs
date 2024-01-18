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
    public class Summoner : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Summoner", "When [creature] is played, its owner may choose from a selection of random cards to add to their hand.",
                      typeof(Summoner),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/summoner.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/summoner_pixel.png"));

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
                    CardInfo toQueue = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), base.Card.Info.GetExtendedProperty("SummonerGivesRareCards") != null).Clone() as CardInfo;
                    if (toQueue != null)
                    {
                        if (base.Card.Info.GetExtendedProperty("SummonerAdoptsMods") != null)
                        {
                            toQueue.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Summoner.ability }));
                        }
                        if (NumberOfSigils > 0)
                        {
                            for (int i = 0; i < NumberOfSigils; i++)
                            {
                                CardModificationInfo inf = new CardModificationInfo();
                                inf.abilities.Add(Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, toQueue, new List<Ability>() { Summoner.ability }));
                                toQueue.Mods.Add(inf);
                            }
                        }

                        PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toQueue);
                        playableCard.SetIsOpponentCard(true);

                        Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                        Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                            Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                        Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                    }
                }


            }
            else
            {


                if (Tools.GetActAsInt() == 1)
                {
                    List<CardInfo> beastCards = CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature);
                    if (base.Card.Info.GetExtendedProperty("SummonerGivesRareCards") != null) beastCards = CardLoader.GetUnlockedCards(CardMetaCategory.Rare, CardTemple.Nature);
                    if (beastCards.Count >= 0)
                    {
                        List<CardInfo> choices = CardLoader.GetDistinctCardsFromPool(SaveManager.SaveFile.GetCurrentRandomSeed() + (timesActivated * 100), Math.Min(beastCards.Count, NumberOfOptions), beastCards, NumberOfSigils, false);
                        if (choices.Count > 0)
                        {
                            if (base.Card.Info.GetExtendedProperty("SummonerAdoptsMods") != null)
                            {
                                foreach (CardInfo info in choices) { info.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Summoner.ability })); }
                            }

                            yield return base.PreSuccessfulTriggerSequence();
                            yield return SpecialCardSelectionHandler.DoSpecialCardSelectionDraw(choices);

                            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                        }
                        else { base.Card.Anim.StrongNegationEffect(); }
                        yield return base.LearnAbility(0.25f);
                    }
                }
                else if (Tools.GetActAsInt() == 3)
                {
                    List<CardInfo> techCards = CardLoader.GetUnlockedCards(CardMetaCategory.Part3Random, CardTemple.Tech);
                    if (base.Card.Info.GetExtendedProperty("SummonerGivesRareCards") != null)
                    {
                        techCards.RemoveAll(x => !x.metaCategories.Contains(CardMetaCategory.Rare));
                    }
                    else
                    {
                        techCards.RemoveAll(x => x.metaCategories.Contains(CardMetaCategory.Rare));
                    }
                    if (techCards.Count >= 0)
                    {
                        List<CardInfo> choices = CardLoader.GetDistinctCardsFromPool(SaveManager.SaveFile.GetCurrentRandomSeed() + (timesActivated * 100), Math.Min(techCards.Count, NumberOfOptions), techCards, NumberOfSigils, false);
                        if (choices.Count > 0)
                        {
                            if (base.Card.Info.GetExtendedProperty("SummonerAdoptsMods") != null)
                            {
                                foreach (CardInfo info in choices) { info.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Summoner.ability })); }
                            }

                            yield return base.PreSuccessfulTriggerSequence();
                            yield return SpecialCardSelectionHandler.DoSpecialCardSelectionDraw(choices);

                            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                        }
                        else { base.Card.Anim.StrongNegationEffect(); }
                        yield return base.LearnAbility(0.25f);
                    }
                }
            }
            timesActivated++;
        }
        public static int timesActivated = 0;
        private int NumberOfOptions
        {
            get
            {
                int num = 3;
                if (base.Card.Info.GetExtendedProperty("NumberOfSummonerOptions") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("NumberOfSummonerOptions"), out num);
                    num = succeed ? num : 3;
                }
                return num;
            }
        }
        private int NumberOfSigils
        {
            get
            {
                int num = 0;
                if (base.Card.Info.GetExtendedProperty("NumberOfSummonerAddedSigils") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("NumberOfSummonerAddedSigils"), out num);
                    num = succeed ? num : 0;
                }
                return num;
            }
        }
    }
}

using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GraveyardHandler;

namespace NevernamedsSigils
{
    public class Exhume : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Exhume", "When [creature] is played, its owner may search their graveyard for a card, and add that card to their hand.",
                      typeof(Exhume),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/exhume.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/exhume_pixel.png"));

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
            return true && GraveyardManager.instance != null;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null) && GraveyardManager.instance.opponentGraveyard.Count > 0)
                {
                    CardInfo toSpawn = Tools.SeededRandomElement<CardInfo>(GraveyardManager.instance.opponentGraveyard, GetRandomSeed());
                    GraveyardManager.instance.opponentGraveyard.Remove(toSpawn);
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toSpawn);
                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }
            }
            else
            {
                List<CardInfo> options = new List<CardInfo>();
                options.AddRange(GraveyardManager.instance.playerGraveyard);
                if (options.Count > 0)
                {
                    yield return base.PreSuccessfulTriggerSequence();

                    if (Tools.GetActAsInt() == 2)
                    {
                        CardInfo selectedCard = null;
                        yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                        {
                            selectedCard = c;
                        }, options);
                        if (selectedCard != null)
                        {
                            GraveyardManager.instance.playerGraveyard.Remove(selectedCard);
                            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(selectedCard, 0.25f);
                        }
                        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    }
                    else
                    {
                        CardInfo selectedCard = null;
                        yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                        {
                            selectedCard = c;
                        }, options);
                        if (selectedCard != null)
                        {
                            GraveyardManager.instance.playerGraveyard.Remove(selectedCard);
                            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(selectedCard, 0.25f);
                        }
                        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    }
                }
            }
        }
    }
}
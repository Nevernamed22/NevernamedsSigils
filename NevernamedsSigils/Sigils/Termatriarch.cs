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
    public class Termatriarch : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Termatriarch", "While [creature] and a different card bearing the Termite King sigil are alive and on the board, a termite will be created in your hand at the start of your turn.",
                      typeof(Termatriarch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/termatriarch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/termatriarch_pixel.png"));

            Termatriarch.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard && base.Card.OnBoard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard));
            foreach (CardSlot slot in availableSlots.FindAll((CardSlot x) => x.Card != null && x.Card != base.Card && x.Card.HasAbility(TermiteKing.ability)))
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                CardInfo termite = CardLoader.GetCardByName("SigilNevernamed Termite");
                termite.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Termatriarch.ability, TermiteKing.ability }));
                termite.Mods.Add(slot.Card.CondenseMods(new List<Ability>() { Termatriarch.ability, TermiteKing.ability }));

                if (base.Card.OpponentCard)
                {
                    if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                    {
                        PlayableCard playableCard = CardSpawner.SpawnPlayableCard(termite);
                        playableCard.SetIsOpponentCard(true);
                        Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                        Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                            Tools.RandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                        Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                    }

                }
                else
                {
                    yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(termite);
                }
                yield return new WaitForSeconds(0.45f);
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}

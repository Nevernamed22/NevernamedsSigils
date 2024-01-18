using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;

namespace NevernamedsSigils
{
    public class LootConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Loot Conduit", "Completes a circuit. At the start of the owner's turn, [creature] will draw 1 card to the owners hand for every empty space in the circuit it completes.",
                      typeof(LootConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/lootconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/lootconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] draws a card for every empty space in its circuit!");

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            int num = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => x.Card == null && Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card)).Count;

            if (num > 0)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();


                if (base.Card.OpponentCard)
                {
                    for (int i = 0; i < num; i++)
                    {
                        if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                        {
                            yield return base.PreSuccessfulTriggerSequence();
                            PlayableCard playableCard = CardSpawner.SpawnPlayableCard(Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), false, Tribe.None, false).Clone() as CardInfo);
                            playableCard.SetIsOpponentCard(true);
                            Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                            Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                                Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                            Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                        }
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                else
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    for (int i = 0; i < num; i++)
                    {
                        yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(null, null);
                        if (Tools.GetActAsInt() != 2)
                        {
                            Singleton<CardDrawPiles3D>.Instance.Pile.Draw();
                        }
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield break;
        }

    }
}

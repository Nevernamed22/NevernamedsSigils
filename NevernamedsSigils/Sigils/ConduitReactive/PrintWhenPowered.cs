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
    public class PrintWhenPowered : AbilityBehaviour
    {
        new public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Print When Powered", "If [creature] is inside a completed circuit, a gem will be created in its owners hand at the start of their turn.",
                      typeof(PrintWhenPowered),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/printwhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/printwhenpowered_pixel.png"),
                      isConduitCell: true,
                      triggerText: "[creature] prints a mox gem for its owner!");

            ability = newSigil.ability;
        }
        new public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep && Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {

            List<string> moxes = new List<string>() { "MoxEmerald", "MoxRuby", "MoxSapphire" };
            if (Tools.GetActAsInt() == 3) { moxes = new List<string>() { "EmptyVessel_BlueGem", "EmptyVessel_GreenGem", "EmptyVessel_OrangeGem" }; }

            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardLoader.GetCardByName(Tools.SeededRandomElement(moxes)));
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
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(Tools.SeededRandomElement(moxes)), null, 0.25f);
            }
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
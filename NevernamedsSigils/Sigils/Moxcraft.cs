using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class Moxcraft : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Moxcraft", "When [creature] is struck, a random mox is created in your hand.",
                      typeof(Moxcraft),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/moxcraft_pixel.png"));

            Moxcraft.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();

                List<string> moxes = new List<string>() { "MoxEmerald", "MoxRuby", "MoxSapphire" };
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
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
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(Tools.SeededRandomElement(moxes)), null, 0.25f);
            }
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
    using DiskCardGame;
    using InscryptionAPI.Card;
    using InscryptionAPI.Dialogue;
    using System.Collections;
    using UnityEngine;

    namespace NevernamedsSigils
    {
        public class PoopDeck : SpecialCardBehaviour
        {
            public static SpecialTriggeredAbility ability;
            public static void Init()
            {
                ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "PoopDeck", typeof(PoopDeck)).Id;

                DialogueManager.GenerateEvent(Plugin.PluginGuid, "RoyalPoopDeck",
                new List<CustomLine>()
                {
                    new CustomLine()
                    {
                         emotion = Emotion.Neutral,
                         letterAnimation = TextDisplayer.LetterAnimation.Jitter,
                         speaker = DialogueEvent.Speaker.PirateSkull,
                         text = "Ye filthy dog! Now one o' me maties needs to swab the poop deck!"
                    }
                }, null, DialogueEvent.MaxRepeatsBehaviour.PlayLastLine, DialogueEvent.Speaker.PirateSkull);
            }
            public override bool RespondsToResolveOnBoard()
            {
                return Singleton<TurnManager>.Instance != null && 
                Singleton<TurnManager>.Instance.opponent != null &&
                Singleton<TurnManager>.Instance.opponent.OpponentType == Opponent.Type.PirateSkullBoss &&
                base.PlayableCard.slot != null && 
                base.PlayableCard.slot.opposingSlot.Card == null;
            }
            public override IEnumerator OnResolveOnBoard()
            {
                yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("RoyalPoopDeck", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("SigilNevernamed Swabber").Clone() as CardInfo, base.PlayableCard.slot.opposingSlot, 0.1f, true);
                yield break;
            }
        }
    }


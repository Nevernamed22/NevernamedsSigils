using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Wingrider : AbilityBehaviour, IOnOtherCardResolveInHand
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Wingrider", "If a friendly airborne creature is played while [creature] is in the owner's hand, this card will be played adjacent to it.",
                      typeof(Wingrider),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/wingrider.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/wingrider_pixel.png"));

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
        public bool RespondsToOtherCardResolveInHand(PlayableCard resolvingCard)
        {
            return resolvingCard && !resolvingCard.OpponentCard && resolvingCard.HasAbility(Ability.Flying);
        }
        public IEnumerator OnOtherCardResolveOpponentQueue(PlayableCard resolvingCard)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(resolvingCard.slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(resolvingCard.slot, false);
            if (toLeft != null && toLeft.Card == null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.QueuedSlot = null;
                base.Card.OnPlayedFromOpponentQueue();
                yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(base.Card, toLeft, 0.1f, null, true);
                Singleton<TurnManager>.Instance.Opponent.Queue.Remove(base.Card);
                yield return base.LearnAbility(0.5f);
            }
            else if (toRight != null && toRight.Card == null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.QueuedSlot = null;
                base.Card.OnPlayedFromOpponentQueue();
                yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(base.Card, toLeft, 0.1f, null, true);
                Singleton<TurnManager>.Instance.Opponent.Queue.Remove(base.Card);
                yield return base.LearnAbility(0.5f);
            }
            yield break;
        }
        public IEnumerator OnOtherCardResolveInHand(PlayableCard resolvingCard)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(resolvingCard.slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(resolvingCard.slot, false);
            if (toLeft != null && toLeft.Card == null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(base.Card, toLeft);
                yield return base.LearnAbility(0.5f);
            }
           else if (toRight != null && toRight.Card == null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(base.Card, toRight);
                yield return base.LearnAbility(0.5f);
            }
            yield break;
        }
    }
}
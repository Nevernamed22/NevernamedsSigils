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
    public class DivisibilityStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Divisibility Strike", "At the end of each turn, [creature] will alternate between striking all odd indexed opponent slots, and all even indexed opponent slots.",
                      typeof(DivisibilityStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/divisibilitystrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/divisibilitystrike_pixel.png"));

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
        private bool flipped;
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return playerTurnEnd != base.Card.OpponentCard;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            base.Card.Anim.NegationEffect(true);
            flipped = !flipped;
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, flipped);
            base.Card.RenderCard();
            yield break;
        }
        public override bool RespondsToGetOpposingSlots()
        {
            return true;
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return true;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            if (flipped)
            {
                return (base.Card.OpponentCard ? BoardManager.Instance.GetSlots(true) : BoardManager.Instance.GetSlots(false)).FindAll((CardSlot x) => !x.Index.isEven());
            }
            return (base.Card.OpponentCard ? BoardManager.Instance.GetSlots(true) : BoardManager.Instance.GetSlots(false)).FindAll((CardSlot x) => x.Index.isEven());
        }
    }
}
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class SwitchStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Switch Strike", "At the end of its owners turn [creature] will alternate between striking to the left or right of the opposing slot when it attacks, in addition to striking the opposing slot.",
                      typeof(SwitchStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.BountyHunter, Plugin.GrimoraModChair3, AbilityMetaCategory.GrimoraRulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/switchstrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/switchstrike_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public bool isLeft = true;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return playerTurnEnd != base.Card.OpponentCard;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            base.Card.Anim.NegationEffect(false);
            isLeft = !isLeft;
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, !isLeft);
            base.Card.RenderCard();
            yield break;
        }
        public override bool RespondsToGetOpposingSlots()
        {
            return true;
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return false;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> toReturn = new List<CardSlot>() { };
            if (base.Card.slot && base.Card.slot.opposingSlot != null)
            {
                if (isLeft && Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, true)) { toReturn.Add(Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, true)); }
                else if (!isLeft && Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, false)) { toReturn.Add(Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, false)); }
            }
            return toReturn;
        }
    }
}
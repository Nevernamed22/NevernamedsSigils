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
    public class FringeStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Fringe Strike", "[creature] will always attack the leftmost and rightmost opponent lanes.",
                      typeof(FringeStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/fringestrike.png"),
                      pixelTex: null);

            FringeStrike.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
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
            List<CardSlot> opposingslots = base.Card.OpponentCard ? BoardManager.Instance.GetSlots(true) : BoardManager.Instance.GetSlots(false);
            return new List<CardSlot>() { opposingslots[0], opposingslots[opposingslots.Count - 1] };
        }
    }
}
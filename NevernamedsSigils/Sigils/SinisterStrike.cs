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
    public class SinisterStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sinister Strike", "[creature] will also strike the slot to the left of the opposing slot when it attacks.",
                      typeof(SinisterStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/sinisterstrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/sinisterstrike_pixel.png"));

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
        public override bool RespondsToGetOpposingSlots()
        {
            return base.Card.slot && base.Card.slot.opposingSlot && Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, true);
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return false;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            return new List<CardSlot>() { Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, true) };
        }
    }
}
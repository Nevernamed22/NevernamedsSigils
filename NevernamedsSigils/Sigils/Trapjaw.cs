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
    public class Trapjaw : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trapjaw", "[creature] will attack to the left and the right of the opposing space if those slots are obstructed. If those slots are not obstructed, the strikes will be transferred to the directly opposing space.",
                      typeof(Trapjaw),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/trapjaw.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/trapjaw_pixel.png"));

            Trapjaw.ability = newSigil.ability;
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
            CardSlot opposingSlot = base.Card.slot.opposingSlot;
            if (opposingSlot)
            {
                List<CardSlot> targetSlots = new List<CardSlot>();
                CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true);
                CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false);
                bool toLeftValid = toLeft != null && toLeft.Card != null;
                bool toRightValid = toRight != null && toRight.Card != null;

                if (toLeftValid) targetSlots.Add(toLeft);
                else targetSlots.Add(opposingSlot);

                if (toRightValid) targetSlots.Add(toRight);
                else targetSlots.Add(opposingSlot);

                return targetSlots;
            }
            return new List<CardSlot>() { };
        }
    }
}
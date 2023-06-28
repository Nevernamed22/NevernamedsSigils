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
    public class QuadStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Quad Strike", "[creature] will strike each opposing space to the left and right of the spaces across from it as well as striking the space in front of it an additional time.",
                      typeof(QuadStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/quadstrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/quadstrike_pixel.png"));

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
            return true;
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return false;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> slots = new List<CardSlot>();
            CardSlot baseslotOp = base.Card.slot.opposingSlot;

            if (Singleton<BoardManager>.Instance.GetAdjacent(baseslotOp, true) != null) slots.Add(Singleton<BoardManager>.Instance.GetAdjacent(baseslotOp, true));
            slots.Add(baseslotOp);
            if (Singleton<BoardManager>.Instance.GetAdjacent(baseslotOp, false) != null) slots.Add(Singleton<BoardManager>.Instance.GetAdjacent(baseslotOp, false));

            return slots;
        }
    }
}
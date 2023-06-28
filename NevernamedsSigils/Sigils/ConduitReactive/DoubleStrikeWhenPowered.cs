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
    public class DoubleStrikeWhenPowered : ExtendedAbilityBehaviour
    {
        new public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Double Strike When Powered", "If [creature] is within a circuit, it will strike the opposing space an additional time when it attacks.",
                      typeof(DoubleStrikeWhenPowered),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular, AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/doublestrikewhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/doublestrikewhenpowered_pixel.png"),
                      isConduitCell: true);

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
        public override bool RespondsToGetOpposingSlots()
        {
            return Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return false;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> toReturn = new List<CardSlot>() { };
            CardSlot opposingSlot = base.Card.slot.opposingSlot;
            if (opposingSlot != null) { toReturn.Add(opposingSlot); }
            return toReturn;
        }
    }
}
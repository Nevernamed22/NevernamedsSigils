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
    public class BifurcatedWhenGempowered : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bifurcated When Gempowered", "If its owner has an Orange Gem, [creature] will strike each opposing space to the left and right of the space across from it.",
                      typeof(BifurcatedWhenGempowered),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular, AbilityMetaCategory.MagnificusRulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bifurcatedwhengempowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bifurcatedwhengempowered_pixel.png"));

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
        public override bool RemoveDefaultAttackSlot()
        {
            return base.Card.OwnerHasGem(GemType.Orange);
        }
        public override bool RespondsToGetOpposingSlots()
        {
            return base.Card.OwnerHasGem(GemType.Orange);
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> adjacents = new List<CardSlot>() {  };
            CardSlot opposingSlot = base.Card.slot.opposingSlot;
            if (opposingSlot)
            {
                return Singleton<BoardManager>.Instance.GetAdjacentSlots(opposingSlot);
            }
            return new List<CardSlot>() { };
        }
    }
}
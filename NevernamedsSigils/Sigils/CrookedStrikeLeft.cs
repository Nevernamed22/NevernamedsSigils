using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{  
    public class CrookedStrikeLeft : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Crooked Strike Left", "[creature] will attack the space to the left of its intended target.",
                      typeof(CrookedStrikeLeft),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/crookedstrikeleft.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/crookedstrikeleft_pixel.png"));

            CrookedStrikeLeft.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToGetOpposingSlots()
        {
            return base.Card.HasAbility(CrookedStrikeRight.ability);
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return base.Card.HasAbility(CrookedStrikeRight.ability);
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            if (base.Card.HasAbility(CrookedStrikeRight.ability))
            {
                CardSlot opposingSlot = base.Card.slot.opposingSlot;
                List<CardSlot> targetSlots = new List<CardSlot>();
                CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, true);
                CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(opposingSlot, false);

                if (toLeft != null) targetSlots.Add(toLeft);
                else targetSlots.Add(opposingSlot);

                if (toRight != null) targetSlots.Add(toRight);
                else targetSlots.Add(opposingSlot);
                return targetSlots;
            }
            else return new List<CardSlot>() { };
        }
        
    }
}

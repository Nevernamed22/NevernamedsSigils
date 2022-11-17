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
    public class PinnacleStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Pinnacle Strike", "[creature] will strike all opponent spaces when it attacks.",
                      typeof(PinnacleStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/pinnaclestrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/pinnaclestrike_pixel.png"));

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
            return true;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            return base.Card.OpponentCard ? BoardManager.Instance.GetSlots(true) : BoardManager.Instance.GetSlots(false);            
        }
    }
}
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
    public class EnemyLines : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Enemy Lines", "When [creature] attacks, it will target all opponent slots that it is NOT directly opposing.",
                      typeof(EnemyLines),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/enemylines.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/enemylines_pixel.png"));

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
            List<CardSlot> opposingslots = base.Card.OpponentCard ? BoardManager.Instance.GetSlots(true) : BoardManager.Instance.GetSlots(false);

            return opposingslots.FindAll((CardSlot x) => x.opposingSlot == null || x.opposingSlot.Card == null || x.opposingSlot.Card != base.Card);
        }
    }
}
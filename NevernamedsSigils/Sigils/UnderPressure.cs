using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class UnderPressure : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Under Pressure", "When [creature] perishes, 2 damage is dealt to the opponent creature. Also, when [creature] perishes, any adjacent creatures with this sigil also take ten damage.",
                      typeof(UnderPressure),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/underpressure.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/underpressure_pixel.png"),
                      triggerText: "[creature] pops, dealing 2 damage to the opposing creature!");

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
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return base.Card.OnBoard;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            CardSlot slot = base.Card.Slot;        
            if (slot.opposingSlot && slot.opposingSlot.Card != null)
            {
                yield return PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.2f);
                yield return slot.opposingSlot.Card.TakeDamage(2, null);
            }
            yield return new WaitForSeconds(0.2f);
            List<CardSlot> adjacents = Singleton<BoardManager>.Instance.GetAdjacentSlots(slot);
            if (adjacents != null && adjacents.Count > 0)
            {
                foreach(CardSlot slot2 in adjacents)
                {
                    if(slot2.Card && slot2.Card.Health > 0 && !slot2.Card.Dead && slot2.Card.HasAbility(UnderPressure.ability))
                    {
                        yield return slot2.Card.TakeDamage(10, null);
                    }
                }
            }
            yield break;
        }
        
    }
}
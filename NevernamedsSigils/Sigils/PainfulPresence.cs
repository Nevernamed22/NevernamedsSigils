using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class PainfulPresence : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Painful Presence", "On upkeep, all creatures adjacent to [creature] vertically, horizontally, or diagonally take 1 damage.",
                      typeof(PainfulPresence),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/painfulpresence.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/painfulpresence_pixel.png"));

            PainfulPresence.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card && base.Card.OnBoard && playerUpkeep != base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            List<CardSlot> slots = new List<CardSlot>();
            if (base.Card.slot.opposingSlot)
            {
                slots.Add(base.Card.slot.opposingSlot);
                slots.AddRange(Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot.opposingSlot));
            }
            slots.AddRange(Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot));
            yield return base.PreSuccessfulTriggerSequence();
            foreach (CardSlot slot in slots)
            {
                if (base.Card && base.Card.OnBoard && slot && slot.Card != null)
                {
                    base.Card.Anim.StrongNegationEffect();
                    yield return new WaitForSeconds(0.1f);
                    yield return slot.Card.TakeDamage(1, base.Card);
                    yield return new WaitForSeconds(0.4f);
                }
            }
            yield return base.LearnAbility(0.4f);
            yield break;
        }      
    }
}

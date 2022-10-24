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
    public class Ravenous : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ravenous", "When [creature] kills a creature, it will gain an additional strike against the opposing space until the end of combat.",
                      typeof(Ravenous),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ravenous.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ravenous_pixel.png"));

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

        public int cardsKilled = 0;
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            cardsKilled++;
            yield return base.LearnAbility(0.25f);
        }
        public override bool RespondsToGetOpposingSlots()
        {
            return cardsKilled > 0;
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return false;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> slots = new List<CardSlot>() { };
            for (int i =0; i < cardsKilled; i++) { if (base.Card.slot && base.Card.slot.opposingSlot) { slots.Add(base.Card.slot.opposingSlot); } }
            return slots;
        }
    }
}

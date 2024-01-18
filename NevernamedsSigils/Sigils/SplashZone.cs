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
    public class SplashZone : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Splash Zone", "When [creature] is struck, the creatures to its left or right are also dealt 1 damage.",
                      typeof(SplashZone),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/splashzone.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/splashzone_pixel.png"),
                      triggerText: "The damage dealt to [creature] splashes out to its comrades."
                      );

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
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return base.Card && base.Card.slot;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            List<CardSlot> adjacents = Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot);
            if (adjacents.Exists(x => x.Card != null))
            {
                yield return base.PreSuccessfulTriggerSequence();
            }
            foreach (CardSlot slot in adjacents)
            {
                if (slot.Card != null && slot.Card.Health > 0)
                {
                    yield return slot.Card.TakeDamage(1, source);
                }
            }
            yield break;
        }      
    }
}
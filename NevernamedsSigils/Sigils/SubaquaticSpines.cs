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
    public class SubaquaticSpines : Submerge
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Subaquatic Spines", "[creature] will submerge during the opponent's turn. While submerged, opposing creatures attack it's owner directly, and will take 1 damage after attacking.",
                      typeof(SubaquaticSpines),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/subaquaticspines.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/subaquaticspines_pixel.png"));

            SubaquaticSpines.ability = newSigil.ability;
        }

        public static Ability ability;
        //override respondstodeal
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Health > 0; 
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            yield return source.TakeDamage(1, base.Card);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}
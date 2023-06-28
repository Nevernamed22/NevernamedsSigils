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
    public class Toxic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Toxic", "When [creature] is struck but does not perish, the striker perishes. Does not affect Made of Stone cards.",
                      typeof(Toxic),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/toxic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/toxic_pixel.png"),
                      triggerText: "[creature]s toxicity kills instantly!");

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
            return source != null && source.Health > 0 && !source.Dead && !source.HasAbility(Ability.MadeOfStone);
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (base.Card.Health > 0 && !base.Card.Dead)
            {
                yield return base.PreSuccessfulTriggerSequence();
                source.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.15f);
                yield return source.Die(false, base.Card, true);
                yield return base.LearnAbility(0.4f);
                yield break;
            }
        }
    }
}

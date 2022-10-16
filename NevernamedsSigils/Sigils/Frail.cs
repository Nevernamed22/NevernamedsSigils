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
    public class Frail : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Frail", "When [creature] is struck, it will perish.",
                      typeof(Frail),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/frail.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/frail_pixel.png"));

            Frail.ability = newSigil.ability;
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
            return base.Card.Health > 0 && !base.Card.Dead;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            yield return base.Card.Die(false);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

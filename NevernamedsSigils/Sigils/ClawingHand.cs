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
    public class ClawingHand : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Clawing Hand", "When [creature] is struck, the striker is dealt damage equal to the number of cards in the owner's hand.",
                      typeof(ClawingHand),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/clawinghand.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/clawinghand_pixel.png"));

            ClawingHand.ability = newSigil.ability;
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
            return source != null && source.Health > 0 && Singleton<PlayerHand>.Instance.CardsInHand.Count > 0;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            yield return source.TakeDamage(Singleton<PlayerHand>.Instance.CardsInHand.Count, base.Card);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

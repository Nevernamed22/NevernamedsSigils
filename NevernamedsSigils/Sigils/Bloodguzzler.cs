using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Bloodguzzler : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Blood Guzzler", "When [creature] deals damage to a creature, it gains 1 Health for each damage dealt.",
                      typeof(Bloodguzzler),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodguzzler.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bloodguzzler_pixel.png"),
                      triggerText: "[creature] consumes the blood of its prey!");

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
        public override bool RespondsToDealDamage(int amount, PlayableCard target)
        {
            return base.Card.NotDead() && amount > 0;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            yield return PreSuccessfulTriggerSequence();
            base.Card.AddTemporaryMod(new CardModificationInfo(0, amount));
            Card.OnStatsChanged();
            Card.Anim.StrongNegationEffect();
            base.Card.RenderCard();
            yield return new WaitForSeconds(0.25f);
            yield return LearnAbility(0.25f);
        }
    }
}
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
    public class Regenerator : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Regenerator", "At the start of the owners turn, [creature] will heal 1 health.",
                      typeof(Regenerator),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, Plugin.GrimoraModChair1, Plugin.Part2Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/regenerator.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/regenerator_pixel.png"),
                      triggerText: "[creature] regenerates one health!");

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard && base.Card.OnBoard && base.Card.Status.damageTaken > 0;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            base.Card.HealDamage(1);
            base.Card.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}
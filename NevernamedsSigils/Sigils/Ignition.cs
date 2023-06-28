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
    public class Ignition : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ignition", "When [creature] strikes a creature, that creature gains the Burning sigil.",
                      typeof(Ignition),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ignition.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ignition_pixel.png"));

            Ignition.ability = newSigil.ability;
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
            return true;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            if (!target.HasAbility(Ability.MadeOfStone) && !target.HasAbility(Burning.ability))
            {
                yield return base.PreSuccessfulTriggerSequence();
                CardModificationInfo fire = new CardModificationInfo();
                fire.abilities.Add(Burning.ability);
                target.AddTemporaryMod(fire);
                target.RenderCard();
            }
            yield break;
        }
    }
}

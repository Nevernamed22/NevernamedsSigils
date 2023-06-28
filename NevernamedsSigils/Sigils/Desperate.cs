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
    public class Desperate : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Desperate", "When [creature] perishes, it deals one point of damage to the opponent.",
                      typeof(Desperate),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/desperate.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/desperate_pixel.png"),
                      triggerText: "[creature] deals a free point of damage as it perishes."
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
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, !base.Card.slot.IsPlayerSlot, 0.25f, null, 0f, true);
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}
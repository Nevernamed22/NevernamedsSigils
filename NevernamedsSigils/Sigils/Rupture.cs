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
    public class Rupture : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Rupture", "When [creature] deals damage to a creature, half of that damage, rounded down, is also dealt to that creature's owner.",
                      typeof(Rupture),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/rupture.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/rupture_pixel.png"));

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
            return amount > 0;
        }

        public override IEnumerator OnDealDamage(int amount, PlayableCard target)
        {
            int amount2 = Mathf.FloorToInt((float)amount * 0.5f);
            if (amount2 > 0)
            {
                yield return PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.1f);
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(amount2, amount2, !target.OpponentCard, 0.25f, null, 0f, true);
                yield return new WaitForSeconds(0.1f);
                yield return LearnAbility(0.25f);
            }
        }       
    }
}
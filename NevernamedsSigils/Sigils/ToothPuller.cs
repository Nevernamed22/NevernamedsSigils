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
    public class ToothPuller : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Tooth Puller", "At the end of the owner's turn, [creature] will add one damage to the scales in the owner's favour, regardless of it's attack power or obstruction.",
                      typeof(ToothPuller),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/toothpuller.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/toothpuller_pixel.png"));

            ToothPuller.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card.slot.IsPlayerSlot == playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            yield return new WaitForSeconds(0.15f);
            yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, !base.Card.slot.IsPlayerSlot, 0.25f, null, 0f, true);
            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

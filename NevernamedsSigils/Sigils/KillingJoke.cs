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
    public class KillingJoke : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Killing Joke", "While [creature] is on the board, all friendly creatures which cost an orange gem will either gain +1 power or -1 power when played.",
                      typeof(KillingJoke),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/killingjoke.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/killingjoke_pixel.png"),
                      triggerText: "[creature] randomly modifies the power of the Orange Gem cost card!"
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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard != base.Card && otherCard.OpponentCard == base.Card.OpponentCard && otherCard.Info && otherCard.Info.gemsCost != null && otherCard.Info.gemsCost.Contains(GemType.Orange);
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
                yield return base.PreSuccessfulTriggerSequence();
            otherCard.temporaryMods.Add(new CardModificationInfo(UnityEngine.Random.value <= 0.5f ? 1 : -1, 0));
            yield break;
        }

    }
}
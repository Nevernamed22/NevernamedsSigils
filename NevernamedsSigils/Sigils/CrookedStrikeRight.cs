using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class CrookedStrikeRight : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Crooked Strike Right", "[creature] will attack the space to the right of its intended target.",
                      typeof(CrookedStrikeRight),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/crookedstrikeright.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/crookedstrikeright_pixel.png"));

            CrookedStrikeRight.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
    }
}

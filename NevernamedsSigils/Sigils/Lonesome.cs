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
    public class Lonesome : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Lonesome", "[creature] gains +1 power for each adjacent empty space.",
                      typeof(Lonesome),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/lonesome.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/lonesome_pixel.png"));

            Lonesome.ability = newSigil.ability;
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

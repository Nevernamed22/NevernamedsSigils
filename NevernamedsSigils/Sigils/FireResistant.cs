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
    public class FireResistant : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Fire Resistant", "[creature] will not take damage from the Burning sigil.",
                      typeof(FireResistant),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/fireresistant.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/fireresistant_pixel.png"));

            FireResistant.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}

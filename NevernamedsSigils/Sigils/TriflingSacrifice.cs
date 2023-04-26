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
    public class TriflingSacrifice : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trifling Sacrifice", "[creature] counts as 2 Blood rather than 1 Blood when sacrificed.",
                      typeof(TriflingSacrifice),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/triflingsacrifice.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/triflingsacrifice_pixel.png"));

            TriflingSacrifice.ability = newSigil.ability;
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
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
    public class MeaninglessBond : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Meaningless Bond", "[creature] may complete a bond, but provides no additional effect.",
                      typeof(MeaninglessBond),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/meaninglessbond.png"),
                      pixelTex: null,
                      isConduit: true);

            MeaninglessBond.ability = newSigil.ability;
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

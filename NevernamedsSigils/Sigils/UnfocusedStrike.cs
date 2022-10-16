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
    public class UnfocusedStrike : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Unfocused Strike", "[creature] will target a random space when it attacks.",
                      typeof(UnfocusedStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/unfocusedstrike.png"),
                      pixelTex: null);

            UnfocusedStrike.ability = newSigil.ability;
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

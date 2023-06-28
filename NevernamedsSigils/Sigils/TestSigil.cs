using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class TestSigil : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("TestSigil", "[creature] will most likely fuck shit up.",
                      typeof(TestSigil),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: -4,
                      stackable: false,
                      opponentUsable: false,
                      null, null);

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
    }
}
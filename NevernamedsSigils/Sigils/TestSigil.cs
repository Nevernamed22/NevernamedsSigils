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
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/debugblacksquare.png")
                      );

            ability = newSigil.ability;

            TestSigil2.Init();
            TestSigil3.Init();
            TestSigil4.Init();
            TestSigil5.Init();
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
    public class TestSigil2 : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("TestSigil2", "[creature] will most likely fuck shit up.",
                      typeof(TestSigil2),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/debugblacksquare.png"));

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
    public class TestSigil3 : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("TestSigil3", "[creature] will most likely fuck shit up.",
                      typeof(TestSigil3),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/debugblacksquare.png"));

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
    public class TestSigil4 : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("TestSigil4", "[creature] will most likely fuck shit up.",
                      typeof(TestSigil4),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/debugblacksquare.png"));

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
    public class TestSigil5 : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("TestSigil5", "[creature] will most likely fuck shit up.",
                      typeof(TestSigil5),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/debugblacksquare.png"));

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
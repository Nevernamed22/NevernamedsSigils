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
    public class Reflective : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Reflective", "When [creature] is opposed by a creature, it gains power equal to the opposing creatures power.",
                      typeof(Reflective),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook,  AbilityMetaCategory.GrimoraRulebook, AbilityMetaCategory.MagnificusRulebook, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair3 },
                      powerLevel:4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/reflective.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/reflective_pixel.png"));

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
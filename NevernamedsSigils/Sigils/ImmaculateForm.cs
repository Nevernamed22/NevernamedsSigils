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
    public class ImmaculateForm : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Immaculate Form", "While [creature] is on the board, the gems cost of all friendly creatures is reduced by 2 gems.",
                      typeof(ImmaculateForm),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/immaculateform.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/immaculateform_pixel.png"));

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
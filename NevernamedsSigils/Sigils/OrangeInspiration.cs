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
    public class OrangeInspiration : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Orange Inspiration", "While [creature] is on the board, all friendly cards which cost an orange gem gain +1 power.",
                      typeof(OrangeInspiration),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/orangeinspiration.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/orangeinspiration_pixel.png"));

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
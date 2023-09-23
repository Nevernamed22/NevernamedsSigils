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
    public class PerfectForm : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Perfect Form", "While [creature] is on the board, the gems cost of all friendly creatures is reduced by 1 gem.",
                      typeof(PerfectForm),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/perfectform.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/perfectform_pixel.png"));

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
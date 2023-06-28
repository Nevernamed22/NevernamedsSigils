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
    public class RepulsiveWhenPowered : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Repulsive When Powered", "While [creature] is within a conduit, if another creature would strike it, it does not.",
                      typeof(RepulsiveWhenPowered),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/repulsivewhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/repulsivewhenpowered_pixel.png"),
                      isConduitCell: true);

            RepulsiveWhenPowered.ability = newSigil.ability;
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
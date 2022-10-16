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
    public class TemptingTarget : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Tempting Target", "If an opponent creature would strike a space adjacent to [creature], it will instead strike [creature].",
                      typeof(TemptingTarget),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/temptingtarget.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/temptingtarget_pixel.png"));

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
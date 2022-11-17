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
    public class TermiteKing : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Termite King", "While [creature] and a different card bearing the Termatriarch sigil are alive and on the board, a termite will be created in your hand at the start of your turn.",
                      typeof(TermiteKing),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/termiteking.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/termiteking_pixel.png"));

            TermiteKing.ability = newSigil.ability;
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

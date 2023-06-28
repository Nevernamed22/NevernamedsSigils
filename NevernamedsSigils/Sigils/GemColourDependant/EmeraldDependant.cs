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
    public class EmeraldDependant : GemColourDependant
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Emerald Dependant", "If [creature]'s owner controls no green mox gems, it will perish.",
                      typeof(EmeraldDependant),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: -4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/emeralddependant.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/emeralddependant_pixel.png"),
                      triggerText: "[creature] requires an emerald mox to exist and perishes.");

            ability = newSigil.ability;
        }
        public override bool reqEmerald => true;
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
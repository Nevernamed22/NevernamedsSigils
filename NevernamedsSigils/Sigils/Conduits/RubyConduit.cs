using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;
using GBC;

namespace NevernamedsSigils
{
    public class RubyConduit : BasicGemConduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ruby Conduit", "If [creature] is part of a completed circuit, its owner gains an orange gem.",
                      typeof(RubyConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/rubyconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/rubyconduit_pixel.png"),
                      isConduit: true);

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
        public override TextBox.Style TextStyle()
        {
            return TextBox.Style.Magic;
        }
        public override string GainText()
        {
            return "[creature] completes a circuit, and grants its owner an orange gem!";
        }
        public override string LoseText()
        {
            return "[creature] no longer completes a circuit, and no longer grants an orange gem!";
        }
        public override Ability GemAbility()
        {
            return Ability.GainGemOrange;
        }
        public override GemType GemColour()
        {
            return GemType.Orange;
        }
    }
}

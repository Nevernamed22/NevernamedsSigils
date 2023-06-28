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
    public class SapphireConduit : BasicGemConduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sapphire Conduit", "If [creature] is part of a completed circuit, its owner gains a blue gem.",
                      typeof(SapphireConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/sapphireconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/sapphireconduit_pixel.png"),
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
            return "[creature] completes a circuit, and grants its owner a blue gem!";
        }
        public override string LoseText()
        {
            return "[creature] no longer completes a circuit, and no longer grants a blue gem!";
        }
        public override Ability GemAbility()
        {
            return Ability.GainGemBlue;
        }
        public override GemType GemColour()
        {
            return GemType.Blue;
        }
    }
}


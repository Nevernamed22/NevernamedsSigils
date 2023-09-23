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
    public class MoxMax : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Mox Max", "[creature] cannot be played unless two cards which grant its respective mox cost colour are present on the owners side of the board..",
                      typeof(MoxMax),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/moxmax.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/moxmax_pixel.png"));

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
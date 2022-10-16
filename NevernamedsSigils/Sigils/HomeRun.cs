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
    public class HomeRun : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Home Run", "[creature] will remember the lane in which it was played, and will always attack that lane.",
                      typeof(HomeRun),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/homerun.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/homerun_pixel.png"));

            HomeRun.ability = newSigil.ability;
        }
        public static Ability ability;
        public CardSlot home;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card && base.Card.slot) home = base.Card.slot;
            yield break;
        }
    }
}
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
    public class L33pLeaver : Strafe
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("L33p Leaver", "At the end of the owner's turn, [creature] will move in the direction inscrybed in the sigil and drop a L33pB0t in its old place.",
                      typeof(L33pLeaver),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/l33pleaver.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/l33pleaver_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
        {
            if (cardSlot.Card == null)
            {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("LeapBot"), cardSlot, 0.1f, true);
            }
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
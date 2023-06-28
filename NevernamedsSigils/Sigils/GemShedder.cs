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
    public class GemShedder : Strafe
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gem Shedder", "At the end of the owner's turn, [creature] will move in the direction inscrybed in the sigil and drop a random Gem in its old place.",
                      typeof(GemShedder),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gemshedder.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/gemshedder_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
        {
            if (cardSlot.Card == null)
            {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(Tools.RandomElement(Gems)), cardSlot, 0.1f, true);
            }
            yield break;
        }
        public static List<string> Gems = new List<string>()
        {
           "MoxEmerald",
           "MoxRuby",
            "MoxSapphire"
        };
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
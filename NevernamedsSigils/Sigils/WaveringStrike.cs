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
    public class WaveringStrike : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Wavering Strike", "[creature] will alternate between striking to the left or right of where it would otherwise attack at the end of each turn.",
                      typeof(WaveringStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/waveringstrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/waveringstrike_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public bool isLeft;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return playerTurnEnd != base.Card.OpponentCard;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            base.Card.Anim.NegationEffect(false);
            isLeft = !isLeft;
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, isLeft);
            base.Card.RenderCard();
            yield break;
        }
    }
}
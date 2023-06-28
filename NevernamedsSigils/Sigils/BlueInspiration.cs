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
    public class BlueInspiration : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Blue Inspiration", "While [creature] is on the board, all friendly creatures which cost a blue gem will gain a random sigil when played.",
                      typeof(BlueInspiration),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/blueinspiration.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/blueinspiration_pixel.png"),
                      triggerText: "[creature] empowers the Blue Gem cost creature with a random sigil!"
                      );

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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard != base.Card && otherCard.OpponentCard == base.Card.OpponentCard && otherCard.Info && otherCard.Info.gemsCost != null && otherCard.Info.gemsCost.Contains(GemType.Blue);
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return base.PreSuccessfulTriggerSequence();
            otherCard.AddTemporaryMod(new CardModificationInfo(Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, otherCard, null)));
            otherCard.RenderCard();
            yield break;
        }
    }
}
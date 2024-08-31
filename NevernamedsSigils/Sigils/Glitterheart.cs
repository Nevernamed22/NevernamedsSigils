using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class Glitterheart : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Glitterheart", "While [creature] is on the board, playing a mox gem will increase its health by 1.",
                      typeof(Glitterheart),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/glitterheart.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/glitterheart_pixel.png"),
                      triggerText: "[creature] is bolstered by the played gem.");

            ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            if (otherCard.HasTrait(Trait.Gem)) { return true; }
            else return false;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return base.PreSuccessfulTriggerSequence();           
            yield return new WaitForSeconds(0.05f);
            base.Card.Anim.StrongNegationEffect();
            base.Card.temporaryMods.Add(new CardModificationInfo(0, 1));
            yield return new WaitForSeconds(0.051f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

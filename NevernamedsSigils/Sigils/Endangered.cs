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
    public class Endangered : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Endangered", "At the end of the turn, [creature] has a one in four chance to perish.",
                      typeof(Endangered),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/endangered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/endangered_pixel.png"));

            Endangered.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return !playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            if (UnityEngine.Random.value <= 0.25f)
            {
                yield return base.PreSuccessfulTriggerSequence();
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.15f);

                yield return base.Card.Die(false, null, false);

                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}

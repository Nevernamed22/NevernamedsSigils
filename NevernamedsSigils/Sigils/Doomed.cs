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
    public class Doomed : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Doomed", "At the end of the turn, [creature] will perish.",
                      typeof(Doomed),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doomed.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/doomed_pixel.png"));

            Doomed.ability = newSigil.ability;
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
            return base.Card.slot.IsPlayerSlot == playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            yield return base.PreSuccessfulTriggerSequence();
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            yield return base.Card.Die(false, null, false);

            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}

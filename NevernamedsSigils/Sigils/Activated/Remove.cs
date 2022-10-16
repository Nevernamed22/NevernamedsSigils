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
    public class Remove : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Remove", "Kill [creature].",
                      typeof(Remove),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/remove.png"),
                      pixelTex: null,
                      isActivated: true);

            Remove.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }      
        public override IEnumerator Activate()
        {
            if (base.Card.OpponentCard) yield break;
            if (base.Card && !base.Card.Dead)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return base.Card.Die(false, null, true);
            }
            yield break;
        }       
    }
}

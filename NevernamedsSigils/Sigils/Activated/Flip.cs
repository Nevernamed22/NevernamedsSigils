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
    public class Flip : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Flip", "50/50 chance to either kill [creature] or give it +1 power and health.",
                      typeof(Flip),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/flip.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/flip_pixel.png"),
                      isActivated: true);

            Flip.ability = newSigil.ability;
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
                if (UnityEngine.Random.value <= 0.5f)
                {
                    base.Card.temporaryMods.Add(new CardModificationInfo(1, 1));
                }
                else
                {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return base.Card.Die(false, null, true);
                }

            }
            yield break;
        }
    }
}

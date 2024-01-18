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
    public class Recharge : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Recharge", "When [creature] is played, its owner regains 1 energy. Does not affect the current energy maximum.",
                      typeof(Recharge),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 1,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/recharge.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/recharge_pixel.png"));

            Recharge.ability = newSigil.ability;
        }
        public static Ability ability;
        public override bool RespondsToResolveOnBoard()
        {
            return !base.Card.OpponentCard;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return ResourcesManager.Instance.AddEnergy(1);
                yield return base.LearnAbility(0f);
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
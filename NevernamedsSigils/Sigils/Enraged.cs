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
    public class Enraged : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Enraged", "When [creature] is struck, it gains 1 attack power.",
                      typeof(Enraged),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/enraged.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/enraged_pixel.png"));

            Enraged.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.temporaryMods.Add(new CardModificationInfo(1, 0));
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

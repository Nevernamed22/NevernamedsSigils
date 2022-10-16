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
    public class Paralytic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Paralytic", "When [creature] strikes a creature, that creature loses 1 power.",
                      typeof(Paralytic),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/paralytic.png"),
                      pixelTex: null);

            Paralytic.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return attacker == base.Card;
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            if (!target.HasTrait(Trait.Giant) && target.Info.Attack > 0)
            {
                yield return base.PreSuccessfulTriggerSequence();
                target.AddTemporaryMod(new CardModificationInfo(-1, 0));
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
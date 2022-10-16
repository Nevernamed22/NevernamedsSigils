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
    public class BloodDependent : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Blood Dependent", "If [creature] does not strike another creature during the attack phase, it will perish at the start of the owner's next turn.",
                      typeof(BloodDependent),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/blooddependent.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/blooddependent_pixel.png"));

            BloodDependent.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard;
        }
        bool hasAttackedThisTurn;
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return attacker == base.Card;
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            hasAttackedThisTurn = true;
            yield break;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (hasAttackedThisTurn) hasAttackedThisTurn = false;
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.Card.Die(false);
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
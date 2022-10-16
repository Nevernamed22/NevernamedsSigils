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
    public class ReturnStrike : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Return Strike", "When [creature] is struck, it attacks in retaliation.",
                      typeof(ReturnStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/returnstrike.png"),
                      pixelTex: null);

            ReturnStrike.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            hasAlreadyAttackedOnceWhileDead = false;
            yield break;
        }
        private bool hasAlreadyAttackedOnceWhileDead = false;
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return (base.Card && base.Card.OnBoard);
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (base.Card && base.Card.Health <= 0)
            {
                if (hasAlreadyAttackedOnceWhileDead) yield break;
                else hasAlreadyAttackedOnceWhileDead = true;
            }
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

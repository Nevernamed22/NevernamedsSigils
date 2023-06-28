using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Commander : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Commander", "When [creature] is struck, the creatures to it's left and right will attack the striker.",
                      typeof(Commander),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/commander.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/commander_pixel.png"));

            Commander.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Health > 0;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool toLeftValid = toLeft != null && toLeft.Card != null;
            bool toRightValid = toRight != null && toRight.Card != null;
            if (toLeftValid || toRightValid)
            {
                yield return new WaitForSeconds(0.55f);
                if (toLeftValid)
                {
                    yield return ForceSupporterAttack(toLeft.Card, source);
                    yield return new WaitForSeconds(0.15f);
                }
                if (toRightValid)
                {
                    yield return ForceSupporterAttack(toRight.Card, source);
                    yield return new WaitForSeconds(0.15f);
                }
                yield return base.LearnAbility(0.4f);
            }
            yield break;
        }
        private IEnumerator ForceSupporterAttack(PlayableCard supporter, PlayableCard target)
        {
            CardModificationInfo removeFlyingMod = null;
            if (supporter.HasAbility(Ability.Flying))
            {
                removeFlyingMod = new CardModificationInfo();
                removeFlyingMod.negateAbilities.Add(Ability.Flying);
                supporter.AddTemporaryMod(removeFlyingMod);
            }

            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!supporter.OpponentCard, null, supporter.Slot, new List<CardSlot>() { target.Slot });

            if (removeFlyingMod != null)
            {
                supporter.RemoveTemporaryMod(removeFlyingMod, true);
            }
            yield break;
        }
    }
}

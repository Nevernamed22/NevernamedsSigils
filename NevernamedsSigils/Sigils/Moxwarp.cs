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
    public class Moxwarp : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Moxwarp", "While [creature] is alive on the board, any enemy attack that enters a sapphire mox will be returned out of all ruby mox, and vice versa.",
                      typeof(Moxwarp),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/moxwarp_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return amount > 0 && (isBlueGem(target) || isOrangeGem(target));
        }
        public static bool isBlueGem(PlayableCard card)
        {
            return card != null && (card.HasAbility(Ability.GainGemBlue) || card.HasAbility(Ability.GainGemTriple)) && card.HasTrait(Trait.Gem);
        }
        public static bool isOrangeGem(PlayableCard card)
        {
            return card != null && (card.HasAbility(Ability.GainGemOrange) || card.HasAbility(Ability.GainGemTriple)) && card.HasTrait(Trait.Gem);
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            bool triggerBlue = isOrangeGem(target);
            yield return base.PreSuccessfulTriggerSequence();
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard))
            {
                if (slot && slot.Card != null)
                {
                    if ((isBlueGem(slot.Card) && triggerBlue) || (isOrangeGem(slot.Card) && !triggerBlue))
                    {
                        yield return new WaitForSeconds(0.1f);

                        CardModificationInfo statalt = new CardModificationInfo();
                        statalt.attackAdjustment = attacker.Attack - slot.Card.Attack;
                        slot.Card.AddTemporaryMod(statalt);

                        FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                        yield return fakecombat.FakeCombat(!slot.Card.OpponentCard, null, slot);
                        yield return new WaitForSeconds(0.1f);

                        slot.Card.RemoveTemporaryMod(statalt);
                    }
                }
            }
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
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
    public class DeusHoof : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Deus Hoof", "All friendly hooved creatures gain +1 Power and Health while [creature] is on the board. Also, all hooved creatures will attack anything that strikes [creature].",
                      typeof(DeusHoof),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/deushoof.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/deushoof_pixel.png"));

            DeusHoof.ability = newSigil.ability;
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

            List<CardSlot> viableslots = new List<CardSlot>();
            if (base.Card.slot.IsPlayerSlot) viableslots = Singleton<BoardManager>.Instance.playerSlots;
            else viableslots = Singleton<BoardManager>.Instance.opponentSlots;

            bool didit = false;
            foreach (CardSlot slot in viableslots)
            {
                if (slot && slot.Card)
                {
                    if (slot.Card != base.Card && slot.Card.Info && slot.Card.Info.tribes.Contains(Tribe.Hooved))
                    {

                        if (!source.Dead)
                        {
                            didit = true;
                            yield return ForceSupporterAttack(slot.Card, source); 
                            yield return new WaitForSeconds(0.15f);
                        }
                    }
                }
            }
            if (didit) yield return base.LearnAbility(0.4f);

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

            yield return Singleton<TurnManager>.Instance.CombatPhaseManager.SlotAttackSlot(supporter.Slot, target.Slot, 0f);

            if (removeFlyingMod != null)
            {
                supporter.RemoveTemporaryMod(removeFlyingMod, true);
            }
            yield break;
        }
    }
}
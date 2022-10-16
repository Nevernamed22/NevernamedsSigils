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
    public class Medicinal : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Medicinal", "When [creature] is played, it heals all friendly creatures by 2 and removes all negative sigils from friendly creatures.",
                      typeof(Medicinal),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/medicinal.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/medicinal_pixel.png"));

            Medicinal.ability = newSigil.ability;
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
        private void CheckAndMaybeRemoveAbility(PlayableCard card, Ability ability)
        {
            AbilityInfo info = AbilitiesUtil.GetInfo(ability);
            if (info && info.powerLevel < 0)
            {
                card.TemporarilyRemoveAbilityFromCard(ability);
            }
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return new WaitForSeconds(0.15f);

            if (base.Card && base.Card.slot)
            {
                List<CardSlot> availableSlots = new List<CardSlot>();
                if (base.Card.slot.IsPlayerSlot) availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(true));
                else availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));

                yield return base.PreSuccessfulTriggerSequence();

                foreach (CardSlot validSlot in availableSlots)
                {
                    if (validSlot && validSlot.Card)
                    {
                        PlayableCard targCard = validSlot.Card;
                        if (targCard.Health < targCard.MaxHealth)
                        {
                            targCard.HealDamage(Mathf.Min(2, (targCard.MaxHealth - targCard.Health)));
                        }

                        CardModificationInfo cardModificationInfo = new CardModificationInfo();
                        foreach (Ability innateAbility in targCard.Info.abilities)
                        {
                            CheckAndMaybeRemoveAbility(targCard, innateAbility);
                        }
                       foreach (CardModificationInfo modinf in targCard.temporaryMods)
                        {
                            foreach (Ability modability in modinf.abilities)
                            {
                                CheckAndMaybeRemoveAbility(targCard, modability);
                            }
                        }
                        foreach (CardModificationInfo addedinf in targCard.Info.mods)
                        {
                            foreach (Ability addability in addedinf.abilities)
                            {
                                CheckAndMaybeRemoveAbility(targCard, addability);
                            }
                        }
                    }
                }

                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
            }

            yield break;

        }

    }
}

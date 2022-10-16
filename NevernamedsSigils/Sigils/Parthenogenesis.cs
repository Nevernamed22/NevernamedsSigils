﻿using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Parthenogenesis : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Parthenogenesis", "When [creature] dies to combat, a larva is left in it's old space that evolves into an exact clone after 1 turn.",
                      typeof(Parthenogenesis),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/parthenogenesis.png"),
                      pixelTex: null);

            Parthenogenesis.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public  CardInfo Grub
        {
            get
            {
                CardInfo grub = CardLoader.GetCardByName("Nevernamed CloneGrub");
                if (base.Card != null && grub != null)
                {
                    grub.evolveParams = new EvolveParams() { evolution = base.Card.Info, turnsToEvolve = 2 };
                }
                return grub;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(Grub, base.Card.slot, 0.1f, true);
            yield return base.LearnAbility(0f);
        }
    }
}
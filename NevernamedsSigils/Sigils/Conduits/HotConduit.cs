﻿using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;

namespace NevernamedsSigils
{
    public class HotConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Hot Conduit", "When a creature within a circuit completed by [creature] is struck, the striker gains the Burning Sigil. Also, creatures within this card's circuit do not take damage from the Burning sigil.",
                      typeof(HotConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/hotconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/hotconduit_pixel.png"),
                      isConduit: true);

            ability = newSigil.ability;
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
            List<CardSlot> affectedSlots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card));
            return attacker != null && attacker.Health > 0 && target != null && target.slot != null && affectedSlots.Contains(target.slot);
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();
            target.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            attacker.AddTemporaryMod(new CardModificationInfo(Burning.ability));
            attacker.RenderCard();
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

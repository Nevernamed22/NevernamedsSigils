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
    public class Confusion : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Confusion", "When [creature] is played opposite an opponents creature, swap that creatures power and health.",
                      typeof(Confusion),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair2 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/confusion.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/confusion_pixel.png"),
                      triggerText: "The card opposing [creature] is confused!");

            ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.slot.opposingSlot.Card != null && !base.Card.slot.opposingSlot.Card.HasTrait(Trait.Giant);
        }

        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();

            PlayableCard opposer = base.Card.slot.opposingSlot.Card;

            opposer.AddTemporaryMod(new CardModificationInfo(-opposer.Attack + opposer.Health, -opposer.Health + opposer.Attack));
            opposer.OnStatsChanged();
            opposer.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.25f);
            if (opposer.Health <= 0) { yield return opposer.Die(false, null, true); }
            else { yield return base.LearnAbility(0.25f); }
            yield break;
        }
    }
}

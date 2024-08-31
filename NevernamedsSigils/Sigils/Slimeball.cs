using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Slimeball : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Slimeball", "When [creature] is played, it permanently stains the opposing slot with slime. Any creatures in a slimy slot without made of stone lose 1 power.",
                      typeof(Slimeball),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/slimeball.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/slimeball_pixel.png"));

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
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.slot && base.Card.slot.opposingSlot && !base.Card.slot.opposingSlot.SlotHasModifier("SlimySlot");
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            GameObject floater = SlotModificationTools.ModifySlot(base.Card.slot.opposingSlot, StickyTrail.slimyMod);
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}
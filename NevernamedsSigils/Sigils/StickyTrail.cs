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
    public class StickyTrail : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sticky Trail", "When [creature] enters a space on the board, it permantently marks that space with its slime. Any creatures that enter that space without the Made of Stone sigil lose 1 power.",
                      typeof(StickyTrail),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/stickytrail.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/stickytrail_pixel.png"));

            Slime = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gooey.png");
            SlimePixel = Tools.LoadTex("NevernamedsSigils/Resources/Other/slimyslot_indicator_pixel.png");
            ability = newSigil.ability;
            slimyMod = new SlotModification()
            {
                identifier = "SlimySlot",
                stacks = false,
                tex = Slime,
                pixelTex = SlimePixel,
            };
        }
        public static SlotModification slimyMod;
        public static Texture Slime;
        public static Texture SlimePixel;
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return otherCard == base.Card && !Card.slot.SlotHasModifier("SlimySlot");
        }
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            GameObject floater = SlotModificationTools.ModifySlot(base.Card.slot, slimyMod);
            yield return base.LearnAbility(0.25f); 
            yield break;
        }

    }
}
using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class BloodAndBone : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Blood and Bone", "The value represented with this sigil will be equal to the bone cost of the opposing card, or double the blood cost of the opposing card.",
               typeof(BloodAndBone),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/bloodandbone.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/bloodandbone_pixel.png"),
               gbcDescription: "[creature]s power is equal to the bone cost of the opposing creature plus twice the blood cost of the opposing creature.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "BloodAndBone", typeof(BloodAndBone)).Id;
            BloodAndBone.specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return BloodAndBone.specialStatIcon;
            }
        }
        public override int[] GetStatValues()
        {
            int damageout = 0;

            if (base.PlayableCard && base.PlayableCard.OnBoard && base.PlayableCard.slot && base.PlayableCard.slot.opposingSlot && base.PlayableCard.slot.opposingSlot.Card)
            {
                PlayableCard opponent = base.PlayableCard.slot.opposingSlot.Card;
                if (opponent.Info.BloodCost > 0) { damageout += opponent.Info.BloodCost * 2; }
                if (opponent.Info.BonesCost > 0) { damageout += opponent.Info.BonesCost; }
            }

            return new int[]
            {
                damageout,
                0
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class PackPowerPlus : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Pack Power Plus", "The value represented with this sigil will be equal to twice the number of items the owner holds.",
               typeof(PackPowerPlus),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/packpowerplus.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/packpowerplus_pixel.png"));

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Pack Power Plus", typeof(PackPowerPlus)).Id;
            specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return specialStatIcon;
            }
        }

        public override int[] GetStatValues()
        {
            if (Tools.GetActAsInt() == 1)
            {
                return new int[]
                {
                    Part1ItemsManager.Instance.consumableSlots.FindAll((ConsumableItemSlot x) => x.Consumable != null).Count * 2,
                    0
                };
            }
            return new int[]
            {
                0,
                0
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

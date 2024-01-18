﻿using APIPlugin;
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
    public class PackPower : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Pack Power", "The value represented with this sigil will be equal to the number of items the owner holds.",
               typeof(PackPower),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/packpower.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/packpower_pixel.png"),
               gbcDescription: "[creature]s power is equal to the number of items its owner holds.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Pack Power", typeof(PackPower)).Id;
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
                    Part1ItemsManager.Instance.consumableSlots.FindAll((ConsumableItemSlot x) => x.Consumable != null).Count,
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

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
    public class TrinketVitality : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Trinket Vitality", "The value represented with this sigil will be equal to double the number of items the owner holds.",
               typeof(TrinketVitality),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/trinketvitality.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/trinketvitality_pixel.png"),
               isForHealth: true,
               gbcDescription: "[creature]s health is equal to twice the number of items it's owner holds.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Trinket Vitality", typeof(TrinketVitality)).Id;
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
                    0,
                    Part1ItemsManager.Instance.consumableSlots.FindAll((ConsumableItemSlot x) => x.Consumable != null).Count * 2
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

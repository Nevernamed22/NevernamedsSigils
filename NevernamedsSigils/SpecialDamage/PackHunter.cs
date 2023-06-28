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
    public class PackHunter : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Pack Hunter", "The value represented with this sigil will be equal to the number of canine tribe creatures on the board, including opponent creatures.",
               typeof(PackHunter),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/packhunter.png"),
               pixelTex: null,
               gbcDescription: "[creature]s power is equal to the number of canine tribe creatures on the board, including opponent creatures.");


            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "PackHunter", typeof(PackHunter)).Id;
            PackHunter.specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return PackHunter.specialStatIcon;
            }
        }
        public override int[] GetStatValues()
        {
            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
            availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
            return new int[]
            {
                availableSlots.FindAll((x) => x != null && x.Card != null && x.Card.Info.tribes.Contains(Tribe.Canine)).Count,
                0
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

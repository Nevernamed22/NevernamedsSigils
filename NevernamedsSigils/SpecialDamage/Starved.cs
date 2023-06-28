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
    public class Starved : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Starved", "The value represented with this sigil will be equal to seven minus the number of cards left in your deck.",
               typeof(Starved),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/starved.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/starved_pixel.png"),
               gbcDescription: "[creature]s power is equal to seven minus the number of cards left in your deck.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Starved", typeof(Starved)).Id;
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
            return new int[]
            {
        Math.Max(0, 7 - Singleton<CardDrawPiles>.Instance.Deck.cards.Count),
                0
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

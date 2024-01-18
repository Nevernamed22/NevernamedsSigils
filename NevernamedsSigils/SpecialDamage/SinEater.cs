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
    public class SinEater : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Sin Eater", "The value represented with this sigil will be equal to the amount of sigils present on the creatures that were sacrificed to play its bearer.",
               typeof(SinEater),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/sineater.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/sineater_pixel.png"),
               gbcDescription: "[creature]s power is equal to the amount of sigils present on the creatures that were sacrificed to play it.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "SinEater", typeof(SinEater)).Id;
            SinEater.specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return SinEater.specialStatIcon;
            }
        }
        public int sigilsOnSacced = 0;
        public override int[] GetStatValues()
        {

            return new int[]
            {
                sigilsOnSacced,
                0
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

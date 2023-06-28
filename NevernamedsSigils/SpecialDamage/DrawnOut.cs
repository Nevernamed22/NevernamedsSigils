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
    public class DrawnOut : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Drawn Out", "The value represented with this sigil will be equal to the number of cards you have drawn this turn while it's bearer is on the board.",
               typeof(DrawnOut),
               categories: new List<AbilityMetaCategory>() {  },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/drawnout.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/drawnout_pixel.png"),
               gbcDescription: "[creature]s power is equal to the number of cards you have drawn this turn while it is on the board.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "DrawnOut", typeof(DrawnOut)).Id;
            specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return specialStatIcon;
            }
        }
        public override bool RespondsToOtherCardDrawn(PlayableCard card) { return true; }
        public override IEnumerator OnOtherCardDrawn(PlayableCard card) { draws++; yield break; }
        public override bool RespondsToUpkeep(bool playerUpkeep) { return playerUpkeep; }
        public override IEnumerator OnUpkeep(bool playerUpkeep) { draws = 0; yield break; }

        public int draws = 0;
        public override int[] GetStatValues()
        {
            return new int[]
            {
                draws,
                0
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

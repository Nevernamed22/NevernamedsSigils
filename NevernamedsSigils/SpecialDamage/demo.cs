using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using InscryptionAPI.Card;
using GBC;
using UnityEngine.EventSystems;

namespace NevernamedsSigils
{
    public class demo : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {

            StatIconInfo info = StatIconManager.New("nevernamed.inscryption.sigils", "Demo", "debug", typeof(demo));

            info.iconGraphic = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/debugblacksquare.png");
            info.pixelIconGraphic  = Tools.GenerateAct2Portrait(Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/damagedice_pixel.png"));
            info.gbcDescription = "debug";
            info.appliesToAttack = true;
            info.appliesToHealth = true;


            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "demo", typeof(demo)).Id;




            demo.specialStatIcon = info.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return demo.specialStatIcon;
            }
        }
        public override int[] GetStatValues()
        {
            return new int[]
            {
                1,
                1
            };
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

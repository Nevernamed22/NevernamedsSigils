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
	public class OneHalfSquirrels : VariableStatBehaviour {
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("One Half Squirrels", "The value represented with this sigil will be equal to half the number of Squirrels remaining in your side deck. ",
			   typeof(OneHalfSquirrels),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/onehalfsquirrels.png"),
			   pixelTex: null);

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "OneHalfSquirrels", typeof(OneHalfSquirrels)).Id;
			OneHalfSquirrels.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return OneHalfSquirrels.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			float num = CardDrawPiles3D.Instance.sidePile.cards.Count;
			num /= 2;
			int  num2 = Mathf.CeilToInt(num);
			return new int[]
			{
				num2,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

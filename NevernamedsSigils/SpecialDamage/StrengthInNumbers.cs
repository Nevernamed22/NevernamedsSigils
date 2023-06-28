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
	public class StrengthInNumbers : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Strength In Numbers", "The value represented with this sigil will be equal to the number of creatures bearing this sigil on the board, including opponent creatures.",
			   typeof(StrengthInNumbers),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/strengthinnumbers.png"),
			  pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/strengthinnumbers_pixel.png"),
			  gbcDescription: "[creature]s power is equal to the number of creatures bearing the same special damage stat on the board, including opponent creatures.");
			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "StrengthInNumbers", typeof(StrengthInNumbers)).Id;

			specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return StrengthInNumbers.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
			availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
			return new int[]
			{
				availableSlots.FindAll((x) => x != null && x.Card != null && x.Card.HasSpecialAbility(StrengthInNumbers.ability)).Count,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

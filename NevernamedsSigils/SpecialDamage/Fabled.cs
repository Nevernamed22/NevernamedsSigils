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
	public class Fabled : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Fabled", "The value represented with this sigil will be equal to twice the number of rare creatures on the board.",
			   typeof(Fabled),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/fabled.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/fabled_pixel.png"),
			   gbcDescription: "[creature]s power is equal to twice the number of rare creatures on the board.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Fabled", typeof(Fabled)).Id;
			Fabled.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return Fabled.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
			availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
			int toReturn = availableSlots.FindAll((x) => x != null && x.Card != null && x.Card.Info.metaCategories.Contains(CardMetaCategory.Rare)).Count;
			return new int[]
			{
				toReturn * 2,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

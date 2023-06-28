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
	public class Lithophile : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Lithophile", "The value represented with this sigil will be equal to the number of terrain cards on the board.",
			   typeof(Lithophile),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/lithophile.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/lithophile_pixel.png"),
			   gbcDescription: "[creature]s power is equal to the number of terrain cards on the board.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Lithophile", typeof(Lithophile)).Id;
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
				Singleton<BoardManager>.Instance.AllSlots.FindAll((x) => x != null && x.Card != null && x.Card.Info.HasTrait(Trait.Terrain)).Count,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

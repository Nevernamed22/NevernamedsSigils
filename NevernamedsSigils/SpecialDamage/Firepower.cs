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
	public class Firepower : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Firepower", "The value represented with this sigil will be equal to the number of creatures on the board with the Burning sigil.",
			   typeof(Firepower),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/firepower.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/firepower_pixel.png"),
			   gbcDescription: "[creature]s power is equal to the number of creatures on the board with the Burning sigil.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Firepower", typeof(Firepower)).Id;
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
				Singleton<BoardManager>.Instance.AllSlots.FindAll((x) => x != null && x.Card != null && x.Card.HasAbility(Burning.ability)).Count,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

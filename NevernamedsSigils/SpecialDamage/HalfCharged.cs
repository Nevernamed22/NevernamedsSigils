using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
	public class HalfCharged : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Half Charged", "The value represented with this sigil will be equal to half the player's current energy, rounded up.",
			   typeof(HalfCharged),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part3Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/halfcharged.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/halfcharged_pixel.png"),
			   gbcDescription: "[creature]s power is equal to half your current energy, rounded up.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Half Charged", typeof(HalfCharged)).Id;
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
			int damageout = 0;
			if (Singleton<ResourcesManager>.Instance != null && Singleton<ResourcesManager>.Instance.PlayerEnergy > 0)
			{
				damageout = Mathf.CeilToInt(Singleton<ResourcesManager>.Instance.PlayerEnergy * 0.5f);
			}
			return new int[]
			{
				damageout,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
	public class HealthDamage : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Health Damage", "The value represented with this sigil will be equal to it's bearer's current health.",
			   typeof(HealthDamage),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/healthdamage.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/healthdamage_pixel.png"));

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "HealthDamage", typeof(HealthDamage)).Id;
			HealthDamage.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return HealthDamage.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			int damageout = 0;
			if (base.PlayableCard && base.PlayableCard.Health > 0)
			{
				damageout = base.PlayableCard.Health;
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

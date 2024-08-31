using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
	public class DamageScaling : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Damage Scaling", "The value represented with this sigil will be equal to five minus the amount of damage until you lose the game.",
			   typeof(DamageScaling),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.GrimoraRulebook, AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/damagescaling.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/damagescaling_pixel.png"),
			   gbcDescription: "[creature]s power is equal to five minus the amount of damage until you lose the game.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Damage Scaling", typeof(DamageScaling)).Id;
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
			if (base.PlayableCard.IsPlayerCard()) { damageout = 5 - ((LifeManager.GOAL_BALANCE * 2) - Singleton<LifeManager>.Instance.DamageUntilPlayerWin); }
            else { damageout = 5 - Singleton<LifeManager>.Instance.DamageUntilPlayerWin; }
			damageout = Mathf.Max(damageout, 0);
			return new int[]
			{
				damageout,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

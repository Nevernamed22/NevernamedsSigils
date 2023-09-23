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
	public class OneDFour : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("1d4", "The value represented with this sigil starts at 2, and will become a random number between 1 and 4 at the start of each subsequent turn.",
			   typeof(OneDFour),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/1d4.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/1d4_pixel.png"),
			   gbcDescription: "[creature]s power starts at 2, and will become a random number between 1 and 4 at the start of each subsequent turn.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "1d4", typeof(OneDFour)).Id;
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
				Damage,
				0
			};
		}
		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return playerUpkeep && base.PlayableCard && base.PlayableCard.OnBoard;
		}
		private int Damage = 2;
		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			base.PlayableCard.Anim.LightNegationEffect();
			Damage = UnityEngine.Random.Range(1, 5);
			yield break;
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

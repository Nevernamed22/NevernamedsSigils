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
	public class MagickePower : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Magicke Power", "The value represented with this sigil is equal to the number of sigils this card has removed with the Bleach Sigil or Artistic License Sigil.",
			   typeof(MagickePower),
			   categories: new List<AbilityMetaCategory>() {  },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/magickepower.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/magickepower_pixel.png"),
			   gbcDescription: "[creature]s power is equal to the number of sigils it has removed with the Bleach Sigil or Artistic License Sigil.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "MagickePower", typeof(MagickePower)).Id;
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
				RemovedSigilAmount,
				0
			};
		}
		public int RemovedSigilAmount = 0;
		public static SpecialStatIcon specialStatIcon;
	}
}


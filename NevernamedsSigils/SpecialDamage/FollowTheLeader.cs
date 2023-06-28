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
	public class FollowTheLeader : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Follow The Leader", "The value represented with this sigil will be equal to the attack power of the first card in your hand.",
			   typeof(FollowTheLeader),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/followtheleader.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/followtheleader_pixel.png"),
			   gbcDescription: "[creature]s power is equal to the attack power of the first card in your hand.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "FollowTheLeader", typeof(FollowTheLeader)).Id;
			specialStatIcon = icon.iconType;
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
			int toReturn = 0;
			if (Singleton<PlayerHand>.Instance.CardsInHand.Count > 0) { toReturn = Singleton<PlayerHand>.Instance.CardsInHand[0].Attack; }
			return new int[]
			{
				toReturn,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

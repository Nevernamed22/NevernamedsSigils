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
	public class HerdingBeast : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Herding Beast", "The value represented with this sigil will be equal to the number of hooved tribe creatures on the board, including opponent creatures.",
			   typeof(HerdingBeast),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/herdingbeast.png"),
			   pixelTex: null);

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "HerdingBeast", typeof(HerdingBeast)).Id;
			HerdingBeast.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return HerdingBeast.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
			availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
			return new int[]
			{
				availableSlots.FindAll((x) => x != null && x.Card != null && x.Card.Info.tribes.Contains(Tribe.Hooved)).Count,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

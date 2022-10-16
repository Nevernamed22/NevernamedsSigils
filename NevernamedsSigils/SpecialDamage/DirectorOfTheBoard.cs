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
	public class DirectorOfTheBoard : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Director Of The Board", "The value represented with this sigil will be equal to the number of cards on the board, including opponent cards.",
			   typeof(DirectorOfTheBoard),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/directoroftheboard.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/directoroftheboard_pixel.png"));

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "DirectorOfTheBoard", typeof(DirectorOfTheBoard)).Id;
			DirectorOfTheBoard.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return DirectorOfTheBoard.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
			availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
			return new int[]
			{
				availableSlots.FindAll((x) => x != null && x.Card != null).Count,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

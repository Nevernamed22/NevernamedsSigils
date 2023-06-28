﻿using APIPlugin;
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
	public class WorldWideWeb : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("World Wide Web", "The value represented with this sigil will be equal to the number of arachnid tribe creatures on the board, including opponent creatures.",
			   typeof(WorldWideWeb),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Plugin.arachnophobiaMode.Value	? Tools.LoadTex("NevernamedsSigils/Resources/Sigils/web.png") : Tools.LoadTex("NevernamedsSigils/Resources/Other/worldwideweb.png"),
			   pixelTex: null,
			   gbcDescription: "[creature]s power is equal to the number of arachnid tribe creatures on the board, including opponent creatures.");
			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "WorldWideWeb", typeof(WorldWideWeb)).Id;

			WorldWideWeb.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return WorldWideWeb.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
			availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
			return new int[]
			{
				availableSlots.FindAll((x) => x != null && x.Card != null && x.Card.Info.tribes.Contains(NevernamedsTribes.Arachnid)).Count,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

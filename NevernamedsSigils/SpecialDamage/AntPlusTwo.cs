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
	public class AntPlusTwo : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Ant Plus Two", "The value represented with this sigil is equal to the number of ants on the board, plus two.",
			   typeof(AntPlusTwo),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/antplustwo.png"),
			   pixelTex: null,
			   gbcDescription: "[creature]s power is equal to the number of friendly ants on the board, plus two.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "AntPlusTwo", typeof(AntPlusTwo)).Id;
			AntPlusTwo.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return AntPlusTwo.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			List<CardSlot> list = base.PlayableCard.Slot.IsPlayerSlot ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy;
			int num = 0;
			foreach (CardSlot cardSlot in list)
			{
				bool flag = cardSlot.Card != null && cardSlot.Card.Info.HasTrait(Trait.Ant);
				if (flag)
				{
					num++;
				}
			}
			num += 2;
			return new int[]
			{
				num,
				0
			};
		}
		public static SpecialStatIcon specialStatIcon;
	}
}


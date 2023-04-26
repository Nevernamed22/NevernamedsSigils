using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
	public class Greenhorn : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Greenhorn", "The value represented with this sigil will be equal to the number of Green gems on the on the owner's side of the board when the bearer was played, multiplied by two.",
			   typeof(Greenhorn),
			   categories: new List<AbilityMetaCategory>() {  },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/greenhorn.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/greenhorn_pixel.png"),
			   isForHealth: true);

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Greenhorn", typeof(Greenhorn)).Id;
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
				0,
				greenGemsWhenPlaced * 2
			};
		}
		private int greenGemsWhenPlaced;
		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}
		public override IEnumerator OnResolveOnBoard()
		{
			yield return new WaitForSeconds(0.01f);
			List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(base.PlayableCard.OpponentCard ? false : true));
			greenGemsWhenPlaced = availableSlots.FindAll((x) => x != null && x.Card != null && (x.Card.HasAbility(Ability.GainGemGreen) || x.Card.HasAbility(Ability.GainGemTriple))).Count;
			yield break;
		}
		public static SpecialStatIcon specialStatIcon;
	}
}

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
	public class HandOnHeart : VariableStatBehaviour
	{
		public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Hand On Heart", "The value represented with this sigil will be equal to the number of cards in your hand.",
			   typeof(HandOnHeart),
			   categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/handonheart.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/handonheart_pixel.png"),
			   isForHealth: true,
			   gbcDescription: "[creature]s health is equal to the number of cards in your hand.");

			ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "HandOnHeart", typeof(HandOnHeart)).Id;
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
				Singleton<PlayerHand>.Instance.CardsInHand.Count
			};
		}
        public override bool RespondsToResolveOnBoard() { return true; }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard) { return true; }
        public override bool RespondsToUpkeep(bool playerUpkeep) { return true; }
        public override bool RespondsToTurnEnd(bool playerTurnEnd) { return true; }
        public override IEnumerator OnResolveOnBoard()
        {
			yield return CheckDeath();
			yield break;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
			yield return CheckDeath();
			yield break;
		}
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
			yield return CheckDeath();
			yield break;
		}
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
			yield return CheckDeath();
			yield break;
		}
        public IEnumerator CheckDeath()
        {
			if (PlayableCard != null  && !PlayableCard.Dead && PlayableCard.Health <= 0)
			{
				yield return new WaitForSeconds(1f);
				yield return PlayableCard.Die(false, null, true);
			}
			yield break;
        }
		public static SpecialStatIcon specialStatIcon;
	}
}

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
	public class DamageDice : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Damage Dice", "The value represented with this sigil starts at 3, and will become a random number between 1 and 6 at the start of each subsequent turn.",
			   typeof(DamageDice),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/damagedice.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/damagedice_pixel.png"),
			   gbcDescription: "[creature]s power starts at 3, and will become a random number between 1 and 6 at the start of each subsequent turn.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "DamageDice", typeof(DamageDice)).Id;
			DamageDice.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return DamageDice.specialStatIcon;
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
		private int Damage = 3;
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
			base.PlayableCard.Anim.LightNegationEffect();
			Damage = UnityEngine.Random.Range(1, 7);
			yield break;
        }
		public static SpecialStatIcon specialStatIcon;
	}
}

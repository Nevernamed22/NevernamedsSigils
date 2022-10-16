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
	public class Boned : VariableStatBehaviour
	{
        public static SpecialTriggeredAbility ability;
		public static void Init()
		{
			StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Boned", "The value represented with this sigil will be equal to the number of bones the owner possessed when the bearer was placed. Does not fluctuate with bone count changes post placement.",
			   typeof(Boned),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
			   tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/boned.png"),
			   pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/boned_pixel.png"),
			   isForHealth: true);

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Boned", typeof(Boned)).Id;
			Boned.specialStatIcon = icon.iconType;
		}
		public override SpecialStatIcon IconType
		{
			get
			{
				return Boned.specialStatIcon;
			}
		}
		public override int[] GetStatValues()
		{
			return new int[]
			{
				0,
				bonesWhenplaced
			};
		}
		private int bonesWhenplaced;
        public override bool RespondsToResolveOnBoard()
        {
			return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
			yield return new WaitForSeconds(0.01f);
			bonesWhenplaced = ResourcesManager.Instance.PlayerBones;
			yield break;
        }
		public static SpecialStatIcon specialStatIcon;
	}
}

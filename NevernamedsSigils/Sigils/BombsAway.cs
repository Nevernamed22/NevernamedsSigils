using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class BombsAway : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bombs Away", "When [creature] is played, a Payload is created in the opposing slot. [define:SigilNevernamed Payload]",
                      typeof(BombsAway),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bombsaway.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bombsaway_pixel.png"),
                      triggerText: "[creature] drops a payload into the opposing slot!");

            ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
		public override bool RespondsToResolveOnBoard()
		{
			return true;
		}
		public override IEnumerator OnResolveOnBoard()
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			yield return new WaitForSeconds(0.1f);
			CardSlot opposingSlot = base.Card.Slot.opposingSlot;
			if (opposingSlot.Card == null)
			{
				yield return base.PreSuccessfulTriggerSequence();
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("SigilNevernamed Payload"), opposingSlot, 0.15f, true);
			}
			else
			{
				base.Card.Anim.StrongNegationEffect();
			}
			yield break;
		}
	}
}

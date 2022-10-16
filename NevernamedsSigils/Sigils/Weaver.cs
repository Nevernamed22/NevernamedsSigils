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
	public class Weaver : AbilityBehaviour
	{
		public static void Init()
		{
			AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Weaver", "At the end of the owner's turn, [creature] will move in the direction inscribed on the sigil and leave a Web in it's old space. [creature] can move back over webs it has placed.",
					  typeof(Weaver),
					  categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
					  powerLevel: 2,
					  stackable: false,
					  opponentUsable: false,
					  tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/weaver.png"),
					  pixelTex: null);

			Weaver.ability = newSigil.ability;
		}
		public static Ability ability;
		public override Ability Ability
		{
			get
			{
				return ability;
			}
		}
		public override bool RespondsToTurnEnd(bool playerTurnEnd)
		{
			return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
		}
		public override IEnumerator OnTurnEnd(bool playerTurnEnd)
		{
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			yield return new WaitForSeconds(0.25f);
			yield return this.DoStrafe(toLeft, toRight);
			yield break;
		}
		protected virtual IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
		{
			bool toLeftValid = toLeft != null && (toLeft.Card == null || toLeft.Card.Info.name == "Nevernamed Web");
			bool toRightValid = toRight != null && (toRight.Card == null || toRight.Card.Info.name == "Nevernamed Web");

			if (this.movingLeft && !toLeftValid)
			{
				this.movingLeft = false;
			}
			if (!this.movingLeft && !toRightValid)
			{
				this.movingLeft = true;
			}

			CardSlot destination = this.movingLeft ? toLeft : toRight;
			bool destinationValid = this.movingLeft ? toLeftValid : toRightValid;

			yield return this.MoveToSlot(destination, destinationValid);
			bool flag3 = destination != null && destinationValid;
			if (flag3)
			{
				yield return base.PreSuccessfulTriggerSequence();
				yield return base.LearnAbility(0f);
			}
			yield break;
		}
		protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
		{
			base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
			base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
			base.Card.RenderCard();
			if (destination != null && destinationValid)
			{
				if (destination.Card != null)
				{
					yield return destination.Card.Die(false, base.Card, false);
					yield return new WaitForSeconds(0.1f);
				}
				CardSlot oldSlot = base.Card.Slot;
				yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
				yield return this.PostSuccessfulMoveSequence(oldSlot);
				yield return new WaitForSeconds(0.25f);
				oldSlot = null;
			}
			else
			{
				base.Card.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.15f);
			}
			yield break;
		}
		protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
		{
				yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("Nevernamed Web"), oldSlot, 0.1f, true);
			yield break;
		}
		protected bool movingLeft;
	}
}
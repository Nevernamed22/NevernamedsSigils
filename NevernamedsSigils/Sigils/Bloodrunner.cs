using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Bloodrunner : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bloodrunner", "When [creature] deals damage to a creature, it will move in the direction inscribed in the sigil.",
                      typeof(Bloodrunner),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodrunner.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bloodrunner_pixel.png"));

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
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target) { return base.Card != null && attacker == base.Card && !base.Card.HasAbility(Stalwart.ability); }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
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
			bool toLeftBlocked = toLeft != null && toLeft.Card == null;
			bool toRightBlocked = toRight != null && toRight.Card == null;
			if (this.movingLeft && !toLeftBlocked) { this.movingLeft = false; }
			if (!this.movingLeft && !toRightBlocked) { this.movingLeft = true; }

			CardSlot destination = this.movingLeft ? toLeft : toRight;
			bool destinationValid = this.movingLeft ? toLeftBlocked : toRightBlocked;
			yield return this.MoveToSlot(destination, destinationValid);

			if (destination != null && destinationValid)
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
				CardSlot oldSlot = base.Card.Slot;
				yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
				yield return this.PostSuccessfulMoveSequence(oldSlot);
				yield return new WaitForSeconds(0.25f);
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
			if (base.Card.Info.GetExtendedProperty("BloodrunnerLeaveBehind") != null)
			{
				yield return new WaitForSeconds(0.1f);
				if (oldSlot && oldSlot.Card == null)
				{
					CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("BloodrunnerLeaveBehind"));
					segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Bloodrunner.ability }));
					yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
				}
			}
			yield break;
		}
		protected bool movingLeft;
	}
}
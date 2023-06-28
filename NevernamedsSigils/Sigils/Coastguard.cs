using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class CoastGuard : Submerge , IActivateWhenFacedown
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Coastguard", "Submerges during the opponent's turn. When an opposing creature is placed opposite to an empty space, [creature] will move to that empty space while submerged.",
                      typeof(CoastGuard),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair1 },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/coastguard.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/coastguard_pixel.png"));

            CoastGuard.ability = newSigil.ability;
        }
        public static Ability ability;
		public bool ShouldTriggerWhenFaceDown(Trigger trigger, object[] objects)
        {
            if (trigger == Trigger.OtherCardResolve) return true;
			return false;
        }
        public bool ShouldTriggerCustomWhenFaceDown(Type customTrigger)
        {
            return false;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
		public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
		{
			return otherCard != null && otherCard.Slot != null && ((otherCard.Slot.IsPlayerSlot && !base.Card.Slot.IsPlayerSlot) || (!otherCard.Slot.IsPlayerSlot && base.Card.Slot.IsPlayerSlot)) && otherCard.Slot != base.Card.Slot.opposingSlot && !base.Card.HasAbility(Stalwart.ability);
		}

		public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
			yield return new WaitForSeconds(0.15f);
			CardSlot targetSlot = otherCard.Slot.opposingSlot;
			if (targetSlot.Card == null)
			{
				yield return base.PreSuccessfulTriggerSequence();
				Vector3 a = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;
				Tween.Position(base.Card.transform, a + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
				yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.1f, null, true);
				yield return new WaitForSeconds(0.3f);
				yield return base.LearnAbility(0.1f);
			}
			yield break;
		}

        
    }
}
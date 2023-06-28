using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class GemColourDependant : AbilityBehaviour
    {
        public override Ability Ability => Ability.None;
		public virtual bool reqSapphire => false;
		public virtual bool reqEmerald => false;
		public virtual bool reqRuby => false;
		public override bool RespondsToUpkeep(bool playerUpkeep) { return base.Card != null && base.Card.OnBoard && base.Card.OpponentCard != playerUpkeep && !base.Card.Dead && !this.HasGems(false); }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer) { return base.Card != null && base.Card.OnBoard && !base.Card.Dead && !this.HasGems(false); }
		public override bool RespondsToResolveOnBoard() { return base.Card != null && !base.Card.Dead && !this.HasGems(true); }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.Card.Die(false, null, true);
			yield break;
		}
        public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.Card.Die(false, null, true);
			yield break;
		}
		public override IEnumerator OnResolveOnBoard()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.Card.Die(false, null, true);
			yield return base.LearnAbility(0.25f);
			yield break;
		}
		private bool HasGems(bool onResolve)
        {
			return HasGem(onResolve, Ability.GainGemBlue, reqSapphire) && HasGem(onResolve, Ability.GainGemGreen, reqEmerald) && HasGem(onResolve, Ability.GainGemOrange, reqRuby);
		}
		private bool HasGem(bool onResolve, Ability sigil, bool req)
		{
			if (!req) { return true; }
			if (onResolve && base.Card.OpponentCard)
			{
				if (Singleton<TurnManager>.Instance.Opponent.Queue.Exists((PlayableCard x) => x != null && !x.Dead && (!req || x.HasAbility(sigil) || x.HasAbility(Ability.GainGemTriple)) && x.Slot.Card == null))
				{
					return true;
				}
			}
			return (base.Card.OpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy).Exists((CardSlot x) => x.Card != null && !x.Card.Dead && (!req || x.Card.HasAbility(sigil) || x.Card.HasAbility(Ability.GainGemTriple)));
		}
	}
}
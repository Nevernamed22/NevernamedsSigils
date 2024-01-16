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
    public class SquirrelDependent : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Squirrel Dependent", "If [creature]'s owner controls no Squirrels, [creature] will perish. If this card is a squirrel, it cannot support itself.",
                      typeof(SquirrelDependent),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/squirreldependent.png"),
					  pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/squirreldependent_pixel.png"));

            SquirrelDependent.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return base.Card != null && base.Card.OpponentCard != playerUpkeep && base.Card.OnBoard && !base.Card.Dead && !this.HasSquirrels(false);
		}
		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.Card.Die(false, null, true);
			yield break;
		}
		public override bool RespondsToResolveOnBoard()
		{
			return base.Card != null && !base.Card.Dead && !this.HasSquirrels(true);
		}
		public override IEnumerator OnResolveOnBoard()
		{
			yield return base.PreSuccessfulTriggerSequence();
			yield return base.Card.Die(false, null, true);
			yield return base.LearnAbility(0.25f);
			yield break;
		}
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return base.Card != null && !base.Card.Dead && base.Card.OnBoard &&  !this.HasSquirrels(false);
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.Card.Die(false, null, true);
            yield return base.LearnAbility(0.25f);
        }

       
        private bool HasSquirrels(bool onResolve)
		{
			if (onResolve)
			{
				bool flag;
				if (base.Card.OpponentCard)
				{
					flag = Singleton<TurnManager>.Instance.Opponent.Queue.Exists((PlayableCard x) => x != null && x.Info.tribes.Contains(Tribe.Squirrel) && x != base.Card && x.Slot.Card == null);
				}
				else
				{
					flag = false;
				}
				if (flag) return true;
			}
			List<CardSlot> list = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy;
			return list.Exists((CardSlot x) => x.Card != null && x.Card.Info.tribes.Contains(Tribe.Squirrel) && x.Card != base.Card);
		}
	}
}

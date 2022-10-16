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
    public class ChaosStrike : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Chaos Strike", "When [creature] is drawn, this sigil is replaced by a random strike modifier sigil.",
                      typeof(ChaosStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/chaosstrike.png"),
                      pixelTex: null);

            ChaosStrike.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
		public override bool RespondsToDrawn()
		{
			return true;
		}
		public override IEnumerator OnDrawn()
		{
			(Singleton<PlayerHand>.Instance as PlayerHand3D).MoveCardAboveHand(base.Card);
			yield return base.Card.FlipInHand(new Action(this.AddMod));
			yield return base.LearnAbility(0.5f);
			yield break;
		}
		private void AddMod()
		{
			base.Card.Status.hiddenAbilities.Add(this.Ability);
			CardModificationInfo cardModificationInfo = new CardModificationInfo(this.ChooseAbility());
			CardModificationInfo cardModificationInfo2 = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
			if (cardModificationInfo2 == null)
			{
				cardModificationInfo2 = base.Card.Info.Mods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
			}
			if (cardModificationInfo2 != null)
			{
				cardModificationInfo.fromTotem = cardModificationInfo2.fromTotem;
				cardModificationInfo.fromCardMerge = cardModificationInfo2.fromCardMerge;
			}
			base.Card.AddTemporaryMod(cardModificationInfo);
		}
		private Ability ChooseAbility()
		{
			List<Ability> validSigils = new List<Ability>()
			{ 
				Ability.SplitStrike,
				Ability.TriStrike,
				Ability.DoubleStrike,
				FringeStrike.ability,
				Trapjaw.ability,
				TripleStrike.ability,
				SwoopingStrike.ability
			};
			validSigils.RemoveAll((Ability x) => base.Card.HasAbility(x));

			return Tools.SeededRandomElement(validSigils, Tools.GetRandomSeed());
		}
	}
}
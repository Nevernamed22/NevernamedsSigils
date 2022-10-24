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
    public class Incontinent : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Incontinent", "When [creature] would be struck, a mess is created in its place and a card bearing this sigil moves to the right.",
                      typeof(Incontinent),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/incontinent.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/incontinent_pixel.png"));

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
		private bool Shitself;
		public override bool RespondsToCardGettingAttacked(PlayableCard source)
		{
			return source == base.Card && !this.Shitself;
		}
		public override IEnumerator OnCardGettingAttacked(PlayableCard card)
		{
			CardSlot slot = base.Card.Slot;
			CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
			CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
			bool flag = toLeft != null && toLeft.Card == null;
			bool toRightValid = toRight != null && toRight.Card == null;
			if (flag || toRightValid)
			{
				yield return base.PreSuccessfulTriggerSequence();
				yield return new WaitForSeconds(0.2f);
				if (toRightValid)
				{
					yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, toRight, 0.1f, null, true);
				}
				else
				{
					yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, toLeft, 0.1f, null, true);
				}
				base.Card.Anim.StrongNegationEffect();
				base.Card.Status.hiddenAbilities.Add(this.Ability);
				base.Card.RenderCard();

				Shitself = true;

				yield return new WaitForSeconds(0.2f);
				CardInfo info = CardLoader.GetCardByName("SigilNevernamed Mess");
				
				PlayableCard tail = CardSpawner.SpawnPlayableCardWithCopiedMods(info, base.Card, Incontinent.ability);
				tail.transform.position = slot.transform.position + Vector3.back * 2f + Vector3.up * 2f;
				tail.transform.rotation = Quaternion.Euler(110f, 90f, 90f);
				yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(tail, slot, 0.1f, null, true);
				Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
				yield return new WaitForSeconds(0.2f);
				tail.Anim.StrongNegationEffect();
				yield return base.StartCoroutine(base.LearnAbility(0.5f));
				yield return new WaitForSeconds(0.2f);
			}
			yield break;
		}

	}
}
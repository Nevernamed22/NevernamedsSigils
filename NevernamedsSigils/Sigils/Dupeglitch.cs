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
    public class Dupeglitch : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Dupeglitch", "When [creature] is played, you may search your deck for any card and create a duplicate of it in your hand.",
                      typeof(Dupeglitch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/dupeglitch.png"),
                      pixelTex: null);

            Dupeglitch.ability = newSigil.ability;
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
            return Singleton<CardDrawPiles>.Instance.Deck.CardsInDeck > 0;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            List<CardInfo> cards = Tools.CloneAllCardsInDeck();
            if (cards.Count > 0)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return SpecialCardSelectionHandler.DoSpecialCardSelectionDraw(cards, false);

                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
            }
            yield return base.LearnAbility(0.25f);
        }

    }
}

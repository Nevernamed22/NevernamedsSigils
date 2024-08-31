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
    public class Duppelgang : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Duppelgang", "When [creature] is played, draw copies of cards adjacent to it from your deck to your hand.",
                      typeof(Duppelgang),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/duppelgang.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/duppelgang_pixel.png"));

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
            return base.Card.slot && !base.Card.OpponentCard;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();           
            base.Card.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.1f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Hand);
                yield return new WaitForSeconds(0.1f);
            }

            List<CardInfo> toDraws = new List<CardInfo>();

            List<CardSlot> adjacents = Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot);
            foreach(CardSlot slot in adjacents)
            {
                if (slot.Card && Singleton<CardDrawPiles>.Instance.Deck.cards.Exists(y => y.name == slot.Card.Info.name))
                {
                    if (Singleton<CardDrawPiles>.Instance is CardDrawPiles3D) { (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).pile.Draw(); }
                    yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(Singleton<CardDrawPiles>.Instance.Deck.cards.Find(y => y.name == slot.Card.Info.name), null);
                }
            }

            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}
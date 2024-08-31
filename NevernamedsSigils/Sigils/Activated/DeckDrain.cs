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
    public class DeckDrain : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Deck Drain", "Up to 3 times per turn, pay 1 blood to draw a random card from your deck.",
                      typeof(DeckDrain),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/deckdrain.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/deckdrain_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override int BloodRequired()
        {
            return 1;
        }

        public override bool AdditionalActivationParameters()
        {
            return (activationsthisturn < 3) && (Singleton<CardDrawPiles>.Instance.Deck.cards.Count > 0);
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard;
        }
        public int activationsthisturn = 0;
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            activationsthisturn = 0;
            yield break;
        }
        public override IEnumerator OnBloodAbilityPostAllSacrifices()
        {
            yield return new WaitForSeconds(0.15f);
            activationsthisturn++;

            View preview = Singleton<ViewManager>.Instance.CurrentView;
            Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
            yield return new WaitForSeconds(0.1f);

            PlayableCard cardToDiscard = null;

            yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(null, delegate (PlayableCard x)
            {
                cardToDiscard = x;
            });
            if (Tools.GetActAsInt() != 2) Singleton<CardDrawPiles3D>.Instance.Pile.Draw();

            yield return new WaitForSeconds(0.6f);
            Singleton<ViewManager>.Instance.SwitchToView(preview, false, false);

            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

    }
}
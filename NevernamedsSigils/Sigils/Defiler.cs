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
    public class Defiler : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Defiler", "When [creature] perishes, its owner must draw a card from their deck and immediately discard it.",
                      typeof(Defiler),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/defiler.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/defiler_pixel.png"));

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

        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return !base.Card.OpponentCard && Singleton<CardDrawPiles>.Instance.Deck.cards.Count > 0;
        }

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();

            PlayableCard cardToDiscard = null;
            yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(null, delegate (PlayableCard x)
            {
                cardToDiscard = x;
            });

            yield return new WaitForSeconds(0.5f);
            if (cardToDiscard != null && cardToDiscard.InHand)
            {
                cardToDiscard.SetInteractionEnabled(false);
                cardToDiscard.Anim.PlayDeathAnimation(true);
                UnityEngine.Object.Destroy(cardToDiscard.gameObject, 1f);
                Singleton<PlayerHand>.Instance.RemoveCardFromHand(cardToDiscard);
            }

            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
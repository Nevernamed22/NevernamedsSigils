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
    public class DataBreach : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Data Breach", "When [creature] perishes, its owner must draw a card from their deck and immediately discard it. The opponent is dealt damage equal to the drawn creatures attack power.",
                      typeof(DataBreach),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/databreach.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/databreach_pixel.png"));

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

        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            yield return base.PreSuccessfulTriggerSequence();

            PlayableCard cardToDiscard = null;

            View preview = Singleton<ViewManager>.Instance.CurrentView;
            Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
            yield return new WaitForSeconds(0.1f);

            yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(null, delegate (PlayableCard x)
            {
                cardToDiscard = x;
            });
            if (Tools.GetActAsInt() != 2) Singleton<CardDrawPiles3D>.Instance.Pile.Draw();

            int dmg = 0;
            yield return new WaitForSeconds(0.6f);
            if (cardToDiscard != null && cardToDiscard.InHand)
            {
                dmg = cardToDiscard.Attack;
                cardToDiscard.SetInteractionEnabled(false);
                cardToDiscard.Anim.PlayDeathAnimation(true);
                UnityEngine.Object.Destroy(cardToDiscard.gameObject, 1f);
                Singleton<PlayerHand>.Instance.RemoveCardFromHand(cardToDiscard);
            }

            if (dmg > 0)
            {
                yield return new WaitForSeconds(0.6f);
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(dmg, dmg, !base.Card.slot.IsPlayerSlot, 0.1f, null, 0f, true);
            }
            yield return new WaitForSeconds(0.3f);
            Singleton<ViewManager>.Instance.SwitchToView(preview, false, false);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
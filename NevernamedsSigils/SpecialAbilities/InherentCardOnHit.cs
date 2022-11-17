using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class InherentCardOnHit : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "InherentCardOnHit", typeof(InherentCardOnHit)).Id;
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            string cardId = "Squirrel";
            if (base.Card.Info.GetExtendedProperty("InherentCardOnHitDef") != null) cardId = base.Card.Info.GetExtendedProperty("InherentCardOnHitDef");

            base.PlayableCard.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.4f);

            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }

            CardInfo cardByName = CardLoader.GetCardByName(cardId);
            cardByName.Mods.Add(base.PlayableCard.CondenseMods());

            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, null);

            yield break;
        }       
    }
}

using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class InherentGooey : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "InherentGooey", typeof(InherentGooey)).Id;
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Attack > 0 && !source.HasTrait(Trait.Giant);
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            source.AddTemporaryMod(new CardModificationInfo(-1, 0));
            yield break;
        }
    }
}

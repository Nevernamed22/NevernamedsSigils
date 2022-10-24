using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;using System.Collections.Generic;

using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class ContinualEvolution : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "ContinualEvolution", typeof(ContinualEvolution)).Id;
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.PlayableCard.OpponentCard != playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
            CardModificationInfo cardModificationInfo2 = new CardModificationInfo(1, 1);
            base.PlayableCard.temporaryMods.Add(cardModificationInfo2);
            yield return new WaitForSeconds(0.15f);
            yield break;
        }
    }
}

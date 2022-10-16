using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
   public  class InherentFecundity : SpecialCardBehaviour
    {
        public  static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Special Ability", typeof(SpecialCardBehaviour)).Id;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(base.PlayableCard.Info, null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);
            yield break;
        }
    }
}

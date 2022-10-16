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
    public class SigilShedder : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "SigilShedder", typeof(SigilShedder)).Id;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return new WaitForSeconds(0.1f);
            if (base.PlayableCard.FaceDown)
            {
                base.PlayableCard.SetFaceDown(false, true);
            }
            base.PlayableCard.Anim.PlayTransformAnimation();
            CardModificationInfo cardModificationInfo = new CardModificationInfo();
            cardModificationInfo.negateAbilities = new List<Ability>();
            List<Ability> mice = new List<Ability>();

            foreach (CardModificationInfo cardModificationInfo2 in base.PlayableCard.TemporaryMods)
            {
                cardModificationInfo.negateAbilities.AddRange(cardModificationInfo2.abilities);
                mice.AddRange(cardModificationInfo2.abilities);
            }
            cardModificationInfo.negateAbilities.AddRange(base.PlayableCard.Info.Abilities);
            mice.AddRange(base.PlayableCard.Info.Abilities);
            base.PlayableCard.AddTemporaryMod(cardModificationInfo);

            if (mice.Count > 0)
            {
            foreach (Ability ability in mice)
            {
                if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                    yield return new WaitForSeconds(0.2f);
                }

                CardInfo mouse = CardLoader.GetCardByName("Nevernamed Ratling");
                CardModificationInfo newsigil = new CardModificationInfo(ability);
                newsigil.fromCardMerge = true;
                mouse.mods.Add(newsigil);

                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(mouse, null, 0.25f, null);
                yield return new WaitForSeconds(0.1f);
            }
            }
            else
            {
                if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Nevernamed Ratling"), null, 0.25f, null);
                yield return new WaitForSeconds(0.1f);
            }

        }
    }
}
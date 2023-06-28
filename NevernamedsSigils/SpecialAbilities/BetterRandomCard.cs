using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using GBC;

using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class BetterRandomCard : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "BetterRandomCard", typeof(BetterRandomCard)).Id;
        }
        public override bool RespondsToDrawn()
        {
            return true;
        }
        public override IEnumerator OnDrawn()
        {
            if (Singleton<PlayerHand>.Instance is PlayerHand3D)
            {
                (Singleton<PlayerHand>.Instance as PlayerHand3D).MoveCardAboveHand(base.PlayableCard);
                yield return new WaitForSeconds(0.5f);
            }
            if (Singleton<UIManager>.Instance != null && Singleton<UIManager>.Instance.Effects.GetEffect<ScreenGlitchEffect>() != null)
            {
                Singleton<UIManager>.Instance.Effects.GetEffect<ScreenGlitchEffect>().SetIntensity(1f, 0.2f);
            }
            else if (Singleton<GBC.CameraEffects>.Instance != null)
            {
                Singleton<GBC.CameraEffects>.Instance.EffectsLayer.GetEffect<ScreenGlitchEffect>().SetIntensity(1f, 0.3f);
            }
            AudioController.Instance.PlaySound2D("glitch", MixerGroup.None, 1f, 0f, null, null, null, null, false);
            base.Card.Anim.LightNegationEffect();
            if (Tools.GetActAsInt() == 2)
            {
                CardInfo randomcard = Tools.GetRandomCardOfTempleAndQuality(
                    Tools.RandomElement(new List<CardTemple>() { CardTemple.Nature, CardTemple.Tech, CardTemple.Undead, CardTemple.Wizard }),
                    2, false, Tribe.None, false);

                int inc = 1;
                while (randomcard.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard))
                {
                    randomcard = Tools.GetRandomCardOfTempleAndQuality(
                Tools.RandomElement(new List<CardTemple>() { CardTemple.Nature, CardTemple.Tech, CardTemple.Undead, CardTemple.Wizard }),
                2, false, Tribe.None, false, null, inc);
                    inc++;
                }

                base.PlayableCard.ClearAppearanceBehaviours();
                base.PlayableCard.SetInfo(randomcard);
            }
            else if (Tools.GetActAsInt() == 3)
            {
                CardInfo randomcard = Tools.GetRandomCardOfTempleAndQuality(
                    CardTemple.Tech,
                    3, false, Tribe.None, false);

                int inc = 1;
                while (randomcard.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard))
                {
                   randomcard = Tools.GetRandomCardOfTempleAndQuality(
                    CardTemple.Tech,
                    3, false, Tribe.None, false, null, inc);
                    inc++;
                }

                base.PlayableCard.ClearAppearanceBehaviours();
                base.PlayableCard.SetInfo(randomcard);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                CardInfo randomcard = Tools.GetRandomCardOfTempleAndQuality(
                    CardTemple.Undead,
                    4, false, Tribe.None, false);

                int inc = 1;
                while (randomcard.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard))
                {
                    randomcard = Tools.GetRandomCardOfTempleAndQuality(
                     CardTemple.Undead,
                     4, false, Tribe.None, false, null, inc);
                    inc++;
                }

                base.PlayableCard.ClearAppearanceBehaviours();
                base.PlayableCard.SetInfo(randomcard);
                yield return new WaitForSeconds(0.5f);
            }
            yield break;
        }
    }
}

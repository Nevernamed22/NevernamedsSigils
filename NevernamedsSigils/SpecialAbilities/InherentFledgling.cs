using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class InherentFledgling : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "InherentFledgling", typeof(InherentFledgling)).Id;
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.PlayableCard.OpponentCard != playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            int num = (base.Card.Info.evolveParams != null) ? base.Card.Info.evolveParams.turnsToEvolve : 1;
            this.numTurnsInPlay++;
            int num2 = Mathf.Max(1, num - this.numTurnsInPlay);
            if (this.numTurnsInPlay >= num)
            {
                CardInfo evolution = this.GetTransformCardInfo();
                foreach (CardModificationInfo cardModificationInfo in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo cardModificationInfo2 = (CardModificationInfo)cardModificationInfo.Clone();
                    if (cardModificationInfo2.HasAbility(Ability.Evolve))
                    {
                        cardModificationInfo2.abilities.Remove(Ability.Evolve);
                    }
                    evolution.Mods.Add(cardModificationInfo2);
                }

                if (Card.Info.GetExtendedProperty("InherentFledglingGlitcher") != null)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.1f);
                    Tween.Shake(base.PlayableCard.transform, base.PlayableCard.transform.localPosition, Vector3.one * 0.2f, 0.2f, 0f, Tween.LoopType.None, null, null, false);
                    AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX, 1f, 0f, null, null, null, null, false);
                    yield return new WaitForSeconds(0.1f);
                    base.PlayableCard.SetInfo(evolution);
                }
                else
                {
                    yield return base.PlayableCard.TransformIntoCard(evolution, null, null);
                }

                yield return new WaitForSeconds(0.15f);
                evolution = null;
            }
            yield break;
        }
        protected virtual CardInfo GetTransformCardInfo()
        {
            if (base.Card.Info.evolveParams == null)
            {
                return EvolveParams.GetDefaultEvolution(base.Card.Info);
            }
            return base.Card.Info.evolveParams.evolution.Clone() as CardInfo;
        }
        private int numTurnsInPlay;
    }
}

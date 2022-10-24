using APIPlugin;
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
    public class Doomed : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doomed.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Doomed", "At the end of the turn, [creature] will perish.",
                      typeof(Doomed),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: baseIcon,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/doomed_pixel.png"));

            Doomed.ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doomed2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doomed3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doomed4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/doomed5.png") },
            };
        }
        public static Ability ability;
        public static Dictionary<int, Texture> countDownIcons;
        public static Texture baseIcon;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        private void ReRenderCard(int num)
        {
            base.Card.RenderInfo.OverrideAbilityIcon(Doomed.ability, countDownIcons.ContainsKey(num) ? countDownIcons[num] : baseIcon);
            base.Card.RenderCard();
        }
        private int LifeSpan
        {
            get
            {
                int customLifespan = 1;
                if (base.Card.Info.GetExtendedProperty("CustomDoomedDuration") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomDoomedDuration"), out customLifespan);
                    customLifespan = succeed ? customLifespan : 1;
                }
                return customLifespan;
            }
        }
        public override bool RespondsToDrawn()
        {
            return true;
        }
        public override IEnumerator OnDrawn()
        {
            ReRenderCard(LifeSpan);
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            ReRenderCard(LifeSpan);
            yield break;
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card.slot.IsPlayerSlot == playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            livedTurns++;
            int life = LifeSpan;
            int lifeRemaining = Mathf.Max(1, life - livedTurns);
            ReRenderCard(lifeRemaining);
            if (livedTurns >= life)
            {
                yield return base.PreSuccessfulTriggerSequence();
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.15f);

                yield return base.Card.Die(false, null, false);

                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
        private int livedTurns;
    }
}

using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Soak : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak.png");
            basePixelIcon = Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak_pixel.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Soak", "Any damage taken by [creature] is reduced by 1. Taking damage decrements the timer rendered on this sigil. When the timer rendered on this sigil reaches 0, the bearer will attack with power equal to it's health, then perish.",
                      typeof(Soak),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: baseIcon,
                      pixelTex: basePixelIcon);

            Soak.ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak1.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak5.png") },
                {6, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak6.png") },
                {7, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak7.png") },
                {8, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak8.png") },
                {9, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/soak9.png") },
            };
            countDownPixelIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak1_pixel.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak2_pixel.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak3_pixel.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak4_pixel.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak5_pixel.png") },
                {6, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak6_pixel.png") },
                {7, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak7_pixel.png") },
                {8, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak8_pixel.png") },
                {9, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/soak9_pixel.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Dictionary<int, Texture> countDownPixelIcons;
        public static Texture baseIcon;
        public static Texture2D basePixelIcon;
        public static Ability ability;
        private bool initialised;
        private int hitsRemaining;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        private int Counter
        {
            get
            {
                int customLifespan = 4;
                if (base.Card.Info.GetExtendedProperty("CustomSoakCounter") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomSoakCounter"), out customLifespan);
                    customLifespan = succeed ? customLifespan : 4;
                }
                return customLifespan;
            }
        }
        private IEnumerator Initialise()
        {
            hitsRemaining = Counter;
            initialised = true;
            ReRenderCard();
            yield break;
        }
        private void ReRenderCard()
        {
            if (Tools.GetActAsInt() == 2)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Soak.ability, countDownPixelIcons.ContainsKey(hitsRemaining) ? countDownPixelIcons[hitsRemaining] : basePixelIcon);
            }
            else
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Soak.ability, countDownIcons.ContainsKey(hitsRemaining) ? countDownIcons[hitsRemaining] : baseIcon);
            }
            base.Card.RenderCard();
        }
        public override bool RespondsToDrawn()
        {
            return true;
        }
        public override IEnumerator OnDrawn()
        {
            if (!initialised) { yield return Initialise(); }
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (!initialised) { yield return Initialise(); }
            yield break;
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (!initialised) { yield return Initialise(); }
            if (hitsRemaining > 0)
            {
                hitsRemaining--;
                ReRenderCard();
                if (hitsRemaining == 0)
                {
                    yield return new WaitForSeconds(0.2f);
                    FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                    yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot, null, base.Card.Health);
                    yield return new WaitForSeconds(0.2f);
                    if (base.Card.NotDead())
                    {
                        yield return base.Card.Die(false, null, true);
                    }
                }
            }
            yield break;
        }
    }
}
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
    public class Docile : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile.png");
            basePixelIcon = Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile_pixel.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Docile", "[creature] will wait a set number of turns in between being allowed to attack.",
                      typeof(Docile),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: baseIcon,
                      pixelTex: basePixelIcon);

            ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {0, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile0.png") },
                {1, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile1.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/docile5.png") },
            };
            countDownPixelIcons = new Dictionary<int, Texture>()
            {
                {0, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile0_pixel.png") },
                {1, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile1_pixel.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile2_pixel.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile3_pixel.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile4_pixel.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/docile5_pixel.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Dictionary<int, Texture> countDownPixelIcons;
        public static Texture baseIcon;
        public static Texture2D basePixelIcon;
        public static Ability ability;
        private bool initialised;

        public int turnsUntilNextAttack;
        public int cooldownTotal;
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
                int customLifespan = 1;
                if (base.Card.Info.GetExtendedProperty("CustomDocileCounter") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomDocileCounter"), out customLifespan);
                    customLifespan = succeed ? customLifespan : 1;
                }
                return customLifespan;
            }
        }
        private IEnumerator Initialise()
        {
            turnsUntilNextAttack = 0;
            cooldownTotal = Counter;
            initialised = true;
            ReRenderCard();
            yield break;
        }
        private void ReRenderCard()
        {
            if (Tools.GetActAsInt() == 2)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Docile.ability, countDownPixelIcons.ContainsKey(turnsUntilNextAttack) ? countDownPixelIcons[turnsUntilNextAttack] : basePixelIcon);
            }
            else
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Docile.ability, countDownIcons.ContainsKey(turnsUntilNextAttack) ? countDownIcons[turnsUntilNextAttack] : baseIcon);
            }
            base.Card.RenderCard();
        }
        public override bool RespondsToDrawn() { return true; }
        public override IEnumerator OnDrawn()
        {
            if (!initialised) { yield return Initialise(); }
            yield break;
        }
        public override bool RespondsToResolveOnBoard() { return true; }
        public override IEnumerator OnResolveOnBoard()
        {
            if (!initialised) { yield return Initialise(); }
            yield break;
        }


        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (turnsUntilNextAttack == 0)
            {
                turnsUntilNextAttack = cooldownTotal;
                ReRenderCard();
            }
            else
            {
                turnsUntilNextAttack--;
                ReRenderCard();
            }
            yield break;
        }
    }
}
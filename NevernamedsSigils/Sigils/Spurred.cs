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
    public class Spurred : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/spurred.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Spurred", "[creature] gains 1 power while the slot opposing it is occupied.",
                      typeof(Spurred),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.BountyHunter },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: baseIcon,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/spurred_pixel.png"));

            Spurred.ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {             
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/spurred2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/spurred3.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Texture baseIcon;
        public static Ability ability;
        private bool initialised;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public int BuffAmount
        {
            get
            {
                int customLifespan = 1;
                if (base.Card.Info.GetExtendedProperty("CustomSpurredBuff") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomSpurredBuff"), out customLifespan);
                    customLifespan = succeed ? customLifespan : 1;
                }
                return customLifespan;
            }
        }
        private IEnumerator Initialise()
        {
            initialised = true;
            ReRenderCard();
            yield break;
        }
        private void ReRenderCard()
        {
            base.Card.RenderInfo.OverrideAbilityIcon(Spurred.ability, countDownIcons.ContainsKey(BuffAmount) ? countDownIcons[BuffAmount] : baseIcon);
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
       
    }
}
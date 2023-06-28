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
    public class Draw : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/draw.png");
            basePixelIcon = Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/draw_pixel.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Draw", "When [creature] is played, its owner draws cards equal to the number inscribed on this sigil.",
                      typeof(Draw),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.GrimoraRulebook, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, Plugin.GrimoraModChair1 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: baseIcon,
                      pixelTex: basePixelIcon);

            ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/draw.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/draw2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/draw3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/draw4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/draw5.png") },
            };
            countDownPixelIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/draw_pixel.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/draw2_pixel.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/draw3_pixel.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/draw4_pixel.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/draw5_pixel.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Dictionary<int, Texture> countDownPixelIcons;
        public static Texture baseIcon;
        public static Texture2D basePixelIcon;
        public static Ability ability;
        private bool initialised;
        public int numToDraw;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        private int NumToDraw
        {
            get
            {
                int customLifespan = 1;
                if (base.Card.Info.GetExtendedProperty("DrawSigilNumberToDraw") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("DrawSigilNumberToDraw"), out customLifespan);
                    customLifespan = succeed ? customLifespan : 1;
                }
                return customLifespan;
            }
        }
        private IEnumerator Initialise()
        {
            numToDraw = NumToDraw;
            initialised = true;
            ReRenderCard();
            yield break;
        }
        private void ReRenderCard()
        {
            if (Tools.GetActAsInt() == 2)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Draw.ability, countDownPixelIcons.ContainsKey(numToDraw) ? countDownPixelIcons[numToDraw] : basePixelIcon);
            }
            else
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Draw.ability, countDownIcons.ContainsKey(numToDraw) ? countDownIcons[numToDraw] : baseIcon);
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

            base.Card.Anim.LightNegationEffect();
            yield return new WaitForSeconds(0.1f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Hand);
                yield return new WaitForSeconds(0.1f);
            }
            int num = NumToDraw;
            for (int i = 0; i < NumToDraw; i = num + 1)
            {
                if (Singleton<CardDrawPiles>.Instance is CardDrawPiles3D)
                {
                    (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).pile.Draw();
                }
                yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(null, null);
            }
            yield break;
        }      
    }
}
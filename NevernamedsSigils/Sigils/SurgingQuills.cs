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
    public class SurgingQuills : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills.png");
            basePixelIcon = Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills_pixel.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Surging Quills", "When [creature] is struck, the striker is dealt damage equal to the amount of times this has been struck.",
                      typeof(SurgingQuills),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: baseIcon,
                      pixelTex: basePixelIcon);

            ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills5.png") },
                {6, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills6.png") },
                {7, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills7.png") },
                {8, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills8.png") },
                {9, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills9.png") },
                {10, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/surgingquills10.png") },
            };
            countDownPixelIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills_pixel.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills2_pixel.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills3_pixel.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills4_pixel.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills5_pixel.png") },
                {6, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills6_pixel.png") },
                {7, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills7_pixel.png") },
                {8, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills8_pixel.png") },
                {9, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills9_pixel.png") },
                {10, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills10_pixel.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Dictionary<int, Texture> countDownPixelIcons;
        public static Texture baseIcon;
        public static Texture2D basePixelIcon;
        public static Ability ability;
        private int hitstaken = 1;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        private void ReRenderCard()
        {
            int capped = Math.Min(10, hitstaken);
            if (Tools.GetActAsInt() == 2)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(SurgingQuills.ability, countDownPixelIcons.ContainsKey(capped) ? countDownPixelIcons[capped] : basePixelIcon);
            }
            else
            {
                base.Card.RenderInfo.OverrideAbilityIcon(SurgingQuills.ability, countDownIcons.ContainsKey(capped) ? countDownIcons[capped] : baseIcon);
            }
            base.Card.RenderCard();
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            if (source != null && source.Health > 0) yield return source.TakeDamage(hitstaken, base.Card);
            yield return base.LearnAbility(0.4f);

            if (base.Card.NotDead())
            {
                hitstaken++;
                ReRenderCard();
            }
            yield break;
        }
    }
}
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
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Surging Quills", "When [creature] is struck, the striker is dealt damage equal to the amount of times the bearer has been struck.",
                      typeof(SurgingQuills),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: baseIcon,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/surgingquills_pixel.png"));

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
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Texture baseIcon;
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
            base.Card.RenderInfo.OverrideAbilityIcon(SurgingQuills.ability, countDownIcons.ContainsKey(capped) ? countDownIcons[capped] : baseIcon);
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
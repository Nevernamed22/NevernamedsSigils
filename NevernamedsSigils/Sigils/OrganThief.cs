using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class OrganThief : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Organ Thief", "When [creature] kills another creature, Guts are created in your hand. Guts are defined as 0 power, 1 health.",
                      typeof(OrganThief),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/organthief_pixel.png"));

            OrganThief.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card;
        }
        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo guts = CardLoader.GetCardByName("NevernamedGuts");             
                return guts;
            }
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.3f);

            yield return base.CreateDrawnCard();

            if (!base.Card.Dead)
            {
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

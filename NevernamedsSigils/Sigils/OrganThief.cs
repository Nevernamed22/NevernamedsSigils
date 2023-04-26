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
    public class OrganThief : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Organ Thief", "When [creature] kills another creature, it's remains are created in your hand.",
                      typeof(OrganThief),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/organthief.png"),
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
                CardInfo guts = (base.Card.Info.GetExtendedProperty("OrganThiefGutOverride") != null) ? CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("OrganThiefGutOverride")) : CardLoader.GetCardByName("SigilNevernamed Guts");
                if (lastKilled != null) guts.Mods.Add(lastKilled);
                return guts;
            }
        }
        private CardModificationInfo lastKilled;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            lastKilled = card.CondenseMods();
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

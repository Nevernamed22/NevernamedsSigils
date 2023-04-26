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
    public class Bisection : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bisection", "When [creature] kills another creature, two copies of the victim are created in the owner's hand with zero power and halved health.",
                      typeof(Bisection),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bisection.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bisection_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            victim = CardLoader.Clone(card.Info);




            CardModificationInfo mod = new CardModificationInfo(victim.Attack * -1, 0);

            mod.decalIds = new List<string>()
            {
                AlternatingBloodDecal.GetBloodDecalId(),
                "decal_stitches"
            };

            if (!victim.HasTrait(Trait.Terrain) && !victim.HasTrait(Trait.Pelt)) mod.nameReplacement = victim.displayedName + " Corpse";

            victim.cost = 0;
            victim.bonesCost = 0;
            victim.energyCost = 0;

            victim.gemsCost = new List<GemType>() { };

            victim.baseHealth = Math.Max(Mathf.FloorToInt(((float)(victim.Health * 0.5) * -1)), 1);
            victim.mods.Add(mod);

            yield return base.CreateDrawnCard();
            yield return base.CreateDrawnCard();

            yield return base.LearnAbility(0.4f);
            yield break;
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return base.Card.OnBoard && killer == base.Card && !card.HasAbility(FatalFlank.ability) && !card.HasTrait(Trait.Giant) && !card.HasTrait(Trait.Uncuttable);
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        private CardInfo victim;
        public override CardInfo CardToDraw
        {
            get
            {
                return victim;
            }
        }
        public override List<CardModificationInfo> CardToDrawTempMods
        {
            get
            {
                return null;
            }
        }
    }
}
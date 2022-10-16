using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class HartsWithin : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Harts Within", "When [creature] is struck, a random creature from the hooved tribe is created in your hand.",
                      typeof(HartsWithin),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 3,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/hartswithin.png"),
                      pixelTex: null);

            HartsWithin.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo gift = Tools.GetRandomCardOfTempleAndQuality(CardTemple.Nature, 1, UnityEngine.Random.value <= 0.1f ? true : false, Tribe.Hooved, false, new List<string>() { "Nevernamed HeartOfHarts" }).Clone() as CardInfo;
                gift.Mods.Add(base.Card.CondenseMods(new List<Ability>() { HartsWithin.ability }));
                return gift;
            }
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.CreateDrawnCard();
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}
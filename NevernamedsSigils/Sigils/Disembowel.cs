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
    public class Disembowel : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Disembowel", "When [creature] is sacrificed, an amount of guts equal to its remaining health are created in its owners hand.",
                      typeof(Disembowel),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/disembowel.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/disembowel_pixel.png"),
                      triggerText: "[creature]s guts explode into your hands."
                      );

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToSacrifice()
        {
            return true;
        }
        public override IEnumerator OnSacrifice()
        {
            //Debug.Log($"Ran ({base.Card.Health})");
            yield return base.PreSuccessfulTriggerSequence();
            for (int i = 0; i < base.Card.Health; i++)
            {
                yield return base.CreateDrawnCard();
            }
            yield return base.LearnAbility(0f);
            yield break;
        }

        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo guts = (base.Card.Info.GetExtendedProperty("DisembowelGutOverride") != null) ? CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("DisembowelGutOverride")) : CardLoader.GetCardByName("SigilNevernamed Guts");
                guts.mods.Add(base.Card.CondenseMods(new List<Ability>() { Disembowel.ability }, true));
                return guts;
            }
        }
    }
}
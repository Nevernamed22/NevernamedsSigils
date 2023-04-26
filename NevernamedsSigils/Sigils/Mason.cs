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
    public class Mason : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Mason", "When [creature] is played, a random obelisk is created in your hand.",
                      typeof(Mason),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/mason.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/mason_pixel.png"));

            Mason.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo obelisk = null;

                List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.HasTrait(NevernamedsTraits.Obelisk));
                if (cards != null && cards.Count > 0)
                {
                    obelisk = Tools.RandomElement(cards);
                }
                if (obelisk == null)
                {
                    obelisk = CardLoader.GetCardByName("Boulder");
                }
                return obelisk;
            }
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.CreateDrawnCard();
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}

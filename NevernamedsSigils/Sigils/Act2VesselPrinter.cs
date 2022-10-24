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
    public class Act2VesselPrinter : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Vessel Printer", "When [creature] is struck, an empty vessel is created in your hand. An empty vessel is defined as 0 power, 2 health",
                      typeof(Act2VesselPrinter),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/vesselprinter_pixel.png"));

            Act2VesselPrinter.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override CardInfo CardToDraw
        {
            get
            {
                return CardLoader.GetCardByName("SigilNevernamed Act2EmptyVessel");
            }
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
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

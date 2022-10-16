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
    public class DeckedOut : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Decked Out", "When [creature] is struck, a card from a random temple's side deck is created in your hand.",
                      typeof(DeckedOut),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/deckedout_pixel.png"));

            DeckedOut.ability = newSigil.ability;
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
                int sel = UnityEngine.Random.Range(1, 5);
                CardInfo card =  CardLoader.GetCardByName("Squirrel");
                switch (sel)
                {
                    case 2:
                        card = CardLoader.GetCardByName("Nevernamed Act2EmptyVessel");
                        break;
                    case 3:
                        card = CardLoader.GetCardByName("Skeleton");
                        break;
                    case 4:
                        List<string> moxes = new List<string>() { "MoxEmerald", "MoxRuby", "MoxSapphire" };
                        card = CardLoader.GetCardByName(Tools.RandomElement(moxes));
                        break;
                }
                return card;
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

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
    public class GiftBearerCustom : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gift Bearer", "When [creature] perishes, a random creature is created in your hand.",
                      typeof(GiftBearerCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/giftbearer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/giftbearer_pixel.png"));

            GiftBearerCustom.ability = newSigil.ability;
        }

        public static Dictionary<string, List<string>> presetcardpools = new Dictionary<string, List<string>>()
        {
            {"Nevernamed AderynYCorph" ,  new List<string>(){ "Nevernamed FauxKaycee", "Nevernamed FauxReginald", "Nevernamed FauxLouis", "Nevernamed FauxKaminski", "Nevernamed FauxJonah", "Nevernamed FauxKevin", "Nevernamed FauxSean", "Nevernamed FauxTamara", "Nevernamed FauxDaniel", "Nevernamed FauxCody", "Nevernamed FauxDavid", "Nevernamed FauxTahnee", "Nevernamed FauxBerke", "Nevernamed FauxLuke", "Nevernamed FauxYou" } }
        };

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public override CardInfo CardToDraw
        {
            get
            {

                CardInfo result = CardLoader.GetCardByName("Nevernamed Guts");
                if (presetcardpools.ContainsKey(base.Card.Info.name))
                {
                    List<string> cardList = presetcardpools[base.Card.Info.name];
                    if (cardList != null && cardList.Count > 0)
                    {
                        int index = SeededRandom.Range(0, cardList.Count - 1, base.GetRandomSeed());
                        CardInfo gift =  CardLoader.GetCardByName(cardList[index]);
                        gift.Mods.Add(base.Card.CondenseMods(new List<Ability>() { GiftBearerCustom.ability }));
                        result = gift;
                    }
                }
                else
                {
                    bool isRare = false;
                    if (base.Card.Info.name == "Nevernamed Gruagach") isRare = true;
                    CardInfo gift = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), isRare, Tribe.None, false).Clone() as CardInfo;
                    gift.Mods.Add(base.Card.CondenseMods(new List<Ability>() { GiftBearerCustom.ability }));
                    result = gift;
                }
                return result;
            }
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.CreateDrawnCard();
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}
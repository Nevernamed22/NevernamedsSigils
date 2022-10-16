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
    public class GutSpewer : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gut Spewer", "When [creature] is played, Guts are created in your hand. Guts are defined as 0 power, 1 health.",
                      typeof(GutSpewer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gutspewer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/gutspewer_pixel.png"));

            GutSpewer.ability = newSigil.ability;
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
                CardInfo guts = CardLoader.GetCardByName("Nevernamed Guts");
                if (base.Card != null)
                {
                    List<Ability> abilities = base.Card.Info.Abilities;
                    foreach (CardModificationInfo cardModificationInfo in base.Card.TemporaryMods)
                    {
                        abilities.AddRange(cardModificationInfo.abilities);
                    }
                    abilities.RemoveAll((Ability x) => x == GutSpewer.ability);

                    if (abilities.Count > 0)
                    {
                        CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
                        cardModificationInfo2.fromCardMerge = true;
                        cardModificationInfo2.abilities = abilities;
                        guts.Mods.Add(cardModificationInfo2);
                    }
                }
                return guts;
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

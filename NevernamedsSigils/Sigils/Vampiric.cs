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
    public class Vampiric : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Vampiric", "When [creature] attacks an opposing creature and it perishes, this card gains 1 health.",
                      typeof(Vampiric),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/vampiric.png"),
                      pixelTex: null);

            Vampiric.ability = newSigil.ability;
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
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (base.Card.Info.name == "Nevernamed Biscione")
            {
                CardModificationInfo cardModificationInfo = base.Card.Info.Mods.Find((CardModificationInfo x) => x.singletonId == "biscione");
                if (cardModificationInfo == null)
                {
                    cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.singletonId = "biscione";
                    RunState.Run.playerDeck.ModifyCard(base.Card.Info, cardModificationInfo);
                }
                cardModificationInfo.healthAdjustment++;
            }
            else
            {
                base.Card.temporaryMods.Add(new CardModificationInfo(0, 1));
            }
            yield return base.LearnAbility(0.25f);
        }
    }
}

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
    public class TrainedSwimmer : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trained Swimmer", "Creature will gain the effect of the Waterborne sigil for one turn.",
                      typeof(TrainedSwimmer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/trainedswimmer.png"),
                     pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/trainedswimmer_pixel.png"),
                      isActivated: true);

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            base.Card.Status.hiddenAbilities.Remove(Ability.Submerge);
            base.Card.RenderCard();
            usedThisTurn = false;
            if (base.Card.OnBoard && base.Card.slot.opposingSlot && base.Card.OpponentCard)
            {
                bool shouldFly = false;
                if (base.Card.slot.opposingSlot.Card != null && base.Card.slot.opposingSlot.Card.Attack  > base.Card.Health) { shouldFly = true; }                
                if (shouldFly && !base.Card.HasAbility(Ability.Submerge))
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Submerge);
                    cardModificationInfo.RemoveOnUpkeep = true;
                    base.Card.Status.hiddenAbilities.Add(Ability.Submerge);
                    base.Card.AddTemporaryMod(cardModificationInfo);
                }
                base.Card.RenderCard();
            }
            yield break;
        }
        private bool usedThisTurn;
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (!usedThisTurn)
            {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Submerge);
                    cardModificationInfo.RemoveOnUpkeep = true;
                    base.Card.Status.hiddenAbilities.Add(Ability.Submerge);
                    base.Card.AddTemporaryMod(cardModificationInfo);
                base.Card.RenderCard();
                usedThisTurn = true;
            }
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}

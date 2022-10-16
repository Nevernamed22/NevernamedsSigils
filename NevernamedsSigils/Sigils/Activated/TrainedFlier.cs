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
    public class TrainedFlier : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trained Flier", "Toggle [creature]'s ability to fly.",
                      typeof(TrainedFlier),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/trainedflier.png"),
                      pixelTex: null,
                      isActivated: true);

            TrainedFlier.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        bool hasBeenToggledOnce = false;
        bool removeflierfromhidden = false;
        CardModificationInfo flying = new CardModificationInfo(Ability.Flying);
        CardModificationInfo negateFlying = new CardModificationInfo() { negateAbilities = new List<Ability>() { Ability.Flying } };
        public override IEnumerator Activate()
        {
            if (base.Card.OpponentCard) yield break;
            bool botherHidingFlight = true;
            if (!hasBeenToggledOnce)
            {
                if (base.Card.HasAbility(Ability.Flying)) botherHidingFlight = false;
            }
            hasBeenToggledOnce = true;
            if (base.Card && !base.Card.Dead)
            {
                if (base.Card.HasAbility(Ability.Flying))
                {
                    //Debug.Log("Card has flying");
                    if (botherHidingFlight) base.Card.Status.hiddenAbilities.Add(Ability.Flying);
                    base.Card.RemoveTemporaryMod(flying);
                    base.Card.AddTemporaryMod(negateFlying);
                }
                else
                {
                    //Debug.Log("Card does not have flying");
                    if (botherHidingFlight) base.Card.Status.hiddenAbilities.Add(Ability.Flying);
                    base.Card.RemoveTemporaryMod(negateFlying);
                    base.Card.AddTemporaryMod(flying);
                }
                base.Card.RenderCard();

            }
            yield break;
        }
    }
}

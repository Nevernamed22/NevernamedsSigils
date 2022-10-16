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
    public class EnergyDependent : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Energy Dependent", "If the user has 0 Energy, [creature] will perish.",
                      typeof(EnergyDependent),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/energydependent.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/energydependent_pixel.png"));

            EnergyDependent.ability = newSigil.ability;
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
            return base.Card != null && base.Card.OpponentCard != playerUpkeep && !base.Card.Dead && ResourcesManager.Instance.PlayerEnergy == 0;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.Card.Die(false, null, true);
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card != null && !base.Card.Dead && ResourcesManager.Instance.PlayerEnergy == 0;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.Card.Die(false, null, true);
            yield return base.LearnAbility(0.25f);
            yield break;
        }         
    }
}

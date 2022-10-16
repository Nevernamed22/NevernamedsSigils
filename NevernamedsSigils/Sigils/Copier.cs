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
    public class Copier : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Copier", "When [creature] perishes, a copy of it's murderer is created in your hand.",
                      typeof(Copier),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/copier.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/copier_pixel.png"));

            Copier.ability = newSigil.ability;
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
                return storedKiller.Info;
            }
        }
        public override List<CardModificationInfo> CardToDrawTempMods
        {
            get
            {
                return null;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        private PlayableCard storedKiller;
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (killer != null && !wasSacrifice && !killer.Info.traits.Contains(Trait.Uncuttable))
            {
                storedKiller = killer;
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.CreateDrawnCard();
                yield return base.LearnAbility(0.5f);
            }
            yield break;
        }
    }
}

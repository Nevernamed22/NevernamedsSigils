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
    public class PickyEater : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Picky Eater", "Pay 1 blood to draw bones equal to the health of the sacrificed creature.",
                      typeof(PickyEater),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/pickyeater.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/pickyeater_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override int BloodRequired()
        {
            return 1;
        }
        public override IEnumerator OnIndividualCardPreSacrifice(PlayableCard sacrifice)
        {
            if (sacrifice && sacrifice.slot)
            {
                yield return new WaitForSeconds(0.15f);
                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<ResourcesManager>.Instance.AddBones(sacrifice.Health, sacrifice.slot);
            }
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

    }
}
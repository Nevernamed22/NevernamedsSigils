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
    public class Carnivore : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Carnivore", "Pay 2 blood to increase the power of [creature] by 3.",
                      typeof(Carnivore),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/carnivore.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/carnivore_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override int BloodRequired()
        {
            return 2;
        }
        public override IEnumerator OnBloodAbilityPostAllSacrifices()
        {
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
           base.Card.temporaryMods.Add(new CardModificationInfo(3, 0));
            yield return new WaitForSeconds(0.15f);
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
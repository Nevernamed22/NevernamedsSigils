using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using OpponentBones;

namespace NevernamedsSigils
{   
    public class Crossbones : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Crossbones", "When [creature] is played, it grants 2 bones to it's owner.",
                      typeof(Crossbones),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair1 },
                      powerLevel: 1,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/crossbones.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/crossbones_pixel.png"));

            Crossbones.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToResolveOnBoard()
        {
            return !base.Card.OpponentCard;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.55f);
            yield return Singleton<ResourcesManager>.Instance.AddBones(2, base.Card.Slot);
            yield return base.LearnAbility(0.4f);
            yield break;
        }        
    }
}

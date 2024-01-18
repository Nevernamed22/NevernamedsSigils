using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GraveyardHandler;

namespace NevernamedsSigils
{
    public class Magitech : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Magitech", "While [creature] is alive and on the board, playing a mox gem will provide its owner with an energy cell.",
                      typeof(Magitech),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular  },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/magitech.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/magitech_pixel.png"),
                      triggerText: "[creature] channels electrical power through your gems!");

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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return base.Card.OnBoard && otherCard.HasTrait(Trait.Gem) && !base.Card.OpponentCard && !otherCard.OpponentCard;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (Singleton<ResourcesManager>.Instance is Part3ResourcesManager)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<ResourcesManager>.Instance.AddMaxEnergy(1);
            yield return Singleton<ResourcesManager>.Instance.AddEnergy(1);
            if (Singleton<ResourcesManager>.Instance is Part3ResourcesManager)
            {
                yield return new WaitForSeconds(0.3f);
            }
            yield break;
        }
    }
}
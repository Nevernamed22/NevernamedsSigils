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
    public class TemporarilyGemified : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Temporarily Gemified", "[creature] has been temporarily gemified until the end of battle.",
                      typeof(TemporarilyGemified),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook  },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/temporarilygemified.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/temporarilygemified_pixel.png"));

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
    }
    public class GemLatch : Latch
    {
        public static void Init()
        {
            TemporarilyGemified.Init();

            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gem Latch", "When [creature] perishes, its owner chooses a creature to become gemified.",
                      typeof(GemLatch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Latches/gemlatch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Latches/gemlatch_pixel.png"));

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
        public override Ability LatchAbility
        {
            get
            {
                return TemporarilyGemified.ability;
            }
        }
        public override void OnSuccessfullyLatched(PlayableCard target)
        {
            if (target)
            {
                CardInfo newInfo = target.Info.Clone() as CardInfo;
                CardModificationInfo cardModificationInfo = new CardModificationInfo();
                cardModificationInfo.gemify = true;
                newInfo.mods.Add(cardModificationInfo);

                target.SetInfo(newInfo);
                target.RenderCard();
            }
            base.OnSuccessfullyLatched(target);
        }
    }
}
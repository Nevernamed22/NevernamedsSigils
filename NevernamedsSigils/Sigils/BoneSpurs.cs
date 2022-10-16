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
    public class BoneSpurs : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bone Spurs", "When [creature] is struck, it generates a bone for it's owner.",
                      typeof(BoneSpurs),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bonespurs.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bonespurs_pixel.png"));

            BoneSpurs.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return (base.Card && base.Card.OnBoard);
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            yield return Singleton<ResourcesManager>.Instance.AddBones(1, base.Card.Slot);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

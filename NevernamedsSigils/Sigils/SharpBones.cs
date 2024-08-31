using APIPlugin;
using DiskCardGame;
using OpponentBones;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class SharpBones : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sharp Bones", "When [creature] is struck, the striker is dealt damage equal to half the number of bones its owner has, rounded down.",
                      typeof(SharpBones),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/sharpbones.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/sharpbones_pixel.png"));

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
        public float bones
        {
            get
            {
                if (base.Card.OpponentCard && OpponentResourceManager.instance != null)
                {
                    return OpponentResourceManager.instance.OpponentBones;
                }
                else
                {
                    return ResourcesManager.Instance.PlayerBones;
                }
            }
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Health > 0 && Mathf.FloorToInt(bones / 2f) > 0;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.55f);
            yield return source.TakeDamage(Mathf.FloorToInt(bones / 2f), base.Card);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

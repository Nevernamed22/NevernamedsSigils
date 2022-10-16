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
    public class Cute : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Cute", "Creatures that strike [creature] feel guilty, and will skip their next strike.",
                      typeof(Cute),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/cute.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/cute_pixel.png"));

            Cute.ability = newSigil.ability;
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
            if (source != null) return true;
            else return false;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (source != null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (!source.gameObject.GetComponent<AffectedByCute>())
                {
                  AffectedByCute guilt =  source.gameObject.AddComponent<AffectedByCute>();
                    guilt.turnInflicted = Singleton<TurnManager>.Instance.TurnNumber;
                }
                else
                {
                    AffectedByCute guilt = source.gameObject.GetComponent<AffectedByCute>();
                    guilt.turnInflicted = Singleton<TurnManager>.Instance.TurnNumber;

                }
            }
            yield break;
        }
        public class AffectedByCute : MonoBehaviour { public int turnInflicted; }
    }
}
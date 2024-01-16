/*using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MergeSigils;
using InscryptionMod.Abilities;

namespace NevernamedsSigils
{
    public class Slimy : MergeKillSelf
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Slimy", "Other cards can be played over top of [creature]. When this occurs, the played card has its power reduced by 1, and the creature underneath perishes.",
                      typeof(Slimy),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/slimy.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/slimy_pixel.png"));

            ability = newSigil.ability;
        }

        public override IEnumerator OnPreCreatureMerge(PlayableCard mergeCard)
        {
            mergeCard.AddTemporaryMod(new CardModificationInfo(-1, 0));
            yield break;
        }
        public override bool IsActualDeath => true;
        public override IEnumerator OnPreMergeDeath(PlayableCard mergeCard)
        {
            yield break;
        }
        public override bool CanMergeWith(PlayableCard mergeCard)
        {
            return true;
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
}*/
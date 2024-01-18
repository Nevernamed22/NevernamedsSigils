﻿using APIPlugin;
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
    public class Bonelust : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bonelust", "When [creature] kills another creature, it generates 3 bones for its owner.",
                      typeof(Bonelust),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.Part2Modular, Plugin.GrimoraModChair3, AbilityMetaCategory.GrimoraRulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bonelust.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bonelust_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            if (base.Card.OpponentCard)
            {
                yield return OpponentResourceManager.instance.AddOpponentBones(deathSlot, 3);
            }
            else yield return Singleton<ResourcesManager>.Instance.AddBones(3, deathSlot);
            yield return base.LearnAbility(0.4f);
            yield break;
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return (base.Card.OnBoard && !base.Card.OpponentCard && killer == base.Card);
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
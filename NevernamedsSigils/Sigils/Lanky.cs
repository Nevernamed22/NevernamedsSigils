using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Lanky : AbilityBehaviour, IModifyDirectDamage
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Lanky", "When [creature] deals damage directly to the opponent, it will always deal 1 less damage than it should.",
                      typeof(Lanky),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.GrimoraRulebook },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/lanky.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/lanky_pixel.png"));

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

        public bool RespondsToModifyDirectDamage(CardSlot target, int damage, PlayableCard attacker, int originalDamage)
        {
            return (damage > 0 && attacker == base.Card);
        }

        public int OnModifyDirectDamage(CardSlot target, int damage, PlayableCard attacker, int originalDamage)
        {
            return damage -1;
        }

        public int TriggerPriority(CardSlot target, int damage, PlayableCard attacker) => 0;
    }
}

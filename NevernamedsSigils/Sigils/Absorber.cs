using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Absorber : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Absorber", "If a creature adjacent to [creature] is struck, [creature] will gain health equal to the amount of damage dealt.",
                      typeof(Absorber),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair3, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/absorber.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/absorber_pixel.png"));

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
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return amount > 0 && amount != 100 && base.Card.slot != null && target.slot != null && Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot).Contains(target.slot);
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return PreSuccessfulTriggerSequence();
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.1f);
            base.Card.AddTemporaryMod(new CardModificationInfo(0, amount));
            yield return new WaitForSeconds(0.1f);
            yield break;
        }
    }
}
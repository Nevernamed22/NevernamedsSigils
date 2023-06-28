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
    public class Deadringer : AbilityBehaviour, IOnBellRung
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Deadringer", "While [creature] is alive and on the board, it gains +1 power each time the bell is rung, if it has less than 5 power.",
                      typeof(Deadringer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/deadringer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/deadringer_pixel.png"),
                      triggerText: "[creature] powers up!"
                      );

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

        public bool RespondsToBellRung(bool playerCombatPhase)
        {
            return base.Card.OnBoard && playerCombatPhase && base.Card.Attack < 5;
        }

        public IEnumerator OnBellRung(bool playerCombatPhase)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            base.Card.AddTemporaryMod(new CardModificationInfo(1, 0));
            yield break;
        }
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return target.Info.name.ToLowerInvariant().Contains("bell") || target.Info.DisplayedNameEnglish.ToLowerInvariant().Contains("bell");
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            base.Card.AddTemporaryMod(new CardModificationInfo(1, 0));
            yield break;
        }
    }
}
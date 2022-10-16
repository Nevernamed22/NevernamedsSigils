using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class FrailSacrifice : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Frail Sacrifice", "When [creature] is sacrificed, the creature it was sacrificed for gains the Frail Sigil.",
                      typeof(FrailSacrifice),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/frailsacrifice.png"),
                      pixelTex: null);

            FrailSacrifice.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToSacrifice()
        {
            return true;
        }
        public override IEnumerator OnSacrifice()
        {
            yield return base.PreSuccessfulTriggerSequence();
            CardModificationInfo mod = new CardModificationInfo() { abilities = new List<Ability>() { Frail.ability } };
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.AddTemporaryMod(mod);
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.RenderCard();
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}

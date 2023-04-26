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
    public class ExceptionalSacrifice : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Exceptional Sacrifice", "[creature] counts as 6 Blood rather than 1 Blood when sacrificed. Does not pair well with many lives.",
                      typeof(ExceptionalSacrifice),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/exceptionalsacrifice.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/exceptionalsacrifice_pixel.png"));

            ExceptionalSacrifice.ability = newSigil.ability;
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
            if (base.Card && base.Card.HasAbility(Ability.Sacrificial)) return true;
            else return false;
        }
        public override IEnumerator OnSacrifice()
        {
            base.Card.TemporarilyRemoveAbilityFromCard(ExceptionalSacrifice.ability);
            base.Card.temporaryMods.Add(new CardModificationInfo(Ability.TripleBlood));
            if (base.Card.Info.name == "BeastNevernamed YaraMaYhaWho")
            {
                base.Card.temporaryMods.Add(new CardModificationInfo(3, 0) { nameReplacement = "Bloodstarved Beast" });
            }
            base.Card.RenderCard();
            yield break;
        }
    }
}
using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class Parthenogenesis : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Parthenogenesis", "When [creature] dies to combat, a larval form is left in its old space that evolves into an exact clone after 1 turn.",
                      typeof(Parthenogenesis),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/parthenogenesis.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/parthenogenesis_pixel.png"));

            Parthenogenesis.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public CardInfo Grub
        {
            get
            {
                CardInfo grub = (base.Card.Info.GetExtendedProperty("ParthenogenesisOverride") != null) ? CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("ParthenogenesisOverride")) : CardLoader.GetCardByName("SigilNevernamed CloneGrub");
                if (base.Card != null && grub != null)
                {
                    int evol = 2;
                    if (grub.evolveParams != null) { evol = grub.evolveParams.turnsToEvolve <= 1 ? 2 : grub.evolveParams.turnsToEvolve + 1; }
                    grub.evolveParams = new EvolveParams() { evolution = base.Card.Info, turnsToEvolve = evol };
                }
                return grub;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(Grub, base.Card.slot, 0.1f, true);
            yield return base.LearnAbility(0f);
        }
    }
}

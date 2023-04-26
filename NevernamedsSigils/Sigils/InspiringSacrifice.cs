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
    public class InspiringSacrifice : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Inspiring Sacrifice", "When [creature] is sacrificed, it grants its sigils to the creature it was sacrificed for.",
                      typeof(InspiringSacrifice),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/inspiringsacrifice.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/inspiringsacrifice_pixel.png"));

            InspiringSacrifice.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToSacrifice()
        {
            return true;
        }
        public override IEnumerator OnSacrifice()
        {
            yield return base.PreSuccessfulTriggerSequence();
            CardModificationInfo mod = new CardModificationInfo();
            foreach (Ability ability in Card.GetAllAbilities().FindAll((x) => x != InspiringSacrifice.ability))
            {
                mod.abilities.Add(ability);
            }
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.AddTemporaryMod(mod);
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.RenderCard();
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}

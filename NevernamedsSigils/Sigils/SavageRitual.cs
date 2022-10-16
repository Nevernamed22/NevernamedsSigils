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
    public class SavageRitual : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Savage Ritual", "When [creature] is sacrificed, it attacks once before dying.",
                      typeof(SavageRitual),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/savageritual.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/savageritual_pixel.png"));

            SavageRitual.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return SavageRitual.ability;
            }
        }
        public int DamageDealtThisPhase { get; private set; }

        public override bool RespondsToSacrifice()
        {
            return true;
        }
        public override IEnumerator OnSacrifice()
        {
            yield return new WaitForSeconds(0.1f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
        }     
    }
}

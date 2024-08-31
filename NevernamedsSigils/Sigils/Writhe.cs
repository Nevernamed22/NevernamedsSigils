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
    public class Writhe : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Writhe", "When [creature] perishes, it will attack all opposing spaces for 1 damage.",
                      typeof(Writhe),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/writhe.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/writhe_pixel.png"));

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
        public int DamageDealtThisPhase { get; private set; }

        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            yield return new WaitForSeconds(0.1f);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot, Singleton<BoardManager>.Instance.GetSlots(base.Card.OpponentCard), 1);
            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.25f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

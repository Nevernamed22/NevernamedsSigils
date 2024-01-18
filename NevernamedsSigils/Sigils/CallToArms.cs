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
    public class CallToArms : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Call To Arms", "When [creature] is played, the creatures to its left and right will immediately attack.",
                      typeof(CallToArms),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/calltoarms.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/calltoarms_pixel.png"));

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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);

            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            if (toLeft&& toLeft.Card) { yield return fakecombat.FakeCombat(toLeft.IsPlayerSlot, null, toLeft); yield return new WaitForSeconds(0.2f); }
            if (toRight && toRight.Card) { yield return fakecombat.FakeCombat(toRight.IsPlayerSlot, null, toRight); }
        }
    }
}

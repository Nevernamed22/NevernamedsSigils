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
    public class LashOut : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Lash Out", "At the end of the owner's turn, [creature] will attack a random opposing slot.",
                      typeof(LashOut),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/lashout.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/lashout_pixel.png"));

            LashOut.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            List<CardSlot> viableslots = Singleton<BoardManager>.Instance.playerSlots;
            if (base.Card.slot.IsPlayerSlot) viableslots = Singleton<BoardManager>.Instance.opponentSlots;
            CardSlot cardSlot = Tools.SeededRandomElement(viableslots);
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot, new List<CardSlot>() { cardSlot });
            yield break;
        }              
    }
}


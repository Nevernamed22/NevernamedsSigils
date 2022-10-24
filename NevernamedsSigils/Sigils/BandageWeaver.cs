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
    public class BandageWeaver : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bandage Weaver", "At the end of the turn, [creature] will heal the cards to it's left and right for 1 health.",
                      typeof(BandageWeaver),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bandageweaver.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bandageweaver_pixel.png"));

            BandageWeaver.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card.slot.IsPlayerSlot == playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            List<CardSlot> adjacents = Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.Slot);

            foreach (CardSlot slot in adjacents)
            {
                if (slot && slot.Card)
                {
                    if (!slot.Card.Dead && (slot.Card.Health < slot.Card.MaxHealth))
                    {
                        slot.Card.Anim.StrongNegationEffect();
                        slot.Card.HealDamage(1);
                        yield return base.LearnAbility(0f);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
            yield break;
        }
    }
}

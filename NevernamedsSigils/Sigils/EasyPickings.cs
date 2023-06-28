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
    public class EasyPickings : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Easy Pickings", "When [creature] is played, all other creatures on the board with 1 health will perish.",
                      typeof(EasyPickings),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/easypickings.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/easypickings_pixel.png")
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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            List<CardSlot> slots = Singleton<BoardManager>.Instance.AllSlots;
            foreach(CardSlot slot in slots)
            {
                if (slot.Card && slot.Card.Health == 1 && slot.Card != base.Card)
                {
                    yield return slot.Card.Die(false, base.Card);
                    yield return new WaitForSeconds(0.5f);
                }
            }
            yield break;
        }

    }
}
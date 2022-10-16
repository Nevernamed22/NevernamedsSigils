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
    public class BuzzOff : Strafe
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Buzz Off", "At the end of the owner's turn, [creature] will move in the direction inscribed in the sigil and leave a mayfly in it's old place. A mayfly is defined as 1 power, 1 health, airborne, brittle.",
                      typeof(BuzzOff),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/buzzoff.png"),
                      pixelTex: null);

            BuzzOff.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
        {
            if (cardSlot.Card == null)
            {
                CardInfo mayfly = CardLoader.GetCardByName("Nevernamed SwarmedMayfly");
                mayfly.Mods.Add(base.Card.CondenseMods(new List<Ability>() { BuzzOff.ability }));
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(mayfly, cardSlot, 0.1f, true);
            }
            yield break;
        }
    }
}
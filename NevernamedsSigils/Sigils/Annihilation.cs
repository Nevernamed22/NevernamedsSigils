using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Annihilation : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Annihilation", "When [creature] perishes, all creatures on the board are dealt 10 damage.",
                      typeof(Annihilation),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/annihilation.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/annihilation_pixel.png"));

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
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            List<PlayableCard> cards = new List<PlayableCard>();
            cards.AddRange(Singleton<BoardManager>.Instance.CardsOnBoard.FindAll(x => x != base.Card && !x.Dead));
            if (cards.Count > 0)
            {
                yield return base.PreSuccessfulTriggerSequence();
                for (int i = cards.Count - 1; i >= 0; i--)
                {
                    yield return cards[i].TakeDamage(10, base.Card);
                }
                yield return base.LearnAbility(0.5f);
            }
            yield break;
        }
    }
}
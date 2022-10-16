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
    public class PointNemo : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Point Nemo", "When [creature] is played, all creatures on the board gain the Waterborne sigil.",
                      typeof(PointNemo),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/pointnemo.png"),
                      pixelTex: null);

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
            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
            availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
            bool didIt = false;
            foreach (CardSlot slot in availableSlots)
            {
                if (slot.Card != null && slot.Card != base.Card && !slot.Card.HasTrait(Trait.Uncuttable) && !slot.Card.CardHasSigilInList(waterbornes))
                {
                    slot.Card.Anim.PlayTransformAnimation();
                    slot.Card.AddTemporaryMod(new CardModificationInfo(Ability.Submerge));
                    slot.Card.RenderCard();
                    base.Card.Anim.LightNegationEffect();
                    didIt = true;
                    yield return new WaitForSeconds(0.1f);
                }
            }
            if (!didIt)
            {
                base.Card.Anim.StrongNegationEffect();
            }
            yield break;
        }
        private static List<Ability> waterbornes = new List<Ability>() {
            Ability.Submerge,
            Ability.SubmergeSquid,
            SubaquaticSpines.ability,
            CoastGuard.ability
        };

    }
}
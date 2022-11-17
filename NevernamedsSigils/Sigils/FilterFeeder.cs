using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class FilterFeeder : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Filter Feeder", "When a creature perishes adjacent to [creature], the card will gain either 1 power or 1 health.",
                      typeof(FilterFeeder),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/filterfeeder.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/filterfeeder_pixel.png"));

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
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return base.Card.OnBoard && Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.slot).Contains(deathSlot);
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            base.Card.Anim.StrongNegationEffect();
            if (UnityEngine.Random.value <= 0.5f) { base.Card.temporaryMods.Add(new CardModificationInfo(1, 0)); }
            else { base.Card.temporaryMods.Add(new CardModificationInfo(0, 1)); }

            yield break;
        }
    }
}

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
    public class Herald : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Herald", "When [creature] attacks an opposing creature and it perishes, creatures to the left and right of this card gain 1 power.",
                      typeof(Herald),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/herald.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/herald_pixel.png"));

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
            return killer == base.Card;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();

            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            if (toLeft != null && toLeft.Card != null)
            {
                toLeft.Card.Anim.NegationEffect(true);
                toLeft.Card.temporaryMods.Add(new CardModificationInfo(1, 0));
            }

            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            if (toRight != null && toRight.Card != null)
            {
                toRight.Card.Anim.NegationEffect(true);
                toRight.Card.temporaryMods.Add(new CardModificationInfo(1, 0));
            }
            yield return base.LearnAbility(0.25f);
        }
    }
}

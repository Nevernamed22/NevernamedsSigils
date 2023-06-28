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
    public class BloodGusher : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Blood Gusher", "When [creature] attacks an opposing creature and it perishes, this card, as well as creatures to the left and right of this card, gain 1 health.",
                      typeof(BloodGusher),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodgusher.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bloodgusher_pixel.png"));

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
                toLeft.Card.temporaryMods.Add(new CardModificationInfo(0, 1));
            }

            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            if (toRight != null && toRight.Card != null)
            {
                toRight.Card.Anim.NegationEffect(true);
                toRight.Card.temporaryMods.Add(new CardModificationInfo(0, 1));
            }
            base.Card.temporaryMods.Add(new CardModificationInfo(0, 1));

            yield return base.LearnAbility(0.25f);
        }
    }
}

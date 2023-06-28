using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class BatchDelete : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Batch Delete", "When [creature] strikes a creature and it perishes, any creatures on the board which share the same cost as the target perish also.",
                      typeof(BatchDelete),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/batchdelete.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/batchdelete_pixel.png"),
                      triggerText: "[creature] shreds any other cards with the same cost as it's victim...");

            ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card && Singleton<BoardManager>.Instance.AllSlots.Exists( x=>x.Card != null && x.Card != card && 
            x.Card.Info.BloodCost == card.Info.BloodCost &&
            x.Card.Info.BonesCost == card.Info.BonesCost &&
            x.Card.EnergyCost == card.EnergyCost &&
            gemsCostIsEqual(x.Card.Info, card.Info));
        }
        public bool gemsCostIsEqual(CardInfo one, CardInfo two)
        {
            return (one.gemsCost.Contains(GemType.Blue) == two.gemsCost.Contains(GemType.Blue)) &&
                (one.gemsCost.Contains(GemType.Green) == two.gemsCost.Contains(GemType.Green)) &&
                (one.gemsCost.Contains(GemType.Orange) == two.gemsCost.Contains(GemType.Orange));
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
                    yield return base.PreSuccessfulTriggerSequence();
            foreach(CardSlot slot in Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.Card != null && x.Card != card &&
           x.Card.Info.BloodCost == card.Info.BloodCost &&
           x.Card.Info.BonesCost == card.Info.BonesCost &&
           x.Card.EnergyCost == card.EnergyCost &&
           gemsCostIsEqual(x.Card.Info, card.Info)))
            {
                yield return slot.Card.Die(false);
            }
            yield break;
        }
    }
}
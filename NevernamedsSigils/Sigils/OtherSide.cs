using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class OtherSide : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Other Side", "While [creature] is on the board, any friendly creatures that die in combat will be returned to the owner's hand as skeletons.",
                      typeof(OtherSide),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/otherside.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/otherside_pixel.png"));

            OtherSide.ability = newSigil.ability;
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
            return fromCombat && !card.OpponentCard && card != base.Card && base.Card.OnBoard && card.Info.name != "Skeleton";
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName("Skeleton"), null, 0.25f);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}

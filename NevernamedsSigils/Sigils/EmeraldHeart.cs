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
    public class EmeraldHeart : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Emerald Heart", "When [creature] perishes, an Emerald Mox is created in its place.",
                      typeof(EmeraldHeart),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/emeraldheart.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/emeraldheart_pixel.png"));

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
            return card == base.Card && fromCombat && base.Card.OnBoard;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("MoxEmerald"), base.Card.Slot, 0.1f, true);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
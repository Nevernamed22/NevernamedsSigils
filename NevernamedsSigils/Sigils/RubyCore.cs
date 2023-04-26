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
    public class RubyCore : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ruby Core", "When [creature] perishes, a Ruby Vessel is created in its place.",
                      typeof(RubyCore),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/rubycore.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/rubycore_pixel.png"));

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
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("EmptyVessel_OrangeGem"), base.Card.Slot, 0.1f, true);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
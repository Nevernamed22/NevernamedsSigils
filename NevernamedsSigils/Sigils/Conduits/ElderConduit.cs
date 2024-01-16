using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;

namespace NevernamedsSigils
{
    public class ElderConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Elder Conduit", "When a creature within a circuit completed by [creature] perishes, a Gem Skeleton will be created in it's place.",
                      typeof(ElderConduit),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/elderconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/elderconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] calls forth a Gem Skeleton with the old magics!");

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
            CardSlot slot = card.slot;
            List<CardSlot> affectedSlots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card));
            return fromCombat && card.OnBoard && slot != null && (slot.Card == null || slot.Card.Dead) && affectedSlots.Contains(slot) && !GemSkeletons.Contains(card.Info.name);
        }
        public static List<string> GemSkeletons = new List<string>()
        {
            "SigilNevernamed SapphireSkeleton",
            "SigilNevernamed RubySkeleton",
            "SigilNevernamed EmeraldSkeleton"
        };
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.3f);
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(Tools.SeededRandomElement(GemSkeletons)), deathSlot, 0.15f, true);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
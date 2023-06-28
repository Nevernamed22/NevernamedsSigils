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
    public class PutSentryHere : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Put Sentry Here", "When [creature] is played, a Sentry Drone is created to its left and right.",
                      typeof(PutSentryHere),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.BountyHunter, AbilityMetaCategory.Part3BuildACard, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/putsentryhere.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/putsentryhere_pixel.png"));

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
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {
            string cardToSpawn = "SentryBot";
            if (base.Card.Info.GetExtendedProperty("PutSentryHereOverride") != null) { cardToSpawn = base.Card.Info.GetExtendedProperty("PutSentryHereOverride"); }
            CardInfo cardByName = CardLoader.GetCardByName(cardToSpawn);
            this.ModifySpawnedCard(cardByName);
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, slot, 0.15f, true);
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            yield return new WaitForSeconds(0.1f);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;

            if (toLeftValid || toRightValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
            }
            if (toLeftValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toLeft);
            }

            if (toRightValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toRight);
            }

            yield break;
        }
        private void ModifySpawnedCard(CardInfo card)
        {
            List<Ability> abilities = base.Card.Info.Abilities;
            foreach (CardModificationInfo cardModificationInfo in base.Card.TemporaryMods)
            {
                abilities.AddRange(cardModificationInfo.abilities);
            }
            abilities.RemoveAll((Ability x) => x == this.Ability);
            CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
            cardModificationInfo2.fromCardMerge = true;
            cardModificationInfo2.abilities = abilities;
            card.Mods.Add(cardModificationInfo2);
        }
    }
}

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
    public class CircuitMaker : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Circuit Maker", "When [creature] is played, a conduit card is created to its left and right.",
                      typeof(CircuitMaker),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/circuitmaker.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/circuitmaker_pixel.png"));

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
            string cardToSpawn = "NullConduit";
            if (base.Card.Info.GetExtendedProperty("CircuitMakerOverride") != null) { cardToSpawn = base.Card.Info.GetExtendedProperty("CircuitMakerOverride"); }
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

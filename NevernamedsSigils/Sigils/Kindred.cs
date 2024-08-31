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
    public class Kindred : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Kindred", "When [creature] is played, random common creatures of the same tribe will be played for free to its left and right.",
                      typeof(Kindred),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/kindred.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/kindred_pixel.png"));

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
        int seedDifferentiator = 1;
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {
            Tribe required = Tribe.None;
            if ((base.Card.Info.temple == CardTemple.Nature) && (base.Card.Info.tribes.Count > 0)) required = Tools.SeededRandomElement(base.Card.Info.tribes, Tools.GetRandomSeed() + seedDifferentiator);
            CardInfo cardToSpawn = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), false, required, false).Clone() as CardInfo;
            cardToSpawn.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Kindred.ability }));
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardToSpawn, slot, 0.15f, true);
            seedDifferentiator++;
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
    }
}

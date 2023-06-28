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
    public class Downdraft : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Downdraft", "[creature] will grant the power of flight to creatures to it's left and right.",
                      typeof(Downdraft),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/downdraft.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/downdraft_pixel.png"));

            Downdraft.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return TriggerGiveFlight();
        }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard.OpponentCard == base.Card.OpponentCard;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return TriggerGiveFlight();
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return true;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return TriggerGiveFlight();
            yield break;
        }
        public IEnumerator TriggerGiveFlight()
        {
            if (GetValidTargets().Count > 0)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.1f);
                foreach (PlayableCard playableCard in this.GetValidTargets())
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Flying);
                    cardModificationInfo.singletonId = "Downdraft_flight";
                    cardModificationInfo.RemoveOnUpkeep = true;
                    playableCard.Status.hiddenAbilities.Add(Ability.Flying);
                    playableCard.AddTemporaryMod(cardModificationInfo);
                }
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.25f);

            }
        }
        private List<PlayableCard> GetValidTargets()
        {
            List<PlayableCard> slots = new List<PlayableCard>();
            if (base.Card.slot)
            {

            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);

                if (toLeft && toLeft.Card && !toLeft.Card.HasAbility(Ability.Flying)) slots.Add(toLeft.Card);
                if (toRight && toRight.Card && !toRight.Card.HasAbility(Ability.Flying)) slots.Add(toRight.Card);
            }
            return slots;
        }
    }
}
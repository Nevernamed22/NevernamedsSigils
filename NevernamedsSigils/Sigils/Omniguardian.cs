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
    public class Omniguardian : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Omni Guardian", "When [creature] is played, all friendly creatures on the board gain armour.",
                      typeof(Omniguardian),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/omniguardian.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/omniguardian_pixel.png"),
                      triggerText: "[creature] gives the Armored sigil to all friendly creatures.");

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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return PreSuccessfulTriggerSequence();
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            ShieldGeneratorItem.AddShieldsToCards(this.GetValidTargets(), base.Card.OpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy);
            yield return new WaitForSeconds(0.75f);
            yield break;
        }
        private List<PlayableCard> GetValidTargets()
        {
            return Singleton<BoardManager>.Instance.CardsOnBoard.FindAll((PlayableCard x) => x.OpponentCard == base.Card.OpponentCard && x != base.Card && !x.HasShield());
        }
        public static void AddShieldsToCards(List<PlayableCard> addShields, List<CardSlot> resetShieldsSlots)
        {
            foreach (PlayableCard playableCard in addShields)
            {
                CardModificationInfo mod = new CardModificationInfo(Ability.DeathShield);
                if (!playableCard.HasAbility(Ability.DeathShield) && Tools.GetActAsInt() == 3)
                {
                    playableCard.Status.hiddenAbilities.Add(Ability.DeathShield);
                }
                playableCard.AddTemporaryMod(mod);
            }
            foreach (CardSlot cardSlot in resetShieldsSlots)
            {
                if (cardSlot.Card != null)
                {
                    cardSlot.Card.ResetShield();
                }
            }
        }
    }
}

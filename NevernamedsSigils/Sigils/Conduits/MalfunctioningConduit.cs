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
    public class MalfunctioningConduit : Conduit
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Malfunctioning Conduit", "Completes a circuit. At the end of its owner's turn, random cards from its owners hand will be played in random spaces inside of the circuit completed by [creature]. Those cards gain the Burning Sigil.",
                      typeof(MalfunctioningConduit),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Conduits/malfunctioningconduit.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Conduits/malfunctioningconduit_pixel.png"),
                      isConduit: true,
                      triggerText: "[creature] draws cards into its circuit!");

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return !playerUpkeep && !base.Card.OpponentCard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            List<CardSlot> emptySlots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).FindAll(x => x.Card == null && Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(x).Contains(base.Card));

            if (emptySlots.Count > 0 && Singleton<PlayerHand>.Instance.CardsInHand != null && Singleton<PlayerHand>.Instance.CardsInHand.Count > 0)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.1f);
                base.Card.Anim.LightNegationEffect();

                yield return base.PreSuccessfulTriggerSequence();

                for (int i = 0; i < emptySlots.Count; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    if (emptySlots[i].Card == null && Singleton<PlayerHand>.Instance.CardsInHand != null && Singleton<PlayerHand>.Instance.CardsInHand.Count > 0)
                    {
                        PlayableCard card = Tools.SeededRandomElement<PlayableCard>(Singleton<PlayerHand>.Instance.CardsInHand, base.GetRandomSeed());
                        card.AddTemporaryMod(new CardModificationInfo(Burning.ability));
                        card.RenderCard();
                        yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(card, emptySlots[i]);
                    }

                    yield return base.LearnAbility(0.5f);
                }

                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield break;
        }

    }
}

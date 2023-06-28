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
    public class Reroute : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Reroute", "At the end of the turn, [creature] will swap places with a random creature on the same side of the board.",
                      typeof(Reroute),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/reroute.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/reroute_pixel.png"));

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
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return (!playerTurnEnd && !base.Card.HasAbility(Stalwart.ability));
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard));
            for (int i = availableSlots.Count - 1; i >= 0; i--)
            {
                if (availableSlots[i].Card == null || availableSlots[i].Card == base.Card) availableSlots.RemoveAt(i);
            }
            if (availableSlots.Count > 0)
            {

                CardSlot oldSlot = base.Card.slot;
                CardSlot targetSlot = Tools.RandomElement(availableSlots);
                PlayableCard targetSlotCard = targetSlot.Card;
                yield return base.PreSuccessfulTriggerSequence();

                Vector3 midpoint = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;

                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.1f, null, true);
                Tween.Position(targetSlotCard.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(targetSlotCard, oldSlot, 0.1f, null, true);

               
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.3f);
            }
            yield break;
        }
    }
}
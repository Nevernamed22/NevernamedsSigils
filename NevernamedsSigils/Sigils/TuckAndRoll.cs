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
    public class TuckAndRoll : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Tuck And Roll", "At the end of combat, [creature] will move to the free space opposed by the least amount of attack power, taking into account queued creatures, flight, and Touch of Death.",
                      typeof(TuckAndRoll),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/tuckandroll.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/tuckandroll_pixel.png"));

            TuckAndRoll.ability = newSigil.ability;
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
            return playerUpkeep != base.Card.OpponentCard && !base.Card.HasAbility(Stalwart.ability);
        }
        private int GetPower(CardSlot slot, bool checkQueue)
        {
            if (slot.Card != null)
            {
                if (slot.Card.HasAbility(Ability.Deathtouch) && !base.Card.HasAbility(Ability.MadeOfStone)) return 999;
                if (slot.Card.HasAbility(Ability.Flying) && !base.Card.HasAbility(Ability.Reach)) return 0;
                return slot.Card.Attack;
            }
            else
            {
                if (checkQueue && Singleton<BoardManager>.Instance.GetCardQueuedForSlot(slot))
                {
                    if (Singleton<BoardManager>.Instance.GetCardQueuedForSlot(slot).HasAbility(Ability.Deathtouch) && !base.Card.HasAbility(Ability.MadeOfStone)) return 999;
                    if (Singleton<BoardManager>.Instance.GetCardQueuedForSlot(slot).HasAbility(Ability.Flying) && !base.Card.HasAbility(Ability.Reach)) return 0;
                    return Singleton<BoardManager>.Instance.GetCardQueuedForSlot(slot).Attack;

                }
                else return 0;
            }
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            CardSlot currentBestSlot = null;
            int currentConsideredAttk = 9999999;

            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard)); //Get all slots on the same side

            for (int i = availableSlots.Count - 1; i >= 0; i--)
            {
                if (availableSlots[i].Card == null || availableSlots[i].Card == base.Card)
                {
                    if (availableSlots[i].opposingSlot != null && GetPower( availableSlots[i].opposingSlot, !base.Card.OpponentCard) <= currentConsideredAttk)
                    {
                        currentBestSlot = availableSlots[i];
                        currentConsideredAttk = GetPower(availableSlots[i].opposingSlot, !base.Card.OpponentCard);
                    }
                }
            }

            if (currentBestSlot != null && currentBestSlot != base.Card.slot)
            {
                yield return base.PreSuccessfulTriggerSequence();
                Vector3 midpoint = (base.Card.Slot.transform.position + currentBestSlot.transform.position) / 2f;
                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, currentBestSlot, 0.1f, null, true);
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

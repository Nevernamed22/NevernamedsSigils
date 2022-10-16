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
    public class Fearsome : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Fearsome", "When [creature] targets a slot occupied by another creature, that creature will move to make way for it if there is room to do so.",
                      typeof(Fearsome),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/fearsome.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/fearsome_pixel.png"));

            Fearsome.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        private IEnumerator ForceOpposingCardToFlee(CardSlot slot)
        {
            if (base.Card.OnBoard && base.Card.slot != null && slot != null)
            {
              
                if (slot.Card != null && !slot.Card.HasTrait(Trait.Giant))
                {
                    PlayableCard fleer = slot.Card;
                    CardSlot moveto = SpacetoFleeTo(fleer);
                    if (moveto != null)
                    {
                        Vector3 midpoint = (fleer.Slot.transform.position + moveto.transform.position) / 2f;
                        Tween.Position(fleer.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                        yield return Singleton<BoardManager>.Instance.AssignCardToSlot(fleer, moveto, 0.1f, null, true);
                        yield return base.LearnAbility(0.1f);
                    }
                    else fleer.Anim.StrongNegationEffect();
                }
            }
            yield break;
        }
        private CardSlot SpacetoFleeTo(PlayableCard card)
        {
            if (card && card.slot)
            {
                List<CardSlot> adjacents = Singleton<BoardManager>.Instance.GetAdjacentSlots(card.slot);
                List<CardSlot> emptyAdjacents = new List<CardSlot>();
                if (adjacents.Count > 0)
                {
                    foreach (CardSlot slot in adjacents)
                    {
                        if (slot && slot.Card == null)
                        {
                            emptyAdjacents.Add(slot);
                        }
                    }
                }
                if (emptyAdjacents.Count > 0) return Tools.RandomElement(emptyAdjacents);
                else return null;
            }
            else return null;
        }
        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (attacker != null && attacker == base.Card) return true;
            return false;
        }
        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (slot && slot.Card)
            {
                if (!slot.Card.HasAbility(Ability.WhackAMole)) yield return ForceOpposingCardToFlee(slot);
            }         
            yield break;
        }
    }
}

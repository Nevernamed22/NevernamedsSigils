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
    public class Coward : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Coward", "When [creature] is targeted, it will leap out of the way.",
                      typeof(Coward),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair1, Plugin.Part2Modular, AbilityMetaCategory.MagnificusRulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/coward.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/coward_pixel.png"));

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
                if (emptyAdjacents.Count > 0) return Tools.SeededRandomElement(emptyAdjacents);
                else return null;
            }
            else return null;
        }
        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if ((slot == base.Card.slot) && !base.Card.HasAbility(Ability.WhackAMole) && !base.Card.HasAbility(Stalwart.ability) && base.Card.OnBoard && base.Card.slot != null) return true;
            return false;
        }
        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
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
            yield break;
        }
    }
}

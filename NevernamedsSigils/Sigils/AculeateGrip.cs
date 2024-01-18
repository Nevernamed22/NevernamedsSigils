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
    public class AculeateGrip : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Aculeate Grip", "When a creature is played on the same side of the board as [creature], [creature] will move towards them as far as possible, dealing them one damage and increasing its own health by 1.",
                      typeof(AculeateGrip),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/aculeategrip.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/aculeategrip_pixel.png"));

            AculeateGrip.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard != null && otherCard.Slot != null && otherCard.OpponentCard == base.Card.OpponentCard && otherCard.Slot != base.Card.Slot;
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);
            List<CardSlot> list = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!otherCard.OpponentCard));
            bool flag = Singleton<BoardManager>.Instance.SlotIsLeftOfSlot(otherCard.Slot, base.Card.Slot, list);
            List<CardSlot> list2 = new List<CardSlot>();
            if (flag)
            {
                using (List<CardSlot>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        CardSlot cardSlot = enumerator.Current;
                        if (cardSlot.Index > otherCard.Slot.Index && cardSlot.Index < base.Card.Slot.Index)
                        {
                            list2.Add(cardSlot);
                        }
                    }
                    goto IL_184;
                }
            }
            list.Reverse();
            foreach (CardSlot cardSlot2 in list)
            {
                if (cardSlot2.Index > base.Card.Slot.Index && cardSlot2.Index < otherCard.Slot.Index)
                {
                    list2.Add(cardSlot2);
                }
            }
        IL_184:
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, flag);
            base.Card.RenderCard();
            CardSlot targetSlot = list2.Find((CardSlot x) => x.Card == null);
            if (targetSlot == null)
            {
                List<CardSlot> adjacents = new List<CardSlot>();
                if (Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, true) != null) adjacents.Add(Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, true));
                if (Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, false) != null) adjacents.Add(Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, false));
                if (adjacents.Contains(otherCard.slot))
                {
                    yield return otherCard.TakeDamage(1, base.Card);
                    base.Card.Anim.LightNegationEffect();
                    base.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                }
                else if (flag && Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, true) != null)
                {
                    yield return Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, true).Card.TakeDamage(1, base.Card);
                    base.Card.Anim.LightNegationEffect();
                    base.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                }
                else if (!flag && Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, false) != null)
                {
                    yield return Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, false).Card.TakeDamage(1, base.Card);
                    base.Card.Anim.LightNegationEffect();
                    base.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                }
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (!base.Card.HasAbility(Stalwart.ability))
                {
                    Vector3 a = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;
                    Tween.Position(base.Card.transform, a + Vector3.up * 0.5f, 0.05f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.05f, null, true);
                }
                yield return new WaitForSeconds(0.2f);

                CardSlot hurtSlot = Singleton<BoardManager>.Instance.GetAdjacent(targetSlot, flag);
                if (hurtSlot != null && hurtSlot.Card != null)
                {
                    yield return hurtSlot.Card.TakeDamage(1, base.Card);
                    base.Card.Anim.LightNegationEffect();
                    base.Card.AddTemporaryMod(new CardModificationInfo(0, 1));
                }

                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
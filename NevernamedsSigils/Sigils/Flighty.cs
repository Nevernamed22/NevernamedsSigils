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
    public class Flighty : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Flighty", "At the end of the turn, [creature] will move to a random available space.",
                      typeof(Flighty),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/flighty.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/flighty_pixel.png"));

            Flighty.ability = newSigil.ability;
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
            return !playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard));
            for (int i = availableSlots.Count - 1; i >= 0; i--)
            {
                if (availableSlots[i].Card != null) availableSlots.RemoveAt(i);
            }
            if (availableSlots.Count > 0)
            {
                CardSlot targetSlot = Tools.RandomElement(availableSlots);
                yield return base.PreSuccessfulTriggerSequence();
                Vector3 midpoint = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;
                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.1f, null, true);
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
                midpoint = default(Vector3);
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

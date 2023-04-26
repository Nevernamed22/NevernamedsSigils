using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class CollateralDamage : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Collateral Damage", "At the end of the owner's turn, if [creature] is on the board, the nearest creature to this card will be dealt ten damage.",
                      typeof(CollateralDamage),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/collateraldamage.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/collateraldamage_pixel.png"));

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
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);

            PlayableCard target = null;
            float lastTargetDistance = 1000000000000;
         
            List<CardSlot> viableslots = Singleton<BoardManager>.Instance.AllSlots;
            
            foreach(CardSlot slot in viableslots)
            {
                if (slot && slot.Card && slot.Card != base.Card)
                {
                    float dist = Vector2.Distance(base.Card.transform.position, slot.Card.transform.position);
                    if (dist < lastTargetDistance)
                    {
                        target = slot.Card;
                        lastTargetDistance = dist;
                    }
                }
            }

            if (target != null)
            {
                base.Card.Anim.StrongNegationEffect();
                yield return target.TakeDamage(10, base.Card);
            }
            yield return new WaitForSeconds(0.35f);

            yield break;
        }
    }
}


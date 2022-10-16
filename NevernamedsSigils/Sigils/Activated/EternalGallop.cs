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
    public class EternalGallop : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Eternal Gallop", "Move [creature] in the direction inscribed on the sigil.",
                      typeof(EternalGallop),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/eternalgallop.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/eternalgallop_pixel.png"),
                      isActivated: true);

            EternalGallop.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }       
        bool movingLeft;
        public override IEnumerator Activate()
        {
            if (base.Card.OpponentCard) yield break;
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;

            //Flip if Obstructed
            if (this.movingLeft && !toLeftValid) this.movingLeft = false;
            if (!this.movingLeft && !toRightValid) this.movingLeft = true;


            CardSlot destination = this.movingLeft ? toLeft : toRight;
            bool destinationValid = this.movingLeft ? toLeftValid : toRightValid;
            yield return this.MoveToSlot(destination, destinationValid);

            if (destination != null && destinationValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        protected IEnumerator MoveToSlot(CardSlot destination, bool destinationValid)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = this.movingLeft;
            base.Card.RenderCard();

            if (destination != null && destinationValid)
            {
                CardSlot oldSlot = base.Card.Slot;
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                yield return new WaitForSeconds(0.25f);
                oldSlot = null;
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
    }
}

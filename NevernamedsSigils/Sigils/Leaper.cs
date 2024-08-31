using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Leaper : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Leaper", "At the end of the owner's turn, [creature] will move in the direction inscribed in the sigil, leaping over single card obstructions.",
                      typeof(Leaper),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.BountyHunter, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/leaper.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/leaper_pixel.png"));

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
            return base.Card != null && base.Card.OpponentCard != playerTurnEnd;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot hardleft = toLeft != null ? Singleton<BoardManager>.Instance.GetAdjacent(toLeft, true) : null;
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            CardSlot hardright = toRight != null ? Singleton<BoardManager>.Instance.GetAdjacent(toRight, false) : null;
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.25f);
            yield return this.DoStrafe(toLeft, toRight, hardleft, hardright);
            yield break;
        }
        private IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight, CardSlot hardleft, CardSlot hardright)
        {

            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool hardleftValid = hardleft != null && hardleft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            bool hardrightValid = hardright != null && hardright.Card == null;

            CardSlot destination = null;
            bool doHop = false;
            if (movingLeft)
            {            
                if (toLeftValid) { destination = toLeft; }
                else if (hardleftValid) { destination = hardleft; doHop = true; }
                else if (toRightValid) { destination = toRight; movingLeft = false; }
                else if (hardrightValid) { destination = hardright; movingLeft = false; doHop = true; }
            }
            else
            {              
                if (toRightValid) { destination = toRight;  }
                else if (hardrightValid) { destination = hardright; doHop = true;  }
                else if (toLeftValid) { destination = toLeft; movingLeft = true;}
                else if (hardleftValid) { destination = hardleft; movingLeft = true; doHop = true;  }
            }
            yield return this.MoveToSlot(destination, doHop);
            if (destination != null)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        private IEnumerator MoveToSlot(CardSlot destination, bool dolittlehop)
        {
            base.Card.RenderInfo.SetAbilityFlipped(this.Ability, this.movingLeft);
            base.Card.RenderInfo.flippedPortrait = (this.movingLeft && base.Card.Info.flipPortraitForStrafe);
            base.Card.RenderCard();

            if (destination != null)
            {
                CardSlot oldSlot = base.Card.Slot;

                if (dolittlehop)
                {
                    float num = (destination.transform.position.x + oldSlot.transform.position.x) / 2f;
                    float num2 = base.Card.gameObject.transform.position.y + 0.35f;
                    float z = base.Card.gameObject.transform.position.z;
                    Tween.Position(base.Card.transform, new Vector3(num, num2, z), 0.3f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                    yield return new WaitForSeconds(0.25f);
                }

                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, destination, 0.1f, null, true);
                yield return this.PostSuccessfulMoveSequence(oldSlot);
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
        protected virtual IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
        {
            if(base.Card.Info.GetExtendedProperty("LeaperLeaveBehind") != null)

            {
                yield return new WaitForSeconds(0.1f);
                if (oldSlot && oldSlot.Card == null)
                {
                    CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("LeaperLeaveBehind"));
                    segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Leaper.ability }));
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.1f, true);
                }
            }
            yield break;
        }


        protected bool movingLeft;
    }
}
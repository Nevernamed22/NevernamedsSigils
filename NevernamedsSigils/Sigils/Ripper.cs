using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class Ripper : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Ripper", "[creature] will move to and destroy any free, 1 health, non-terrain creatures played, and becomes stronger for each one consumed.",
                      typeof(Ripper),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ripper.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ripper_pixel.png"));

            Ripper.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            if (((otherCard.Info.IsFree() && otherCard.MaxHealth == 1) || otherCard.HasTrait(Trait.Goat)) && (otherCard.OpponentCard == base.Card.OpponentCard) && (otherCard != base.Card) && (!otherCard.Info.traits.Contains(Trait.Terrain))) { return true; }
            else return false;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return base.PreSuccessfulTriggerSequence();
            bool isGoat = otherCard.HasTrait(Trait.Goat);
            CardSlot targetSlot = otherCard.slot;

            yield return otherCard.Die(true, base.Card, true);

            if (!base.Card.HasAbility(Stalwart.ability))
            {
                Vector3 midpoint = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;
                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.1f, null, true);
            }

            yield return new WaitForSeconds(0.1f);
            base.Card.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.1f);

            int amt = 1;
            if (isGoat) amt = 3;
            base.Card.temporaryMods.Add(new CardModificationInfo(amt, amt));

            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

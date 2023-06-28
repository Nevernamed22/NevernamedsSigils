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
    public class Antimagic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Antimagic", "Any mox gems played while [creature] is on the board will immediately perish. [creature] will move to the position of the destroyed gem, gain 1 health, and immediately attack.",
                      typeof(Antimagic),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/antimagic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/antimagic_pixel.png"),
                      triggerText: "[creature] destroys the played gem!");

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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            if (otherCard.HasTrait(Trait.Gem)) { return true; }
            else return false;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return base.PreSuccessfulTriggerSequence();           
            CardSlot targetSlot = otherCard.slot;

            yield return otherCard.Die(true, base.Card, true);

            if (!base.Card.HasAbility(Stalwart.ability) && targetSlot.Card == null)
            {
                Vector3 midpoint = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;
                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.1f, null, true);
            }

            yield return new WaitForSeconds(0.1f);
            base.Card.Anim.StrongNegationEffect();
            base.Card.temporaryMods.Add(new CardModificationInfo(0, 1));
            yield return new WaitForSeconds(0.1f);

            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot);

            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

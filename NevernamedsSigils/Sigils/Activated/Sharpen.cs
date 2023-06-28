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
    public class Sharpen : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sharpen", "If you have an orange mox gem, sacrifice it to give [creature] +1 attack power.",
                      typeof(Sharpen),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/sharpen.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/sharpen_pixel.png"),
                      isActivated: true,
                      triggerText: "[creature] sacrifices an orange gem to sharpen it's attack!");

            ability = newSigil.ability;
        }

        public static Ability ability;


        public override bool CanActivate()
        {
            return !base.Card.OpponentCard && Singleton<BoardManager>.Instance.PlayerSlotsCopy.Exists(x => x.Card != null && x.Card.HasTrait(Trait.Gem) && (x.Card.HasAbility(Ability.GainGemOrange) || x.Card.HasAbility(Ability.GainGemTriple)));
        }
        public override IEnumerator Activate()
        {
            yield return new WaitForSeconds(0.15f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return TriggerGems();

        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard && !playerUpkeep && Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => x.Card != null && x.Card.HasTrait(Trait.Gem) && (x.Card.HasAbility(Ability.GainGemOrange) || x.Card.HasAbility(Ability.GainGemTriple)));
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
                yield return TriggerGems();
            yield break;
        }
        public IEnumerator TriggerGems()
        {
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard))
            {
                if (slot.Card != null && !slot.Card.Dead && slot.Card.HasTrait(Trait.Gem) && (slot.Card.HasAbility(Ability.GainGemOrange) || slot.Card.HasAbility(Ability.GainGemTriple)))
                {            
                   yield return base.PreSuccessfulTriggerSequence(); 
                    yield return slot.Card.Die(false, base.Card);
                    yield return new WaitForSeconds(0.2f);
                    base.Card.AddTemporaryMod(new CardModificationInfo(1, 0));
                    base.Card.Anim.StrongNegationEffect();
                    yield return new WaitForSeconds(0.1f);
                    break;
                }
            }
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

    }
}
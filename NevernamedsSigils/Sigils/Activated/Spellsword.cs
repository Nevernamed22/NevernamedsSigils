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
    public class Spellsword : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Spellsword", "All gems on the same side of the board as [creature] gain 2 power, attack, and perish.",
                      typeof(Spellsword),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/spellsword.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/spellsword_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;


        public override bool CanActivate()
        {
            return !base.Card.OpponentCard && Singleton<BoardManager>.Instance.PlayerSlotsCopy.Exists(x => x.Card != null && x.Card.HasTrait(Trait.Gem));
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
            return base.Card.OpponentCard && !playerUpkeep && Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => x.Card != null && x.Card.HasTrait(Trait.Gem));
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (UnityEngine.Random.value <= 0.5f)
            {
                yield return TriggerGems();
            }
            yield break;
        }
        public IEnumerator TriggerGems()
        {
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard))
            {
                if (slot.Card != null && !slot.Card.Dead && slot.Card.HasTrait(Trait.Gem))
                {
                    slot.Card.AddTemporaryMod(new CardModificationInfo(2, 0));
                    FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                    yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, slot);
                    yield return new WaitForSeconds(0.1f);
                    if (slot.Card != null && !slot.Card.Dead) yield return slot.Card.Die(false);
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
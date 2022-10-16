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
    public class Trio : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trio", "When [creature] is played, a copy of it is created to the left and right.",
                      typeof(Trio),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/trio.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/trio_pixel.png"));

            Trio.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {

            CardInfo inf = Tools.TrueClone(base.Card.Info);
            inf.mods.Add(new CardModificationInfo() { negateAbilities = new List<Ability>() { Trio.ability } });
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(inf, slot, 0.15f, true);
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            yield return new WaitForSeconds(0.1f);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            yield return base.PreSuccessfulTriggerSequence();

            if (toLeftValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toLeft);
            }

            if (toRightValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toRight);
            }

            if (toLeftValid || toRightValid)
            {
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

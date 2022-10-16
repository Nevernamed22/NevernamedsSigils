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
    public class Clawed : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Clawed", "When [creature] is played, claws are created to the left and right. Any creatures in those spaces will be killed.",
                      typeof(Clawed),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/clawed.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/clawed_pixel.png"));

            Clawed.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        private IEnumerator SpawnCardOnSlot(CardSlot slot, bool Left)
        {
            CardInfo claw = CardLoader.GetCardByName("Nevernamed ClawRight").Clone() as CardInfo;
            if (Left) claw = CardLoader.GetCardByName("Nevernamed ClawLeft").Clone() as CardInfo;

            foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                claw.mods.Add(clone);
            }

            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(claw, slot, 0.1f, true);
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
            yield return base.PreSuccessfulTriggerSequence();

            if (toLeft != null)
            {
                if (toLeft.Card != null) yield return toLeft.Card.Die(false, null, true);
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toLeft, true);
            }

            if (toRight != null)
            {
                if (toRight.Card != null) yield return toRight.Card.Die(false, null, true);
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toRight, false);
            }

            if (toLeft != null || toRight != null)
            {
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

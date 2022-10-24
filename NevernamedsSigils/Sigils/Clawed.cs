using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

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
            CardInfo claw;
            if (Left)
            {
                string clawName = "SigilNevernamed ClawLeft";
                if (Card.Info.GetExtendedProperty("ClawedLeftClawOverride") != null) { clawName = Card.Info.GetExtendedProperty("ClawedLeftClawOverride"); }
                claw = CardLoader.GetCardByName(clawName).Clone() as CardInfo;
            }
            else
            {
                string clawName = "SigilNevernamed ClawRight";
                if (Card.Info.GetExtendedProperty("ClawedRightClawOverride") != null) { clawName = Card.Info.GetExtendedProperty("ClawedRightClawOverride"); }
                claw = CardLoader.GetCardByName(clawName).Clone() as CardInfo;
            }

            claw.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Clawed.ability, Ability.DeathShield }));

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

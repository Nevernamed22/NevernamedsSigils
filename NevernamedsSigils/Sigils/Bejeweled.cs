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
    public class Bejeweled : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bejeweled", "While [creature] is on the field, lining up three gems of the same colour will remove those gems from the field. Any creatures which opposed those gems will be removed as well, and the opponent will take damage directly equal to their combined attack power. ",
                      typeof(Bejeweled),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bejeweled.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bejeweled_pixel.png"));

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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard) { return true; }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard) { yield return RunGemsCheck(); yield break; }
        public override bool RespondsToResolveOnBoard() { return true; }
        public override IEnumerator OnResolveOnBoard() { yield return RunGemsCheck(); yield break; }
        public override IEnumerator OnUpkeep(bool playerUpkeep) { yield return RunGemsCheck(); yield break; }
        public override bool RespondsToUpkeep(bool playerUpkeep) { return true; }

        private IEnumerator RunGemsCheck()
        {
            List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard);

            List<CardSlot> foundGems = new List<CardSlot>();
            bool gemsvalid = false;
            foreach (CardSlot slot in slots)
            {
                if (!gemsvalid)
                {
                    if (slot.Card != null && slot.Card.HasTrait(Trait.Gem))
                    {
                        bool toLeftValid = false;
                        bool toRightValid = false;

                        bool trimox = slot.Card.HasAbility(Ability.GainGemTriple);
                        bool hasGreenGem = slot.Card.HasAbility(Ability.GainGemGreen) || trimox;
                        bool hasBlueGem = slot.Card.HasAbility(Ability.GainGemBlue) || trimox;
                        bool hasOrangeGem = slot.Card.HasAbility(Ability.GainGemOrange) || trimox;

                        CardSlot left = Singleton<BoardManager>.Instance.GetAdjacent(slot, true);
                        CardSlot right = Singleton<BoardManager>.Instance.GetAdjacent(slot, false);
                        if (left != null && left.Card)
                        {
                            bool leftHastri = left.Card.HasAbility(Ability.GainGemTriple);
                            if ((left.Card.HasAbility(Ability.GainGemGreen) || leftHastri) && hasGreenGem) { toLeftValid = true; }
                            else if ((left.Card.HasAbility(Ability.GainGemBlue) || leftHastri) && hasBlueGem) { toLeftValid = true; }
                            else if ((left.Card.HasAbility(Ability.GainGemOrange) || leftHastri) && hasOrangeGem) { toLeftValid = true; }
                        }
                        if (right != null && right.Card)
                        {
                            bool rightHastri = right.Card.HasAbility(Ability.GainGemTriple);
                            if ((right.Card.HasAbility(Ability.GainGemGreen) || rightHastri) && hasGreenGem) { toRightValid = true; }
                            else if ((right.Card.HasAbility(Ability.GainGemBlue) || rightHastri) && hasBlueGem) { toRightValid = true; }
                            else if ((right.Card.HasAbility(Ability.GainGemOrange) || rightHastri) && hasOrangeGem) { toRightValid = true; }
                        }

                        if (toLeftValid && toRightValid)
                        {
                            gemsvalid = true;
                            foundGems.AddRange(new List<CardSlot>() { left, slot, right });
                        }
                    }
                }
            }
            if (gemsvalid)
            {
                yield return DoGemBuster(foundGems);
            }
            yield break;
        }

        private IEnumerator DoGemBuster(List<CardSlot> gems)
        {
            yield return base.PreSuccessfulTriggerSequence();


            List<CardSlot> enemycards = new List<CardSlot>();
            foreach (CardSlot slot in gems)
            {
                if (slot.opposingSlot != null) { enemycards.Add(slot.opposingSlot); }
                if (slot.Card != null)
                {
                    slot.Card.Anim.PlayDeathAnimation(true);
                    GameObject objec = slot.Card.gameObject;
                    slot.Card.UnassignFromSlot();
                    yield return new WaitForSeconds(0.2f);
                    UnityEngine.Object.Destroy(objec);
                }
            }
            Singleton<ResourcesManager>.Instance.ForceGemsUpdate();
            View preview = Singleton<ViewManager>.Instance.CurrentView;

            foreach (CardSlot slot in enemycards)
            {
                if (slot.Card != null)
                {
                    int damage = slot.Card.Attack;
                    slot.Card.Anim.PlayDeathAnimation(true);
                    GameObject objec = slot.Card.gameObject;
                    slot.Card.UnassignFromSlot();
                    yield return new WaitForSeconds(0.2f);
                    UnityEngine.Object.Destroy(objec);
                    if (damage > 0) { yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, !base.Card.slot.IsPlayerSlot, 0.1f, null, 0f, true); }
                    yield return new WaitForSeconds(0.1f);
                    Singleton<ViewManager>.Instance.SwitchToView(preview, false, false);
                }
            }

            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

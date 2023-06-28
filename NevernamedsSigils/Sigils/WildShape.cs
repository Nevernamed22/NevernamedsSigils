using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class WildShape : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Wild Shape", "When [creature] is played, it transforms based on what gem colours its owner controls.",
                      typeof(WildShape),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/wildshape.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/wildshape_pixel.png"),
                      triggerText: "[creature] transforms based on its owner's gems..."
                      );

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
        public override bool RespondsToResolveOnBoard()
        {
            List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard);
            return OwnerHasGemSigil(Ability.GainGemOrange, slots) || OwnerHasGemSigil(Ability.GainGemGreen, slots) || OwnerHasGemSigil(Ability.GainGemBlue, slots);
        }
        public override IEnumerator OnResolveOnBoard()
        {
            List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard);
            yield return base.PreSuccessfulTriggerSequence();

            if (OwnerHasGemSigil(Ability.GainGemOrange, slots) && OwnerHasGemSigil(Ability.GainGemBlue, slots) && OwnerHasGemSigil(Ability.GainGemGreen, slots))
            { //Magnus
                if (base.Card.Info.GetExtendedProperty("WildShapeFormAllGems") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormAllGems")); }
                else
                {
                    CardModificationInfo toGive = new CardModificationInfo(1, 2);
                    toGive.abilities.Add(Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, base.Card, null));
                    base.Card.AddTemporaryMod(toGive);
                    base.Card.RenderCard();
                }
            }
            else if (OwnerHasGemSigil(Ability.GainGemOrange, slots) && OwnerHasGemSigil(Ability.GainGemBlue, slots))
            { //Orlu
                if (base.Card.Info.GetExtendedProperty("WildShapeFormOrangeBlue") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormOrangeBlue")); }
                else
                {
                    CardModificationInfo toGive = new CardModificationInfo(1, 0);
                    toGive.abilities.Add(Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, base.Card, null));
                    base.Card.AddTemporaryMod(toGive);
                    base.Card.RenderCard();
                }
            }
            else if (OwnerHasGemSigil(Ability.GainGemOrange, slots) && OwnerHasGemSigil(Ability.GainGemGreen, slots))
            { //Goranj
                if (base.Card.Info.GetExtendedProperty("WildShapeFormGreenOrange") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormGreenOrange")); }
                else
                {
                    base.Card.AddTemporaryMod(new CardModificationInfo(1, 2));
                    base.Card.RenderCard();
                }
            }
            else if (OwnerHasGemSigil(Ability.GainGemGreen, slots) && OwnerHasGemSigil(Ability.GainGemBlue, slots))
            { //Bleene
                if (base.Card.Info.GetExtendedProperty("WildShapeFormBlueGreen") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormBlueGreen")); }
                else
                {
                    CardModificationInfo toGive = new CardModificationInfo(0, 2);
                    toGive.abilities.Add(Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, base.Card, null));
                    base.Card.AddTemporaryMod(toGive);
                    base.Card.RenderCard();
                }
            }
            else if (OwnerHasGemSigil(Ability.GainGemOrange, slots))
            { //Orange
                if (base.Card.Info.GetExtendedProperty("WildShapeFormOrange") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormOrange")); }
                else
                {
                    base.Card.AddTemporaryMod(new CardModificationInfo(1, 0));
                    base.Card.RenderCard();
                }
            }
            else if (OwnerHasGemSigil(Ability.GainGemGreen, slots))
            { //Green
                if (base.Card.Info.GetExtendedProperty("WildShapeFormGreen") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormGreen")); }
                else
                {
                    base.Card.AddTemporaryMod(new CardModificationInfo(0, 2));
                    base.Card.RenderCard();
                }
            }
            else if (OwnerHasGemSigil(Ability.GainGemBlue, slots))
            { //Blue
                if (base.Card.Info.GetExtendedProperty("WildShapeFormBlue") != null) { yield return EvolveIntoSetCard(base.Card.Info.GetExtendedProperty("WildShapeFormBlue")); }
                else
                {
                    base.Card.AddTemporaryMod(new CardModificationInfo(Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, base.Card, null)));
                    base.Card.RenderCard();
                }
            }
        }
        private bool OwnerHasGemSigil(Ability sigil, List<CardSlot> overrideslots = null)
        {
            if (overrideslots != null)
            {
                return overrideslots.Exists(x => x.Card != null && (x.Card.HasAbility(sigil) || x.Card.HasAbility(Ability.GainGemTriple)));
            }
            else return Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).Exists(x => x.Card != null && (x.Card.HasAbility(sigil) || x.Card.HasAbility(Ability.GainGemTriple)));
        }
        private IEnumerator EvolveIntoSetCard(string name)
        {
            CardInfo form = CardLoader.GetCardByName(name);
            foreach (CardModificationInfo cardModificationInfo in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo cardModificationInfo2 = (CardModificationInfo)cardModificationInfo.Clone();
                if (cardModificationInfo2.HasAbility(WildShape.ability))
                {
                    cardModificationInfo2.abilities.Remove(WildShape.ability);
                }
                form.Mods.Add(cardModificationInfo2);
            }
            yield return base.Card.TransformIntoCard(form, new Action(this.TriggerRandomAbilities), null);
            yield return new WaitForSeconds(0.5f);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
        private void TriggerRandomAbilities()
        {
            if (base.Card.GetComponent<RandomAbility>()) { base.Card.GetComponent<RandomAbility>().AddMod(); }
            if (base.Card.GetComponent<ChaosStrike>()) { base.Card.GetComponent<ChaosStrike>().AddMod(); }
        }
    }
}
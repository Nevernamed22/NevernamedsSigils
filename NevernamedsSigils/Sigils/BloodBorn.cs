using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class BloodBorn : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bloodborn", "When [creature] is sacrificed, it transforms to or from its alternate form instead of perishing.",
                      typeof(BloodBorn),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodborn.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bloodborn_pixel.png"));

            BloodBorn.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        bool isCurrentlyStatTransformed;
        public CardModificationInfo statTransformation;
        public override bool RespondsToUpkeep(bool playerUpkeep) { return true; }
        public override IEnumerator OnUpkeep(bool playerUpkeep) { RecalculateGhostSigil(false); yield break; }
        public override bool RespondsToResolveOnBoard() { return true; }
        public override IEnumerator OnResolveOnBoard() { RecalculateGhostSigil(true); yield break; }
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard) { return true; }
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard) { RecalculateGhostSigil(false); yield break; }
        private void RecalculateGhostSigil(bool callerResolve)
        {
            if (base.Card.HasAbility(Ability.Sacrificial))
            {
                return;
            }
            base.Card.Status.hiddenAbilities.Remove(Ability.Sacrificial);
            base.Card.RenderCard();
            if (base.Card.OnBoard)
            {
                CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Sacrificial);
                cardModificationInfo.RemoveOnUpkeep = true;
                cardModificationInfo.singletonId = "BloodBornManyLivesPhantom";
                base.Card.Status.hiddenAbilities.Add(Ability.Sacrificial);
                base.Card.AddTemporaryMod(cardModificationInfo);
                base.Card.RenderCard();
            }
        }
        public override bool RespondsToSacrifice() { return true; }
        public override IEnumerator OnSacrifice()
        {
            if (Card.Info.GetExtendedProperty("BloodBornTransformation") != null)
            {
                CardInfo target = CardLoader.GetCardByName(Card.Info.GetExtendedProperty("BloodBornTransformation"));
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (target.HasAbility(BloodBorn.ability) && clone.HasAbility(BloodBorn.ability))
                    {
                        clone.abilities.Remove(BloodBorn.ability);
                    }
                    target.Mods.Add(clone);
                }
                yield return base.PreSuccessfulTriggerSequence();

                if (!target.HasAbility(BloodBorn.ability))
                {
                    base.Card.temporaryMods.RemoveAll(x => x.singletonId == "BloodBornManyLivesPhantom");
                }

                yield return base.Card.TransformIntoCard(target);
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.5f);
            }
            else //Gonna do a generic stat transformation
            {
                if (statTransformation == null)
                {
                    statTransformation = new CardModificationInfo(2, 0);
                }
                if (isCurrentlyStatTransformed)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.15f);
                    base.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    base.Card.RemoveTemporaryMod(statTransformation);
                    base.Card.RenderCard();
                    isCurrentlyStatTransformed = false;
                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.15f);
                    base.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    base.Card.AddTemporaryMod(statTransformation);
                    base.Card.RenderCard();
                    isCurrentlyStatTransformed = true;

                }
            }
            yield break;
        }
    }
}

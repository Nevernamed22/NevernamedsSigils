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
    public class TrainedFlier : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Trained Flier", "Grant [creature] the effect of the Airborne sigil until next upkeep. If the card already has Airborne, this will remove its effect until next upkeep.",
                      typeof(TrainedFlier),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/trainedflier.png"),
                     pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/trainedflier_pixel.png"),
                      isActivated: true);

            TrainedFlier.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard != playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            base.Card.Status.hiddenAbilities.Remove(Ability.Flying);
            base.Card.RenderCard();
            usedThisTurn = false;
            if (base.Card.OnBoard && base.Card.slot.opposingSlot && base.Card.OpponentCard)
            {
                bool shouldFly = false;
                if (base.Card.slot.opposingSlot.Card != null && base.Card.slot.opposingSlot.Card.Attack == 0) { shouldFly = true; }
                if (base.Card.slot.opposingSlot.Card == null && Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x != null && x.Card != null && x.Card.HasAbility(Ability.WhackAMole)).Count > 0) { shouldFly = true; }
                if (shouldFly && !base.Card.HasAbility(Ability.Flying))
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Flying);
                    cardModificationInfo.RemoveOnUpkeep = true;
                    base.Card.Status.hiddenAbilities.Add(Ability.Flying);
                    base.Card.AddTemporaryMod(cardModificationInfo);
                    Vector3 position = base.Card.transform.position;
                    Tween.Position(base.Card.transform, position + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                    Tween.Position(base.Card.transform, position, 1f, 0.1f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                }
                else if (!shouldFly && base.Card.HasAbility(Ability.Flying))
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.RemoveOnUpkeep = true;
                    cardModificationInfo.negateAbilities = new List<Ability>() { Ability.Flying };
                    base.Card.AddTemporaryMod(cardModificationInfo);
                }
                base.Card.RenderCard();
            }
            yield break;
        }
        private bool usedThisTurn;
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (!usedThisTurn)
            {
                if (base.Card.HasAbility(Ability.Flying))
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.RemoveOnUpkeep = true;
                    cardModificationInfo.negateAbilities = new List<Ability>() { Ability.Flying };
                    base.Card.AddTemporaryMod(cardModificationInfo);
                }
                else
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Flying);
                    cardModificationInfo.RemoveOnUpkeep = true;
                    base.Card.Status.hiddenAbilities.Add(Ability.Flying);
                    base.Card.AddTemporaryMod(cardModificationInfo);
                    Vector3 position = base.Card.transform.position;
                    Tween.Position(base.Card.transform, position + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                    Tween.Position(base.Card.transform, position, 1f, 0.1f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                }
                base.Card.RenderCard();
                usedThisTurn = true;
            }
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}

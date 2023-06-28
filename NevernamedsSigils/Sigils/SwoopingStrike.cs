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
    public class SwoopingStrike : ExtendedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Swooping Strike", "[creature] will strike the opposing space a second time when attacking. Also, [creature]'s first strike each turn will occur directly, over opponent creatures.",
                      typeof(SwoopingStrike),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.BountyHunter },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/swoopingstrike.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/swoopingstrike_pixel.png"));

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

        //Various Variables
        bool isFlying;
        bool hideFlight = true;
        int strikesthisturn = 0;
        CardModificationInfo flying = new CardModificationInfo(Ability.Flying);
        CardModificationInfo negateFlying = new CardModificationInfo() { negateAbilities = new List<Ability>() { Ability.Flying } };

        public void SetFlying()
        {
            //If the card is not already flying
            if (!isFlying)
            {
                if (hideFlight) base.Card.Status.hiddenAbilities.Add(Ability.Flying); //Only hide flight if the card does not have flying by default
                if (base.Card.temporaryMods.Contains(negateFlying)) base.Card.RemoveTemporaryMod(negateFlying); // Only remove negateflying if the card has it.
               if (hideFlight) base.Card.AddTemporaryMod(flying); //Only adds flight if the card does not have flying by default
                isFlying = true;
            }
        }
        public void SetGrounded()
        {
            if (isFlying)
            {
                if (hideFlight) base.Card.Status.hiddenAbilities.Add(Ability.Flying); 
                if (base.Card.temporaryMods.Contains(flying)) base.Card.RemoveTemporaryMod(flying);
                base.Card.AddTemporaryMod(negateFlying);
                isFlying = false;
            }
        }
        //Upkeep to make the card fly again at the beginning of each turn.
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard && !base.Card.HasAbility(TrainedFlier.ability);
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            SetFlying();
            strikesthisturn = 0;
            yield break;
        }

        //Makes sure the ability is properly overridden by Trained Flier.
        public override bool RespondsToResolveOnBoard()
        {
            return !base.Card.HasAbility(TrainedFlier.ability);
        }
        public override IEnumerator OnResolveOnBoard()
        {
            hideFlight = true;
            if (base.Card.HasAbility(Ability.Flying) && !base.Card.Status.hiddenAbilities.Contains(Ability.Flying)) hideFlight = false; //Don't bother hiding flight of the card has flying by default.
            SetFlying();
            strikesthisturn = 0;
            yield break;
        }

        //On slots targeted, we can check the number of times the card has attacked and remove flight if it's more than one.
        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return attacker == base.Card;
        }
        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (strikesthisturn > 0)
            {
                yield return new WaitForSeconds(0.2f);
                SetGrounded();
            }
            strikesthisturn++;
            yield break;
        }

        //Make the card attack a second time
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            return new List<CardSlot>() { base.Card.slot.opposingSlot };
        }
        public override bool RespondsToGetOpposingSlots()
        {
            return base.Card && base.Card.slot && base.Card.slot.opposingSlot;
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return false;
        }
    }
}

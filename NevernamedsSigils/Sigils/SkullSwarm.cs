using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Triggers;

namespace NevernamedsSigils
{
    public class SkullSwarm : AbilityBehaviour, IOnBellRung
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Skull Swarm", "While [creature] is alive and on the board, all Skeletons will strike twice when attacking.",
                      typeof(SkullSwarm),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/skullswarm.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/skullswarm_pixel.png"));

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
        public bool RespondsToBellRung(bool playerCombatPhase)
        {
            return playerCombatPhase != base.Card.OpponentCard;
        }

        public IEnumerator OnBellRung(bool playerCombatPhase)
        {
            List<CardSlot> applicableSlots = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.opponentSlots : Singleton<BoardManager>.Instance.playerSlots;
            foreach(CardSlot slot in applicableSlots)
            {
                if (slot.Card && (slot.Card.Info.name.ToLower().Contains("skeleton") || slot.Card.Info.displayedName.ToLower().Contains("skeleton")) && !slot.Card.HasAbility(Ability.DoubleStrike))
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.DoubleStrike);
                    cardModificationInfo.singletonId = "skull_swarm";
                    cardModificationInfo.RemoveOnUpkeep = true;
                    slot.Card.Status.hiddenAbilities.Add(Ability.DoubleStrike);
                    slot.Card.AddTemporaryMod(cardModificationInfo);
                }
            }
            yield break;
        }
    }
}
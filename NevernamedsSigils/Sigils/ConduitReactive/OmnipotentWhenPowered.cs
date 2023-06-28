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
    public class OmnipotentWhenPowered : ExtendedAbilityBehaviour
    {
        new public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Omnipotent When Powered", "If [creature] is within a circuit, it will strike every opposing space containing a card when it attacks. [creature] will strike directly if no creatures oppose it, or if it is not in a circuit.",
                      typeof(OmnipotentWhenPowered),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/omnipotentwhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/omnipotentwhenpowered_pixel.png"),
                      isConduitCell: true);

            ability = newSigil.ability;
        }
        new public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToGetOpposingSlots()
        {
            return Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot) && (base.Card.OpponentCard ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy).Exists(x => x.Card != null && !base.Card.CanAttackDirectly(x));
        }
        public override bool RemoveDefaultAttackSlot()
        {
            return true;
        }
        public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
        {
            List<CardSlot> toReturn = new List<CardSlot>() { };
            List<CardSlot> opponentSlots = base.Card.OpponentCard ? Singleton<BoardManager>.Instance.PlayerSlotsCopy : Singleton<BoardManager>.Instance.OpponentSlotsCopy;
            foreach(CardSlot slot in opponentSlots)
            {
                if (slot && slot.Card != null && !base.Card.CanAttackDirectly(slot)) { toReturn.Add(slot);  }
            }
            return toReturn;
        }
    }
}
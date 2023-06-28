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
    public class GiftWhenPoweredCustom : GiftBearerCustom
    {
        new public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gift When Powered", "If [creature] is within a circuit when it perishes, a random card is created in your hand.",
                      typeof(GiftWhenPoweredCustom),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/giftwhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/giftwhenpowered_pixel.png"),
                      isConduitCell: true);

            ability = newSigil.ability;
        }
        new public static Ability ability;
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
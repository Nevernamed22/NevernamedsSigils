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
    public class Siphon : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Siphon", "Creatures to the left of [creature] lose all attack power. That attack power is instead added to this card's power.",
                      typeof(Siphon),
                      categories: new List<AbilityMetaCategory> {  AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/siphon.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/siphon_pixel.png"));

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
        public int siphonedDamamge = 0;
        public override void ManagedUpdate()
        {
            if (base.Card && base.Card.OnBoard && base.Card.slot && siphonedDamamge > 0)
            {
                CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot, true);
                if (toLeft != null && toLeft.Card == null)
                {
                    siphonedDamamge = 0;
                }

            }
            base.ManagedUpdate();
        }
    }
}
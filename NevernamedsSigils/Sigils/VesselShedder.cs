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
    public class VesselShedder : Strafe
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Vessel Shedder", "At the end of the owner's turn, [creature] will move in the direction inscrybed in the sigil and drop an Empty Vessel in its old place.",
                      typeof(VesselShedder),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/vesselshedder.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/vesselshedder_pixel.png"));

            VesselShedder.ability = newSigil.ability;
        }
        public static Ability ability;
        public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
        {
            bool flag = cardSlot.Card == null;
            if (flag)
            {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("EmptyVessel"), cardSlot, 0.1f, true);
            }
            yield break;
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
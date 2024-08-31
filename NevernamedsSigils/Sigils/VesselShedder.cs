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
                      categories: new List<AbilityMetaCategory> { },
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
                string card = "EmptyVessel";
                if (Tools.GetActAsInt() == 3 && StoryEventsData.EventCompleted(StoryEvent.GemsModuleFetched)) { card = Tools.SeededRandomElement(new List<string>() { "EmptyVessel_BlueGem", "EmptyVessel_GreenGem", "EmptyVessel_OrangeGem" }); }
                CardInfo vessel = CardLoader.GetCardByName(card);
                if (Tools.GetActAsInt() == 3)
                {
                    foreach (Ability ability in Part3SaveData.Data.sideDeckAbilities)
                    {
                        CardModificationInfo cardModificationInfo = new CardModificationInfo(ability);
                        cardModificationInfo.sideDeckMod = true;
                        vessel.Mods.Add(cardModificationInfo);
                    }
                }
                vessel.Mods.Add(base.Card.CondenseMods(new List<Ability>() { VesselShedder.ability }));
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(vessel, cardSlot, 0.1f, true);
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
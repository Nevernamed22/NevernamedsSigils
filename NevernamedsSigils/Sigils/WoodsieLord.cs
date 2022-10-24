using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class WoodsieLord : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Woodsie Lord", "When [creature] is played, Enchanted Pines are created on every empty space. An enchanted pine is defined as 0 power, 3 health.",
                      typeof(WoodsieLord),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/woodsielord_pixel.png"));

            WoodsieLord.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
    {
            yield return new WaitForSeconds(0.15f);

            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
            List<CardSlot> availableEnSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(false));
            availableSlots.AddRange(availableEnSlots);
            for (int i = availableSlots.Count - 1; i >= 0; i--)
            {
                if (availableSlots[i].Card != null) availableSlots.RemoveAt(i);
            }
            if (availableSlots.Count > 0)
            {
                yield return base.PreSuccessfulTriggerSequence();

                foreach (CardSlot targetSlot in availableSlots)
                {
                    yield return new WaitForSeconds(0.1f);
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("SigilNevernamed EnchantedPine"), targetSlot, 0.15f, true);
                }

                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.1f);
            }
        }
    }
}

using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Visceral : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Visceral", "When [creature] is sacrificed, any leftover blood that is not used for the sacrifice will be converted into bones.",
                      typeof(Visceral),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/visceral.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/visceral_pixel.png"));

            Visceral.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToSacrifice()
        {
            return true;
        }
        public override IEnumerator OnSacrifice()
        {
            if (base.Card)
            {
                int blood = Singleton<BoardManager>.Instance.GetValueOfSacrifices(new List<CardSlot>() { base.Card.slot });
                if (Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard != null)
                {
                    int leftovers = blood - Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.Info.BloodCost;
                  if (leftovers > 0)  yield return Singleton<ResourcesManager>.Instance.AddBones(leftovers, base.Card.Slot);
                }
            }
            yield break;
        }
    }
}
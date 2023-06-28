using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class GreenInspiration : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Green Inspiration", "While [creature] is on the board, all friendly creatures which cost a green gem gain +2 health.",
                      typeof(GreenInspiration),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/greeninspiration.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/greeninspiration_pixel.png"));

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
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard))
            {
                if (slot.Card != null && slot.Card.Info.gemsCost != null && slot.Card.Info.gemsCost.Contains(GemType.Green) && !slot.Card.Dead && slot.Card.Health <= 0)
                {
                    yield return new WaitForSeconds(1f);
                    yield return slot.Card.Die(false, null, true);
                }
            }
            yield break;
        }
    }
}
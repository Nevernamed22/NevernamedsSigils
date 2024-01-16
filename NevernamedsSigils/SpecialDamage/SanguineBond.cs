using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;
using OpponentBones;

namespace NevernamedsSigils
{
    public class SanguineBond : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Sanguine Bond", "The value represented with this sigil will be equal to the number of sacrifices made on the turn when the bearer was placed.",
               typeof(SanguineBond),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook },
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/sanguinebond.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/sanguinebond_pixel.png"),
               isForHealth: false,
               gbcDescription: "[creature]s power is equal to the number of sacrifices made on the turn when it was placed.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Sanguine Bond", typeof(SanguineBond)).Id;
            specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return specialStatIcon;
            }
        }
        public override int[] GetStatValues()
        {
            return new int[]
            {
                sacrifices,
                0,
            };
        }
        public int turnWhenPlayed;
        public bool resolved;
        public int sacrifices;
        public bool lockedIn = false;
        public override void ManagedUpdate()
        {
            if (resolved && !lockedIn)
            {
                if (Singleton<TurnManager>.Instance.TurnNumber == turnWhenPlayed && Singleton<BoardManager>.Instance.SacrificesMadeThisTurn != sacrifices)
                {
                    sacrifices = Singleton<BoardManager>.Instance.SacrificesMadeThisTurn;
                }
                if (Singleton<TurnManager>.Instance.TurnNumber != turnWhenPlayed) { lockedIn = true; }
            }
            base.ManagedUpdate();
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            turnWhenPlayed = Singleton<TurnManager>.Instance.TurnNumber;
            resolved = true;
            yield break;
        }
        public static SpecialStatIcon specialStatIcon;
    }
}

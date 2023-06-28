using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class Ambitious : VariableStatBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            StatIconInfo icon = SigilSetupUtility.MakeNewStatIcon("Ambitious", "The value represented with this sigil will be equal to highest power number present on the board, regardless of whether the card bearing it is friend or foe. Updates at the start of both the player and opponent turn.",
               typeof(Ambitious),
               categories: new List<AbilityMetaCategory>() { AbilityMetaCategory.Part1Rulebook},
               tex: Tools.LoadTex("NevernamedsSigils/Resources/Other/ambitious.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelOther/ambitious_pixel.png"),
               gbcDescription: "[creature]s power is equal to the power of the creature with the highest attack power on the board. Recalculates each turn.");

            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Ambitious", typeof(Ambitious)).Id;

            Ambitious.specialStatIcon = icon.iconType;
        }
        public override SpecialStatIcon IconType
        {
            get
            {
                return Ambitious.specialStatIcon;
            }
        }
        public override int[] GetStatValues()
        {

            return new int[]
            {
                damage,
                0
            };
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            Recalc();
            yield break;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            Recalc();
            yield break;
        }
        private void Recalc()
        {
            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
            availableSlots.AddRange(Singleton<BoardManager>.Instance.GetSlots(false));
            int max = 0;
            foreach (CardSlot slot in availableSlots)
            {
                if (slot.Card != null && slot.Card.Attack > max) max = slot.Card.Attack;
            }
            damage = max;
        }
        int damage = 0;

        public static SpecialStatIcon specialStatIcon;
    }
}

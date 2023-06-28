using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using NevernamedsSigils;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class InherentGraveyardShift : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "InherentGraveyardShift", typeof(InherentGraveyardShift)).Id;
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return (base.PlayableCard.OnBoard && !wasSacrifice);
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (!base.PlayableCard.HasAbility(InstantEffect.ability)) yield return new WaitForSeconds(0.35f);

            CardInfo lump = CardLoader.GetCardByName("Opossum");
            if (base.Card.Info.iceCubeParams != null && base.Card.Info.iceCubeParams.creatureWithin != null)
            {
                lump = base.Card.Info.iceCubeParams.creatureWithin.Clone() as CardInfo;
            }
            lump.Mods.Add(base.PlayableCard.CondenseMods());
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(lump, base.PlayableCard.Slot, 0.1f, true);
            yield return new WaitForSeconds(0.35f);
            yield break;
        }
    }
}

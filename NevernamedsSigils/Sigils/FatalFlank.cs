using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class FatalFlank : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Fatal Flank", "When [creature] perishes to combat, it creates a specific creature in all empty adjacent slots.",
                      typeof(FatalFlank),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/fatalflank.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/fatalflank_pixel.png"));

            FatalFlank.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public CardInfo flanker
        {
            get
            {
                CardInfo flanker = (base.Card.Info.GetExtendedProperty("FatalFlankOverride") != null) ? CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("FatalFlankOverride")) : CardLoader.GetCardByName("SigilNevernamed UnnaturalCreature");
                flanker.Mods.Add(base.Card.CondenseMods(new List<Ability>() { FatalFlank.ability }));
                return flanker;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            if (toLeft != null && toLeft.Card == null) { yield return Singleton<BoardManager>.Instance.CreateCardInSlot(flanker, toLeft, 0.1f, true); }
            if (toRight != null && toRight.Card == null) { yield return Singleton<BoardManager>.Instance.CreateCardInSlot(flanker, toRight, 0.1f, true); }       
            yield return base.LearnAbility(0f);
        }
    }
}

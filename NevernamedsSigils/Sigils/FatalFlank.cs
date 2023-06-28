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
    public class FatalFlank : DrawCreatedCard
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
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            storedSlot = base.Card.slot;
            yield break;
        }
        public CardSlot storedSlot;
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            if (killer != null && killer.HasAbility(Bisection.ability))
            {
                yield return base.CreateDrawnCard();
                yield return base.CreateDrawnCard();
            }
            else
            {
                CardSlot toUse = null;
                if (base.Card.slot != null) { toUse = base.Card.slot; }
                else if (storedSlot != null) { toUse = storedSlot; }
                if (toUse != null)
                {
                    CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(toUse, true);
                    CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(toUse, false);
                    if (toLeft != null && toLeft.Card == null) { yield return Singleton<BoardManager>.Instance.CreateCardInSlot(flanker, toLeft, 0.1f, true); }
                    if (toRight != null && toRight.Card == null) { yield return Singleton<BoardManager>.Instance.CreateCardInSlot(flanker, toRight, 0.1f, true); }
                }
            }
            yield return base.LearnAbility(0f);
        }
        public override CardInfo CardToDraw
        {
            get
            {
                return flanker;
            }
        }
        public override List<CardModificationInfo> CardToDrawTempMods
        {
            get
            {
                return null;
            }
        }
    }
}

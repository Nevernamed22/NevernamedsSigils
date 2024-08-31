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
    public class FleaRidden : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Flea Ridden", "When [creature] perishes to combat, it creates a Flea in all empty adjacent slots. A Flea is defined as 0 Power, 1 Health, Eager, Frail",
                      typeof(FleaRidden),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/flearidden.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/flearidden_pixel.png"));

            ability = newSigil.ability;
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
                CardInfo flanker = (base.Card.Info.GetExtendedProperty("FleaRiddenOverride") != null) ? CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("FleaRiddenOverride")) : CardLoader.GetCardByName("SigilNevernamed Flea");
                flanker.Mods.Add(base.Card.CondenseMods(new List<Ability>() { FleaRidden.ability }));
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
            yield return base.LearnAbility(0f);
        }       
    }
}

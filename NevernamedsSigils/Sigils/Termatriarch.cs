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
    public class Termatriarch : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Termatriarch", "While [creature] and a different card bearing the Termite King sigil are alive and on the board, a termite will be created in your hand at the start of your turn.",
                      typeof(Termatriarch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/termatriarch.png"),
                      pixelTex: null);

            Termatriarch.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo guts = CardLoader.GetCardByName("Nevernamed Termite");
                if (base.Card != null)
                {
                    //Get fathers
                    List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard));
                    List<CardSlot> fathers = availableSlots.FindAll((x) => x != null && x.Card != base.Card && x.Card.HasAbility(TermiteKing.ability));

                    //Mother
                    guts.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Termatriarch.ability, TermiteKing.ability }));

                    //Father
                    if (fathers != null && fathers.Count > 0)
                    {
                        PlayableCard chosenFather = Tools.RandomElement(fathers).Card;
                        if (chosenFather != null)
                        {
                            guts.Mods.Add(chosenFather.CondenseMods(new List<Ability>() { Termatriarch.ability, TermiteKing.ability }));
                        }
                    }                                 
                }
                return guts;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard && base.Card.OnBoard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard));
            if (availableSlots.Exists((CardSlot x) => x.Card != null && x.Card != base.Card &&  x.Card.HasAbility(TermiteKing.ability)))
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.CreateDrawnCard();
                yield return base.LearnAbility(0f);
            }
        }
    }
}

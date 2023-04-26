using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Propagation : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Propagation", "When [creature] attacks an opposing creature and it perishes, a creature is created on the board to the left or right of this card.",
                      typeof(Propagation),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/propagation.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/propagation_pixel.png"));

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


        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();

            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            if (toLeft != null && toLeft.Card == null)
            {
              yield return  SpawnCardOnSlot(toLeft);
            }
            else if (toRight != null && toRight.Card == null)
            {
                yield return SpawnCardOnSlot(toRight);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
            }
            yield return base.LearnAbility(0.25f);
        }
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {
            CardInfo bud;
            string budName = "SigilNevernamed Bud";
            if (Card.Info.GetExtendedProperty("PropagationOverride") != null) { budName = Card.Info.GetExtendedProperty("PropagationOverride"); }
            bud = CardLoader.GetCardByName(budName).Clone() as CardInfo;

            bud.Mods.Add(base.Card.CondenseMods(new List<Ability>() { Propagation.ability }));

            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(bud, slot, 0.1f, true);
            yield break;
        }
    }
}

using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class RockEater : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Rock Eater", "When a Terrain card is played on the same side of the board as [creature], it will be consumed, granting it's stats and sigils to the card.",
                      typeof(RockEater),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/rockeater.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/rockeater_pixel.png"));

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
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            if (base.Card.OnBoard && otherCard && otherCard.OnBoard && otherCard != base.Card && otherCard.OpponentCard == base.Card.OpponentCard &&( otherCard.HasTrait(Trait.Terrain) || otherCard.HasTrait(Trait.Pelt))) return true;
            else return false;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            CardModificationInfo assimilation = new CardModificationInfo(otherCard.Attack, otherCard.Health);

            foreach (Ability ab in otherCard.GetAllAbilities())
            {
                assimilation.abilities.Add(ab);
            }

            base.Card.AddTemporaryMod(assimilation);
            otherCard.UnassignFromSlot();
            UnityEngine.Object.Destroy(otherCard.gameObject);

            base.Card.Anim.StrongNegationEffect();
            base.Card.RenderCard();
            yield break;
        }
    }
}
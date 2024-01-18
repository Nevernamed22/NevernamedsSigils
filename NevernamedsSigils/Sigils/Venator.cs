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
    public class Venator : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Venator", "When [creature] kills another creature, it will transform to or from its alternate form.",
                      typeof(Venator),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/venator.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/venator_pixel.png"));

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
            return base.Card.OnBoard && killer == base.Card;
        }

        bool isCurrentlyStatTransformed;
        public CardModificationInfo statTransformation;

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            if (Card.Info.GetExtendedProperty("VenatorTransformation") != null)
            {
                CardInfo target = CardLoader.GetCardByName(Card.Info.GetExtendedProperty("VenatorTransformation"));
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (target.HasAbility(Venator.ability) && clone.HasAbility(Venator.ability))
                    {
                        clone.abilities.Remove(Venator.ability);
                    }
                    target.Mods.Add(clone);
                }
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.Card.TransformIntoCard(target);
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0.5f);
            }
            else //Gonna do a generic stat transformation
            {
                if (statTransformation == null)
                {
                    statTransformation = new CardModificationInfo(2, 0);
                }
                if (isCurrentlyStatTransformed)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.15f);
                    base.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    base.Card.RemoveTemporaryMod(statTransformation);
                    base.Card.RenderCard();
                    isCurrentlyStatTransformed = false;
                }
                else
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    yield return new WaitForSeconds(0.15f);
                    base.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    base.Card.AddTemporaryMod(statTransformation);
                    base.Card.RenderCard();
                    isCurrentlyStatTransformed = true;

                }
            }
            yield break;
        }
    }
}
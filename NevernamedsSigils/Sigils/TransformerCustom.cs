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
    public class TransformerCustom : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Transformer", "At the end of the turn, [creature] will transform to or from it's alternate form.",
                      typeof(TransformerCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/transformer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/transformer_pixel.png"));

            TransformerCustom.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return TransformerCustom.ability;
            }
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep;
        }
        private bool hasWaitedForOneTurn;
        bool isCurrentlyStatTransformed;
        public CardModificationInfo statTransformation;
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (Card.Info.GetExtendedProperty("WaitsForOneTurnOnCustomTransform") != null && !hasWaitedForOneTurn) { hasWaitedForOneTurn = true; }
            else
            {
                if (Card.Info.GetExtendedProperty("CustomTransformerTransformation") != null) //Is actually gonna transform into a card
                {
                    CardInfo target = CardLoader.GetCardByName(Card.Info.GetExtendedProperty("CustomTransformerTransformation"));
                    foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                    {
                        CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                        if (target.HasAbility(TransformerCustom.ability) && clone.HasAbility(TransformerCustom.ability))
                        {
                            clone.abilities.Remove(TransformerCustom.ability);
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
                        statTransformation.nameReplacement = "Twisted " + base.Card.Info.DisplayedNameLocalized;
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
                if (base.Card.Health <= 0)
                {
                    yield return base.Card.Die(false, null, true);
                }
            }
            yield break;
        }
    }
}
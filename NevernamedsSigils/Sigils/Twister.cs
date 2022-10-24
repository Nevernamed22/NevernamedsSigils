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
    public class Twister : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Twister", "When struck, [creature] will transform to or from it's alternate form.",
                      typeof(Twister),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/twister.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/twister_pixel.png"));

            Twister.ability = newSigil.ability;
        }
        /*public static Dictionary<string, string> baseTransforms = new Dictionary<string, string>()
            {
                //Test Transformations


                //Real ones
                {"Nevernamed Macracantha", "Nevernamed MacracanthaBi"},
                {"Nevernamed MacracanthaBi", "Nevernamed MacracanthaTri"},
                {"Nevernamed MacracanthaTri", "Nevernamed Macracantha"},

                {"Nevernamed MauiDolphin", "Nevernamed MauiDolphinSubmerged"},

            };*/
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return TransformerCustom.ability;
            }
        }

        
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return !base.Card.Dead;
        }

        bool isCurrentlyStatTransformed;
        public CardModificationInfo statTransformation;



        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (Card.Info.GetExtendedProperty("TwisterTransformation") != null)
            {
                CardInfo target = CardLoader.GetCardByName(Card.Info.GetExtendedProperty("TwisterTransformation"));
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (target.HasAbility(Twister.ability) && clone.HasAbility(Twister.ability))
                    {
                        clone.abilities.Remove(Twister.ability);
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
            yield break;
        }
    }
}
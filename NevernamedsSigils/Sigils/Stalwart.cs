using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Stalwart : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Stalwart", "If [creature] is moved from the space it was placed in, it will immediately move back, shunting any card which may have moved there in the meantime out of the way.",
                      typeof(Stalwart),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/stalwart.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/stalwart_pixel.png"));

            Stalwart.ability = newSigil.ability;
        }
        public static Ability ability;
        public CardSlot home;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override void ManagedUpdate()
        {
            if (base.Card && base.Card.OnBoard && !base.Card.InOpponentQueue && base.Card.slot != null && home == null) { home = base.Card.slot; }
            base.ManagedUpdate();
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card && base.Card.slot)
            {
                home = base.Card.slot;
                //Debug.Log($"Card '{base.Card.Info.displayedName}' was assigned a home slot in lane '{home.Index}'.");

            }
            yield break;
        }
        public bool beingMoved = false;
        public IEnumerator ReturnToHome()
        {
            if (home != null && base.Card.slot == null || base.Card.slot != home)
            {
                beingMoved = true;
                //Debug.Log($"Card '{base.Card.Info.displayedName}' needs to return to home slot in lane '{home.Index}'.");
                yield return base.PreSuccessfulTriggerSequence();
                PlayableCard swapCard = null;
                if (home && home.Card != null)
                {
                    swapCard = home.Card; swapCard.UnassignFromSlot();
                }
                base.Card.UnassignFromSlot();
                base.Card.Anim.StrongNegationEffect();
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, home, 0f, null, true);

                //Debug.Log($"Successfully assigned {base.Card.Info.displayedName}' returned to home slot in lane '{home.Index}");
                if (swapCard != null)
                {
                    //Debug.Log($"Card {swapCard.Info.displayedName} was in the way, and needed to be removed.");

                    if (swapCard.HasAbility(Stalwart.ability))
                    {
                        yield return swapCard.GetComponent<Stalwart>().ReturnToHome();
                        //Debug.Log($"Card had stalwart and was returned to its home in lane {swapCard.GetComponent<Stalwart>().home.Index}");
                    }
                    else
                    {
                        CardSlot target = null;
                        CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(home, true);
                        CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(home, false);
                        if (toRight != null && toRight.Card == null) { target = toRight; }
                        else if (toLeft != null && toLeft.Card == null) { target = toLeft; }
                        if (target != null)
                        {
                            swapCard.Anim.StrongNegationEffect();
                            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(swapCard, target, 0f, null, true);
                        }
                        else
                        {
                            GlitchOutAssetEffect.GlitchModel(swapCard.transform, true, true);
                            AudioController.Instance.PlaySound2D("glitch_error", MixerGroup.TableObjectsSFX, 1f, 0f, null, null, null, null, false);
                        }
                    }
                }

                base.Card.Anim.StrongNegationEffect();
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, home, 0f, null, true);
                base.Card.SetIsOpponentCard(!home.IsPlayerSlot);
                yield return base.LearnAbility(0f);
                beingMoved = false;
                //Debug.Log($"ClearSlot Finished");
            }
            yield break;
        }
    }
}
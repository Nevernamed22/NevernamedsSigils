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
    public class Allure : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Allure", "At the end of the round, [creature] will draw opposing creatures in front of itself.",
                      typeof(Allure),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/allure.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/allure_pixel.png"));

            Allure.ability = newSigil.ability;
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.15f);

            if (base.Card && base.Card.slot && base.Card.slot.opposingSlot)
            {
                if (base.Card.slot.opposingSlot.Card == null)
                {
                    CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, true);
                    CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.slot.opposingSlot, false);

                    yield return new WaitForSeconds(0.1f);
                    yield return base.PreSuccessfulTriggerSequence();

                    PlayableCard fleer = null;

                    if (toLeft != null && toLeft.Card != null && !toLeft.Card.HasTrait(Trait.Giant) && !toLeft.Card.HasAbility(Stalwart.ability))
                    {
                        fleer = toLeft.Card;
                    }
                    else if (toRight != null && toRight.Card != null && !toRight.Card.HasTrait(Trait.Giant)&& !toRight.Card.HasAbility(Stalwart.ability))
                    {
                        fleer = toRight.Card;
                    }
                    if (fleer != null)
                    {
                        base.Card.Anim.StrongNegationEffect();
                        Vector3 midpoint = (fleer.Slot.transform.position + base.Card.slot.opposingSlot.transform.position) / 2f;
                        Tween.Position(fleer.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                        yield return Singleton<BoardManager>.Instance.AssignCardToSlot(fleer, base.Card.slot.opposingSlot, 0.1f, null, true);
                        yield return base.LearnAbility(0.1f);
                    }
                }
                else
                {
                    base.Card.Anim.StrongNegationEffect();
                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield break;
        }

    }
}

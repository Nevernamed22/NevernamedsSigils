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
    public class Snip : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Snip", "When [creature] is played, the creature opposing it is removed from the board without triggering on-death effects.",
                      typeof(Snip),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/snip.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/snip_pixel.png"),
                      triggerText: "[creature] cuts its opponent in half."
                      );

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
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card && base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card && !base.Card.slot.opposingSlot.Card.HasTrait(Trait.Uncuttable);
        }
        public override IEnumerator OnResolveOnBoard()
        {
            CardSlot slot = base.Card.slot;
            CardSlot opposingSlot = slot.opposingSlot;
            PlayableCard targetCard = opposingSlot.Card;
            if (Tools.GetActAsInt() == 1)
            {
                Tween.LocalPosition(targetCard.transform, new Vector3(0f, 1.25f, -0.5f), 0.1f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                Tween.LocalRotation(targetCard.transform, this.CARD_ROT, 0.1f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                yield return new WaitForSeconds(0.65f);
                AudioController.Instance.PlaySound2D("consumable_scissors_use", MixerGroup.TableObjectsSFX, 1f, 0f, null, null, null, null, false);
                GameObject gameObject = Singleton<FirstPersonController>.Instance.AnimController.PlayOneShotAnimation("SplitCard", null);
                gameObject.transform.parent = null;
                gameObject.transform.position = targetCard.transform.position;
                gameObject.transform.eulerAngles = this.CARD_ROT;
            }
            else
            {
                if (base.Card.Anim is PaperCardAnimationController)
                {
                    ((PaperCardAnimationController)base.Card.Anim).Play("death", 0f);
                }
                yield return new WaitForSeconds(0.2f);
            }
            targetCard.UnassignFromSlot();
            UnityEngine.Object.Destroy(targetCard.gameObject);
            yield return new WaitForSeconds(0.15f);

            yield break;
        }
        private readonly Vector3 CARD_ROT = new Vector3(90f, 0f, 70f);
    }
}
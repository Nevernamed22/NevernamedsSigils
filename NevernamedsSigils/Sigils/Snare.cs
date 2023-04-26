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
    public class Snare : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Snare", "When [creature] is played opposite a creature, that creature is ensnared and stored within the bearer, being released when the bearer perishes.",
                      typeof(Snare),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/snare.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/snare_pixel.png"));

            ability = newSigil.ability;
            closedIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/snare_closed.png");
        }
        public static Texture closedIcon;
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return storedCard != null && base.Card.slot && base.Card.slot.opposingSlot;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            CardSlot target = base.Card.slot.opposingSlot;
            yield return new WaitForSeconds(0.25f);
            if (target.Card != null && !target.Card.HasTrait(Trait.Giant) && !target.Card.HasTrait(Trait.Uncuttable))
            {
                yield return target.Card.Die(false, null);
                yield return new WaitForSeconds(0.75f);
            }
            if (target.Card == null)
            {
                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(storedCard, target, 0.15f, true);
                if (target.Card != null)
                {
                    target.Card.temporaryMods.AddRange(storedTempMods);
                    target.Card.Status.damageTaken = storedDamage;
                    target.Card.RenderCard();
                }
            }
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card && base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card && !base.Card.slot.opposingSlot.Card.HasTrait(Trait.Giant) && !base.Card.slot.opposingSlot.Card.HasTrait(Trait.Uncuttable);
        }
        public CardInfo storedCard;
        public List<CardModificationInfo> storedTempMods;
        public int storedDamage;
        public override IEnumerator OnResolveOnBoard()
        {
            yield return new WaitForSeconds(0.25f);
            PlayableCard opposing = base.Card.slot.opposingSlot.Card;
            opposing.UnassignFromSlot();
            AudioController.Instance.PlaySound3D("dial_metal", MixerGroup.TableObjectsSFX, base.Card.transform.position, 1f, 0f, null, null, null, null, false);
            Tween.Position(opposing.transform, base.Card.transform.position + new Vector3(0, -0.1f, 0), 0.25f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, delegate ()
            {
                storedCard = opposing.Info;
                storedTempMods = new List<CardModificationInfo>();
                if (opposing.temporaryMods != null && opposing.temporaryMods.Count > 0) storedTempMods.AddRange(opposing.temporaryMods);
                storedDamage = opposing.Status.damageTaken;
                UnityEngine.Object.Destroy(opposing.gameObject);
                base.Card.RenderInfo.OverrideAbilityIcon(Snare.ability, Snare.closedIcon);
                base.Card.RenderCard();
                base.Card.Anim.LightNegationEffect();
            }, true);

            yield break;
        }
    }
}
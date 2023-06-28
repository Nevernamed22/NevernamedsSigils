

using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class Harbinger : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Harbinger", "When a friendly creatures perishes, [creature] will move to fill it's empty space.",
                      typeof(Harbinger),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3BuildACard, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.BountyHunter, Plugin.GrimoraModChair2, Plugin.Part2Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/harbinger.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/harbinger_pixel.png"));

            Harbinger.ability = newSigil.ability;
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
            if (base.Card.slot != null && (base.Card.slot.IsPlayerSlot == deathSlot.IsPlayerSlot) && fromCombat && card.slot != null && !card.InOpponentQueue && !base.Card.HasAbility(Stalwart.ability) && !base.Card.Dead)
            {
                return true;
            }
            else return false;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            if (deathSlot.Card == null || deathSlot.Card.Dead)
            {
                yield return new WaitForSeconds(0.1f);
                CardSlot oldSlot = base.Card.slot;
                CardSlot targetSlot = deathSlot;
                yield return base.PreSuccessfulTriggerSequence();
                Vector3 midpoint = (base.Card.Slot.transform.position + targetSlot.transform.position) / 2f;
                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, targetSlot, 0.1f, null, true);

                if (base.Card.Info.GetExtendedProperty("HarbingerLeaveBehind") != null)
                {
                    yield return new WaitForSeconds(0.1f);
                    if (oldSlot && oldSlot.Card == null)
                    {
                        CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("HarbingerLeaveBehind"));
                        segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Harbinger.ability }));
                        yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.15f, true);
                    }
                }

                yield return base.LearnAbility(0.5f);
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.3f);
            }
            yield break;
        }
    }
}

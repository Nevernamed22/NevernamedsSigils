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
    public class Retreat : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Retreat", "When [creature] is struck, it will move to a random empty space on the same side of the board.",
                      typeof(Retreat),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/retreat.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/retreat_pixel.png"));

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
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return !base.Card.Dead && !base.Card.HasAbility(Stalwart.ability);
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            base.Card.Anim.StrongNegationEffect();

            List<CardSlot> openSlots = Singleton<BoardManager>.Instance.AllSlots.FindAll(x => x.IsPlayerSlot != base.Card.OpponentCard && x.Card == null);
            if (openSlots.Count > 0)
            {
                yield return new WaitForSeconds(0.55f);

                CardSlot oldSlot = base.Card.slot;
                CardSlot target = Tools.SeededRandomElement(openSlots, Tools.GetRandomSeed());
                yield return base.PreSuccessfulTriggerSequence();
                Vector3 midpoint = (base.Card.Slot.transform.position + target.transform.position) / 2f;
                Tween.Position(base.Card.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, target, 0.1f, null, true);

                if (base.Card.Info.GetExtendedProperty("RetreatLeaveBehind") != null)
                {
                    yield return new WaitForSeconds(0.1f);
                    if (oldSlot && oldSlot.Card == null)
                    {
                        CardInfo segment = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("RetreatLeaveBehind"));
                        segment.mods.Add(base.Card.CondenseMods(new List<Ability>() { Retreat.ability }));
                        yield return Singleton<BoardManager>.Instance.CreateCardInSlot(segment, oldSlot, 0.15f, true);
                    }
                }

            }





            yield return base.LearnAbility(0.4f);
            yield break;
        }
    }
}

using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Firebomb : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Firebomb", "When [creature] perishes, creatures adjacent to it and opposing it take 2 damage and gain the Burning sigil.",
                      typeof(Firebomb),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard, Plugin.Part2Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/firebomb.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/firebomb_pixel.png"),
                      triggerText: "[creature] explodes, spraying fire everywhere!");
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
        private void Awake()
        {
            this.bombPrefab = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/DetonatorHoloBomb");
        }
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return base.Card.OnBoard;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();
            yield return this.ExplodeFromSlot(base.Card.Slot);
            yield return base.LearnAbility(0.25f);
            yield break;
        }
        protected IEnumerator ExplodeFromSlot(CardSlot slot)
        {
            List<CardSlot> adjacentSlots = Singleton<BoardManager>.Instance.GetAdjacentSlots(slot);
            if (adjacentSlots.Count > 0 && adjacentSlots[0].Index < slot.Index)
            {
                if (adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
                {
                    yield return this.BombCard(adjacentSlots[0].Card, slot.Card);
                }
                adjacentSlots.RemoveAt(0);
            }
            if (slot.opposingSlot.Card != null && !slot.opposingSlot.Card.Dead)
            {
                yield return this.BombCard(slot.opposingSlot.Card, slot.Card);
            }
            if (adjacentSlots.Count > 0 && adjacentSlots[0].Card != null && !adjacentSlots[0].Card.Dead)
            {
                yield return this.BombCard(adjacentSlots[0].Card, slot.Card);
            }
            yield break;
        }
        private IEnumerator BombCard(PlayableCard target, PlayableCard attacker)
        {
            GameObject bomb = null;
            if (Tools.GetActAsInt() == 3)
            {
                bomb = UnityEngine.Object.Instantiate<GameObject>(this.bombPrefab);
                bomb.transform.position = attacker.transform.position + Vector3.up * 0.1f;
                Tween.Position(bomb.transform, target.transform.position + Vector3.up * 0.1f, 0.5f, 0f, Tween.EaseLinear, Tween.LoopType.None, null, null, true);
                yield return new WaitForSeconds(0.5f);
            }
            else { yield return new WaitForSeconds(0.25f); }
            target.Anim.PlayHitAnimation();
            if (bomb != null) UnityEngine.Object.Destroy(bomb);
            target.AddTemporaryMod(new CardModificationInfo(Burning.ability));
            target.RenderCard();
            yield return target.TakeDamage(2, attacker);
            yield break;
        }
        private GameObject bombPrefab;
    }
}
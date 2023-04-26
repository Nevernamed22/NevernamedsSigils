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
    public class FlankBlast : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Flank Blast", "When [creature] dies, adjacent friendly creatures are dealt 10 damage.",
                      typeof(FlankBlast),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: -2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/flankblast.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/flankblast_pixel.png"));

            FlankBlast.ability = newSigil.ability;
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
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice) { return base.Card.OnBoard; }
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
            foreach (CardSlot slotto in adjacentSlots)
            {
                if (slotto != null && slotto.Card != null && !slotto.Card.Dead)
                {
                    yield return this.BombCard(slotto.Card, slot.Card);
                }
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
            }
            yield return new WaitForSeconds(Tools.GetActAsInt() == 3 ? 0.5f : 0.25f);
            target.Anim.PlayHitAnimation();
            if (bomb) UnityEngine.Object.Destroy(bomb);
            yield return target.TakeDamage(10, attacker);
            yield break;
        }
        private const string BOMB_PREFAB_PATH = "Prefabs/Cards/SpecificCardModels/DetonatorHoloBomb";
        private GameObject bombPrefab;
    }
}

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
    public class Burning : AbilityBehaviour
    {
        public static Texture2D fireTex;
        public static Texture2D blueFireTex;
        public static void Init()
        {
            fireTex = Tools.LoadTex("NevernamedsSigils/Resources/Other/fireparticle.png");
            blueFireTex = Tools.LoadTex("NevernamedsSigils/Resources/Other/bluefire_particle.png");


            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Burning", "At the end of the owner's turn, [creature] takes 1 damage. Also, when [creature] is struck, the striker gains this sigil.",
                      typeof(Burning),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: -1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/burning.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/burning_pixel.png"));

            Burning.ability = newSigil.ability;
        }
        public static Ability ability;
        public override void ManagedFixedUpdate()
        {
            if (base.Card && base.Card.OnBoard && !base.Card.Dead/* && SaveManager.SaveFile.IsPart1*/)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = UnityEngine.Random.Range(0.1f, 0.5f);
                    Card.SpawnParticlesOnCard(SaveManager.SaveFile.IsPart1 ?  Burning.fireTex : Burning.blueFireTex, SaveManager.SaveFile.IsPart1 ? -0.1f : 0.1f);
                }
            }
            base.ManagedFixedUpdate();
        }
        float timer = 0.1f;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return base.Card.OpponentCard != playerTurnEnd && base.Card.OnBoard;
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (source != null)
            {
                if (!source.HasAbility(Ability.MadeOfStone) && !source.HasAbility(Burning.ability))
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    CardModificationInfo fire = new CardModificationInfo();
                    fire.abilities.Add(Burning.ability);
                    source.AddTemporaryMod(fire);
                    source.RenderCard();
                }
            }
            yield break;
        }
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            bool isInHotConduit = Singleton<ConduitCircuitManager>.Instance.GetConduitsForSlot(base.Card.slot).Exists(x => x.HasAbility(HotConduit.ability));
            if (!base.Card.HasAbility(FireResistant.ability) && !isInHotConduit)
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (base.Card.HasAbility(Ability.ExplodeOnDeath)) { yield return base.Card.Die(false); }
                else { yield return base.Card.TakeDamage(1, null); }
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}

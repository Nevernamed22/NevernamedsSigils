using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpponentBones;
using UnityEngine;

namespace NevernamedsSigils
{
    public class ScavengeWhenPowered : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Scavenge When Powered", "While [creature] is inside a completed circuit, opposing creatures also grant Bones upon death.",
                      typeof(ScavengeWhenPowered),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/ConduitReactive/scavengewhenpowered.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/ConduitReactive/scavengewhenpowered_pixel.png"),
                      isConduitCell: true);

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
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return base.Card.OnBoard && card.OpponentCard != base.Card.OpponentCard && Singleton<ConduitCircuitManager>.Instance.SlotIsWithinCircuit(base.Card.Slot);
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            if (base.Card.OpponentCard && OpponentResourceManager.instance)
            {
                yield return OpponentResourceManager.instance.AddOpponentBones(deathSlot, 1);
            }
            else
            {
                yield return Singleton<ResourcesManager>.Instance.AddBones(1, deathSlot);
            }
            yield return base.LearnAbility(0f);
            yield return new WaitForSeconds(0.25f);
            yield break;
        }
    }
}

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
    public class Entangle : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Entangle", "When [creature] perishes, its killer has their power reduced by half and rounded down for one turn.",
                      typeof(Entangle),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.GrimoraRulebook, AbilityMetaCategory.MagnificusRulebook, Plugin.Part2Modular, Plugin.GrimoraModChair2 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/entangle.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/entangle_pixel.png"));

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
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            //Debug.Log("ODie");
            yield return base.PreSuccessfulTriggerSequence();          
            TangledEffect addedTangle = null;
            if (killer.gameObject.GetComponent<TangledEffect>() != null) { addedTangle = killer.gameObject.GetComponent<TangledEffect>(); }
            else { addedTangle = killer.gameObject.AddComponent<TangledEffect>(); }
            if (addedTangle != null)
            {
                addedTangle.stacks++;
            }
            yield break;
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice && killer != null && killer.gameObject != null;
        }
    }
    public class TangledEffect : NonCardTriggerReceiver
    {
        public override bool TriggerBeforeCards => true;
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep && primed;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            stacks = 0;
            primed = false;
            yield break;
        }
        public bool primed;
        public int stacks;
    }
}
using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Landscaper : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Landscaper", "When [creature] is played, any terrain cards on the board are removed and added to it's owner's hand.",
                      typeof(Landscaper),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/landscaper.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/landscaper_pixel.png"));

            Landscaper.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {

            List<CardSlot> availableSlots = Singleton<BoardManager>.Instance.AllSlotsCopy;


            if (availableSlots.Exists((CardSlot x) => x.Card != null && x.Card != base.Card && x.Card.HasTrait(Trait.Terrain)))
            {
                List<CardSlot> terrains = availableSlots.FindAll((x) => x != null && x.Card != base.Card && x.Card.HasTrait(Trait.Terrain));
            yield return base.PreSuccessfulTriggerSequence();
                Debug.Log("Terrains: " + terrains.Count);
                for (int i = terrains.Count - 1; i >= 0; i--)
                {
                Debug.Log("Checking Terrain: " + i);
                    if (terrains[i] && terrains[i].Card && terrains[i].Card.Info && terrains[i].Card.gameObject)
                    {
                        yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(terrains[i].Card.Info, null, 0.25f, null);
                        yield return new WaitForSeconds(0.3f);
                        terrains[i].Card.Anim.StrongNegationEffect();
                        yield return new WaitForSeconds(0.2f);
                     yield return   terrains[i].Card.Die(false);
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

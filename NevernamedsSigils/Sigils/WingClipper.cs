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
    public class WingClipper : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Wing Clipper", "When [creature] is played, all opponent creatures with the Airborne sigil lose the Airborne sigil.",
                      typeof(WingClipper),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3BuildACard, Plugin.GrimoraModChair1 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/wingclipper.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/wingclipper_pixel.png"),
                      triggerText: "[creature] grounds all its opponents!");

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
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            List<CardSlot> slots = Singleton<BoardManager>.Instance.GetSlots(base.Card.OpponentCard);
            foreach(CardSlot slot in slots)
            {
                if (slot.Card != null && slot.Card.HasAbility(Ability.Flying))
                {
                    yield return PreSuccessfulTriggerSequence();
                    CardModificationInfo newMod = new CardModificationInfo();
                    newMod.negateAbilities = new List<Ability>() { Ability.Flying };
                    slot.Card.AddTemporaryMod(newMod);
                    slot.Card.Status.hiddenAbilities.Add(Ability.Flying);
                    slot.Card.RenderCard();
                    slot.Card.Anim.StrongNegationEffect();
                    yield return new WaitForSeconds(0.2f);
                }
            }
            yield break;
        }
    }
}
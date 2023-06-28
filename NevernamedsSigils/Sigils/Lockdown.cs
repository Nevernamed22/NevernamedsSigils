using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GraveyardHandler;

namespace NevernamedsSigils
{
    public class Lockdown : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Lockdown", "When [creature] is played, the opposing creature loses attack power equal to the power of this card and gains the Stalwart sigil.",
                      typeof(Lockdown),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/lockdown.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/lockdown_pixel.png"),
                      triggerText: "[creature] locks the opposing creature in place!");

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
            return base.Card && base.Card.slot != null && base.Card.slot.opposingSlot != null && base.Card.slot.opposingSlot.Card != null;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return PreSuccessfulTriggerSequence();
            CardModificationInfo newmod = new CardModificationInfo();
            newmod.fromCardMerge = true;
            newmod.abilities = new List<Ability>() { Stalwart.ability };
            newmod.attackAdjustment = base.Card.Attack * -1;
            if (Tools.GetActAsInt() != 2)
            {
                base.Card.slot.opposingSlot.Card.Anim.PlayTransformAnimation();
                yield return new WaitForSeconds(0.15f);
            }
            base.Card.slot.opposingSlot.Card.AddTemporaryMod(newmod);
            base.Card.slot.opposingSlot.Card.RenderCard();
            yield break;
        }
    }
}
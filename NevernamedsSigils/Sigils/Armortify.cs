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
    public class Armortify : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Armortify", "When [creature] attacks an opposing creature and it perishes, this card gains the armored sigil.",
                      typeof(Armortify),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/armortify.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/armortify_pixel.png"));

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
        public static void Rearmor(PlayableCard target)
        {
            target.Anim.NegationEffect(true);
            target.ResetShield();
            if (Tools.GetActAsInt() == 1)
            {
                target.Status.hiddenAbilities.Remove(Ability.DeathShield);
            }
            else if (!target.HasAbility(Ability.DeathShield))
            {
                target.Status.hiddenAbilities.Add(Ability.DeathShield);
            }
            target.temporaryMods.Add(new CardModificationInfo(Ability.DeathShield));
            target.RenderCard();
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            
            if (!base.Card.HasShield()) { Rearmor(base.Card); }
            
            yield return base.LearnAbility(0.25f);
        }
    }
}
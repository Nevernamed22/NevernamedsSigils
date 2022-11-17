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
    public class Healshield : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Healshield", "If [creature] does not take damage during a turn, it will gain Armor at the start of its owner's next turn.",
                      typeof(Healshield),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/healshield.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/healshield_pixel.png"));

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard && base.Card.OnBoard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (!tookDamageThisTurn && !base.Card.HasShield()) { Rearmor(base.Card); }
            tookDamageThisTurn = false;
            yield break;
        }
        public bool tookDamageThisTurn;
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            tookDamageThisTurn = true;
            yield break;
        }
    }
}
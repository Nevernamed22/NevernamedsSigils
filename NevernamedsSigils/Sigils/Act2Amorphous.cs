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
    public class Act2Amorphous : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Amorphous", "When [creature] is drawn, this sigil is replaced with another sigil at random.",
                      typeof(Act2Amorphous),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 3,
                      stackable: true,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/amorphous_pixel.png"));

            Act2Amorphous.ability = newSigil.ability;
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToDrawn()
        {
            return true;
        }
        public override IEnumerator OnDrawn()
        {
            AddMod();
            yield return base.LearnAbility(0.5f);
            yield break;
        }
        private void AddMod()
        {
            base.Card.Status.hiddenAbilities.Add(this.Ability);
            CardModificationInfo cardModificationInfo = new CardModificationInfo(this.ChooseAbility());
            base.Card.AddTemporaryMod(cardModificationInfo);
            
            base.Card.RenderCard();
        }
        private Ability ChooseAbility()
        {
            List<Ability> learnedAbilities = new List<Ability>()
            {
                Ability.Reach,
                Ability.SplitStrike,
                Ability.TriStrike,
                Ability.DrawRabbits,
                Ability.Strafe, 
                Ability.Deathtouch,
                Ability.Evolve,
                Ability.WhackAMole,
                Ability.DrawCopy,
                Ability.QuadrupleBones,
                Ability.DrawCopyOnDeath,
                Ability.Sharp,
                Ability.StrafePush,
                Ability.GuardDog,
                Ability.Flying,
                Ability.Sacrificial,
                Ability.PreventAttack,
                Ability.TripleBlood,
                Ability.BoneDigger,
                Ability.SkeletonStrafe,
                Ability.GainGemTriple,
                Ability.BuffGems,
                Ability.DrawNewHand,
                Ability.SquirrelStrafe,
                Ability.GainBattery,
                Ability.ExplodeOnDeath,
                Ability.Sentry,
                Ability.DoubleDeath,
                Ability.Loot,
                Ability.Submerge,

                //Customs
                Copier.ability,
                Flighty.ability,
                Harbinger.ability,
                GutSpewer.ability,
                ExplodingCorpseCustom.ability,
                OrganThief.ability,
                ToothPuller.ability,
                TrophyHunter.ability,
                Fearsome.ability,
                Medicinal.ability,
                Enraged.ability,
                Ignition.ability,
                SavageRitual.ability,
                Trapjaw.ability,
                Lonesome.ability,
                Ripper.ability,
                Mockery.ability,
                Cute.ability,
                Trampler.ability,
                TwinBond.ability,
            };
            learnedAbilities.RemoveAll((Ability x) => x == Ability.RandomAbility || base.Card.HasAbility(x));
            bool flag = learnedAbilities.Count > 0;
            Ability result;
            if (flag)
            {
                result = learnedAbilities[SeededRandom.Range(0, learnedAbilities.Count, base.GetRandomSeed())];
            }
            else
            {
                result = Ability.Sharp;
            }
            return result;
        }

    }
}

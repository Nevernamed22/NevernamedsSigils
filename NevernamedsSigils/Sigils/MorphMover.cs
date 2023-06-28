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
    public class MorphMover : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Morph Mover", "When [creature] is drawn, this sigil is replaced by a random movement related sigil.",
                      typeof(MorphMover),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular, Plugin.GrimoraModChair3, Plugin.Part2Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/morphmover.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/morphmover_pixel.png"));

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
        public override bool RespondsToDrawn()
        {
            return true;
        }
        public override IEnumerator OnDrawn()
        {
            if (Singleton<PlayerHand>.Instance is PlayerHand3D)
            {
                (Singleton<PlayerHand>.Instance as PlayerHand3D).MoveCardAboveHand(base.Card);
                yield return base.Card.FlipInHand(new Action(this.AddMod));
            }
            else
            {
                AddMod();
            }
            yield return base.LearnAbility(0.5f);
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return !hasAdded;
        }

        public override IEnumerator OnResolveOnBoard()
        {
            AddMod();
            yield break;
        }
        public void AddMod()
        {
            base.Card.Status.hiddenAbilities.Add(this.Ability);
            CardModificationInfo cardModificationInfo = new CardModificationInfo(this.ChooseAbility());
            CardModificationInfo cardModificationInfo2 = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
            if (cardModificationInfo2 == null)
            {
                cardModificationInfo2 = base.Card.Info.Mods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
            }
            if (cardModificationInfo2 != null)
            {
                cardModificationInfo.fromTotem = cardModificationInfo2.fromTotem;
                cardModificationInfo.fromCardMerge = cardModificationInfo2.fromCardMerge;
            }
            base.Card.AddTemporaryMod(cardModificationInfo);
            hasAdded = true;
        }
        public bool hasAdded = false;
        private Ability ChooseAbility()
        {
            List<Ability> validSigils = new List<Ability>()
            {
                Ability.Strafe,
                Ability.StrafePush,
                Ability.StrafeSwap,
                Flighty.ability,
                RunningStrike.ability,
                Trampler.ability,
                TuckAndRoll.ability,
                Ability.MoveBeside,
                AculeateGrip.ability,
                Erratic.ability,
                Phantasmic.ability,
                Bloodrunner.ability,
                Collector.ability,
                Reroute.ability,
                Globetrotter.ability,
                Obedient.ability
            };
            switch (Tools.GetActAsInt())
            {
                case 1:
                    validSigils.AddRange(new List<Ability>()
                    {
                        Weaver.ability,
                        BuzzOff.ability,
                        Ability.SquirrelStrafe                      
                    });
                    break;
                case 2:
                    validSigils.AddRange(new List<Ability>()
                    {
                        VesselShedder.ability,
                        BuzzOff.ability,
                        Ability.SquirrelStrafe,                      
                        Ability.SkeletonStrafe,
                    });
                    break;
                case 3:
                    validSigils.AddRange(new List<Ability>()
                    {
                        VesselShedder.ability,
                    });
                    break;
                case 4: //Grimoramod
                    validSigils.AddRange(new List<Ability>()
                    {
                        Ability.SkeletonStrafe,
                    });
                    break;
            }
            
            validSigils.RemoveAll((Ability x) => base.Card.HasAbility(x));

            return Tools.SeededRandomElement(validSigils, Tools.GetRandomSeed());
        }
    }
}
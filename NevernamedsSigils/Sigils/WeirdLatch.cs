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
    public class WeirdLatch : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Weird Latch", "When [creature] is drawn, this sigil is replaced by a random latch sigil.",
                      typeof(WeirdLatch),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair2, Plugin.Part2Modular },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/weirdlatch.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Latches/weirdlatch_pixel.png"));

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
            else { AddMod(); }
            yield return base.LearnAbility(0.5f);
            yield break;
        }
        private void AddMod()
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
        }
        private Ability ChooseAbility()
        {
            List<Ability> validSigils = new List<Ability>()
            {
                Ability.LatchBrittle,
                Ability.LatchDeathShield,
                Ability.LatchExplodeOnDeath,
                BurningLatch.ability,
                BurrowerLatch.ability,
                SprinterLatch.ability,
                WaterborneLatch.ability,
                FrailLatch.ability,
                AirborneLatch.ability,
                AnnoyingLatch.ability,
                SniperLatch.ability
            };
            if (Tools.GetActAsInt() == 2 || Tools.GetActAsInt() == 3)
            {
                validSigils.AddRange(new List<Ability>()
                {
                    NullLatch.ability,
                    GemLatch.ability,
                });
            }
            if (Tools.GetActAsInt() != 2)
            {
                validSigils.AddRange(new List<Ability>()
                {
                HaunterLatch.ability,
                    OverclockedLatch.ability,
                });
            }

            validSigils.RemoveAll((Ability x) => base.Card.HasAbility(x));

            return Tools.SeededRandomElement(validSigils, Tools.GetRandomSeed());
        }
    }
}
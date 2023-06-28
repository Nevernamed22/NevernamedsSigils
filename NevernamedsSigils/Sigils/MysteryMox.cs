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
    public class MysteryMox : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Mystery Mox", "When [creature] is drawn, this sigil is replaced by a random gem-granting sigil.",
                      typeof(MysteryMox),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3Modular },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/mysterymox.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/mysterymox_pixel.png"));

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
            Singleton<ResourcesManager>.Instance.ForceGemsUpdate();
            hasAdded = true;
        }
        public bool hasAdded = false;
        private Ability ChooseAbility()
        {
            List<Ability> validSigils = new List<Ability>()
            {
                Ability.GainGemBlue,
                Ability.GainGemGreen,
                Ability.GainGemOrange
            };         
            validSigils.RemoveAll((Ability x) => base.Card.HasAbility(x));
            return Tools.SeededRandomElement(validSigils, Tools.GetRandomSeed());
        }
    }
}
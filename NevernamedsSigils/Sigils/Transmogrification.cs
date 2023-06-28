using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Transmogrification : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Transmogrification", "When [creature] strikes a creature, that creature will be changed into another form.",
                      typeof(Transmogrification),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/transmogrification.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/transmogrification_pixel.png"));

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
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            string invalidName = "Bullfrog";
            if (base.Card.Info.GetExtendedProperty("TransmogrificationOverride") != null) { invalidName = Card.Info.GetExtendedProperty("TransmogrificationOverride"); }
            return target.Health > 0 && attacker == base.Card && target.Info.name != invalidName && !target.HasTrait(Trait.Giant) && !target.HasTrait(Trait.Uncuttable);
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            CardInfo newData = base.Card.Info.GetExtendedProperty("TransmogrificationOverride") != null ? CardLoader.GetCardByName(Card.Info.GetExtendedProperty("TransmogrificationOverride")) : CardLoader.GetCardByName("Bullfrog");

            foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                newData.Mods.Add(clone);
            }
            yield return base.PreSuccessfulTriggerSequence();
            yield return target.TransformIntoCard(newData);
            yield return new WaitForSeconds(0.3f);
            yield return base.LearnAbility(0.5f);

            yield break;
        }
    }
}
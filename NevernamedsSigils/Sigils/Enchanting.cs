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
    public class Enchanting : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Enchanting", "When [creature] is played, all creatures you control gain one of its other sigils, at random.",
                      typeof(Enchanting),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/enchanting.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/enchanting_pixel.png"));

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
            return base.Card.GetAllAbilities().Exists(x => x != Enchanting.ability) && Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard).Exists(x => x.Card != base.Card);
        }
        int seedDifferentiator = 0;
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();

            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard))
            {
                if (slot && slot.Card && slot.Card != base.Card)
                {
                    List<Ability> allAbil = base.Card.GetAllAbilities();
                    allAbil.Remove(Enchanting.ability);
                    slot.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    CardModificationInfo inf = new CardModificationInfo(Tools.SeededRandomElement(allAbil, Tools.GetRandomSeed() + seedDifferentiator));
                    inf.fromCardMerge = Tools.GetActAsInt() == 1;
                    slot.Card.AddTemporaryMod(inf);
                    slot.Card.RenderCard();
                    seedDifferentiator++;
                }
            }
            yield return base.LearnAbility(0.25f);
            yield break;
        }
    }
}
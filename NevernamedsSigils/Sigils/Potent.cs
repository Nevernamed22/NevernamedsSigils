using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Potent : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Potent", "When [creature] is sacrificed, the creature it was sacrificed for gains a random sigil.",
                      typeof(Potent),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/potent.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/potent_pixel.png"));

            ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToSacrifice()
        {
            return true;
        }
        public override IEnumerator OnSacrifice()
        {
            yield return base.PreSuccessfulTriggerSequence();
            CardModificationInfo mod = new CardModificationInfo() { abilities = new List<Ability>() { Tools.GetModularSigilForActAndCard(Tools.GetActAsInt(), 0, 5, base.Card, null) }, fromCardMerge = Tools.GetActAsInt() == 1 };
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.AddTemporaryMod(mod);
            Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard.RenderCard();
            
            yield return base.LearnAbility(0f);
            yield break;
        }
    }
}

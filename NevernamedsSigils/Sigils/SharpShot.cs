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
    public class SharpShot : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sharp Shot", "When [creature] perishes, it gains +2 power and attacks once before dying.",
                      typeof(SharpShot),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/sharpshot.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/sharpshot_pixel.png"));

            SharpShot.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return SharpShot.ability;
            }
        }
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            if (base.Card)
            {
                base.Card.temporaryMods.Add(new CardModificationInfo(2, 0));
                yield return new WaitForSeconds(0.1f);
                FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot);
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.25f);
                yield return new WaitForSeconds(0.1f);
                if (wasSacrifice) Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
            }
        }
        
    }
}
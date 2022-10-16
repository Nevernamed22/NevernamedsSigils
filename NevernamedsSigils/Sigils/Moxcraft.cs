using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class Moxcraft : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Moxcraft", "When [creature] is struck, a random mox is created in your hand.",
                      typeof(Moxcraft),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/moxcraft_pixel.png"));

            Moxcraft.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            List<string> moxes = new List<string>() { "MoxEmerald", "MoxRuby", "MoxSapphire" };
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(Tools.RandomElement(moxes)), null, 0.25f);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}

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
    public class Eternal : AbilityBehaviour, IOnPreScalesChangedRef
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Eternal", "While [creature] is alive and on the board, its owner may not take fatal damage.",
                      typeof(Eternal),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/eternal.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/eternal_pixel.png"));

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
        public bool RespondsToPreScalesChangedRef(int damage, int numWeights, bool toPlayer)
        {
            return base.Card.OnBoard && toPlayer;
        }

        public int CollectPreScalesChangedRef(int damage, ref int numWeights, ref bool toPlayer)
        {
            int damageUntilPlayerLoss = (LifeManager.GOAL_BALANCE * 2) - Singleton<LifeManager>.Instance.DamageUntilPlayerWin;
            //Debug.Log($"until {damageUntilPlayerLoss}");
            int toReturn = damage;
            //Debug.Log($"pre {toReturn}");
            if (damage >= damageUntilPlayerLoss) { toReturn =  damageUntilPlayerLoss - 1; }
            //Debug.Log($"mid {toReturn}");
            toReturn = Math.Max(0, toReturn);
           // Debug.Log($"fin {toReturn}");
            return toReturn;
        }      
    }
}

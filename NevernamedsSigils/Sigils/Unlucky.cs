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
    public class Unlucky : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Unlucky", "While [creature] is alive on the board, cards played by the opponent have a chance to have debuffed stats, or lose a random sigil.",
                      typeof(Unlucky),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/unlucky.png"),
                      pixelTex: null);

            Unlucky.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return (otherCard.OpponentCard != base.Card.OpponentCard);
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            if (UnityEngine.Random.value <= 0.5f)
            {
                Debug.Log("Triggered");
                int max = 3;
                if (otherCard.GetAllAbilities().Count > 0) max = 4;
                switch (UnityEngine.Random.Range(1, max))
                {
                    case 1:
                        Debug.Log("Debuffed Attack");
                        if (otherCard.Attack > 0)
                        {
                            otherCard.AddTemporaryMod(new CardModificationInfo(-1, 0));
                            otherCard.Anim.StrongNegationEffect();
                        }
                        break;
                    case 2:
                        Debug.Log("Debuffed Health");
                        if (otherCard.Health > 1)
                        {
                            int reduc = 2;
                            if (otherCard.Health == 2) reduc = 1;
                            otherCard.AddTemporaryMod(new CardModificationInfo(0, (-1 * reduc)));
                            otherCard.Anim.StrongNegationEffect();
                        }
                        break;
                    case 3:
                        Debug.Log("Debuffed Sigils");
                        otherCard.TemporarilyRemoveAbilityFromCard(Tools.RandomElement(otherCard.GetAllAbilities()));
                            otherCard.Anim.StrongNegationEffect();
                        break;
                }
                otherCard.RenderCard();
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
    }
}

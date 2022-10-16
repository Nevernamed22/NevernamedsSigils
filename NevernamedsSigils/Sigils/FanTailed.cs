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
    public class FanTailed : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Fan Tailed", "When [creature] is played, all friendly creatures on the board take flight.",
                      typeof(FanTailed),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/fantailed.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/fantailed_pixel.png"));

            FanTailed.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;        
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            
            if (GetValidTargets().Count > 0)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.1f);
                foreach (PlayableCard playableCard in this.GetValidTargets())
                {
                    CardModificationInfo cardModificationInfo = new CardModificationInfo(Ability.Flying);
                    cardModificationInfo.singletonId = "bird_leg_fan";
                    cardModificationInfo.RemoveOnUpkeep = true;
                    playableCard.Status.hiddenAbilities.Add(Ability.Flying);
                    playableCard.AddTemporaryMod(cardModificationInfo);
                    Vector3 position = playableCard.transform.position;
                    Tween.Position(playableCard.transform, position + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
                    Tween.Position(playableCard.transform, position, 1f, 0.1f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                }
                yield return new WaitForSeconds(0.1f);
                yield return base.LearnAbility(0.25f);

            }
            yield break;
        }
        private List<PlayableCard> GetValidTargets()
        {
            return Singleton<BoardManager>.Instance.CardsOnBoard.FindAll((PlayableCard x) => (x.OpponentCard == base.Card.OpponentCard) && !x.HasAbility(Ability.Flying));
        }
    }
}

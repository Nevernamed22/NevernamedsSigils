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
    public class HookLineAndSinker : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Hook Line And Sinker", "When [creature] perishes by combat, the opposing creature will be pulled into it's old space.",
                      typeof(HookLineAndSinker),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/hooklineandsinker.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/hooklineandsinker_pixel.png"));

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

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice && base.Card.OnBoard && base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card && !base.Card.slot.opposingSlot.Card.HasTrait(Trait.Uncuttable) && !base.Card.slot.opposingSlot.Card.HasAbility(Stalwart.ability);
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.51f);

            PlayableCard targetCard = base.Card.slot.opposingSlot.Card;
            targetCard.SetIsOpponentCard(false);
            targetCard.transform.eulerAngles += new Vector3(0f, 0f, -180f);
            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(targetCard, base.Card.slot, 0.33f, null, true);
            if (targetCard.FaceDown)
            {
                targetCard.SetFaceDown(false, false);
                targetCard.UpdateFaceUpOnBoardEffects();
            }
            yield return new WaitForSeconds(0.66f);         
            if (targetCard.Status != null && targetCard.Status.anglerHooked)
            {
                AchievementManager.Unlock(Achievement.PART1_SPECIAL3);
            }

            yield return base.LearnAbility(0.25f);
        }
    }
}
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
    public class Traitor : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Traitor", "When [creature] is played, it will move onto the opponent's side of the board if unobstructed.",
                      typeof(Traitor),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: -3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/traitor.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/traitor_pixel.png"));

            Traitor.ability = newSigil.ability;
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
            return (base.Card && base.Card.slot && base.Card.slot.opposingSlot && (base.Card.slot.opposingSlot.Card == null) && !base.Card.HasAbility(Stalwart.ability));
        }
        public override IEnumerator OnResolveOnBoard()
        {
            PlayableCard fleer = base.Card;
            CardSlot moveto = fleer.slot.opposingSlot;

                yield return base.PreSuccessfulTriggerSequence();
            Vector3 midpoint = (fleer.Slot.transform.position + moveto.transform.position) / 2f;
            Tween.Position(fleer.transform, midpoint + Vector3.up * 0.5f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
            yield return Singleton<BoardManager>.Instance.AssignCardToSlot(fleer, moveto, 0.1f, null, true);
            fleer.SetIsOpponentCard(!moveto.IsPlayerSlot);
            yield return base.LearnAbility(0.1f);
            yield break;
        }
    }
}

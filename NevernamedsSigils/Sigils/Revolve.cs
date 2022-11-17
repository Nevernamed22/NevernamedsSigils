using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Revolve : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Revolve", "When [creature] is played, all cards on the board are moved one space clockwise.",
                      typeof(Revolve),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/revolve.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/revolve_pixel.png"));

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
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (Singleton<BoardManager>.Instance.GetSlots(false).Exists((CardSlot x) => x.Card != null && x.Card.Info.HasTrait(Trait.Giant)))
            {
                base.Card.Anim.StrongNegationEffect();
                yield break;
            }
            View prev = Singleton<ViewManager>.Instance.CurrentView;
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.75f);
            AudioController.Instance.PlaySound2D("consumable_pocketwatch_use", MixerGroup.TableObjectsSFX, 0.8f, 0f, null, null, null, null, false);
            yield return Singleton<BoardManager>.Instance.MoveAllCardsClockwise();
            yield return new WaitForSeconds(1f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            Singleton<ViewManager>.Instance.SwitchToView(prev, false, false);
            yield break;
        }
    }
}

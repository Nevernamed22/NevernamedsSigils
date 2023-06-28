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
    public class VivaLaRevolution : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Viva La Revolution", "At the start of its owners turn, [creature] will revolve all creatures on the board clockwise. Also, [creature] will not attack it's original owner.",
                      typeof(VivaLaRevolution),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/vivalarevolution.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/vivalarevolution_pixel.png"));

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
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return playerUpkeep != base.Card.OpponentCard && !Singleton<BoardManager>.Instance.GetSlots(false).Exists((CardSlot x) => x.Card != null && x.Card.Info.HasTrait(Trait.Giant));
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            View prev = Singleton<ViewManager>.Instance.CurrentView;
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return Singleton<BoardManager>.Instance.MoveAllCardsClockwise();
            yield return new WaitForSeconds(1f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            Singleton<ViewManager>.Instance.SwitchToView(prev, false, false);
            yield break;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card.OpponentCard) { wasOpponent = true; }
            yield break;
        }
        public bool wasOpponent = false;
    }
}

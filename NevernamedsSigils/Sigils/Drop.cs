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
    public class Drop : AbilityBehaviour, IOnAddedToHand
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Drop", "If [creature] is drawn as part of your opening hand, it will be automatically played on a random board space for free.",
                      typeof(Drop),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/drop.png"),
                      pixelTex: null);

            Drop.ability = newSigil.ability;
        }

       

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public bool RespondsToAddedToHand() { return Singleton<TurnManager>.Instance.TurnNumber == 0; }
        public IEnumerator OnAddedToHand()
        {
            if (Singleton<BoardManager>.Instance.GetSlots(true).Exists((x) => x != null && x.Card == null))
            {

                yield return new WaitForSeconds(0.1f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                yield return new WaitForSeconds(0.4f);

                List<CardSlot> availableSlots = Singleton<BoardManager>.Instance.GetSlots(true).FindAll((x) => x != null && x.Card == null);

                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(base.Card, Tools.RandomElement(availableSlots));
                yield return base.LearnAbility(0.5f);
                yield return new WaitForSeconds(0.1f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                Singleton<ViewManager>.Instance.SwitchToView(View.DefaultHandDown, false, false);
            }
            yield break;
        }

        /*   public override bool RespondsToTurnEnd(bool playerTurnEnd)
           {
               Debug.Log("Checked for turn end");
               Debug.Log(Singleton<TurnManager>.Instance.TurnNumber.ToString());
               return playerTurnEnd;
           }

           public override IEnumerator OnTurnEnd(bool playerTurnEnd)
           {
               Debug.Log(Singleton<TurnManager>.Instance.TurnNumber.ToString());
               if (Singleton<BoardManager>.Instance.GetSlots(true).Exists((x) => x != null && x.Card == null))
               {

                   yield return new WaitForSeconds(0.1f);
                   Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                   yield return new WaitForSeconds(0.4f);

                   List<CardSlot> availableSlots = Singleton<BoardManager>.Instance.GetSlots(true).FindAll((x) => x != null && x.Card == null);

                   yield return base.PreSuccessfulTriggerSequence();
                   yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(base.Card, Tools.RandomElement(availableSlots));
                   yield return base.LearnAbility(0.5f);
                   yield return new WaitForSeconds(0.1f);
                   Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                   Singleton<ViewManager>.Instance.SwitchToView(View.DefaultHandDown, false, false);
               }
               yield break;
           }*/
        /*   public override bool RespondsToTurnEnd(bool playerTurnEnd)
           {
               return playerTurnEnd && Singleton<TurnManager>.Instance.TurnNumber == 0;
           }
           public override IEnumerator OnDrawn()
           {
               if (Singleton<BoardManager>.Instance.GetSlots(true).Exists((x) => x != null && x.Card != null))
               {

                   yield return new WaitForSeconds(0.1f);
                   Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                   yield return new WaitForSeconds(0.4f);

                   List<CardSlot> availableSlots = Singleton<BoardManager>.Instance.GetSlots(true).FindAll((x) => x != null && x.Card != null);

                   yield return base.PreSuccessfulTriggerSequence();
                   yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(base.Card, Tools.RandomElement(availableSlots));
                   yield return base.LearnAbility(0.5f);
                   yield return new WaitForSeconds(0.1f);
                   Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                   Singleton<ViewManager>.Instance.SwitchToView(View.DefaultHandDown, false, false);
               }

               yield break;
           }*/
    }
}

using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Dredge : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Dredge", "Pay 4 Bones to search your deck for any card, and add it to your hand.",
                      typeof(Dredge),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/dredge.png"),
                      pixelTex: null,
                      isActivated: true);

            Dredge.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        protected override int BonesCost
        {
            get
            {
                return 4;
            }
        }
        public override IEnumerator Activate()
        {

            if (base.Card.OpponentCard) yield break;
            if (Singleton<CardDrawPiles>.Instance.Deck.CardsInDeck > 0)
            {
                string classFullName = "NevernamedsInscryptionMod.DecoratorCrabTalker";
                if (Card.Info.name == "Nevernamed DecoratorCrab" && Card.TriggerHandler.specialAbilities.ConvertAll((x) => x.Item2).ToList().Exists((x) => x is TalkingCard && x.GetType().FullName == classFullName))
                {
                    (Card.TriggerHandler.specialAbilities.ConvertAll((x) => x.Item2).ToList().Find((x) => x is TalkingCard && x.GetType().FullName == classFullName) as TalkingCard).TriggerSoloDialogue("DecoratorCrabUseSigil");
                }

                yield return base.PreSuccessfulTriggerSequence();
                yield return Singleton<CardDrawPiles>.Instance.Deck.Tutor();
                yield return base.LearnAbility(0.3f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            else
            {
                string classFullName = "NevernamedsInscryptionMod.DecoratorCrabTalker";
                if (Card.Info.name == "Nevernamed DecoratorCrab" && Card.TriggerHandler.specialAbilities.ConvertAll((x) => x.Item2).ToList().Exists((x) => x is TalkingCard && x.GetType().FullName == classFullName))
                {
                    (Card.TriggerHandler.specialAbilities.ConvertAll((x) => x.Item2).ToList().Find((x) => x is TalkingCard && x.GetType().FullName == classFullName) as TalkingCard).TriggerSoloDialogue("DecoratorCrabUseSigilFail");
                }
            }
            yield break;
        }
    }
}
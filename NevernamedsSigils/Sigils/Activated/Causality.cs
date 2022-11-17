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
    public class Causality : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Causality", "Pay 1 golden tooth to trigger a random effect.",
                      typeof(Causality),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/causality.png"),
                      pixelTex: null,
                      isActivated: true);

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
        private bool hasWarnedBroke;
        public override bool CanActivate()
        {
            return true;
        }
        public override IEnumerator Activate()
        {
            //yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(string.Format(Localization.Translate("You shook off the viscera of the poor [c:bR]{0}[c:] and carried onwards."), sacrificedInfo.DisplayedNameLocalized), -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            CurrencyBowl bowl = Singleton<CurrencyBowl>.Instance;

            View preActivateView = Singleton<ViewManager>.Instance.CurrentView;

            if (hasWarnedBroke && RunState.Run.currency <= 0)
            {
                List<string> dialogue = new List<string>()
                {
                    "You have no teeth.",
                    "You need more teeth to use that ability.",
                    "You cannot pay the cost"
                };
                yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(Tools.RandomElement(dialogue), -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
            }
            else
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, true);
                bowl.MoveIntoPlace(bowl.NEAR_SCALES_POS, bowl.NEAR_SCALES_ROT, Tween.EaseInOutStrong, false);
                bool paidInFull = false;
                yield return new WaitForSeconds(0.75f);
                if (RunState.Run.currency > 0)
                {
                    List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(1);
                    foreach (Rigidbody rigidbody in list)
                    {
                        yield return new WaitForSeconds(0.1f);
                        float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
                        Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, 0, null, null, true);
                        Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, 0, null, null, true);
                        UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
                    }
                    yield return new WaitForSeconds(0.5f);
                    RunState.Run.currency--;
                    paidInFull = true;
                }
                else //"You shook off the viscera of the poor [c:bR]{0}[c:] and carried onwards."
                {
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Hmm.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You have no teeth to pay the cost.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You'll need to perform better in battle if you wish to use that ability.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    hasWarnedBroke = true;
                }

                bowl.MoveAway(bowl.NEAR_SCALES_POS);
                if (paidInFull)
                {
                    yield return new WaitForSeconds(0.2f);
                    yield return DoRandomEffect();
                    yield return new WaitForSeconds(0.2f);
                }
                Singleton<ViewManager>.Instance.SwitchToView(preActivateView, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;

            }
            yield return base.LearnAbility(0.1f);

            yield break;
        }
        private IEnumerator DoRandomEffect()
        {
            int effect = UnityEngine.Random.Range(0, 4);
            switch (effect)
            {
                case 0: //Nothing
                    Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("The universe seems in harmony already... nothing changes.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    break;
                case 1: //Buff DMG
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    List<CardSlot> slotsWithCreatures = Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x.Card != null);
                    PlayableCard card = Tools.RandomElement(slotsWithCreatures).Card;
                    card.temporaryMods.Add(new CardModificationInfo(1, 0));
                    card.Anim.LightNegationEffect();
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput($"Your [c:bR]{card.Info.displayedName}[c:] seems empowered by your sacrifice.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    break;
                case 2: //Buff HP
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    List<CardSlot> slotsWithCreatures2 = Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x.Card != null);
                    PlayableCard card2 = Tools.RandomElement(slotsWithCreatures2).Card;
                    card2.temporaryMods.Add(new CardModificationInfo(0, 1));
                    card2.Anim.LightNegationEffect();
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput($"Your [c:bR]{card2.Info.displayedName}[c:] seems bolstered by your sacrifice.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    break;
                case 3: //Hurt Leshy
                Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
                    yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, !base.Card.slot.IsPlayerSlot, 0.25f, null, 0f, true);
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You have dealt me a blow...", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    break;
            }
            yield break;
        }
    }
}
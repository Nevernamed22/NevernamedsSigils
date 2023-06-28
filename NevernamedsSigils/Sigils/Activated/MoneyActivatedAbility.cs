using APIPlugin;
using DiskCardGame;
using GBC;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class MoneyActivatedAbility : ActivatedAbilityBehaviour
    {
        public override Ability Ability { get { return Ability.None; } }

        private bool hasWarnedBroke;
        public override bool CanActivate()
        {
            return true;
        }
        public virtual int CurrencyRequired()
        {
            return 1;
        }
        public virtual IEnumerator OnPostCostPaid()
        {
            yield break;
        }
        public int Currency()
        {
            switch (Tools.GetActAsInt())
            {
                case 1: return RunState.Run.currency;
                case 2: return SaveData.Data.currency;
                case 3: return Part3SaveData.Data.currency;
                default: return 0;
            }
        }
        public IEnumerator SpendCurrency(int amount)
        {
            int act = Tools.GetActAsInt();
            if (act == 1)
            {
                List<Rigidbody> list = Singleton<CurrencyBowl>.Instance.TakeWeights(amount);
                foreach (Rigidbody rigidbody in list)
                {
                    yield return new WaitForSeconds(0.1f);
                    float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
                    Tween.Position(rigidbody.transform, rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3, Tween.EaseIn, 0, null, null, true);
                    Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f, 0.125f + num3, Tween.EaseOut, 0, null, null, true);
                    UnityEngine.Object.Destroy(rigidbody.gameObject, 0.5f);
                }
                yield return new WaitForSeconds(0.5f);
                RunState.Run.currency -= amount;
            }
            else if (act == 2)
            {
                AudioController.Instance.PlaySound2D("chipDelay_2", MixerGroup.None, 1f, 0f, null, null, null, null, false);
                yield return new WaitForSeconds(0.1f);
                SaveData.Data.currency -= amount;
                if (SaveData.Data.currency == 0)
                {
                    yield return Singleton<TextBox>.Instance.ShowUntilInput($"You spent the last of your foils!.", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                }
                else
                {
                    yield return Singleton<TextBox>.Instance.ShowUntilInput($"You have {SaveData.Data.currency} foil{(SaveData.Data.currency == 1 ? "" : "s")} remaining.", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                }
            }
            else if (act == 3)
            {
                yield return P03AnimationController.Instance.ShowChangeCurrency(-amount, false);
                Part3SaveData.Data.currency -= amount;
            }
            yield break;
        }
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            base.Card.Anim.LightNegationEffect();
            CurrencyBowl bowl = Singleton<CurrencyBowl>.Instance;

            View preActivateView = Singleton<ViewManager>.Instance.CurrentView;
            P03AnimationController.Face prevFace = P03AnimationController.Face.NUM_FACES;
            if (P03AnimationController.Instance != null) { prevFace = P03AnimationController.Instance.CurrentFace; }

            int act = Tools.GetActAsInt();

            if (hasWarnedBroke && Currency() < CurrencyRequired())
            {
                if (act == 2)
                {
                    base.Card.Anim.LightNegationEffect();
                    AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f, 0f, null, null, null, null, false);
                }
                else
                {
                    List<string> dialogue = new List<string>()
                {
                    "You do not have enough teeth.",
                    "You need more teeth to use that ability.",
                    "You cannot pay the cost"
                };
                    if (act == 3)
                    {
                        dialogue = new List<string>()
                    {
                        "Are you deaf? Get more Robobucks.",
                        "I'm not running a charity, get more money.",
                        "No robobucks, no sigil.",
                        "No dice."
                    };
                    }
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(Tools.RandomElement(dialogue), -0.65f, 0.4f, Emotion.Neutral, act == 3 ? TextDisplayer.LetterAnimation.Jitter : TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                }
            }
            else
            {
                if (act == 1)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, true);
                    bowl.MoveIntoPlace(bowl.NEAR_SCALES_POS, bowl.NEAR_SCALES_ROT, Tween.EaseInOutStrong, false);
                }
                else if (act == 3)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.P03Face, false, true);
                }
                bool paidInFull = false;
                yield return new WaitForSeconds(0.75f);
                if (Currency() >= CurrencyRequired())
                {
                    yield return SpendCurrency(CurrencyRequired());
                    paidInFull = true;
                }
                else
                {
                    if (act == 1)
                    {
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Hmm.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You do not have enough teeth to pay the cost.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You'll need to perform better in battle if you wish to use that ability.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    }
                    else if (act == 2)
                    {
                        base.Card.Anim.LightNegationEffect();
                        AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f, 0f, null, null, null, null, false);
                    yield return Singleton<TextBox>.Instance.ShowUntilInput($"You require more foils to use this ability! You have {SaveData.Data.currency} foils.", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                    }
                    else if (act == 3)
                    {
                        P03AnimationController.Instance.SwitchToFace(P03AnimationController.Face.Angry, true, true);
                        yield return new WaitForSeconds(0.1f);
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Ugh.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You need more Robobucks to use that sigil, and I don't give credit.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                        yield return new WaitForSeconds(0.15f);
                        P03AnimationController.Instance.SwitchToFace(P03AnimationController.Face.Happy, true, true);
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Come back when you're a little... mmm... richer.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                    }
                    hasWarnedBroke = true;
                }

                if (act == 1) bowl.MoveAway(bowl.NEAR_SCALES_POS);
                if (P03AnimationController.Instance != null)
                {
                    P03AnimationController.Instance.SwitchToFace(prevFace, true, true);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                }

                if (paidInFull)
                {
                    yield return OnPostCostPaid();
                }
                Singleton<ViewManager>.Instance.SwitchToView(preActivateView, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = 0;

            }
            yield return base.LearnAbility(0.1f);

            yield break;
        }        
    }
}
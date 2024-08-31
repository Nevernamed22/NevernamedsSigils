using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;
using GBC;

namespace NevernamedsSigils
{
    public class TimeTravel : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Time Travel", "While [creature] is played, the opponent is obligated to skip their next turn.",
                      typeof(TimeTravel),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/timetravel.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/timetravel_pixel.png")
                      );

            ability = newSigil.ability;
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
            if (base.Card.OpponentCard)
            {

            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                Singleton<TurnManager>.Instance.Opponent.SkipNextTurn = true;
                switch (Tools.GetActAsInt())
                {
                    case 1:
                        Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("I'll pass my next turn.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));
                        break;
                    case 2:
                        yield return Singleton<TextBox>.Instance.ShowUntilInput("Your opponent must skip their next turn!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                        break;
                    case 3:
                        Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("Ugh. Fine. I'll pass.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));
                        break;
                    case 4:
                        Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("Fair enough dear, I'll pass.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));
                        break;
                }
            }
            yield break;
        }
    }
}

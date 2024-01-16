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
    public class Causality : MoneyActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Causality", "Pay 1 currency to trigger a random effect.",
                      typeof(Causality),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/causality.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/causality_pixel.png"),
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
        public override int CurrencyRequired()
        {
            return 1;
        }
        public override IEnumerator OnPostCostPaid()
        {
            yield return new WaitForSeconds(0.2f);
            int act = Tools.GetActAsInt();
            int effect = UnityEngine.Random.Range(0, 4);
            switch (effect)
            {
                case 0:
                    Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                    switch (act)
                    {
                        case 1:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("The universe seems in harmony already... nothing changes.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                        case 2:
                            yield return Singleton<TextBox>.Instance.ShowUntilInput($"Nothing happened...", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                            break;
                        case 3:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Nothing? Huh, that's disappointing.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                    }
                    break;
                case 1:
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    List<CardSlot> slotsWithCreatures = Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x.Card != null);
                    PlayableCard card = Tools.SeededRandomElement(slotsWithCreatures).Card;
                    card.temporaryMods.Add(new CardModificationInfo(1, 0));
                    card.Anim.LightNegationEffect();
                    switch (act)
                    {
                        case 1:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput($"Your [c:bR]{card.Info.displayedName}[c:] seems empowered by your sacrifice.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                        case 2:
                            yield return Singleton<TextBox>.Instance.ShowUntilInput($"Your {card.Info.displayedName} was empowered!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                            break;
                        case 3:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Power buff, lucky you...", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                    }
                    break;
                case 2:
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    List<CardSlot> slotsWithCreatures2 = Singleton<BoardManager>.Instance.playerSlots.FindAll((CardSlot x) => x.Card != null);
                    PlayableCard card2 = Tools.SeededRandomElement(slotsWithCreatures2).Card;
                    card2.temporaryMods.Add(new CardModificationInfo(0, 1));
                    card2.Anim.LightNegationEffect();
                    switch (act)
                    {
                        case 1:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput($"Your [c:bR]{card2.Info.displayedName}[c:] seems bolstered by your sacrifice.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                        case 2:
                            yield return Singleton<TextBox>.Instance.ShowUntilInput($"Your {card2.Info.displayedName} was bolstered!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                            break;
                        case 3:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Health buff, could be a lot better...", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                    }
                    break;
                case 3:
                    Singleton<ViewManager>.Instance.SwitchToView(View.Scales, false, false);
                    yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, !base.Card.slot.IsPlayerSlot, 0.25f, null, 0f, true);
                    switch (act)
                    {
                        case 1:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You have dealt me a blow...", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                        case 2:
                            yield return Singleton<TextBox>.Instance.ShowUntilInput($"You dealt one direct damage!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                            break;
                        case 3:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Guess playing a perfect game is no match for pay to win mechanics.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                    }
                    break;
            }
            yield return new WaitForSeconds(0.2f);
            yield break;
        }
    }
}
using APIPlugin;
using DiskCardGame;
using GBC;
using InscryptionAPI.Saves;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Prophecy : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Prophecy", "When [creature] is played, you may look at three cards from your deck and choose them in order. The next three cards you draw will be the cards you were shown, in the order that you chose them.",
                      typeof(Prophecy),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair1, Plugin.Part2Modular, AbilityMetaCategory.MagnificusRulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/prophecy.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/prophecy_pixel.png"));

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
        public static List<CardInfo> ChosenCards = new List<CardInfo>();
        public override bool RespondsToResolveOnBoard()
        {
            if (!base.Card.OpponentCard) { return true; }
            return base.RespondsToResolveOnBoard();
        }
        public static int TimesProphecyTriggeredthisRun = 0;
        public static Dictionary<int, Dictionary<int, string>> triggerDialogue = new Dictionary<int, Dictionary<int, string>>()
        {
            {1, new Dictionary<int, string>()
            {
                {0, "I will allow you to choose from three cards. These three cards will be the next three cards you draw from your deck, in the order you choose them here."},
                {1, "Choose three creatures, and alter your next draws."},
                {2, "You know how this goes."},
            }},
            {2, new Dictionary<int, string>()
            {
                {0, "Choose from three cards. These three cards will be the next three cards you draw from your deck, in the order you chose them." },
                {1, "Choose your next three draws." }
            } },
        };
        public override IEnumerator OnResolveOnBoard()
        {
            TimesProphecyTriggeredthisRun = ModdedSaveManager.RunState.GetValueAsInt(Plugin.PluginGuid, "TimesProphecyTriggered");

            List<CardInfo> choices = new List<CardInfo>();
            for (int i = 0; i < 3; i++)
            {
                if (Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll(x => !choices.Contains(x)).Count > 0)
                {
                    choices.Add(Tools.SeededRandomElement(Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll(x => !choices.Contains(x))));
                }
            }
            if (choices.Count > 1)
            {
                if (ChosenCards.Count > 0)
                {
                    switch (Tools.GetActAsInt())
                    {
                        case 1:
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Hmmm... your new prophecy overwrites the old, it would seem.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                            break;
                        case 2:
                            yield return Singleton<TextBox>.Instance.ShowUntilInput("Your new prophecy overwrites the old!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                            break;
                    }
                    ChosenCards.Clear();
                }
                else
                {
                    if (!string.IsNullOrEmpty(triggerDialogue[Tools.GetActAsInt()][TimesProphecyTriggeredthisRun]))
                    {
                        if (Tools.GetActAsInt() == 2)
                        {
                            yield return Singleton<TextBox>.Instance.ShowUntilInput(triggerDialogue[Tools.GetActAsInt()][TimesProphecyTriggeredthisRun], TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                        }
                        else
                        {
                            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(triggerDialogue[Tools.GetActAsInt()][TimesProphecyTriggeredthisRun], -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true);
                        }

                    }
                }

                ModdedSaveManager.RunState.SetValue(Plugin.PluginGuid, "TimesProphecyTriggered", TimesProphecyTriggeredthisRun + 1);
                CardPile pile = Singleton<CardDrawPiles>.Instance is CardDrawPiles3D ? (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile : null;
                int numCards = 0;
                if (Tools.GetActAsInt() != 2 && pile != null)
                {
                    numCards = pile.NumCards;
                    yield return new WaitUntil(() => !pile.DoingCardOperation);
                    Singleton<BoardManager>.Instance.CardSelector.StartCoroutine(pile.DestroyCards(0.5f));
                }
                for (int i = 0; i < choices.Count - 1; i++)
                {
                    CardInfo selectedCard = null;
                    if (Tools.GetActAsInt() == 2)
                    {
                        yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                        {
                            selectedCard = c;
                        }, choices.FindAll(x => !ChosenCards.Contains(x)));
                    }
                    else
                    {
                        yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                        {
                            selectedCard = c;
                        }, choices.FindAll(x => !ChosenCards.Contains(x)), false);
                    }
                    ChosenCards.Add(selectedCard);
                }
                if (choices.Exists(x => !ChosenCards.Contains(x))) { ChosenCards.Add(choices.Find(x => !ChosenCards.Contains(x))); }
                if (pile != null && Tools.GetActAsInt() != 2)
                {
                    Singleton<BoardManager>.Instance.CardSelector.StartCoroutine(pile.SpawnCards(numCards, 1f));
                }
                switch (Tools.GetActAsInt())
                {
                    case 1:
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Hmm... very well", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                        break;
                }
                ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            else
            {
                switch (Tools.GetActAsInt())
                {
                    case 1:
                        yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("You do not have enough cards left for a prophecy.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                        break;
                    case 2:
                        yield return Singleton<TextBox>.Instance.ShowUntilInput("You don't have enough cards left to make a new prophecy!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);
                        break;
                }
            }

            yield break;
        }
    }
}

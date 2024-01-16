using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GBC;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.StartGame), typeof(EncounterData))]
    public class BattleStart
    {
        [HarmonyPrefix]
        public static void BattleStartPatch(TurnManager __instance)
        {
            Prophecy.ChosenCards.Clear();
            //if (CardSelectionHandler.instance != null) { UnityEngine.Object.Destroy(CardSelectionHandler.instance.gameObject); }
            //GameObject inst = new GameObject();
            //CardSelectionHandler resinst = inst.AddComponent<CardSelectionHandler>();
            //CardSelectionHandler.instance = resinst;

        }
    }


    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.CleanupPhase))]
    public class BattleEnd
    {
        [HarmonyPostfix]
        public static IEnumerator BattleEndPatch(IEnumerator enumerator, TurnManager __instance)
        {
            //if (CardSelectionHandler.instance != null) { UnityEngine.Object.Destroy(CardSelectionHandler.instance.gameObject); CardSelectionHandler.instance = null; }
            Prophecy.ChosenCards.Clear();
            yield return enumerator;
            yield break;
        }
    }
    [HarmonyPatch(typeof(CardDrawPiles3D), "DrawOpeningHand", MethodType.Normal)]
    public class OpeningHandDraw
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, CardDrawPiles3D __instance, List<CardInfo> fixedHand)
        {
            Deck deck = __instance.Deck;
            for (int i = deck.cards.Count - 1; i >= 0; i--)
            {
                List<CardSlot> FreePlayerSlots = Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card == null);
                if (FreePlayerSlots.Count > 0)
                {
                    if (deck.cards[i].HasAbility(Quickdraw.ability))
                    {
                        __instance.pile.Draw();
                        CardInfo info = deck.Draw(deck.cards[i]);
                        PlayableCard playable = CardSpawner.SpawnPlayableCard(info);
                        yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(playable, Tools.SeededRandomElement<CardSlot>(FreePlayerSlots, Tools.GetRandomSeed()), 0.1f, null, true);
                    }
                }
            }
            yield return enumerator;
            yield break;
        }
    }
    [HarmonyPatch(typeof(PixelCardDrawPiles), "DrawOpeningHand", MethodType.Normal)]
    public class OpeningHandDrawPixel
    {
        [HarmonyPostfix]
        public static IEnumerator Postfix(IEnumerator enumerator, PixelCardDrawPiles __instance, List<CardInfo> fixedHand)
        {
            Deck deck = __instance.Deck;
            for (int i = deck.cards.Count - 1; i >= 0; i--)
            {
                List<CardSlot> FreePlayerSlots = Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card == null);
                if (FreePlayerSlots.Count > 0)
                {
                    if (deck.cards[i].HasAbility(Quickdraw.ability))
                    {
                        CardInfo info = deck.Draw(deck.cards[i]);
                        PlayableCard playable = CardSpawner.SpawnPlayableCard(info);
                        yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(playable, Tools.SeededRandomElement<CardSlot>(FreePlayerSlots, Tools.GetRandomSeed()), 0.1f, null, true);
                    }
                }
            }
            yield return enumerator;
            yield break;
        }
    }
    [HarmonyPatch(typeof(Opponent), nameof(Opponent.TryOfferSurrender))]
    public class TryOfferSurrender
    {
        [HarmonyPostfix]
        public static IEnumerator SurrenderTry(IEnumerator enumerator, Opponent __instance)
        {
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(true).FindAll(x => x.Card && x.Card.HasAbility(HighPowered.ability)))
            {
                if (slot.Card.GetComponent<HighPowered>()) yield return slot.Card.GetComponent<HighPowered>().SpendEnergy();
            }
            yield return enumerator;
            yield break;
        }
    }

    [HarmonyPatch(typeof(HintsHandler), nameof(HintsHandler.OnNonplayableCardClicked))]
    public class NonPlayableClicked
    {
        [HarmonyPrefix]
        public static bool NonPlayableClickedPatch(PlayableCard card, List<PlayableCard> cardsInHand)
        {
            if (card && card.Info && !SaveManager.SaveFile.IsPart2)
            {
                int act = Tools.GetActAsInt();
                List<string> overrideDialogue = new List<string>();

                if (card.Info.HasAbility(MoxMax.ability) && card.GemsCost() != null && card.GemsCost().Count > 0)
                {
                    bool satisfied = true;
                    GemType unsatisfiedGem = GemType.Blue;
                    foreach (GemType gem in card.GemsCost())
                    {
                        if (Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && (x.Card.HasAbility(gemCostToAbility[gem]) || x.Card.HasAbility(Ability.GainGemTriple))).Count < 2) { satisfied = false; unsatisfiedGem = gem; }
                    }
                    if (!satisfied)
                    {
                        overrideDialogue.Clear();
                        overrideDialogue.Add($"You need two {HintsHandler.GetColorCodeForGem(unsatisfiedGem) + Localization.Translate(unsatisfiedGem.ToString()) + " </color>"} gems to play that {card.Info.DisplayedNameLocalized}");
                    }
                }
                if (card.HasTrait(Trait.Gem) && Singleton<BoardManager>.Instance.playerSlots.Exists(x => x.Card != null && x.Card.HasAbility(GemSkeptic.ability)))
                {
                    overrideDialogue.Clear();
                    overrideDialogue.Add("The gem skeptic sigil prevents you from playing that gem.");
                }
                if (card.Info.GetExtendedProperty("PreventPlay") != null)
                {
                    overrideDialogue.Clear();
                    if (act == 3) { overrideDialogue.Add("You can't play that one."); }
                    else if (act == 4) { overrideDialogue.Add("I'm afraid that one's forbidden, dear."); }
                    else { overrideDialogue.Add("I won't allow you to play that card."); }
                }

                if (overrideDialogue.Count > 0)
                {
                    CustomCoroutine.Instance.StartCoroutine(
                        Singleton<TextDisplayer>.Instance.ShowThenClear(
                            Tools.RandomElement(overrideDialogue),
                           1.5f,
                            0.1f,
                            Emotion.Neutral,
                            act == 3 ? TextDisplayer.LetterAnimation.Jitter : TextDisplayer.LetterAnimation.WavyJitter,
                            DialogueEvent.Speaker.Single));
                    return false;
                }
            }

            return true;
        }
        public static Dictionary<GemType, Ability> gemCostToAbility = new Dictionary<GemType, Ability>()
        {
            { GemType.Blue, Ability.GainGemBlue },
            { GemType.Orange, Ability.GainGemOrange },
            { GemType.Green, Ability.GainGemGreen },
        };

    }

    [HarmonyPatch(typeof(HintsHandler), nameof(HintsHandler.OnGBCNonPlayableCardPressed))]
    public class NonPlayableClickedGBC
    {
        [HarmonyPrefix]
        public static bool NonPlayableClickedPatchGBC(PlayableCard card)
        {
            if (card && card.Info)
            {
                string text = null;

                if (card.Info.HasAbility(MoxMax.ability) && card.GemsCost() != null && card.GemsCost().Count > 0)
                {
                    bool satisfied = true;
                    GemType unsatisfiedGem = GemType.Blue;
                    foreach (GemType gem in card.GemsCost())
                    {
                        if (Singleton<BoardManager>.Instance.playerSlots.FindAll(x => x.Card != null && (x.Card.HasAbility(NonPlayableClicked.gemCostToAbility[gem]) || x.Card.HasAbility(Ability.GainGemTriple))).Count < 2) { satisfied = false; unsatisfiedGem = gem; }
                    }
                    if (!satisfied)
                    {
                        string arg = HintsHandler.GetDialogueColorCodeForGem(unsatisfiedGem) + Localization.Translate(unsatisfiedGem.ToString()) + "[c:]";
                        text = $"You do not have the two {arg} Gems needed to play that. Gain Gems by playing Mox cards.";
                    }
                }
                if (card.HasTrait(Trait.Gem) && Singleton<BoardManager>.Instance.playerSlots.Exists(x => x.Card != null && x.Card.HasAbility(GemSkeptic.ability)))
                {
                    text = "You can't play a gem card while a creature with the Gem Skeptic sigil is on your side of the board!";
                }
                if (card.Info.GetExtendedProperty("PreventPlay") != null)
                {
                    text = "This card cannot be played.";
                }

                if (text != null)
                {
                    CustomCoroutine.Instance.StartCoroutine(Singleton<TextBox>.Instance.ShowUntilInput(
                        text, TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceTop, 0f, true, false, null, false, Emotion.Neutral));
                    return false;
                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CardDrawPiles), nameof(CardDrawPiles.DrawCardFromDeck))]
    public class DrawCardFromDeckPatch
    {
        [HarmonyPrefix]
        public static void DrawCard(CardDrawPiles __instance, ref CardInfo specificCard, Action<PlayableCard> cardAssignedCallback)
        {
            if (specificCard == null && Prophecy.ChosenCards.Count > 0)
            {
                CardInfo prophetic = null;
                for (int i = 0; i < Prophecy.ChosenCards.Count; i++)
                {
                    if (Prophecy.ChosenCards[i] != null && Singleton<CardDrawPiles>.Instance.Deck.Cards.Contains(Prophecy.ChosenCards[i]))
                    {
                        prophetic = Prophecy.ChosenCards[i];
                        Prophecy.ChosenCards.RemoveAt(i);
                        break;
                    }
                }
                specificCard = prophetic;
            }
        }
    }
}

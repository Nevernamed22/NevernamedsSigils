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
    public class FileExplorer : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("File Explorer", "When [creature] is played, choose two creatures from random selection of your deck. These creatures will define this cards power and health at random. If this card dies, the creatures used to define its stats perish as well.",
                      typeof(FileExplorer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/fileexplorer.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/fileexplorer_pixel.png"));

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
        public CardInfo healthCard;
        public CardInfo damageCard;
        public CardInfo firstCardChosen;
        public override IEnumerator OnResolveOnBoard()
        {
            if (Singleton<CardDrawPiles>.Instance.Deck.Cards.Count >= 2)
            {
                //Despawn Pile              
                CardPile pile = Singleton<CardDrawPiles>.Instance is CardDrawPiles3D ? (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Pile : null;
                int numCards = 0;
                if (Tools.GetActAsInt() != 2 && pile != null)
                {
                    numCards = pile.NumCards;
                    yield return new WaitUntil(() => !pile.DoingCardOperation);
                    Singleton<BoardManager>.Instance.CardSelector.StartCoroutine(pile.DestroyCards(0.5f));
                }

                //Get options for first choice
                List<CardInfo> firstchoices = new List<CardInfo>();
                for (int i = 0; i < 3; i++)
                {
                    if (Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll(x => !firstchoices.Contains(x)).Count > 0)
                    {
                        firstchoices.Add(Tools.SeededRandomElement(Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll(x => !firstchoices.Contains(x)), Tools.GetRandomSeed(0)));
                    }
                }

                //Select the first card
                CardInfo selectedCard = null;
                if (Tools.GetActAsInt() == 2)
                {
                    yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                    {
                        selectedCard = c;
                    }, firstchoices);
                }
                else
                {
                    yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                    {
                        selectedCard = c;
                    }, firstchoices, false);
                }
                if (SeededRandom.Value(Tools.GetRandomSeed()) <= 0.5f) { healthCard = selectedCard; }
                else { damageCard = selectedCard; }
                firstCardChosen = selectedCard;

                

                //Get options for the second choice
                List<CardInfo> secondchoices = new List<CardInfo>();
                for (int i = 0; i < 3; i++)
                {
                    if (Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll(x => (Singleton<CardDrawPiles>.Instance.Deck.Cards.Count <= 2 || firstCardChosen != x) && !secondchoices.Contains(x)).Count > 0)
                    {
                        secondchoices.Add(Tools.SeededRandomElement(Singleton<CardDrawPiles>.Instance.Deck.Cards.FindAll(x => (Singleton<CardDrawPiles>.Instance.Deck.Cards.Count <= 2 || firstCardChosen != x) && !secondchoices.Contains(x)), Tools.GetRandomSeed(1)));
                    }
                }

                //Select the Second Choice
                CardInfo secondSelected = null;
                if (Tools.GetActAsInt() == 2)
                {
                    yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                    {
                        secondSelected = c;
                    }, secondchoices);
                }
                else
                {
                    yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                    {
                        secondSelected = c;
                    }, secondchoices, false);
                }
                if (healthCard == null) { healthCard = secondSelected; }
                else { damageCard = secondSelected; }

                //Apply Stat Change
                if (healthCard && damageCard != null)
                {
                    base.Card.Anim.StrongNegationEffect();
                    base.Card.AddTemporaryMod(new CardModificationInfo(damageCard.Attack - base.Card.Attack, healthCard.Health - base.Card.Health));
                    base.Card.RenderCard();
                }

                //Respawn card pile
                if (pile != null && Tools.GetActAsInt() != 2)
                {
                    Singleton<BoardManager>.Instance.CardSelector.StartCoroutine(pile.SpawnCards(numCards, 1f));
                }
                ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield break;
        }

        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {

            for (int i = 0; i < 2; i++)
            {
                CardInfo toKill = i == 0 ? healthCard : damageCard;

                if (Singleton<BoardManager>.Instance.AllSlots.Exists(x => x.Card != null && x.Card.Info == toKill))
                {
                    CardSlot slot = Singleton<BoardManager>.Instance.AllSlots.Find(x => x.Card != null && x.Card.Info == toKill);
                    yield return slot.Card.Die(false, null, true);
                }
                else if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) != null && Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x).Info == toKill))
                {
                    yield return Singleton<BoardManager>.Instance.GetCardQueuedForSlot(Singleton<BoardManager>.Instance.OpponentSlotsCopy.Find(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) != null && Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x).Info == toKill)).Die(false, null, true);
                }
                else if (Singleton<PlayerHand>.Instance.CardsInHand.Exists(x => x.Info == toKill))
                {
                    View oldView = Singleton<ViewManager>.Instance.CurrentView;
                    bool resetView = false;
                    if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
                    {
                        resetView = true;
                        yield return new WaitForSeconds(0.1f);
                        Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                        yield return new WaitForSeconds(0.1f);
                    }
                    PlayableCard inHand = Singleton<PlayerHand>.Instance.CardsInHand.Find(x => x.Info == toKill);
                    inHand.SetInteractionEnabled(false);
                    inHand.Anim.PlayDeathAnimation(true);
                    UnityEngine.Object.Destroy(inHand.gameObject, 1f);
                    Singleton<PlayerHand>.Instance.RemoveCardFromHand(inHand);
                    if (resetView)
                    {
                        yield return new WaitForSeconds(0.75f);
                        Singleton<ViewManager>.Instance.SwitchToView(oldView, false, false);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (Singleton<CardDrawPiles>.Instance.Deck.Cards.Exists(x => x = toKill))
                {
                    View oldView = Singleton<ViewManager>.Instance.CurrentView;
                    bool resetView = false;
                    if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
                    {
                        yield return new WaitForSeconds(0.1f);
                        resetView = true;
                        Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                        yield return new WaitForSeconds(0.1f);
                    }

                    PlayableCard cardToDiscard = null;
                    yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck(toKill, delegate (PlayableCard x)
                    {
                        cardToDiscard = x;
                    });
                    if (Singleton<CardDrawPiles>.Instance is CardDrawPiles3D) { (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).Deck.Draw(); }
                    yield return new WaitForSeconds(0.5f);
                    if (cardToDiscard != null && cardToDiscard.InHand)
                    {
                        cardToDiscard.SetInteractionEnabled(false);
                        cardToDiscard.Anim.PlayDeathAnimation(true);
                        UnityEngine.Object.Destroy(cardToDiscard.gameObject, 1f);
                        Singleton<PlayerHand>.Instance.RemoveCardFromHand(cardToDiscard);
                    }

                    if (resetView)
                    {
                        yield return new WaitForSeconds(0.75f);
                        Singleton<ViewManager>.Instance.SwitchToView(oldView, false, false);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                yield return new WaitForSeconds(0.15f);
                if (damageCard == null) { break; }
            }
            yield break;
        }
    }
}

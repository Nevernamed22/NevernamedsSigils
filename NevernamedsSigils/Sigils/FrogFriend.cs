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
    public class FrogFriend : Strafe
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Frog Friend", "At the end of the owner's turn, [creature] will move in the direction inscrybed in the sigil. You may search your deck for any 'frog' or 'toad' creatures, and play them in [creature]'s old space.",
                      typeof(FrogFriend),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/frogfriend.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/frogfriend_pixel.png"));

            ability = newSigil.ability;
        }
        public static Ability ability;
        int numtriggers = 0;
        public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
        {
            if (cardSlot.Card == null)
            {
                CardInfo chosen = null;

                if (base.Card.OpponentCard)
                {
                    if (numtriggers <= 4)
                    {
                        CardMetaCategory desired = CardMetaCategory.NUM_CATEGORIES;
                        CardTemple required = CardTemple.NUM_TEMPLES;
                        switch (Tools.GetActAsInt())
                        {
                            case 1: desired = CardMetaCategory.ChoiceNode; required = CardTemple.Nature; break;
                            case 2: desired = CardMetaCategory.GBCPlayable; break;
                            case 3: desired = CardMetaCategory.Part3Random; required = CardTemple.Tech; break;
                            case 4: desired = Plugin.GrimoraChoiceNode; break;
                        }

                        List<CardInfo> valids = ScriptableObjectLoader<CardInfo>.AllData.FindAll(x => (required == CardTemple.NUM_TEMPLES || x.temple == required) && x.metaCategories.Contains(desired) && (x.DisplayedNameEnglish.ToLower().Contains("frog") || x.DisplayedNameEnglish.ToLower().Contains("toad")));
                        if (valids.Count > 0) chosen = Tools.SeededRandomElement(valids);
                        numtriggers++;
                    }
                }
                else
                {
                    List<CardInfo> validCards = new List<CardInfo>();
                    validCards.AddRange(Singleton<CardDrawPiles>.Instance.Deck.cards.FindAll(x => x.DisplayedNameEnglish.ToLower().Contains("frog") || x.DisplayedNameEnglish.ToLower().Contains("toad")));
                    if (validCards.Count > 0)
                    {
                        if (Tools.GetActAsInt() == 2)
                        {
                            yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                            {
                                chosen = c;
                            }, validCards);
                            Singleton<CardDrawPiles>.Instance.Deck.cards.Remove(chosen);
                            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                            yield return new WaitForSeconds(0.2f);
                        }
                        else
                        {
                            yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                            {
                                chosen = c;
                            }, validCards);
                            Singleton<CardDrawPiles>.Instance.Deck.cards.Remove(chosen);
                            if (Singleton<CardDrawPiles>.Instance is CardDrawPiles3D)
                            {
                                (Singleton<CardDrawPiles>.Instance as CardDrawPiles3D).pile.Draw();
                            }
                            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                        }

                        Singleton<ViewManager>.Instance.SwitchToView(View.Board);
                    }
                }
                if (chosen != null)
                {
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(chosen, cardSlot, 0.1f, true);
                }
            }
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GraveyardHandler;

namespace NevernamedsSigils
{
    public class Rememberance : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Rememberance", "When [creature] is played, its owner may search their graveyard for a card. This sigil is replaced by that cards sigils.",
                      typeof(Rememberance),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/rememberance.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/rememberance_pixel.png"));

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
            return true && GraveyardManager.instance != null;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null) && GraveyardManager.instance.opponentGraveyard.Count > 0)
                {
                    CardInfo toSteal = null;
                    foreach(CardInfo inf in GraveyardManager.instance.opponentGraveyard.FindAll(x => x.abilities.Count > 0))
                    {
                        if (toSteal == null || inf.Abilities.CombinedPower() > toSteal.Abilities.CombinedPower())
                        {
                            toSteal = inf;
                        }
                    }
                    if (toSteal != null && toSteal.Abilities.Count > 0)
                    {
                        CardModificationInfo info = new CardModificationInfo();
                        info.abilities.AddRange(toSteal.abilities);

                        CardModificationInfo cardModificationInfo2 = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
                        if (cardModificationInfo2 == null) { cardModificationInfo2 = base.Card.Info.Mods.Find((CardModificationInfo x) => x.HasAbility(this.Ability)); }
                        if (cardModificationInfo2 != null)
                        {
                            info.fromTotem = cardModificationInfo2.fromTotem;
                            info.fromCardMerge = cardModificationInfo2.fromCardMerge;
                        }

                        base.Card.Anim.PlayTransformAnimation();
                        yield return new WaitForSeconds(0.15f);
                        base.Card.AddTemporaryMod(info);
                        base.Card.Status.hiddenAbilities.Add(this.Ability);
                        base.Card.RenderCard();
                    }               
                }
            }
            else
            {
                List<Ability> toAdd = new List<Ability>();
                List<CardInfo> options = new List<CardInfo>();
                options.AddRange(GraveyardManager.instance.playerGraveyard.FindAll(x => x.abilities.Count > 0));
                if (options.Count > 0)
                {
                    yield return base.PreSuccessfulTriggerSequence();

                    if (Tools.GetActAsInt() == 2)
                    {
                        CardInfo selectedCard = null;
                        yield return SpecialCardSelectionHandler.ChoosePixelCard(delegate (CardInfo c)
                        {
                            selectedCard = c;
                        }, options);
                        if (selectedCard != null)
                        {
                            toAdd.AddRange(selectedCard.Abilities);
                        }
                        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    }
                    else
                    {
                        CardInfo selectedCard = null;
                        yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                        {
                            selectedCard = c;
                        }, options);
                        if (selectedCard != null)
                        {
                            toAdd.AddRange(selectedCard.Abilities);
                        }
                        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    }
                }
                if (toAdd.Count > 0)
                {
                    CardModificationInfo info = new CardModificationInfo();
                    info.abilities.AddRange(toAdd);

                    CardModificationInfo cardModificationInfo2 = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
                    if (cardModificationInfo2 == null) { cardModificationInfo2 = base.Card.Info.Mods.Find((CardModificationInfo x) => x.HasAbility(this.Ability)); }
                    if (cardModificationInfo2 != null)
                    {
                        info.fromTotem = cardModificationInfo2.fromTotem;
                        info.fromCardMerge = cardModificationInfo2.fromCardMerge;
                    }

                    base.Card.Anim.PlayTransformAnimation();
                    yield return new WaitForSeconds(0.15f);
                    base.Card.AddTemporaryMod(info);
                    base.Card.Status.hiddenAbilities.Add(this.Ability);
                    base.Card.RenderCard();
                }
            }
        }
    }
}
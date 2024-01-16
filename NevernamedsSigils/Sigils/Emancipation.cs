using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class Emancipation : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Emancipation", "When [creature] is played, copies of any bottled cards the owner possesses are created in the owner's hand.",
                      typeof(Emancipation),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/emancipation.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/emancipation_pixel.png"));

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
            
            foreach (ConsumableItemSlot slot in Part1ItemsManager.Instance.consumableSlots)
            {
                if (slot.Consumable is CardBottleItem bottleItem)
                {
                    if (Singleton<ViewManager>.Instance.CurrentView != View.Default) Singleton<ViewManager>.Instance.SwitchToView(View.Default);
                    CardInfo bottleItemCardInfo = bottleItem.cardInfo;
                    if (base.Card.OpponentCard)
                    {
                        if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                        {
                            PlayableCard playableCard = CardSpawner.SpawnPlayableCard(bottleItemCardInfo);
                            playableCard.SetIsOpponentCard(true);
                            Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                            Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                                Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                            Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                        }

                    }
                    else
                    {

                    yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(bottleItemCardInfo, null, 0.25f);
                    }

                }
            }
            yield break;
        }
    }
}

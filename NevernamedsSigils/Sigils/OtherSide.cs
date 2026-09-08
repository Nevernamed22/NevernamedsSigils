using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class OtherSide : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Other Side", "While [creature] is on the board, any friendly creatures that die in combat will be returned to the owner's hand as skeletons.",
                      typeof(OtherSide),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/otherside.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/otherside_pixel.png"));

            OtherSide.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public string CardIDToSpawn
        {
            get
            {
                string cardID = "Skeleton";
                switch (Tools.GetActAsInt())
                {
                    case 1:
                        cardID = "SigilNevernamed SkeletalBeast";
                        break;
                    case 2:
                        cardID = "Skeleton";
                        break;
                    case 3:
                        cardID = "SigilNevernamed Endoskeleton";
                        break;
                    case 4:
                        cardID = "Skeleton";
                        break;
                }
                return cardID;
            }
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return fromCombat && card.OpponentCard == base.Card.OpponentCard && card != base.Card && base.Card.OnBoard && card.Info.name != CardIDToSpawn;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardLoader.GetCardByName(CardIDToSpawn));
                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }

            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(CardIDToSpawn), null, 0.25f);
                yield return base.LearnAbility(0.5f);
            }
            yield break;
        }
    }
}

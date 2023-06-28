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
    public class DeckedOut : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Decked Out", "When [creature] is struck, a card from a random temple's side deck is created in your hand.",
                      typeof(DeckedOut),
                      categories: new List<AbilityMetaCategory> { Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/deckedout_pixel.png"));

            DeckedOut.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override CardInfo CardToDraw
        {
            get
            {
                int sel = UnityEngine.Random.Range(1, 5);
                CardInfo card =  CardLoader.GetCardByName("Squirrel");
                switch (sel)
                {
                    case 2:
                        card = CardLoader.GetCardByName("SigilNevernamed Act2EmptyVessel");
                        break;
                    case 3:
                        card = CardLoader.GetCardByName("Skeleton");
                        break;
                    case 4:
                        List<string> moxes = new List<string>() { "MoxEmerald", "MoxRuby", "MoxSapphire" };
                        card = CardLoader.GetCardByName(Tools.RandomElement(moxes));
                        break;
                }
                return card;
            }
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return true;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.3f);

            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardToDraw);
                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.RandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }

            }
            else { yield return base.CreateDrawnCard(); }

            if (!base.Card.Dead)
            {
                base.Card.Anim.LightNegationEffect();
                yield return new WaitForSeconds(0.3f);
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
    }
}

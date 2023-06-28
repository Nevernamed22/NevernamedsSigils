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
    public class KinBearer : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Kin Bearer", "When [creature] perishes, a random creature of the same tribe is created in your hand.",
                      typeof(KinBearer),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: true,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/kinbearer.png"),
                       pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/kinbearer_pixel.png"));

            KinBearer.ability = newSigil.ability;
        }
        public static Dictionary<string, List<string>> presetcardpools = new Dictionary<string, List<string>>()
        {

        };

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public override CardInfo CardToDraw
        {
            get
            {
                Tribe required = Tribe.None;
                if ((base.Card.Info.temple == CardTemple.Nature) && (base.Card.Info.tribes.Count > 0)) required = Tools.RandomElement(base.Card.Info.tribes);
                CardInfo gift = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), false, required, false).Clone() as CardInfo;
                gift.Mods.Add(base.Card.CondenseMods(new List<Ability>() { KinBearer.ability }));
                return gift;
            }
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {

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
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return base.CreateDrawnCard();
                yield return base.LearnAbility(0.1f);
            }
            yield break;
        }
    }
}
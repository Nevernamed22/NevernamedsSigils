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
    public class ExaltedRune : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Exalted Rune", "Pay 1 blood to create a 'Mox Pillar' in your hand. A mox pillar is defined as: 0 power, 4 health, Mox Sigil, Mighty Leap.",
                      typeof(ExaltedRune),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/exaltedrune.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/exaltedrune_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override int BloodRequired()
        {
            return 1;
        }
        public static List<string> Pillars = new List<string>()
        {
            "SigilNevernamed RubyPillar",
            "SigilNevernamed EmeraldPillar",
            "SigilNevernamed SapphirePillar"
        };
        public override IEnumerator OnBloodAbilityPostAllSacrifices()
        {
            yield return new WaitForSeconds(0.15f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(Tools.SeededRandomElement<string>(Pillars, base.GetRandomSeed())), null, 0.25f, null);
            yield return new WaitForSeconds(0.45f);
            yield return base.LearnAbility(0.1f);
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard && !playerUpkeep;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
            {
                List<CardSlot> cardslots = Singleton<BoardManager>.Instance.GetSlots(false).FindAll(x => x.Card != null && x.Card != base.Card && x.Card.PowerLevel < 4 && x.Card.CanBeSacrificed);
                if (cardslots.Count > 0)
                {
                    yield return Tools.RandomElement(cardslots).Card.Die(true, null);
                    yield return new WaitForSeconds(0.15f);
                }
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardLoader.GetCardByName(Tools.SeededRandomElement<string>(Pillars, base.GetRandomSeed())));
                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.RandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }
            }
            yield break;
        }
    }
}
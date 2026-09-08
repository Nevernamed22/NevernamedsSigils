using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static InscryptionAPI.Slots.SlotModificationManager;

namespace NevernamedsSigils
{
    public class Afterlife : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Afterlife", "When [creature] perishes, a spectral copy of it is created in your hand. This copy lacks the Afterlife sigil, is free, and is both Immaterial and Doomed. ",
                      typeof(Afterlife),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair1 },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/afterlife.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/afterlife_pixel.png"));

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
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return true;
        }
        public CardInfo SpectralCard
        {
            get
            {
                CardInfo inf = CardLoader.GetCardByName(base.Card.Info.name);
                //inf.name = $"Spectral {inf.name}";
                foreach (CardModificationInfo cardModificationInfo in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo item = (CardModificationInfo)cardModificationInfo.Clone();
                    item.abilities.Remove(Afterlife.ability);
                    inf.Mods.Add(item);
                }
                foreach (CardModificationInfo cardModificationInfo in base.Card.temporaryMods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo item = (CardModificationInfo)cardModificationInfo.Clone();
                    item.abilities.Remove(Afterlife.ability);
                    inf.Mods.Add(item);
                }
                inf.mods.Add(new CardModificationInfo() { negateAbilities = new List<Ability>() { Afterlife.ability } });

                inf.mods.Add(new CardModificationInfo { nameReplacement = $"Spectral {inf.displayedName}", abilities = new List<Ability>() { Immaterial.ability, Doomed.ability }, healthAdjustment = -(inf.Health - 1), nonCopyable = true, bloodCostAdjustment = -inf.BloodCost, bonesCostAdjustment = -inf.bonesCost, nullifyGemsCost = true, energyCostAdjustment = -inf.energyCost });
                return inf;
            }
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();

            if (base.Card.OpponentCard)
            {
                if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                {
                    PlayableCard playableCard = CardSpawner.SpawnPlayableCard(SpectralCard);
                    playableCard.SetIsOpponentCard(true);
                    Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);
                    playableCard.ApplyAppearanceBehaviours(new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.SpectralCard });
                    Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                        Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                    Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                }
            }
            else
            {
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                PlayableCard playableCard = CardSpawner.SpawnPlayableCard(this.SpectralCard);
                playableCard.ApplyAppearanceBehaviours(new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.SpectralCard });
                yield return Singleton<PlayerHand>.Instance.AddCardToHand(playableCard, Singleton<CardSpawner>.Instance.spawnedPositionOffset, 0.25f);
                yield return new WaitForSeconds(0.45f);
            }

            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
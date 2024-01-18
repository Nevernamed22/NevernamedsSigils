using APIPlugin;
using DiskCardGame;
using GBC;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class BloodArtist : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Blood Artist", "Every three cards which perish by combat while [creature] is on the board, [creature] will give its owner a deathcard constructed from the cost, stats, and sigils of those creatures.",
                      typeof(BloodArtist),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: baseTex,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bloodartist_pixel.png"),
                      triggerText: "[creature] constructs a deathcard from previous creatures deaths...");

            ability = newSigil.ability;
        }
        public static Texture2D baseTex = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodartist.png");
        public static Ability ability;
        public static Texture2D stored1 = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodartist_coststored.png");
        public static Texture2D stored2 = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodartist_statsstored.png");
        public static Texture2D stored3 = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bloodartist_sigilstored.png");
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return card != base.Card && stored < 3 && base.Card.OnBoard && fromCombat;
        }
        public int storedPower;
        public int storedHealth;
        public List<Ability> storedSigils;

        public bool hasSpecialStat = false;
        public SpecialStatIcon icon;
        public List<SpecialTriggeredAbility> behaviours = new List<SpecialTriggeredAbility>();

        int bloodCost = 0;
        int boneCost = 0;
        int energyCost = 0;
        List<GemType> gemsCost = new List<GemType>();

        int stored = 0;

        int tribe1 = 0;
        int tribe2 = 0;
        int tribe3 = 0;

        public string costCardName;
        public string abilitiesCardName;
        public string statsCardName;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            int act = Tools.GetActAsInt();
            if (stored == 0) //Cost
            {
                bloodCost = card.Info.BloodCost;
                boneCost = card.Info.BonesCost;
                energyCost = card.EnergyCost;
                gemsCost = card.Info.GemsCost;

                switch (act)
                {
                    case 1:
                        if (card.Info.tribes.Count > 0) { tribe1 = CustomDeathcardPortrait.tribeToInt[Tools.SeededRandomElement(card.Info.tribes)]; }
                        else tribe1 = Tools.SeededRandomElement(CustomDeathcardPortrait.validRandomAct1Tribes);
                        break;
                    case 3:
                        tribe1 = 19;
                        break;
                    case 4:
                        tribe1 = 20;
                        break;
                }

                costCardName = card.Info.name;

                stored++;
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                base.Card.Anim.StrongNegationEffect();
                if (Tools.GetActAsInt() != 2) base.Card.RenderInfo.OverrideAbilityIcon(BloodArtist.ability, stored1);
                base.Card.RenderCard();
                yield return new WaitForSeconds(0.25f);
            }
            else if (stored == 1) //Stats
            {
                storedPower = card.Attack;
                storedHealth = card.Info.Health;

                if (card.Info.specialStatIcon != SpecialStatIcon.None)
                {
                    storedPower = 0;
                    hasSpecialStat = true;
                    icon = card.Info.specialStatIcon;
                    behaviours.AddRange(card.Info.specialAbilities);
                }

                switch (act)
                {
                    case 1:
                        if (card.Info.tribes.Count > 0) { tribe2 = CustomDeathcardPortrait.tribeToInt[Tools.SeededRandomElement(card.Info.tribes)]; }
                        else tribe2 = Tools.SeededRandomElement(CustomDeathcardPortrait.validRandomAct1Tribes);
                        break;
                    case 3:
                        tribe2 = 19;
                        break;
                    case 4:
                        tribe2 = 20;
                        break;
                }

                statsCardName = card.Info.name;

                stored++;
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                base.Card.Anim.StrongNegationEffect();
                if (Tools.GetActAsInt() != 2) base.Card.RenderInfo.OverrideAbilityIcon(BloodArtist.ability, stored2);
                base.Card.RenderCard();
                yield return new WaitForSeconds(0.25f);

            }
            else if (stored == 2) //Sigils && Creation
            {
                storedSigils = new List<Ability>();
                storedSigils.AddRange(card.GetAllAbilities());
                stored++;

                switch (act)
                {
                    case 1:
                        if (card.Info.tribes.Count > 0) { tribe3 = CustomDeathcardPortrait.tribeToInt[Tools.SeededRandomElement(card.Info.tribes)]; }
                        else tribe3 = Tools.SeededRandomElement(CustomDeathcardPortrait.validRandomAct1Tribes);
                        break;
                    case 3:
                        tribe3 = 19;
                        break;
                    case 4:
                        tribe3 = 20;
                        break;
                }

                abilitiesCardName = card.Info.name;

                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                base.Card.Anim.StrongNegationEffect();
                if (Tools.GetActAsInt() != 2) base.Card.RenderInfo.OverrideAbilityIcon(BloodArtist.ability, stored3);
                base.Card.RenderCard();
                yield return new WaitForSeconds(1f);
                base.Card.Anim.PlayTransformAnimation();
                yield return new WaitForSeconds(0.15f);
                if (Tools.GetActAsInt() != 2) base.Card.RenderInfo.OverrideAbilityIcon(BloodArtist.ability, baseTex);
                base.Card.RenderCard();

                if (!base.Card.OpponentCard)
                {
                    Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
                    yield return new WaitForSeconds(0.15f);
                }


                yield return base.PreSuccessfulTriggerSequence();
                if (Tools.GetActAsInt() == 2)
                {
                    if (base.Card.OpponentCard)
                    {
                        if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                        {
                            PlayableCard playableCard = Tools.GenerateandSpawnAct2Deathcard(CardLoader.GetCardByName(abilitiesCardName), CardLoader.GetCardByName(statsCardName), CardLoader.GetCardByName(costCardName));
                            playableCard.SetIsOpponentCard(true);
                            Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                            Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                                Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                            Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                        }
                    }
                    else
                    {
                        yield return Tools.GenerateandGiveAct2Deathcard(CardLoader.GetCardByName(abilitiesCardName), CardLoader.GetCardByName(statsCardName), CardLoader.GetCardByName(costCardName));
                    }
                }
                else
                {
                    if (base.Card.OpponentCard)
                    {
                        if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                        {
                            PlayableCard playableCard = CardSpawner.SpawnPlayableCard(CardToDraw);
                            playableCard.SetIsOpponentCard(true);
                            Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                            Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                                Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                            Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                        }

                    }
                    else
                    {
                        yield return base.CreateDrawnCard();
                    }
                }
                yield return base.LearnAbility(0f);

                stored = 0;
            }
            yield break;
        }

        public override CardInfo CardToDraw
        {
            get
            {
                CardInfo deathcard = CustomDeathcardPortrait.GenerateAnimalDeathcard(bloodCost, boneCost, energyCost, gemsCost, 0, storedPower, storedHealth,
                    hasSpecialStat, icon, behaviours,
                    storedSigils, tribe1, tribe2, tribe3, CustomDeathcardPortrait.GenerateRandomName(), new List<Tribe>() { });
                deathcard.mods.Add(base.Card.CondenseMods(new List<Ability>() { BloodArtist.ability }, true));
                return deathcard;
            }
        }
    }
}
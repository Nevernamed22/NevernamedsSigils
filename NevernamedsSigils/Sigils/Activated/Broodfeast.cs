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
    public class Broodfeast : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Broodfeast", "Once per turn, pay 1 blood to create a copy of [creature] in your hand.",
                      typeof(Broodfeast),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/broodfeast.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/broodfeast_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return true;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (!base.Card.OpponentCard && playerUpkeep)
            {
                activatedThisTurn = false;
            }
            else if (!playerUpkeep && base.Card.OpponentCard && Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
            {
                if (base.Card && base.Card.slot)
                {
                    List<CardSlot> cardslots = Singleton<BoardManager>.Instance.GetSlots(false).FindAll(x => x.Card && x.Card.CanBeSacrificed && x.Card.PowerLevel < base.Card.PowerLevel);
                    if (cardslots.Count > 0)
                    {
                        yield return Tools.SeededRandomElement(cardslots).Card.Die(true, null);
                        yield return new WaitForSeconds(0.15f);
                        base.Card.Anim.StrongNegationEffect();

                        CardInfo toDraw = base.Card.Info.Clone() as CardInfo;
                        foreach (CardModificationInfo inf in base.Card.Info.Mods)
                        {
                            CardModificationInfo clonedmod = inf.Clone() as CardModificationInfo;
                            if (clonedmod.abilities.Contains(Broodfeast.ability)) { clonedmod.abilities.Remove(Broodfeast.ability); }
                            toDraw.Mods.Add(clonedmod);
                        }
                        PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toDraw);

                        foreach (CardModificationInfo inf2 in base.Card.temporaryMods)
                        {
                            CardModificationInfo clonedmod2 = inf2.Clone() as CardModificationInfo;
                            if (clonedmod2.abilities.Contains(Broodfeast.ability)) { clonedmod2.abilities.Remove(Broodfeast.ability); }
                            playableCard.AddTemporaryMod(clonedmod2);
                        }

                        if (Singleton<BoardManager>.Instance.OpponentSlotsCopy.Exists(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null))
                        {
                            playableCard.SetIsOpponentCard(true);
                            Singleton<TurnManager>.Instance.Opponent.ModifyQueuedCard(playableCard);

                            Singleton<BoardManager>.Instance.QueueCardForSlot(playableCard,
                                Tools.SeededRandomElement(Singleton<BoardManager>.Instance.OpponentSlotsCopy.FindAll(x => Singleton<BoardManager>.Instance.GetCardQueuedForSlot(x) == null)));
                            Singleton<TurnManager>.Instance.Opponent.Queue.Add(playableCard);
                        }
                    }

                }
            }
            yield break;
        }
        public override bool AdditionalActivationParameters()
        {
            return !activatedThisTurn;
        }
        public override int BloodRequired()
        {
            return 1;
        }
        public bool activatedThisTurn;
        public override IEnumerator OnBloodAbilityPostAllSacrifices()
        {
            yield return new WaitForSeconds(0.15f);
            base.Card.Anim.StrongNegationEffect();
            activatedThisTurn = true;
            CardInfo toDraw = base.Card.Info.Clone() as CardInfo;
            foreach (CardModificationInfo inf in base.Card.Info.Mods)
            {
                CardModificationInfo clonedmod = inf.Clone() as CardModificationInfo;
                if (clonedmod.abilities.Contains(Broodfeast.ability)) { clonedmod.abilities.Remove(Broodfeast.ability); }
                toDraw.Mods.Add(clonedmod);
            }
            PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toDraw);

            foreach (CardModificationInfo inf2 in base.Card.temporaryMods)
            {
                CardModificationInfo clonedmod2 = inf2.Clone() as CardModificationInfo;
                if (clonedmod2.abilities.Contains(Broodfeast.ability)) { clonedmod2.abilities.Remove(Broodfeast.ability); }
                playableCard.AddTemporaryMod(clonedmod2);
            }
            Singleton<ViewManager>.Instance.SwitchToView(View.Hand, false, false);
            yield return Singleton<PlayerHand>.Instance.AddCardToHand(playableCard, Singleton<CardSpawner>.Instance.spawnedPositionOffset, 0.25f);
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
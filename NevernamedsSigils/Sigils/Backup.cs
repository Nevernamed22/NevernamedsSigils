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
    public class Backup : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Backup", "When [creature] perishes by combat, the first card in its owner's hand is played in its place.",
                      typeof(Backup),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair2, Plugin.Part2Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/backup.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/backup_pixel.png"),
                      triggerText: "[creature] calls in backup.");

            ability = newSigil.ability;
        }
        public static Ability ability;
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice && base.Card.slot != null && (base.Card.slot.Card == null || base.Card.slot.Card.Dead);
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            CardSlot slot = base.Card.slot;
            if (base.Card.OpponentCard)
            {
                PlayableCard found = null;
                List<CardSlot> toCheck = BoardManager.Instance.GetSlots(false);
                if (toCheck != null && toCheck.Count > 0)
                {
                    int i = 0;
                    while (found == null)
                    {
                        if (i >= toCheck.Count) { break; }
                        PlayableCard queued = Singleton<BoardManager>.Instance.GetCardQueuedForSlot(toCheck[i]);
                        if (queued != null)
                        {
                            found = queued;
                            break;
                        }
                        i++;
                    }
                    if (found != null)
                    {
                        yield return new WaitForSeconds(0.5f);
                        yield return base.PreSuccessfulTriggerSequence();

                        found.QueuedSlot = null;
                        found.OnPlayedFromOpponentQueue();
                        yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(found, slot, 0.1f, null, true);
                        Singleton<TurnManager>.Instance.Opponent.Queue.Remove(found);
                        yield return base.LearnAbility(0.5f);
                    }
                }
            }
            else
            {
                if (Singleton<PlayerHand>.Instance.CardsInHand != null && Singleton<PlayerHand>.Instance.CardsInHand.Count > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    yield return base.PreSuccessfulTriggerSequence();

                    yield return Singleton<PlayerHand>.Instance.PlayCardOnSlot(Singleton<PlayerHand>.Instance.CardsInHand[0], slot);

                    yield return base.LearnAbility(0.5f);
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
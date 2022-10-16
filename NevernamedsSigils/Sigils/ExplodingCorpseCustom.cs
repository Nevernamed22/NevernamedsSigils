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
    public class ExplodingCorpseCustom : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Exploding Corpse", "When [creature] perishes, all empty spaces on the board are filled with Guts. Guts are defined as 0 power, 1 health.",
                      typeof(ExplodingCorpseCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/explodingcorpse.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/explodingcorpse_pixel.png"));

            ExplodingCorpseCustom.ability = newSigil.ability;
        }
        public CardInfo GetGuts
        {
            get
            {
                CardInfo guts = CardLoader.GetCardByName("Nevernamed Guts");
                if (base.Card != null)
                {
                    List<Ability> abilities = base.Card.Info.Abilities;
                    foreach (CardModificationInfo cardModificationInfo in base.Card.TemporaryMods)
                    {
                        abilities.AddRange(cardModificationInfo.abilities);
                    }
                    abilities.RemoveAll((Ability x) => x == ExplodingCorpseCustom.ability);


                    if (abilities.Count > 0)
                    {
                        CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
                        cardModificationInfo2.fromCardMerge = true;
                        cardModificationInfo2.abilities = new List<Ability>() { Tools.RandomElement(abilities) };
                        guts.Mods.Add(cardModificationInfo2);
                    }
                }
                return guts;
            }
        }
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
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {
            CardInfo card = GetGuts;
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(card, slot, 0.15f, true);
            yield break;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            if (!wasSacrifice)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.15f);

                List<CardSlot> availableSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true));
                List<CardSlot> availableEnSlots = new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(false));
                availableSlots.AddRange(availableEnSlots);
                for (int i = availableSlots.Count - 1; i >= 0; i--)
                {
                    if (availableSlots[i].Card != null) availableSlots.RemoveAt(i);
                }
                if (availableSlots.Count > 0)
                {
                    yield return base.PreSuccessfulTriggerSequence();

                    foreach (CardSlot targetSlot in availableSlots)
                    {
                        yield return new WaitForSeconds(0.1f);
                        yield return this.SpawnCardOnSlot(targetSlot);
                    }

                    yield return new WaitForSeconds(0.3f);
                    yield return base.LearnAbility(0.1f);
                }
            }
            yield break;
        }
    }
}
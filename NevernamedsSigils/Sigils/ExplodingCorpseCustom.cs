using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class ExplodingCorpseCustom : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Exploding Corpse", "When [creature] perishes, all empty spaces on the board are filled with it's innards.",
                      typeof(ExplodingCorpseCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair2, Plugin.Part2Modular },
                      powerLevel: 3,
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
                CardInfo guts = null;
                if ((base.Card.Info.GetExtendedProperty("ExplodingCorpseGutOverride") != null))
                {
                    guts = CardLoader.GetCardByName(base.Card.Info.GetExtendedProperty("ExplodingCorpseGutOverride"));
                }
                else
                {
                    switch (Tools.GetActAsInt())
                    {
                        case 3:
                            guts = CardLoader.GetCardByName("SigilNevernamed Components");
                            break;
                        case 4:
                            guts = CardLoader.GetCardByName("SigilNevernamed GutsGrimora");
                            break;
                        default:
                            guts = CardLoader.GetCardByName("SigilNevernamed Guts");
                            break;
                    }
                }
                if (base.Card != null)
                {

                    CardModificationInfo info = base.Card.CondenseMods(new List<Ability>() {ExplodingCorpseCustom.ability });
                    if (info.abilities.Count > 0)
                    {
                        CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
                        cardModificationInfo2.fromCardMerge = true;
                        cardModificationInfo2.abilities = new List<Ability>() { Tools.RandomElement(info.abilities) };
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
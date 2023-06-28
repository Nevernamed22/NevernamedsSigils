using DiskCardGame;
using GBC;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Act2Shapeshifter : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;

        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "Act2Shapeshifter", typeof(Act2Shapeshifter)).Id;
        }
        public override bool RespondsToDrawn()
        {
            return !base.Card.Info.mods.Exists(x => x.singletonId.Contains("Act2ShapeshifterIDMod"));
        }
        public override IEnumerator OnDrawn()
        {
            DisguiseInBattle();
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card.Info.mods.Exists(x => x.singletonId.Contains("Act2ShapeshifterIDMod"));
        }
        public override IEnumerator OnResolveOnBoard()
        {
            string singleton = base.Card.Info.mods.Find(x => x.singletonId.Contains("Act2ShapeshifterIDMod")).singletonId;
            singleton = singleton.Replace("Act2ShapeshifterIDMod", "");
            CardInfo reve = CardLoader.GetCardByName(singleton);

            if (base.Card.Info.displayedName != reve.displayedName) yield return RevealInBattle();
            yield break;
        }

        private IEnumerator RevealInBattle()
        {
            string singleton = base.Card.Info.mods.Find(x => x.singletonId.Contains("Act2ShapeshifterIDMod")).singletonId;
            singleton = singleton.Replace("Act2ShapeshifterIDMod", "");
            CardInfo reve = CardLoader.GetCardByName(singleton);

            yield return Singleton<TextBox>.Instance.ShowUntilInput($"{reve.displayedName} sheds its disguise!", TextBox.Style.Neutral, null, TextBox.ScreenPosition.ForceBottom, 0f, true, false, null, false, Emotion.Neutral);

            yield return new WaitForSeconds(0.25f);
            AudioController.Instance.PlaySound3D("trial_cave_outro#1", MixerGroup.TableObjectsSFX, base.transform.position, 1f, 0f, null, null, null, null, false);

            yield return base.PlayableCard.TransformIntoCard(reve, null, delegate
            {
                base.PlayableCard.ClearAppearanceBehaviours();
            });
            yield break;
        }
        private void DisguiseInBattle()
        {
            List<CardInfo> list = new List<CardInfo>(Singleton<CardDrawPiles>.Instance.Deck.Cards);
            list.RemoveAll((CardInfo x) => x.HasSpecialAbility(Act2Shapeshifter.ability));

            CardInfo disguise;
            if (list.Count > 0) { disguise = list[SeededRandom.Range(0, list.Count, SaveManager.SaveFile.GetCurrentRandomSeed())]; }
            else { disguise = CardLoader.GetCardByName("Squirrel"); }

            CardInfo clonedDisguise = disguise.Clone() as CardInfo;
            foreach (CardModificationInfo inf in disguise.mods)
            {
                clonedDisguise.mods.Add(inf.Clone() as CardModificationInfo);
            }

            string targetID = base.Card.Info.name;
            if (base.Card.Info.GetExtendedProperty("Act2ShapeshifterOverride") != null) { targetID = base.Card.Info.GetExtendedProperty("Act2ShapeshifterOverride"); }
                
            CardModificationInfo identifyingMod = new CardModificationInfo();
            identifyingMod.singletonId = $"Act2ShapeshifterIDMod{targetID}";
            identifyingMod.specialAbilities.Add(Act2Shapeshifter.ability);
            clonedDisguise.mods.Add(identifyingMod);

            DisguiseAsCard(clonedDisguise);
        }
        private void DisguiseAsCard(CardInfo disguise)
        {
            base.Card.ClearAppearanceBehaviours();
            base.Card.SetInfo(disguise);
        }
    }
}

using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class InherentGooey : SpecialCardBehaviour
    {
        public static SpecialTriggeredAbility ability;
        public static void Init()
        {
            ability = SpecialTriggeredAbilityManager.Add("nevernamed.inscryption.sigils", "InherentGooey", typeof(InherentGooey)).Id;
        }
        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return source != null && source.Attack > 0;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            if (source.HasTrait(Trait.Giant))
            {
                if (source.Info.name == "!GIANTCARD_MOON" && !ModdedSaveManager.SaveData.GetValueAsBoolean(Plugin.PluginGuid, "hasLearnedGooeyMoon"))
                {
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("No amount of ooze can slow the motion of a celestial body...", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    ModdedSaveManager.SaveData.SetValue(Plugin.PluginGuid, "hasLearnedGooeyMoon", true);
                }
                if (source.Info.name == "!GIANTCARD_SHIP" && !ModdedSaveManager.SaveData.GetValueAsBoolean(Plugin.PluginGuid, "hasLearnedGooeyShip"))
                {
                    yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("RoyalGooeyMechanic", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                    ModdedSaveManager.SaveData.SetValue(Plugin.PluginGuid, "hasLearnedGooeyShip", true);
                }
            }
            else
            {
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.55f);
                source.AddTemporaryMod(new CardModificationInfo(-1, 0));
            }
            yield break;
        }
    }
}

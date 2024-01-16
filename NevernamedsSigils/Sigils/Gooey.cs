using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Dialogue;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Saves;

namespace NevernamedsSigils
{
    public class Gooey : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gooey", "When [creature] is struck, the striker loses 1 power.",
                      typeof(Gooey),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair3, Plugin.Part2Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/gooey.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/gooey_pixel.png"));

            ability = newSigil.ability;



            DialogueManager.GenerateEvent(Plugin.PluginGuid, "RoyalGooeyMechanic",
                new List<CustomLine>()
                {
                    new CustomLine()
                    {
                         emotion = Emotion.Neutral,
                         letterAnimation = TextDisplayer.LetterAnimation.Jitter,
                         speaker = DialogueEvent.Speaker.PirateSkull,
                         text = "Scrape that gunk off the hull, lads! Full steam ahead!"
                    }
                }, null, DialogueEvent.MaxRepeatsBehaviour.PlayLastLine, DialogueEvent.Speaker.PirateSkull);
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
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
                if (!base.Card.Dead) { base.Card.Anim.StrongNegationEffect(); }
            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                base.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.55f);
                source.AddTemporaryMod(new CardModificationInfo(-1, 0));
                yield return base.LearnAbility(0.4f);
                yield break;
            }
        }
    }
}

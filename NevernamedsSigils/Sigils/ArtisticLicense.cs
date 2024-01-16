using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Dialogue;
using InscryptionAPI.Saves;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class ArtisticLicense : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Artistic License", "When [creature] is played, the creature opposing it loses all sigils.",
                      typeof(ArtisticLicense),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/artisticlicense.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/artisticlicense_pixel.png"));

            ArtisticLicense.ability = newSigil.ability;

            DialogueManager.GenerateEvent(Plugin.PluginGuid, "RoyalArtisticLicenseMechanic",
                new List<CustomLine>()
                {
                    new CustomLine()
                    {
                         emotion = Emotion.Neutral,
                         letterAnimation = TextDisplayer.LetterAnimation.Jitter,
                         speaker = DialogueEvent.Speaker.PirateSkull,
                         text = "Trying to plunder me sigils? Yer a sour spoilsport!"
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
        public override bool RespondsToResolveOnBoard()
        {
            return base.Card && base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            PlayableCard card = base.Card.slot.opposingSlot.Card;
            if (card.HasTrait(Trait.Giant))
            {
                if (card.Info.name == "!GIANTCARD_MOON" && !ModdedSaveManager.SaveData.GetValueAsBoolean(Plugin.PluginGuid, "hasLearnedArtisticLicenseMoon"))
                {
                    yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Despite your artistry, the soul of the moon is impervious...", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter, DialogueEvent.Speaker.Single, null, true);
                    ModdedSaveManager.SaveData.SetValue(Plugin.PluginGuid, "hasLearnedArtisticLicenseMoon", true);
                }
                if (card.Info.name == "!GIANTCARD_SHIP" && !ModdedSaveManager.SaveData.GetValueAsBoolean(Plugin.PluginGuid, "hasLearnedArtisticLicenseMoon"))
                {
                    yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("RoyalArtisticLicenseMechanic", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
                    ModdedSaveManager.SaveData.SetValue(Plugin.PluginGuid, "hasLearnedArtisticLicenseMoon", true);
                }
            }
            else
            {
                yield return base.PreSuccessfulTriggerSequence();
                AudioController.Instance.PlaySound2D("magnificus_brush_splatter_bleach", MixerGroup.None, 0.5f, 0f, null, null, null, null, false);
                if (card.FaceDown)
                {
                    card.SetFaceDown(false, true);
                }
                card.Anim.PlayTransformAnimation();
                CardModificationInfo cardModificationInfo = new CardModificationInfo();
                cardModificationInfo.negateAbilities = new List<Ability>();

                int sigilsRemoved = 0;
                foreach (CardModificationInfo cardModificationInfo2 in card.TemporaryMods)
                {
                    sigilsRemoved += cardModificationInfo2.abilities.Count;
                    cardModificationInfo.negateAbilities.AddRange(cardModificationInfo2.abilities);
                }
                sigilsRemoved += card.Info.abilities.Count;
                cardModificationInfo.negateAbilities.AddRange(card.Info.Abilities);
                card.AddTemporaryMod(cardModificationInfo);



                if (base.Card.gameObject.GetComponent<MagickePower>()) { base.Card.gameObject.GetComponent<MagickePower>().RemovedSigilAmount += sigilsRemoved; }

                yield return base.LearnAbility(0.1f);

            }
        }
    }
}
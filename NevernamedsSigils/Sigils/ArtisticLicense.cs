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
    public class ArtisticLicense : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Artistic License", "When [creature] is played, the creature opposing it loses all sigils.",
                      typeof(ArtisticLicense),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/artisticlicense.png"),
                      pixelTex: null);

            ArtisticLicense.ability = newSigil.ability;
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
            if (!card.HasTrait(Trait.Giant))
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
                foreach (CardModificationInfo cardModificationInfo2 in card.TemporaryMods)
                {
                    cardModificationInfo.negateAbilities.AddRange(cardModificationInfo2.abilities);
                }
                cardModificationInfo.negateAbilities.AddRange(card.Info.Abilities);
                card.AddTemporaryMod(cardModificationInfo);

                yield return base.LearnAbility(0.1f);

            }
        }
    }
}
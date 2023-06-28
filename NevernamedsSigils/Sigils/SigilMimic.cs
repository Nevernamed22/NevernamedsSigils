using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;
using Pixelplacement;

namespace NevernamedsSigils
{
    public class SigilMimic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Sigil Mimic", "If [creature] is played opposite an opponent's creature, this sigil is replaced by copies of that creatures sigils.",
                      typeof(SigilMimic),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, Plugin.GrimoraModChair2 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/sigilmimic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/sigilmimic_pixel.png"));

            ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToResolveOnBoard()
        {
            if (base.Card && base.Card.slot && base.Card.slot.opposingSlot && base.Card.slot.opposingSlot.Card != null) return true;
            else return false;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return new WaitForSeconds(0.2f);
            PlayableCard opposer = base.Card.slot.opposingSlot.Card;

            List<Ability> abilities = opposer.GetAllAbilities();
            abilities.RemoveAll(x => ForbiddenAbilities.Contains(x));

            CardModificationInfo info = new CardModificationInfo();
            info.abilities.AddRange(abilities);

            CardModificationInfo cardModificationInfo2 = base.Card.TemporaryMods.Find((CardModificationInfo x) => x.HasAbility(this.Ability));
            if (cardModificationInfo2 == null) { cardModificationInfo2 = base.Card.Info.Mods.Find((CardModificationInfo x) => x.HasAbility(this.Ability)); }
            if (cardModificationInfo2 != null)
            {
                info.fromTotem = cardModificationInfo2.fromTotem;
                info.fromCardMerge = cardModificationInfo2.fromCardMerge;
            }

            base.Card.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
            base.Card.AddTemporaryMod(info);
            base.Card.Status.hiddenAbilities.Add(this.Ability);
            base.Card.RenderCard();



            yield break;
        }
        public static List<Ability> ForbiddenAbilities = new List<Ability>()
        {
            Ability.SquirrelOrbit,
            SigilMimic.ability
        };
    }
}

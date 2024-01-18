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
    public class GraveyardShift : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Graveyard Shift", "When [creature] perishes by combat, it leaves something else in its old space. Unlike Frozen Away, this new creature will retain the sigils and buffs of the original.",
                      typeof(GraveyardShift),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook,AbilityMetaCategory.Part1Modular },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/graveyardshift.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/graveyardshift_pixel.png"));

            GraveyardShift.ability = newSigil.ability;
        }
        public static Ability ability;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return !wasSacrifice && base.Card.OnBoard;
        }
        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.3f);
            string name = "Opossum";
            if (base.Card.Info.iceCubeParams != null && base.Card.Info.iceCubeParams.creatureWithin != null)
            {
                name = base.Card.Info.iceCubeParams.creatureWithin.name;
            }
            CardInfo info = CardLoader.GetCardByName(name);
            info.Mods.Add(base.Card.CondenseMods(new List<Ability>() { GraveyardShift.ability }));
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(info, base.Card.Slot, 0.15f, true);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}
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
    public class Hypermorphic : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Hypermorphic", "At the end of each turn, [creature] will transform into a random card of the same type and quality.",
                      typeof(Hypermorphic),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.GrimoraModChair3, AbilityMetaCategory.GrimoraRulebook },
                      powerLevel: 0,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/hypermorphic.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/hypermorphic_pixel.png"));

            Hypermorphic.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return !playerTurnEnd;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            //SaveManger.SaveFile.isAct1, SaveManager.SaveFile.isisAct2, and SaveManager.SaveFile.isAct3 exist for this purpose 


            Tribe required = Tribe.None;
            bool isRare = base.Card.Info.HasRareTagOrBackground();
            if ((base.Card.Info.temple == CardTemple.Nature) && (base.Card.Info.tribes.Count > 0) && !isRare) required = Tools.SeededRandomElement(base.Card.Info.tribes);

            CardInfo orig = Tools.GetRandomCardOfTempleAndQuality(base.Card.Info.temple, Tools.GetActAsInt(), isRare, required, true, new List<string>() { base.Card.Info.name });
            if (orig != null)
            {
                CardInfo evolution = orig.Clone() as CardInfo;
                foreach (CardModificationInfo mod in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
                {
                    CardModificationInfo clone = (CardModificationInfo)mod.Clone();
                    if (clone.abilities.Contains(Hypermorphic.ability)) clone.abilities.Remove(Hypermorphic.ability);
                    evolution.Mods.Add(clone);
                }

                CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
                cardModificationInfo2.abilities = new List<Ability>() { Hypermorphic.ability };
                evolution.mods.Add(cardModificationInfo2);

                yield return base.PreSuccessfulTriggerSequence();
                base.Card.ResetShield();
                if (evolution != null) yield return base.Card.TransformIntoCard(evolution);
                yield return new WaitForSeconds(0.2f);
                ResourcesManager.Instance.ForceGemsUpdate();
                if (base.Card.Health <= 0 && base.Card.Status.damageTaken > 0)
                {
                    base.Card.HealDamage(((base.Card.MaxHealth - base.Card.Status.damageTaken) * -1) + 1);
                }
                yield return base.LearnAbility(0.5f);
            }
            yield break;
        }
    }
}

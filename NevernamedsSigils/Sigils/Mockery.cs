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
    public class Mockery : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Mockery", "If [creature] is played opposite an opponent's creature, it's stats change to mimic that creature's stats.",
                      typeof(Mockery),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular, Plugin.GrimoraModChair2 },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/mockery.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/mockery_pixel.png"));

            Mockery.ability = newSigil.ability;
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
            PlayableCard opposer = base.Card.slot.opposingSlot.Card;

            if (base.Card.Info.name == "BeastNevernamed Niao")
            {
                CardModificationInfo cardModificationInfo = base.Card.Info.Mods.Find((CardModificationInfo x) => x.singletonId == "niao");
                if (cardModificationInfo == null)
                {
                    cardModificationInfo = new CardModificationInfo();
                    cardModificationInfo.singletonId = "niao";
                    RunState.Run.playerDeck.ModifyCard(base.Card.Info, cardModificationInfo);
                }
                cardModificationInfo.attackAdjustment = opposer.Attack;
                cardModificationInfo.healthAdjustment = opposer.Health;
            }
            else
            {
                int AtkMod = opposer.Attack - base.Card.Attack;
                int healthMod = opposer.Health - base.Card.Health;

                base.Card.temporaryMods.Add(new CardModificationInfo(AtkMod, healthMod));
            }


            yield break;
        }
    }
}

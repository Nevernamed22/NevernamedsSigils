using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class RemoteControlled : DrawCreatedCard
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Remote Controlled", "When [creature] is played, a Remote Controller is created in the owner's hand. When the remote controller is struck, [creature] will attack the striker.",
                      typeof(RemoteControlled),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/remotecontrolled.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/remotecontrolled_pixel.png"));

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

        public override CardInfo CardToDraw
        {
            get
            {
                return CardLoader.GetCardByName("SigilNevernamed RemoteController");
            }
        }
        public override List<CardModificationInfo> CardToDrawTempMods
        {
            get
            {
                return new List<CardModificationInfo>() { base.Card.CondenseMods(new List<Ability>() { RemoteControlled.ability }) };
            }
        }
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return base.CreateDrawnCard();
            yield return base.LearnAbility(0.5f);
            yield break;
        }
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return attacker != null && target.OpponentCard == base.Card.OpponentCard && base.Card.OnBoard && attacker.Health > 0 && target.Info.name == "SigilNevernamed RemoteController";
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
            yield return fakecombat.FakeCombat(!base.Card.OpponentCard, null, base.Card.slot, new List<CardSlot>() { attacker.slot });
            yield break;
        }
    }
}

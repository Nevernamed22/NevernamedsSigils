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
    public class Moxwarp : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Moxwarp", "While [creature] is alive on the board, any enemy attack that enters a sapphire mox will be returned out of all ruby mox, and vice versa.",
                      typeof(Moxwarp),
                      categories: new List<AbilityMetaCategory> {  },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: null,
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/moxwarp_pixel.png"));

           ability = newSigil.ability;
        }
        public static Ability ability;
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            Debug.Log("Other card damaged:"+target.Info.name);
            return amount > 0 && (target.Info.name == "MoxRuby" || target.Info.name == "MoxSapphire");
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            bool triggerBlue = target.Info.name == "MoxRuby";
            Debug.Log($"Othercard damaged triggered({target.Info.name}). Triggerblue({triggerBlue})");

            bool triggered = false;
            yield return base.PreSuccessfulTriggerSequence();
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetSlots(!base.Card.OpponentCard))
            {
                Debug.Log("Started slot iteration");
                if (slot && slot.Card != null)
                {
                Debug.Log("Card is not null");
                    if ((slot.Card.Info.name == "MoxSapphire" && triggerBlue) || (slot.Card.Info.name == "MoxRuby" && !triggerBlue))
                    {
                        triggered = true;
                        Debug.Log("Card had the right name");

                        yield return new WaitForSeconds(0.1f);
                        CardModificationInfo statalt = new CardModificationInfo();
                        statalt.attackAdjustment = attacker.Attack - slot.Card.Attack;
                        slot.Card.AddTemporaryMod(statalt);
                        FakeCombatHandler.FakeCombatThing fakecombat = new FakeCombatHandler.FakeCombatThing();
                        yield return fakecombat.FakeCombat(!slot.Card.OpponentCard, null, slot);
                        yield return new WaitForSeconds(0.1f);
                        slot.Card.RemoveTemporaryMod(statalt);
                    }
                }
            }
            if (triggered)
            {
                yield return base.LearnAbility(0.24f);
            }
            yield break;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
    }
}
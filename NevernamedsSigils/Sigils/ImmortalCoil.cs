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
    public class ImmortalCoil : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Immortal Coil", "While [creature] is alive on the board, any friendly creatures that die to combat will return to the owner's hand, though slightly weakened.",
                      typeof(ImmortalCoil),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/immortalcoil.png"),
                      pixelTex: null);

            ImmortalCoil.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return fromCombat && !card.OpponentCard && card != base.Card && base.Card.OnBoard;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return base.PreSuccessfulTriggerSequence();
            bool flag = Singleton<ViewManager>.Instance.CurrentView != View.Default;
            if (flag)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(card.Info, null, 0.25f, delegate (PlayableCard p)
            {
                if (p.Health > 1) p.temporaryMods.Add(new CardModificationInfo(0, -1));
            });
            yield return new WaitForSeconds(0.45f);
            yield return base.LearnAbility(0.5f);
            yield break;
        }
    }
}

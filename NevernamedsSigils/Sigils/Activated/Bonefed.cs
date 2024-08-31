using APIPlugin;
using DiskCardGame;
using OpponentBones;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Bonefed : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bonefed", "Pay 2 bones to heal [creature] for up to 3 health.",
                      typeof(Bonefed),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/bonefed.png"),
                      pixelTex: null,
                      isActivated: true);

            Bonefed.ability = newSigil.ability;
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        protected override int BonesCost
        {
            get
            {
                return 2;
            }
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OpponentCard && !playerUpkeep && OpponentResourceManager.instance && OpponentResourceManager.instance.OpponentBones >= 2;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            bool casualHeal = (float)base.Card.Health / (float)(base.Card.Health + base.Card.Status.damageTaken) < 0.5f;
            if ((base.Card.slot != null && base.Card.slot.opposingSlot != null && base.Card.slot.opposingSlot.Card != null && base.Card.slot.opposingSlot.Card.Attack >= base.Card.Health) || casualHeal)
            {
                if ((base.Card.slot.opposingSlot.Card != null && !(base.Card.slot.opposingSlot.Card.Attack >= (base.Card.Health + Math.Min(3, base.Card.Status.damageTaken)))) || casualHeal)
                {
                    yield return base.PreSuccessfulTriggerSequence();
                    yield return OpponentResourceManager.instance.RemoveOpponentBones(2);
                    yield return new WaitForSeconds(0.1f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                    base.Card.Status.damageTaken -= 3;
                    base.Card.Status.damageTaken = Mathf.Max(0, base.Card.Status.damageTaken);
                    base.Card.Anim.LightNegationEffect();
                    yield return base.LearnAbility(0.1f);
                }
            }
            yield break;
        }
        public override bool CanActivate()
        {
            return base.Card.Status.damageTaken > 0;
        }
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            base.Card.Status.damageTaken -= 3;
            base.Card.Status.damageTaken = Mathf.Max(0, base.Card.Status.damageTaken);
            base.Card.Anim.LightNegationEffect();
            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}
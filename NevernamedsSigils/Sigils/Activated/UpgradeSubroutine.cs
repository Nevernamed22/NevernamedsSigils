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
    public class UpgradeSubroutine : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Upgrade Subroutine", "When [creature] destroys another bot, this gains 1 point. When activated, the owner is presented with a choice of three sigils to permanently augment the card. Selecting a sigil spends points, and the more points accumulated when the sigil is activated, the higher quality the presented sigils will be.",
                      typeof(UpgradeSubroutine),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 3,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_rulebook.png"),
                      pixelTex: null,
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool CanActivate()
        {
            return (CurrentPower > 0 && base.Card.Info.Abilities.Count < 4);
        }
        public override void ManagedUpdate()
        {
            if (CurrentPower > 5) CurrentPower = 5;
            if (CurrentPower < 0) CurrentPower = 0;
            if (LastSetPower != CurrentPower)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(ability, icons[CurrentPower]);
                base.Card.RenderCard();
                LastSetPower = CurrentPower;
            }
            base.ManagedUpdate();
        }
        public int LastSetPower;
        public int CurrentPower;
        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return otherCard == base.Card;
        }
        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            base.Card.RenderInfo.OverrideAbilityIcon(ability, icons[CurrentPower]);
            base.Card.RenderCard();
            yield break;
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return killer == base.Card;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            if (CurrentPower < 5)
            {
                CurrentPower++;
            }
            yield break;
        }

        private static Dictionary<int, Texture> icons = new Dictionary<int, Texture>()
        {
            {0,Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_0.png") },
            {1,Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_1.png") },
            {2,Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_2.png") },
            {3,Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_3.png") },
            {4,Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_4.png") },
            {5,Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/UpgradeSubroutine/upgradesubroutine_5.png") },
        };
        public override IEnumerator Activate()
        {
            yield return base.PreSuccessfulTriggerSequence();

            List<CardInfo> selectionOptions = new List<CardInfo>();
            List<Ability> learnedAbilities = AbilitiesUtil.GetLearnedAbilities(false, CurrentPower - 1, CurrentPower, AbilityMetaCategory.Part3Modular);
            learnedAbilities.RemoveAll((Ability x) => base.Card.HasAbility(x));
            if (learnedAbilities.Count > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Ability ab = learnedAbilities[SeededRandom.Range(0, learnedAbilities.Count, base.GetRandomSeed())];
                    learnedAbilities.Remove(ab);
                    CardInfo newClone = Tools.TrueClone(base.Card.Info);
                    newClone.Mods.Add(new CardModificationInfo(ab) { singletonId = "UpgradeSubroutineCardSelectionChoice" });
                    selectionOptions.Add(newClone);
                }
                CardInfo selectedCard = null;
                yield return SpecialCardSelectionHandler.DoSpecialCardSelectionReturn(delegate (CardInfo c)
                {
                    selectedCard = c;
                },selectionOptions, false, false);
                Ability toAdd = selectedCard.Mods.Find((CardModificationInfo x) => x.singletonId == "UpgradeSubroutineCardSelectionChoice").abilities[0];

                CardModificationInfo permamod = new CardModificationInfo(toAdd);
                if (AbilitiesUtil.GetInfo(toAdd).powerLevel == 0) { CurrentPower -= 1; }
                else CurrentPower -= AbilitiesUtil.GetInfo(toAdd).powerLevel;
                Part3SaveData.Data.deck.ModifyCard(base.Card.Info, permamod);
                base.Card.RenderCard();
                base.Card.Anim.PlayTransformAnimation();
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
            }



            yield return base.LearnAbility(0.1f);

            yield break;
        }
    }
}
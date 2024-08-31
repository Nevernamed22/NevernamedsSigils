using APIPlugin;
using DiskCardGame;
using GBC;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class GoldDigger : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gold Digger", "At the end of the owner's turn, [creature] will generate 1 currency, up to five times per battle.",
                      typeof(GoldDigger),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/golddigger.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/golddigger_pixel.png"));

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
        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return !base.Card.OpponentCard && base.Card.OpponentCard != playerTurnEnd && numActivations < 5;
        }
        public int numActivations = 0;
        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.1f);
            base.Card.Anim.LightNegationEffect();
            yield return base.PreSuccessfulTriggerSequence();

            switch (Tools.GetActAsInt())
            {
                case 1:
                    View prev = Singleton<ViewManager>.Instance.CurrentView;
                    Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, true);
                    yield return Singleton<CurrencyBowl>.Instance.ShowGain(1, false, false);
                    RunState.Run.currency += 1;
                    Singleton<ViewManager>.Instance.SwitchToView(prev, false, false);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    break;
                case 2:
                    PixelCombatPhaseManager pixelManager = Singleton<PixelCombatPhaseManager>.Instance;
                    pixelManager.excessDamagePanel.gameObject.SetActive(true);
                    yield return pixelManager.excessDamagePanel.ShowExcessDamage(1);
                    pixelManager.excessDamagePanel.gameObject.SetActive(false);
                    break;
                case 3:
                    View prev2 = Singleton<ViewManager>.Instance.CurrentView;
                    GameObject coinAnim = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/CardBattle/GainHoloCoinAnim"));
                    coinAnim.transform.position = base.Card.slot.transform.position;
                    UnityEngine.Object.Destroy(coinAnim, 2f);
                    yield return new WaitForSeconds(0.5f);
                    AudioController.Instance.PlaySound3D("holomap_node_pickup_alt", MixerGroup.TableObjectsSFX, coinAnim.transform.position, 1f, 0f, new AudioParams.Pitch(0.95f + 1f * 0.01f), null, null, null, false).spatialBlend = 0.25f;
                    yield return new WaitForSeconds(0.5f);
                    yield return P03AnimationController.Instance.ShowChangeCurrency(1, true);
                    P03AnimationController.Instance.SwitchToFace(P03AnimationController.Face.Angry, true, true);
                    Singleton<ViewManager>.Instance.SwitchToView(prev2, false, false);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    Part3SaveData.Data.currency += 1;
                    Part3SaveData.Data.IncreaseBounty(1);
                    break;
            }

            yield return new WaitForSeconds(0.1f);
            yield return base.LearnAbility(0.1f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}
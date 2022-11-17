using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using GBC;

namespace NevernamedsSigils
{
    public class Valuable : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Valuable", "[creature] will grant an amount of currency when it perishes.",
                      typeof(Valuable),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/valuable.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/valuable_pixel.png"));

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
        private int CurrencyAmount
        {
            get
            {
                int initial = Tools.GetActAsInt() == 1 ? 2 : 3;
                int num = initial;
                if (base.Card.Info.GetExtendedProperty("CustomValuablePayoutAmount") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomValuablePayoutAmount"), out num);
                    num = succeed ? num : initial;
                }
                return num;
            }
        }
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            return true;

        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            int amount = CurrencyAmount;          
            switch (Tools.GetActAsInt())
            {
                case 1:
                    View prev = Singleton<ViewManager>.Instance.CurrentView;
                    Singleton<ViewManager>.Instance.SwitchToView(View.CardMergeSlots, false, true);
                    yield return Singleton<CurrencyBowl>.Instance.ShowGain(amount, false, false);
                    RunState.Run.currency += amount;
                    Singleton<ViewManager>.Instance.SwitchToView(prev, false, false);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    break;
                case 2:
                    PixelCombatPhaseManager pixelManager = Singleton<PixelCombatPhaseManager>.Instance;
                    pixelManager.excessDamagePanel.gameObject.SetActive(true);
                    yield return pixelManager.excessDamagePanel.ShowExcessDamage(amount);
                    pixelManager.excessDamagePanel.gameObject.SetActive(false);
                    break;
                case 3:

                    View prev2 = Singleton<ViewManager>.Instance.CurrentView;
                    for (int i = 0; i < amount; i++)
                    {
                        GameObject coinAnim = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/CardBattle/GainHoloCoinAnim"));
                        coinAnim.transform.position = base.Card.Slot.transform.position;
                        UnityEngine. Object.Destroy(coinAnim, 2f);
                        yield return new WaitForSeconds(0.5f);
                        AudioController.Instance.PlaySound3D("holomap_node_pickup_alt", MixerGroup.TableObjectsSFX, coinAnim.transform.position, 1f, 0f, new AudioParams.Pitch(0.95f + (float)i * 0.01f), null, null, null, false).spatialBlend = 0.25f;
                    }
                    yield return new WaitForSeconds(0.5f);
                    yield return P03AnimationController.Instance.ShowChangeCurrency(amount, true);
                    P03AnimationController.Instance.SwitchToFace(P03AnimationController.Face.Angry, true, true);
                    Singleton<ViewManager>.Instance.SwitchToView(prev2, false, false);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                    Part3SaveData.Data.currency += amount;
                    Part3SaveData.Data.IncreaseBounty(amount);
                    break;
            }            
            yield break;
        }
    }
}
using System;
using System.Collections.Generic;
using DiskCardGame;
using System.Text;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using InscryptionAPI.Card;
using Pixelplacement;
using GBC;

namespace NevernamedsSigils
{
    public class OpponentResourceManager : NonCardTriggerReceiver
    {
        public static GameObject pixelBonesIcon;
        public static PixelNumeral pixelBonesCounter;
        public static OpponentResourceManager instance;
        public static Vector3 opponentBonesPosition = new Vector3(4.1795f, 5.01f, 2.761f);

        private void Start()
        {
            if (pixelBonesIcon == null && Singleton<ResourcesManager>.Instance is PixelResourcesManager)
            {
                pixelBonesIcon = UnityEngine.Object.Instantiate(((PixelResourcesManager)Singleton<ResourcesManager>.Instance).bonesParent.gameObject);
                pixelBonesCounter = pixelBonesIcon.GetComponentInChildren<PixelNumeral>();
                pixelBonesIcon.transform.localPosition = new Vector3(2.0555f, 0.97f, 0f); 
            }
        }
        public override bool TriggerBeforeCards => true;
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return AddOpponentBones(deathSlot, 1);
            yield break;
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return card.OpponentCard && card.Info.GetExtendedProperty("PreventBones") == null;
        }
        public IEnumerator AddOpponentBones(CardSlot slot, int bones)
        {
            opponentBones += bones;
            ResourcesManager res = Singleton<ResourcesManager>.Instance;
            if (res != null && res is Part1ResourcesManager)
            {
                for (int i = 0; i < bones; i++)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((res as Part1ResourcesManager).boneTokenPrefab);
                    UnityEngine.Object.Destroy(gameObject.GetComponent<BoneTokenInteractable>());
                    OpponentBoneToken component = gameObject.AddComponent<OpponentBoneToken>();
                    Rigidbody tokenRB = gameObject.GetComponent<Rigidbody>();

                    if (slot != null) { gameObject.transform.position = slot.transform.position + Vector3.up; }
                    else { gameObject.transform.position = opponentBonesPosition + Vector3.up * 5f; }

                    (res as Part1ResourcesManager).PushTokenDown(tokenRB);

                    Vector3 endValue = opponentBonesPosition + Vector3.up * (float)opponentBones * 0.1f;
                    // gameObject.GetComponent<Collider>().enabled = false;
                    Tween.Rotation(component.transform, new Vector3(-90f, 0f, 0f), 0.1f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, null, true);
                    Tween.Position(component.transform, endValue, 0.25f, 0.5f, Tween.EaseInOut, Tween.LoopType.None, null, delegate ()
                    {
                        gameObject.GetComponent<Collider>().enabled = true;
                    }, true);
                    opponentTokens.Add(component);
                    yield return new WaitForSeconds(0.05f);
                }
            }
            else if (pixelBonesIcon != null)
            {
                AudioController.Instance.PlaySound2D("chipBlip2", MixerGroup.None, 0.4f, 0f, new AudioParams.Pitch(Mathf.Min(0.8f + (float)opponentBones * 0.05f, 1.2f)), null, null, null, false);
                pixelBonesCounter.DisplayValue(opponentBones);
                this.BounceRenderer(pixelBonesIcon.transform);
            }
            yield break;
        }
        private void BounceRenderer(Transform rendererTransform)
        {
            Vector3 vector = rendererTransform.localPosition;
            Tween.LocalPosition(rendererTransform, vector + Vector3.up * 0.02f, 0.025f, 0f, Tween.EaseOut, Tween.LoopType.None, null, null, true);
            Tween.LocalPosition(rendererTransform, vector, 0.025f, 0.075f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
        }
        public IEnumerator RemoveOpponentBones(int number)
        {
            opponentBones -= number;
            for (int i = 0; i < number; i++)
            {
                if (this.opponentTokens.Count > 0)
                {
                    OpponentBoneToken component = this.opponentTokens[this.opponentTokens.Count - 1].GetComponent<OpponentBoneToken>();
                    component.FlyOffBoard();
                    this.opponentTokens.Remove(component);
                    yield return new WaitForSeconds(0.075f);
                }
            }
            yield break;
        }
        public int OpponentBones
        {
            get
            {
                return opponentBones;
            }
        }
        private int opponentBones;
        private List<OpponentBoneToken> opponentTokens = new List<OpponentBoneToken>();

        public IEnumerator CleanupManager()
        {
            yield return RemoveOpponentBones(opponentBones);
            instance = null;
            UnityEngine.Object.Destroy(base.gameObject);
            yield break;
        }
    }

    public class OpponentBoneToken : MonoBehaviour
    {
        public void FlyOffBoard()
        {
            UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
            Tween.Position(base.transform, base.transform.position + new Vector3(4f, 2f, 0f), 0.3f, 0f, null, Tween.LoopType.None, null, null, true);
            Tween.Rotate(base.transform, new Vector3(90f, 0f, 90f), 0, 0.3f, 0f, null, Tween.LoopType.None, null, null, true);
            AudioController.Instance.PlaySound3D("token_enter_higher", MixerGroup.TableObjectsSFX, base.transform.position, 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Small), new AudioParams.Repetition(0.05f, ""), null, null, false);
            UnityEngine.Object.Destroy(base.gameObject, 0.3f);
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.impulse.magnitude > 10f)
            {
                float volume = Mathf.Clamp(Mathf.Sqrt(collision.impulse.magnitude) * 0.1f, 0.5f, 1f);
                AudioController.Instance.PlaySound3D("token_enter", MixerGroup.TableObjectsSFX, base.transform.position, volume, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Small), new AudioParams.Repetition(0.05f, ""), null, null, false);
            }
        }
    }


    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.StartGame), typeof(EncounterData))]
    public class BattleStart
    {
        [HarmonyPrefix]
        public static void BattleStartPatch(TurnManager __instance)
        {
            int act = Tools.GetActAsInt();
            if (act == 1 || act == 2 || act == 4)
            {
                if (OpponentResourceManager.instance != null) { UnityEngine.Object.Destroy(OpponentResourceManager.instance.gameObject); }
                GameObject inst = new GameObject();
                OpponentResourceManager resinst = inst.AddComponent<OpponentResourceManager>();
                OpponentResourceManager.instance = resinst;
            }
        }
    }

    [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.CleanupPhase))]
    public class BattleEnd
    {
        [HarmonyPostfix]
        public static IEnumerator BattleEndPatch(IEnumerator enumerator, TurnManager __instance)
        {
            if (OpponentResourceManager.instance != null) { yield return OpponentResourceManager.instance.CleanupManager(); }
            yield return enumerator;
            yield break;
        }
    }
}

using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Card;
using NevernamedsSigils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NevernamedsSigils
{
    [HarmonyPatch]
    internal static class PixelDecalPatches
    {
        [HarmonyPatch(typeof(PixelCardDisplayer), nameof(PixelCardDisplayer.DisplayInfo))]
        [HarmonyPostfix]
        private static void DecalPatches(PixelCardDisplayer __instance)
        {
            if ((SceneManager.GetActiveScene().name == "GBC_CardBattle" && __instance.gameObject.name != "CardPreviewPanel") || __instance.gameObject.name == "PixelSnap")
            {
                AddDecalToCard(in __instance);
            }
        }

        private static void AddDecalToCard(in PixelCardDisplayer instance)
        {
            if (instance && instance.gameObject && instance.gameObject.transform && instance.gameObject.transform.Find("CardElements"))
            {
                Transform cardElements = instance.gameObject.transform.Find("CardElements");

                List<Transform> existingDecals = new List<Transform>();
                foreach (Transform child in cardElements.transform)
                {
                    if (child && child.gameObject && child.gameObject.GetComponent<DecalIdentifier>()) { existingDecals.Add(child); }
                }
                for (int i = existingDecals.Count - 1; i >= 0; i--) { existingDecals[i].parent = null; UnityEngine.Object.Destroy(existingDecals[i].gameObject); }

                if (instance.info.Gemified && cardElements.Find("PixelGemifiedBorder") == null)
                {
                   GameObject border = CreateDecal(in cardElements, Plugin.PixelGemifiedDecal, "PixelGemifiedBorder");
                    PixelGemificationBorder gemBorder = border.AddComponent<PixelGemificationBorder>();
                    gemBorder.BlueGemLit = CreateDecal(in cardElements, Plugin.PixelGemifiedBlueLit, "PixelGemifiedBlue");
                    gemBorder.GreenGemLit = CreateDecal(in cardElements, Plugin.PixelGemifiedGreenLit, "PixelGemifiedGreen");
                    gemBorder.OrangeGemLit = CreateDecal(in cardElements, Plugin.PixelGemifiedOrangeLit, "PixelGemifiedOrange");
                }

                foreach (CardAppearanceBehaviour.Appearance appearance in instance.info.appearanceBehaviour)
                {
                    CardAppearanceBehaviourManager.FullCardAppearanceBehaviour fullApp = CardAppearanceBehaviourManager.AllAppearances.Find((CardAppearanceBehaviourManager.FullCardAppearanceBehaviour x) => x.Id == appearance);
                    if (fullApp != null && fullApp.AppearanceBehaviour != null)
                    {
                        Component behav = instance.gameObject.GetComponent(fullApp.AppearanceBehaviour);
                        if (behav == null) behav = instance.gameObject.AddComponent(fullApp.AppearanceBehaviour);
                        if (behav is PixelAppearanceBehaviour)
                        {
                            (behav as PixelAppearanceBehaviour).OnAppearanceApplied();
                            if ((behav as PixelAppearanceBehaviour).PixelAppearance() != null && cardElements.Find(appearance.ToString() + "_Displayer") == null)
                            {
                                CreateDecal(in cardElements, (behav as PixelAppearanceBehaviour).PixelAppearance(), appearance.ToString() + "_Displayer");
                            }
                        }
                        UnityEngine.Object.Destroy(behav);
                    }
                }
            }
        }
        private static GameObject CreateDecal(in Transform cardElements, Sprite sprite, string name)
        {
            GameObject decal = new GameObject(name);
            decal.transform.SetParent(cardElements, false);
            decal.layer = LayerMask.NameToLayer("GBCUI");

            decal.AddComponent<DecalIdentifier>();

            SpriteRenderer sr = decal.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            // Find sorting group values
            if (cardElements.Find("Portrait") != null && cardElements.Find("Portrait").gameObject && cardElements.Find("Portrait").gameObject.GetComponent<SpriteRenderer>())
            {
                SpriteRenderer sortingReference = cardElements.Find("Portrait").gameObject.GetComponent<SpriteRenderer>();
                sr.sortingLayerID = sortingReference?.sortingLayerID ?? 0;
                sr.sortingOrder = sortingReference?.sortingOrder + 100 ?? 0;
            }

            return decal;
        }
        private class DecalIdentifier : MonoBehaviour { }
    }
    public class PixelAppearanceBehaviour : CardAppearanceBehaviour
    {
        public override void ApplyAppearance() { }
        public virtual Sprite PixelAppearance() { return null; }
        public virtual Sprite OverrideBackground() { return null; }
        public virtual void OnAppearanceApplied() { }
    }
}

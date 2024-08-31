using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public static class SlotModificationTools
    {
        public static GameObject ModifySlot(this CardSlot slot, SlotModification toAdd)
        {
            if (Tools.GetActAsInt() == 2)
            {
                PixelModifiedSlot modslot = null;
                if (slot.gameObject.GetComponent<PixelModifiedSlot>()) { modslot = slot.gameObject.GetComponent<PixelModifiedSlot>(); }
                else
                {
                    modslot = slot.gameObject.AddComponent<PixelModifiedSlot>();
                    modslot.Setup();
                }
                return modslot.AddModifier(toAdd);
            }
            else
            {
                ModifiedSlot modslot = null;
                if (slot.gameObject.GetComponent<ModifiedSlot>()) { modslot = slot.gameObject.GetComponent<ModifiedSlot>(); }
                else
                {
                    modslot = slot.gameObject.AddComponent<ModifiedSlot>();
                    modslot.Setup();
                }
                return modslot.AddModifier(toAdd);
            }
        }
        public static bool SlotHasModifier(this CardSlot slot, string IDToFind)
        {
            if (Tools.GetActAsInt() == 2)
            {
                if (slot.gameObject.GetComponent<PixelModifiedSlot>() && slot.gameObject.GetComponent<PixelModifiedSlot>().modifications.Exists(x => x.identifier == IDToFind)) return true;
                else return false;
            }
            else
            {
                if (slot.gameObject.GetComponent<ModifiedSlot>() && slot.gameObject.GetComponent<ModifiedSlot>().modifications.Exists(x => x.identifier == IDToFind)) return true;
                else return false;
            }
        }
        public static GameObject SpawnFloater(CardSlot slot, Texture tex)
        {
            GameObject floater = new GameObject("Slot Floater");

            if (Tools.GetActAsInt() == 2)
            {
                SpriteRenderer rende = floater.AddComponent<SpriteRenderer>();
                rende.sprite = Tools.ConvertTexToSprite(tex as Texture2D);
                rende.enabled = true;

                floater.transform.position = slot.transform.position + new Vector3(0, -0.28f, 0);
                foreach (Transform t in floater.GetComponentsInChildren<Transform>())
                {
                    t.localRotation = Quaternion.identity;
                }
                floater.transform.rotation = slot.transform.rotation;
                floater.SetActive(true);

                floater.layer = LayerMask.NameToLayer("GBCUI");

                    rende.sortingLayerName = "UI";

                FloaterController floatcontrol = floater.AddComponent<FloaterController>();
                floatcontrol.baseSlot = slot;
            }
            else
            {
                MeshRenderer mesh = floater.AddComponent<MeshRenderer>();
                MeshFilter filter = floater.AddComponent<MeshFilter>();
                BoxCollider collider = floater.AddComponent<BoxCollider>();

                foreach (Renderer rend in floater.GetComponentsInChildren<Renderer>())
                {
                    rend.enabled = true;
                    rend.material = new Material(rend.material);
                }

                CardAbilityIcons GenericAbilityIcons = Singleton<CardSpawner>.Instance.PlayableCardPrefab.GetComponent<PlayableCard>().AbilityIcons;

                Material totemMat = ResourceBank.Get<GameObject>("Prefabs/Items/TotemPieces/TotemBottom_CardGainAbility").GetComponentInChildren<AbilityIconInteractable>().GetComponentInChildren<MeshRenderer>().material;

                mesh.material = totemMat;
                filter.mesh = GenericAbilityIcons.GetComponentInChildren<MeshFilter>().mesh;
                filter.sharedMesh = GenericAbilityIcons.GetComponentInChildren<MeshFilter>().sharedMesh;

                //Debug.Log("1");
                floater.GetComponent<Renderer>().material = new Material(totemMat);
                //Debug.Log("2");
                floater.GetComponent<Renderer>().material.mainTexture = tex;

                Color colour = GameColors.instance.brightGold;
                switch (Tools.GetActAsInt())
                {
                    case 3: colour = GameColors.instance.brightBlue; break;
                    case 4: colour = GameColors.instance.yellow; break;
                    case 5: colour = GameColors.instance.lightPurple; break;
                }
                floater.GetComponent<Renderer>().material.SetColor("_EmissionColor", colour);
                //Debug.Log("3");




                //Debug.Log("4");
                foreach (Transform t in floater.GetComponentsInChildren<Transform>())
                {
                    t.localRotation = Quaternion.identity;
                    t.localScale = new Vector3((t.localScale.x * 0.35f), (t.localScale.x * 0.35f), t.localScale.z);
                }
                //Debug.Log("5");
                //Debug.Log($"floater transform {floater.transform != null}");
                //Debug.Log($"slot  {slot != null}");
                //Debug.Log($"slot transform {slot.transform != null}");
                floater.transform.position = slot.transform.position + new Vector3(0, 0.1f, ModifiedSlot.verticalOffset);
                //Debug.Log("6");
                floater.transform.rotation = slot.transform.rotation;
                //Debug.Log("7");
                floater.transform.Rotate(Vector3.left, -70f);

                //Debug.Log("8");
                floater.SetActive(true);

                //Debug.Log("9");
                //floater.transform.parent = slot.gameObject.transform;

                FloaterController floatcontrol = floater.AddComponent<FloaterController>();
                floatcontrol.elapsed = UnityEngine.Random.value;
                floatcontrol.baseSlot = slot;
            }
            return floater;
        }
    }
    public class PixelModifiedSlot : MonoBehaviour
    {
        public List<SlotModification> modifications = new List<SlotModification>();
        List<GameObject> floaters = new List<GameObject>();
        private CardSlot baseSlot;
        public Vector3 FloaterCenter;
        public static float adjuster = 0.12f;
        public static float verticalOffset = -0.30f;
        public void Setup()
        {
            baseSlot = base.gameObject.GetComponent<CardSlot>();
            FloaterCenter = baseSlot.transform.position + new Vector3(0, verticalOffset, 0);
        }

        public GameObject AddModifier(SlotModification toAdd)
        {
            if (modifications.Exists(x => x.identifier == toAdd.identifier) && !toAdd.stacks) { return null; }
            GameObject newFloater = SlotModificationTools.SpawnFloater(baseSlot, toAdd.pixelTex);
            floaters.Add(newFloater);
            ReadjustFloaters();
            modifications.Add(toAdd);

            return newFloater;
        }
        public void ReadjustFloaters()
        {
             Vector3 start = floaters.Count == 1 ? FloaterCenter : FloaterCenter - new Vector3(((((float)floaters.Count - 1f) * adjuster) / 2f), 0, 0);
             Vector3 nextPlacement = start;
             foreach (GameObject floater in floaters)
             {
                 floater.transform.position = nextPlacement;
                 floater.GetComponent<FloaterController>().center = nextPlacement;
                 nextPlacement.x += adjuster;
             }
        }
    }
    public class FloaterController : MonoBehaviour
    {
        public Vector3 center;
        public Vector3 adjustment = new Vector3(0f, 0f, 0.05f);
        public bool returning;
        public float elapsed = 0f;
        public CardSlot baseSlot;
    }
    public class SlotModification
    {
        public string identifier;
        public bool stacks;
        public Texture tex;
        public Texture pixelTex;
    }
    public class ModifiedSlot : MonoBehaviour
    {
        public List<SlotModification> modifications = new List<SlotModification>();
        List<GameObject> floaters = new List<GameObject>();
        private CardSlot baseSlot;
        public Vector3 FloaterCenter;


        public void Setup()
        {
            baseSlot = base.gameObject.GetComponent<CardSlot>();
            FloaterCenter = baseSlot.transform.position + new Vector3(0, 0.1f, verticalOffset);
        }
        public GameObject AddModifier(SlotModification toAdd)
        {
            if (modifications.Exists(x => x.identifier == toAdd.identifier) && !toAdd.stacks) { return null; }
            GameObject newFloater = SlotModificationTools.SpawnFloater(baseSlot, toAdd.tex);
            floaters.Add(newFloater);
            ReadjustFloaters();
            modifications.Add(toAdd);

            return newFloater;
        }
        public static float adjuster = 0.25f;
        public static float verticalOffset = -1f;
        public void ReadjustFloaters()
        {
            Vector3 start = floaters.Count == 1 ? FloaterCenter : FloaterCenter - new Vector3(((((float)floaters.Count - 1f) * adjuster) / 2f), 0, 0);
            Vector3 nextPlacement = start;
            foreach (GameObject floater in floaters)
            {
                floater.transform.position = nextPlacement;
                floater.GetComponent<FloaterController>().center = nextPlacement;
                nextPlacement.x += adjuster;
            }
        }
        public void Update()
        {
            if (Singleton<TurnManager>.Instance == null || Singleton<TurnManager>.Instance.GameEnded)
            {
                foreach (GameObject floater in floaters)
                {
                    Destroy(floater);
                }
                Destroy(this);
            }
            else
            {
                foreach (GameObject floater in floaters)
                {
                    FloaterController controller = floater.GetComponent<FloaterController>();
                    controller.elapsed += Time.deltaTime;
                    if (controller.elapsed >= 2)
                    {
                        controller.returning = !controller.returning;
                        controller.elapsed = 0f;
                    }
                    if (controller.returning)
                    {
                        floater.transform.position = Vector3.Lerp(controller.center + controller.adjustment, controller.center, controller.elapsed / 2f);
                    }
                    else
                    {
                        floater.transform.position = Vector3.Lerp(controller.center, controller.center + controller.adjustment, controller.elapsed / 2f);
                    }
                }
            }

        }
    }
}

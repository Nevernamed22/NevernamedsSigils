using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class HaunterCustom : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Haunter", "When [creature] dies to combat, it haunts the space it died in. Creatures that enter this space absorb its old sigils.",
                      typeof(HaunterCustom),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/haunter.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/haunter_pixel.png"));

            HaunterCustom.ability = newSigil.ability;
        }
        public static Ability ability;

        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public override bool RespondsToPreDeathAnimation(bool wasSacrifice)
        {
            if ((Card.GetAllAbilities().FindAll((x) => x != HaunterCustom.ability).Count > 0) && !wasSacrifice) return true;
            else return false;
        }
        public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
        {
            List<GameObject> createdSigils = new List<GameObject>();
            if (Card.GetComponentInChildren<CardAbilityIcons>() == null) { yield break; }
            foreach (AbilityIconInteractable icon in Card.GetComponentInChildren<CardAbilityIcons>().abilityIcons)
            {
                if (icon.Ability == HaunterCustom.ability) continue;

                //Get and Spawn Sigils
                GameObject sigils = UnityEngine.Object.Instantiate(icon.transform.gameObject, (icon.transform.position + new Vector3(0, 0.1f, 0)), icon.transform.rotation);
                foreach (Renderer rend in sigils.GetComponentsInChildren<Renderer>())
                {
                    rend.enabled = true;
                    rend.material = new Material(rend.material);
                }
                Texture tex = sigils.GetComponent<Renderer>().material.mainTexture;
                sigils.GetComponent<Renderer>().material = Card.GetComponentInChildren<CardAbilityIcons>().emissiveIconMat != null ? new Material(Card.GetComponentInChildren<CardAbilityIcons>().emissiveIconMat) : sigils.GetComponent<Renderer>().material = new Material(Card.GetComponentInChildren<CardAbilityIcons>().defaultIconMat);
                sigils.GetComponent<Renderer>().material.mainTexture = tex;
                sigils.GetComponent<Renderer>().material.SetColor("_Color", Tools.GetActAsInt() == 1 ? GameColors.Instance.glowSeafoam : GameColors.Instance.blue);
                //Debug.Log(sigils.GetComponent<Renderer>().material.shader.name);

                if (sigils.GetComponent<AbilityIconInteractable>() != null) UnityEngine.Object.Destroy(sigils.GetComponent<AbilityIconInteractable>());
                if (sigils.GetComponent<InscryptionCommunityPatch.Card.ActivatedAbilityIconInteractable>() != null) UnityEngine.Object.Destroy(sigils.GetComponent<InscryptionCommunityPatch.Card.ActivatedAbilityIconInteractable>());

                //Handle Rotation
                int iterator = 0;
                foreach (Transform t in sigils.GetComponentsInChildren<Transform>())
                {
                    t.localRotation = Quaternion.identity;
                    t.localScale = icon.transform.GetComponentsInChildren<Transform>()[iterator].transform.localScale;
                    t.localScale = new Vector3((t.localScale.x * 1.2f), (t.localScale.x * 1.2f), t.localScale.z);
                    iterator++;
                }
                sigils.transform.rotation = Card.Slot.transform.rotation;
                sigils.transform.Rotate(Vector3.left, -90f);

                //Fix Rend
                List<CardAbilityIcons> icons = sigils.GetComponentsInParent<CardAbilityIcons>().ToList();
                icons.AddRange(sigils.GetComponentsInChildren<CardAbilityIcons>().ToList());
                foreach (CardAbilityIcons i in icons) if (i != null) DestroyImmediate(i);

                sigils.SetActive(true);
                createdSigils.Add(sigils);


            }
            if (createdSigils.Count > 0)
            {
                Card.Slot.gameObject.AddComponent<HauntedSlot>().Init(Card.Slot, createdSigils, Card.Info.Abilities.FindAll((x) => x != HaunterCustom.ability));
                Singleton<GlobalTriggerHandler>.Instance.RegisterNonCardReceiver(Card.Slot.GetComponent<HauntedSlot>());
            }
            yield break;
        }
    }
    public class HauntedSlot : NonCardTriggerReceiver
    {
        public void Init(CardSlot slot, List<GameObject> visualSigils, List<Ability> actualSigils)
        {
            if (hauntSigils == null)
            {
                hauntSigils = new List<GameObject>();
            }
            if (abilities == null)
            {
                abilities = new List<Ability>();
            }
            hauntSigils.Clear();
            abilities.Clear();
            hauntSigils.AddRange(visualSigils);
            abilities.AddRange(actualSigils);
            setUp = true;
            this.slot = slot;

            if (Tools.GetActAsInt() == 3)
            {
                CustomCoroutine.FlickerSequence(delegate
                {
                    foreach (GameObject haunting in hauntSigils) { if (haunting != null) haunting.GetComponent<Renderer>().enabled = true; }
                }, delegate
                {
                    foreach (GameObject haunting in hauntSigils) { if (haunting != null) haunting.GetComponent<Renderer>().enabled = false; }
                }, false, true, 0.1f, 3, null);

                if (slot.conduitFrame != null)
                {
                    conduitFakeFrame = UnityEngine.Object.Instantiate(slot.conduitFrame);
                    conduitFakeFrame.transform.position = slot.conduitFrame.transform.position;
                    conduitFakeFrame.SetActive(true);
                }
            }
        }
        public GameObject conduitFakeFrame;
        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return setUp && otherCard.Slot == slot;
        }

        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {

            otherCard.AddTemporaryMod(new CardModificationInfo { abilities = new List<Ability>(abilities), fromCardMerge = false, fromTotem = false });
            otherCard.Anim.PlayTransformAnimation();
            otherCard.RenderCard();
            if (conduitFakeFrame != null) Destroy(conduitFakeFrame);

            List<GameObject> haunts = hauntSigils;
            foreach (GameObject haunt in haunts)
            {
                if (haunt != null)
                {
                    CustomCoroutine.WaitThenExecute(0.1f, delegate () { Destroy(haunt); }, false);
                }
            }
            CustomCoroutine.WaitThenExecute(0.1f, delegate () { Destroy(this); }, false);
            yield break;
        }
        public override void ManagedUpdate()
        {
            if (Singleton<TurnManager>.Instance == null || Singleton<TurnManager>.Instance.GameEnded)
            {
                foreach (GameObject haunt in hauntSigils)
                {
                    Destroy(haunt);
                }
                if (conduitFakeFrame != null) Destroy(conduitFakeFrame);
                Destroy(this);
            }
            foreach (GameObject floater in hauntSigils)
            {
                if (floater.GetComponent<HauntedSigilFloatData>() != null)
                {
                    HauntedSigilFloatData floatData = floater.GetComponent<HauntedSigilFloatData>();
                    if (floatData.MovingUp)
                    {
                        float movement = 0.001f;
                        if (floatData.currentHeight >= floatData.upperSlowTheshold || floatData.currentHeight <= floatData.lowerSlowThreshold) movement *= 0.5f;
                        if (floatData.currentHeight >= floatData.upperLimit)
                        {
                            movement *= -1;
                            floatData.MovingUp = false;
                        }
                        floater.transform.position += new Vector3(0, movement, 0);
                        floatData.currentHeight = floater.transform.position.y;
                    }
                    else
                    {
                        float movement = -0.001f;
                        if (floatData.currentHeight >= floatData.upperSlowTheshold || floatData.currentHeight <= floatData.lowerSlowThreshold) movement *= 0.5f;
                        if (floatData.currentHeight <= floatData.lowerLimit)
                        {
                            movement *= -1;
                            floatData.MovingUp = true;
                        }
                        floater.transform.position += new Vector3(0, movement, 0);
                        floatData.currentHeight = floater.transform.position.y;
                    }
                    //icon.transform.position + new Vector3(0, 0.1f, 0)
                }
                else
                {
                    HauntedSigilFloatData floatData = floater.AddComponent<HauntedSigilFloatData>();
                    floatData.startHeight = floater.transform.position.y;

                    floatData.lowerLimit = floater.transform.position.y - 0.09f;
                    floatData.upperLimit = floater.transform.position.y + 0.16f;
                    floatData.lowerSlowThreshold = floater.transform.position.y - 0.065f;
                    floatData.upperSlowTheshold = floater.transform.position.y + 0.135f;

                    //Random Start
                    float randomMod = UnityEngine.Random.Range(-0.09f, 0.16f);
                    float startY = floater.transform.position.y + randomMod;
                    floatData.randomlySelectedFloatStart = startY;
                    floater.transform.position += new Vector3(0, randomMod, 0);
                    floatData.currentHeight = floater.transform.position.y;

                    floatData.MovingUp = UnityEngine.Random.value <= 0.5f;
                }
            }
            if (Tools.GetActAsInt() == 1)
            {
                if (particleTimer >= 0)
                {
                    particleTimer -= Time.deltaTime;
                }
                else
                {
                    GameObject particles = Instantiate(SpecialNodeHandler.Instance.cardMerger.transformParticles.gameObject, slot.transform.position, Quaternion.Euler(-90, 0, 0));
                    ParticleSystem.ShapeModule shape = particles.GetComponent<ParticleSystem>().shape;
                    ParticleSystem.VelocityOverLifetimeModule vel = particles.GetComponent<ParticleSystem>().velocityOverLifetime;
                    vel.enabled = false;
                    particles.SetActive(false);
                    particles.SetActive(true);
                    particleTimer = UnityEngine.Random.Range(0.1f, 0.4f);
                }
            }
        }


        public float particleTimer = 0.1f;
        public bool setUp;
        public CardSlot slot;
        public List<GameObject> hauntSigils;
        public List<Ability> abilities;
    }
    public class HauntedSigilFloatData : MonoBehaviour
    {
        public float startHeight;
        public float randomlySelectedFloatStart;
        public float currentHeight;
        public bool MovingUp;

        public float upperLimit;
        public float lowerLimit;
        public float lowerSlowThreshold;
        public float upperSlowTheshold;
    }
}
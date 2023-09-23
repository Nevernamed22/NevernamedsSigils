using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using DiskCardGame;
using UnityEngine;
using BepInEx.Logging;
using System.Collections;
using Pixelplacement;
using InscryptionAPI.Card;
using DigitalRuby.LightningBolt;
using GBC;
using InscryptionAPI.Resource;

namespace NevernamedsSigils
{
    public static class Tools
    {
        public static GameObject act1holotarget = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon");
        public static GameObject act3holotarget = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/SniperTargetIcon");

        public static int CombinedPower(this List<Ability> abilities)
        {
            int toReturn = 0;
            foreach (Ability inst in abilities) { toReturn += AbilitiesUtil.GetInfo(inst).powerLevel; }
            return toReturn;
        }
        public static List<Ability> GetModularSigilsForAct(int act, int minPower, int maxPower)
        {
            List<Ability> all = new List<Ability>();
            switch (act)
            {
                case 1:
                    List<AbilityInfo> newli0 = ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.powerLevel >= minPower && x.powerLevel <= maxPower && x.metaCategories.Contains(AbilityMetaCategory.Part1Modular));
                    foreach (AbilityInfo inf in newli0) { all.Add(inf.ability); }
                    break;
                case 2:
                    List<AbilityInfo> newli = ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.powerLevel >= minPower && x.powerLevel <= maxPower && x.metaCategories.Contains(Plugin.Part2Modular));
                    foreach (AbilityInfo inf in newli) { all.Add(inf.ability); }
                    break;
                case 3:
                    List<AbilityInfo> newli2 = ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.powerLevel >= minPower && x.powerLevel <= maxPower && x.metaCategories.Contains(AbilityMetaCategory.Part3Modular));
                    foreach (AbilityInfo inf in newli2) { all.Add(inf.ability); }
                    break;
                case 4:
                    List<AbilityInfo> newli3 = ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.powerLevel >= minPower && x.powerLevel <= maxPower && (x.metaCategories.Contains(Plugin.GrimoraModChair1) || x.metaCategories.Contains(Plugin.GrimoraModChair2) || x.metaCategories.Contains(Plugin.GrimoraModChair3)));
                    foreach (AbilityInfo inf in newli3) { all.Add(inf.ability); }
                    break;
            }
            return all;
        }
        public static Ability GetModularSigilForAct(int act, int minpower, int maxpower, List<Ability> forbiddenAbilities)
        {
            List<Ability> abilities = GetModularSigilsForAct(act, minpower, maxpower);
            abilities.RemoveAll((Ability x) => forbiddenAbilities != null && forbiddenAbilities.Contains(x));
            if (abilities.Count > 0)
            {
                return abilities[SeededRandom.Range(0, abilities.Count, GetRandomSeed())];
            }
            else return Ability.Sharp;
        }
        public static Ability GetModularSigilForActAndCard(int act, int minpower, int maxpower, PlayableCard card, List<Ability> forbiddenAbilities)
        {
            List<Ability> abilities = GetModularSigilsForAct(act, minpower, maxpower);
            abilities.RemoveAll((Ability x) => card.HasAbility(x) || (forbiddenAbilities != null && forbiddenAbilities.Contains(x)));
            if (abilities.Count > 0)
            {
                return abilities[SeededRandom.Range(0, abilities.Count, GetRandomSeed())];
            }
            else return Ability.Sharp;
        }
        public static Ability GetModularSigilForActAndCard(int act, int minpower, int maxpower, CardInfo card, List<Ability> forbiddenAbilities)
        {
            List<Ability> abilities = GetModularSigilsForAct(act, minpower, maxpower);
            abilities.RemoveAll((Ability x) => card.HasAbility(x) || (forbiddenAbilities != null && forbiddenAbilities.Contains(x)));
            if (abilities.Count > 0)
            {
                return abilities[SeededRandom.Range(0, abilities.Count, GetRandomSeed())];
            }
            else return Ability.Sharp;
        }
        private static void AddResources()
        {
            /*    ResourceBankManager.Add(Plugin.PluginGuid, new ResourceBank.Resource()
                {
                    path = $"Art/Cards/Decals/thing",
                    asset = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/cleaving.png"),
                });*/
        }
        public static bool IsEven(this int num)
        {
            return num % 2 == 0;
        }
        public static GameObject DeathcardCorner = ResourceBank.Get<GameObject>("Prefabs/GBCCardBattle/PixelDeathcardOverlay");
        public static IEnumerator GenerateAndGiveCompletelyRandomAct2Deathcard(int statPoints)
        {
            yield break;
        }
        public static IEnumerator GenerateandGiveAct2Deathcard(CardInfo abilities, CardInfo stats, CardInfo cost)
        {
            CardInfo cardByName = CardLoader.GetCardByName("!DEATHCARD_PIXEL_BASE");
            cardByName.pixelPortrait = abilities.pixelPortrait;
            cardByName.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelDeathcardBackground };
            cardByName.mods.Add(new CardModificationInfo
            {
                attackAdjustment = stats.Attack,
                healthAdjustment = stats.Health,
                abilities = new List<Ability>(abilities.Abilities),
                bloodCostAdjustment = cost.BloodCost,
                bonesCostAdjustment = cost.BonesCost,
                energyCostAdjustment = cost.EnergyCost,
                addGemCost = cost.gemsCost
            });
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(cardByName, null, 0.25f, delegate (PlayableCard c)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(DeathcardCorner);
                PixelCardDisplayer componentInChildren = c.GetComponentInChildren<PixelCardDisplayer>();
                gameObject.transform.parent = componentInChildren.CardElements.transform;
                gameObject.transform.localPosition = Vector2.zero;
                gameObject.transform.Find("Portrait").GetComponent<SpriteRenderer>().sprite = cost.pixelPortrait;
            });
            yield break;
        }
        public static PlayableCard GenerateandSpawnAct2Deathcard(CardInfo abilities, CardInfo stats, CardInfo cost)
        {
            CardInfo cardByName = CardLoader.GetCardByName("!DEATHCARD_PIXEL_BASE");
            cardByName.pixelPortrait = abilities.pixelPortrait;
            cardByName.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelDeathcardBackground };
            cardByName.mods.Add(new CardModificationInfo
            {
                attackAdjustment = stats.Attack,
                healthAdjustment = stats.Health,
                abilities = new List<Ability>(abilities.Abilities),
                bloodCostAdjustment = cost.BloodCost,
                bonesCostAdjustment = cost.BonesCost,
                energyCostAdjustment = cost.EnergyCost,
                addGemCost = cost.gemsCost
            });
            PlayableCard playableCard = CardSpawner.SpawnPlayableCard(cardByName);
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(DeathcardCorner);
            PixelCardDisplayer componentInChildren = playableCard.GetComponentInChildren<PixelCardDisplayer>();
            gameObject.transform.parent = componentInChildren.CardElements.transform;
            gameObject.transform.localPosition = Vector2.zero;
            gameObject.transform.Find("Portrait").GetComponent<SpriteRenderer>().sprite = cost.pixelPortrait;
            return playableCard;
        }
        public static Texture2D textureFromSprite(this Sprite sprite)
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        public static IEnumerator CardZapCard(PlayableCard originator, PlayableCard target, float duration)
        {
            Singleton<TableVisualEffectsManager>.Instance.ThumpTable(0.3f);
            AudioController.Instance.PlaySound3D("teslacoil_overload", MixerGroup.TableObjectsSFX, originator.transform.position, 1f, 0f, null, null, null, null, false);
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(ResourceBank.Get<GameObject>("Prefabs/Environment/TableEffects/LightningBolt"));
            gameObject.GetComponent<LightningBoltScript>().StartObject = originator.gameObject;
            gameObject.GetComponent<LightningBoltScript>().EndObject = target.gameObject;
            yield return new WaitForSeconds(duration);
            UnityEngine.Object.Destroy(gameObject, 0.25f);
            gameObject = null;
            yield break;
        }
        public static bool isEven(this int number)
        {
            if (number % 2 == 0) return true;
            else return false;
        }
        public static int GetNumberOfSigilOnBoard(bool playerSide, Ability sigil)
        {
            return Singleton<BoardManager>.Instance.GetSlots(playerSide).FindAll((CardSlot x) => x != null && x.Card != null && x.Card.HasAbility(sigil)).Count;
        }
        public static bool CardHasSigilInList(this PlayableCard card, List<Ability> sigils)
        {
            bool toReturn = false;
            foreach (Ability sigil in sigils)
            {
                if (card.HasAbility(sigil)) toReturn = true;
            }
            return toReturn;
        }
        public static List<CardInfo> CloneAllCardsInDeck()
        {
            List<CardInfo> cards = new List<CardInfo>();
            foreach (CardInfo inf in Singleton<CardDrawPiles>.Instance.Deck.cards)
            {
                cards.Add(TrueClone(inf));
            }
            return cards;
        }
        public static CardModificationInfo CondenseMods(this PlayableCard card, List<Ability> excludedabilities = null, bool sigilsOnly = false)
        {
            CardModificationInfo mod = new CardModificationInfo();
            foreach (CardModificationInfo mod1 in card.temporaryMods)
            {
                if (excludedabilities != null) mod.abilities.AddRange(mod1.abilities.FindAll((x) => !excludedabilities.Contains(x)));
                else mod.abilities.AddRange(mod1.abilities);
                if (!sigilsOnly)
                {
                    mod.attackAdjustment += mod1.attackAdjustment;
                    mod.healthAdjustment += mod1.healthAdjustment;
                }
                if (mod1.gemify) mod.gemify = true;
            }
            foreach (CardModificationInfo mod2 in card.Info.mods)
            {
                if (excludedabilities != null) mod.abilities.AddRange(mod2.abilities.FindAll((x) => !excludedabilities.Contains(x)));
                if (!sigilsOnly)
                {
                    mod.attackAdjustment += mod2.attackAdjustment;
                    mod.healthAdjustment += mod2.healthAdjustment;
                }
                if (mod2.gemify) mod.gemify = true;
            }
            if (mod.abilities.Count > 0) mod.fromCardMerge = true;
            return mod;
        }
        public static bool CardIsInSideDeck(this CardInfo card)
        {
            if (CardDrawPiles3D.Instance.SideDeck.Cards.FindAll((x) => x != null && x.name == card.name).Count > 0) return true;
            else return false;
        }
        public static void FallOntoTable(GameObject obj, bool reverse, float time = 0.3f)
        {
            if (!reverse)
            {
                Vector3 startval = obj.transform.position;
                obj.transform.position += new Vector3(0, 5, 0);
                Tween.Position(obj.transform, startval, time, 0f, Tween.EaseBounce);
            }
            else
            {
                Vector3 targetval = obj.transform.position + new Vector3(0, 5, 0);
                Tween.Position(obj.transform, targetval, time, 0f, Tween.EaseIn);
            }
        }

        public static CardInfo TrueClone(CardInfo original)
        {
            CardInfo clone = CardLoader.Clone(original);
            clone.abilities = original.abilities;
            clone.baseAttack = original.baseAttack;
            clone.baseHealth = original.baseHealth;
            clone.mods.AddRange(original.mods);
            return clone;
        }
        public static void LogComponents(GameObject obj)
        {
            foreach (Component component in obj.GetComponentsInChildren<Component>())
            {
                Debug.Log(component.GetType().ToString());
            }
        }
        public static Sprite ConvertTexToSprite(Texture2D tex, Vector2? pivot = null)
        {
            Vector2 actualPivot = new Vector2(0.5f, 0.5f);
            if (pivot != null)
            {
                actualPivot = new Vector2(pivot.Value.x, pivot.Value.y);
            }
            tex.filterMode = FilterMode.Point;
            Sprite texSprite1 = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), actualPivot, 100.0f);
            return texSprite1;
        }
        public static bool IsFree(this CardInfo card)
        {
            if (card.bonesCost == 0 && card.BloodCost == 0 && card.energyCost == 0 && card.gemsCost.Count == 0) { return true; }
            else return false;
        }

        public static bool MeetsDependantRequirements(this CardInfo card)
        {
            bool meetsRequirements = true;
            if (card.HasAbility(Ability.GemDependant) && ResourcesManager.Instance.gems.Count <= 0) meetsRequirements = false;
            if (card.HasAbility(EnergyDependent.ability) && ResourcesManager.Instance.PlayerEnergy <= 0) meetsRequirements = false;
            return meetsRequirements;
        }
        public static void ClearBoard(List<CardSlot> exemptions = null, bool eraseInsteadOfKill = false, bool glitchout = false)
        {
            foreach (CardSlot cardSlot in Singleton<BoardManager>.Instance.AllSlots)
            {
                if (exemptions == null || exemptions.Count <= 0 || !exemptions.Contains(cardSlot))
                {
                    if (cardSlot.Card != null)
                    {
                        PlayableCard card = cardSlot.Card;
                        if (!card.Dead)
                        {
                            if (eraseInsteadOfKill)
                            {
                                card.UnassignFromSlot();
                                if (glitchout)
                                {
                                    GlitchOutAssetEffect.GlitchModel(card.transform, true, true);

                                }
                                else
                                {
                                    UnityEngine.Object.Destroy(card.gameObject);
                                }
                            }
                            else
                            {
                                card.Die(false);
                            }
                        }
                    }
                }
            }
        }
        public static Sprite GenerateAct2Portrait(Texture2D tex)
        {
            tex.filterMode = FilterMode.Point;
            Sprite texSprite1 = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            return texSprite1;
        }
        public static IEnumerator ForceCardAttackOutsideOfCombat(this PlayableCard attackercard)
        {
            int damageToAddToScales = 0;
            List<CardSlot> slots = attackercard.GetOpposingSlots();
            foreach (CardSlot slot in slots)
            {
                if (attackercard.CanAttackDirectly(slot)) damageToAddToScales = attackercard.Attack;
                yield return Singleton<TurnManager>.Instance.CombatPhaseManager.SlotAttackSlot(attackercard.Slot, slot, 0f);
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(0.4f);
            yield return Singleton<TurnManager>.Instance.CombatPhaseManager.VisualizeDamageMovingToScales(true);
            int excessDamage = 0;
            excessDamage = Singleton<LifeManager>.Instance.Balance + damageToAddToScales - 5;
            excessDamage = Mathf.Max(0, excessDamage);

            int damage = damageToAddToScales - excessDamage;

            yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, false, 0f, null, 0f, true);

            if (excessDamage > 0 && Singleton<TurnManager>.Instance.Opponent.NumLives == 1 && Singleton<TurnManager>.Instance.Opponent.GiveCurrencyOnDefeat)
            {
                yield return Singleton<TurnManager>.Instance.Opponent.TryRevokeSurrender();
                RunState.Run.currency += excessDamage;
                yield return Singleton<TurnManager>.Instance.CombatPhaseManager.VisualizeExcessLethalDamage(excessDamage, Singleton<TurnManager>.Instance.SpecialSequencer);
            }
            yield break;
        }
        public static GameObject Particle;
        public static void SpawnParticlesOnCard(this PlayableCard target, Texture2D tex, float yOffset = 0)
        {
            if (target.Anim && target.Anim is PaperCardAnimationController)
            {
                PaperCardAnimationController anim = target.Anim as PaperCardAnimationController;
                if (anim.deathParticles)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(anim.deathParticles);
                    ParticleSystem particle = gameObject.GetComponent<ParticleSystem>();
                    if (particle)
                    {
                        ParticleSystem.MainModule mainMod = particle.main;
                        particle.startColor = Color.white;
                        particle.GetComponent<ParticleSystemRenderer>().material = new Material(particle.GetComponent<ParticleSystemRenderer>().material) { mainTexture = tex };
                        mainMod.startColor = new ParticleSystem.MinMaxGradient(Color.white);
                        gameObject.SetActive(true);
                        gameObject.transform.SetParent(anim.transform);
                        gameObject.transform.position = anim.deathParticles.transform.position;
                        gameObject.transform.localScale = anim.deathParticles.transform.localScale;
                        gameObject.transform.rotation = anim.deathParticles.transform.rotation;

                        particle.transform.position = new Vector3(particle.transform.position.x, particle.transform.position.y + yOffset, particle.transform.position.z);

                        UnityEngine.Object.Destroy(gameObject, 6f);
                    }
                }
            }
            else
            {
                if (Particle == null)
                {
                    Particle = Plugin.bundle.LoadAsset<GameObject>("MiscBurningParticles");
                }
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Particle);
                ParticleSystem particle = gameObject.GetComponent<ParticleSystem>();
                if (particle)
                {
                    ParticleSystem.MainModule mainMod = particle.main;
                    particle.startColor = Color.white;
                    particle.GetComponent<ParticleSystemRenderer>().material = new Material(particle.GetComponent<ParticleSystemRenderer>().material) { mainTexture = tex };
                    //particle.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissiveColor", new Color(255, 224, 181) * 2);
                    mainMod.startColor = new ParticleSystem.MinMaxGradient(Color.white);
                    gameObject.SetActive(true);
                    gameObject.transform.position = target.transform.position;
                    gameObject.transform.SetParent(target.transform);
                    gameObject.transform.localScale = new Vector3(0.165f, 0.165f, 0.165f);
                    gameObject.transform.rotation = target.transform.rotation;
                    particle.transform.position = new Vector3(particle.transform.position.x, particle.transform.position.y + yOffset, particle.transform.position.z);

                    UnityEngine.Object.Destroy(gameObject, 6f);
                }
            }
        }
        public static void TemporarilyRemoveAbilityFromCard(this PlayableCard card, Ability toRemove)
        {
            CardModificationInfo removeAbility = new CardModificationInfo();
            removeAbility.negateAbilities.Add(toRemove);
            card.AddTemporaryMod(removeAbility);
            card.Status.hiddenAbilities.Add(toRemove);
            card.RenderCard();
        }
        public static CardInfo GetRandomCardOfTempleAndQuality(CardTemple temple, int act, bool isRare, Tribe requiredTribe = Tribe.None, bool checkDependant = false, List<string> excludedCards = null, int seedIncrement = 0)
        {
            List<CardMetaCategory> allRequired = new List<CardMetaCategory>() { };
            List<CardMetaCategory> oneRequired = new List<CardMetaCategory>() { };
            if (act > 0 && act < 5)
            {
                switch (temple)
                {
                    case CardTemple.Nature:
                        if (act == 1)
                        {
                            if (isRare) allRequired.Add(CardMetaCategory.Rare);
                            else { oneRequired.Add(CardMetaCategory.ChoiceNode); oneRequired.Add(CardMetaCategory.TraderOffer); }
                        }
                        else if (act == 2) oneRequired.Add(CardMetaCategory.GBCPack);
                        else Debug.LogWarning($"Tried to get a card of the {temple} temple in act {act} which is not a valid act for the given temple.");
                        break;
                    case CardTemple.Tech:
                        if (act == 2) oneRequired.Add(CardMetaCategory.GBCPack);
                        else if (act == 3)
                        {
                            if (isRare)
                            {
                                allRequired.Add(CardMetaCategory.Rare);
                            }
                            oneRequired.AddRange(new List<CardMetaCategory>() {
                                CardMetaCategory.ChoiceNode,
                                CardMetaCategory.Part3Random,
                                Plugin.P03KayceesBastionRegion,
                                Plugin.P03KayceesNatureRegion,
                                Plugin.P03KayceesNeutralRegion,
                                Plugin.P03KayceesUndeadRegion,
                                Plugin.P03KayceesWizardRegion,
                            });
                        }
                        else Debug.LogWarning($"Tried to get a card of the {temple} temple in act {act} which is not a valid act for the given temple.");
                        break;
                    case CardTemple.Undead:
                        if (act == 2) oneRequired.Add(CardMetaCategory.GBCPack);
                        else if (act == 4) oneRequired.Add(Plugin.GrimoraChoiceNode);
                        else Debug.LogWarning($"Tried to get a card of the {temple} temple in act {act} which is not a valid act for the given temple.");
                        break;
                    case CardTemple.Wizard:
                        if (act == 2) oneRequired.Add(CardMetaCategory.GBCPack);
                        else Debug.LogWarning($"Tried to get a card of the {temple} temple in act {act} which is not a valid act for the given temple.");
                        break;
                }
            }
            else Debug.LogWarning($"Tried to get a card of act {act} which is not valid.");


            if (act != 1) requiredTribe = Tribe.None;

            List<CardInfo> cards = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) =>
            x.temple == temple &&
            ((checkDependant && x.MeetsDependantRequirements()) || !checkDependant) &&
            !x.specialAbilities.Contains(SpecialTriggeredAbility.RandomCard) &&
            ConceptProgressionTree.Tree.CardUnlocked(x, false) &&
            !x.traits.Contains(Trait.Terrain) &&
            (x.HasRareTagOrBackground() == isRare) &&
            ((requiredTribe == Tribe.None) || x.tribes.Contains(requiredTribe) &&
            (excludedCards == null || !excludedCards.Contains(x.name))));

            if (allRequired != null && allRequired.Count > 0)
            {
                foreach (CardMetaCategory meta1 in allRequired)
                {
                    cards.RemoveAll(x => !x.metaCategories.Contains(meta1));
                }
            }
            List<CardInfo> finality = new List<CardInfo>();
            foreach (CardInfo inf in cards)
            {
                if ((oneRequired == null || oneRequired.Count == 0) || inf.HasAnyOfCardMetaCategories(oneRequired.ToArray())) { finality.Add(inf); }
            }

            if (finality.Count > 0) return SeededRandomElement(finality, GetRandomSeed() + seedIncrement);
            else return null;
        }
        public static int GetActAsInt()
        {
            if (SaveManager.SaveFile.IsPart1) return 1;
            if (SaveManager.SaveFile.IsPart2) return 2;
            if (SaveManager.SaveFile.IsPart3) return 3;
            if (SaveManager.SaveFile.IsGrimora) return 4;
            if (SaveManager.SaveFile.IsMagnificus) return 5;
            else return 0;
        }
        public static bool HasRareTagOrBackground(this CardInfo card)
        {
            if (card.metaCategories.Contains(CardMetaCategory.Rare) || card.appearanceBehaviour.Contains(CardAppearanceBehaviour.Appearance.RareCardBackground)) return true;
            else return false;
        }
        public static CardInfo GetRandomChoosableCardOfTribesWithExceptions(int randomSeed, List<Tribe> tribes, List<CardInfo> exceptions)
        {
            List<CardInfo> list = CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature).FindAll((CardInfo x) => x.IsOfAnyTribe(tribes) && !exceptions.Exists((CardInfo y) => y.name == x.name));
            if (list.Count == 0)
            {
                return CardLoader.GetCardByName("Amalgam");
            }
            else
            {
                return CardLoader.Clone(list[SeededRandom.Range(0, list.Count, randomSeed)]);
            }
        }
        public static bool IsOfAllTribes(this CardInfo inf, List<Tribe> tribes)
        {
            if (tribes != null && tribes.Count > 0)
            {
                foreach (Tribe t in tribes)
                {
                    if (!inf.tribes.Contains(t))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsOfAnyTribe(this CardInfo inf, List<Tribe> tribes)
        {
            if (tribes != null)
            {
                foreach (Tribe t in tribes)
                {
                    if (inf.tribes.Contains(t))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool HasTrait(this PlayableCard card, Trait type)
        {
            if (card != null && card.Info != null && card.Info.traits != null)
            {
                if (card.Info.traits.Count > 0 && card.Info.traits.Contains(type))
                {
                    return true;
                }
            }
            return false;
        }
        public static List<Ability> GetAllAbilities(this PlayableCard c)
        {
            List<Ability> ab = new List<Ability>();
            if (c != null)
            {
                if (c.Info != null && c.Info.Abilities != null)
                {
                    foreach (Ability a in c.Info.Abilities)
                    {
                        if (c.temporaryMods != null)
                        {
                            if (!c.temporaryMods.Exists((CardModificationInfo mo) => mo.negateAbilities.Contains(a)))
                            {
                                ab.Add(a);
                            }
                        }
                        else
                        {
                            ab.Add(a);
                        }
                    }
                }
                if (c.temporaryMods != null)
                {
                    foreach (CardModificationInfo i in c.temporaryMods)
                    {
                        if (i.abilities != null)
                        {
                            foreach (Ability a in i.abilities)
                            {
                                if (!c.temporaryMods.Exists((CardModificationInfo mo) => mo.negateAbilities.Contains(a)))
                                {
                                    ab.Add(a);
                                }
                            }
                        }
                    }
                }
            }
            return ab;
        }
        public static PlayableCard GetStrongestCardOnBoard(bool playerSide, bool weakest = false, Ability sigilToIgnore = Ability.None)
        {
            PlayableCard strongest = null;
            List<CardSlot> viableslots = new List<CardSlot>();
            if (!playerSide) viableslots = Singleton<BoardManager>.Instance.opponentSlots;
            else viableslots = Singleton<BoardManager>.Instance.playerSlots;
            if (sigilToIgnore != Ability.None) viableslots.RemoveAll(x => x.Card != null && x.Card.HasAbility(sigilToIgnore));
            foreach (CardSlot slot in viableslots)
            {
                if (slot && slot.Card)
                {
                    if (weakest)
                    {
                        if (strongest == null || strongest.PowerLevel > slot.Card.PowerLevel)
                        {
                            strongest = slot.Card;
                        }
                    }
                    else
                    {
                        if (strongest == null || strongest.PowerLevel < slot.Card.PowerLevel)
                        {
                            strongest = slot.Card;
                        }
                    }
                }
            }
            return strongest;
        }
        public static Texture2D LoadTex(string path)
        {
            byte[] imgBytes = ExtractEmbeddedResource(path);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            tex.filterMode = FilterMode.Point;
            return tex;
        }
        public static byte[] ExtractEmbeddedResource(String filePath)
        {
            filePath = filePath.Replace("/", ".");
            filePath = filePath.Replace("\\", ".");
            var baseAssembly = Assembly.GetCallingAssembly();
            using (Stream resFilestream = baseAssembly.GetManifestResourceStream(filePath))
            {
                if (resFilestream == null)
                {

                    return null;
                }
                byte[] ba = new byte[resFilestream.Length];
                resFilestream.Read(ba, 0, ba.Length);
                return ba;
            }
        }
        public static T RandomElement<T>(List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public static T SeededRandomElement<T>(List<T> list, int seed)
        {
            return list[SeededRandom.Range(0, list.Count, seed)];
        }
        public static int GetRandomSeed()
        {
            return SaveManager.SaveFile.GetCurrentRandomSeed() + Singleton<GlobalTriggerHandler>.Instance.NumTriggersThisBattle;
        }
    }
}

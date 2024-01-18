using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class CustomFriendPortrait : DynamicCardPortrait
    {
        [SerializeField]
        public SpriteRenderer avatarRenderer;
        public override void ApplyCardInfo(CardInfo card)
        {
            //Debug.Log($"ApplyCardInfo Ran ({base.gameObject.name})");
            GameObject portrait = base.transform.Find("portrait").gameObject;
            avatarRenderer = portrait.GetComponent<SpriteRenderer>();
            if (Tools.GetActAsInt() == 3)
            {
                portrait.transform.localPosition = new Vector3(0f, 0.6395f, 0f);
                portrait.transform.localScale = new Vector3(1.75f, 2.4434f, 0.8852f);
            }
            //if (avatarRenderer != null) { Debug.Log("Avatar Renderer was NOT null"); }
            avatarRenderer.sprite = card.alternatePortrait;
        }
    }
    public class FriendFinder : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Friend Finder", "When [creature] is played, a random friend is created in the slots to its left and right.",
                      typeof(FriendFinder),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/friendfinder.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/friendfinder_pixel.png"));



            GameObject friendlyFace = Plugin.bundle.LoadAsset<GameObject>("CustomFriendPortrait");
            friendlyFace.AddComponent<CustomFriendPortrait>();
            friendlyFace.layer = LayerMask.NameToLayer("CardOffscreen");
            friendlyFace.transform.Find("portrait").gameObject.layer = LayerMask.NameToLayer("CardOffscreen");
            SigilSetupUtility.NewCard("SigilNevernamed Act1Friend", "", 0, 0,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() {  },
               traits: new List<Trait>() { },
               animatedPortrait: friendlyFace,
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.DynamicPortrait,
                  CustomAppearances.TechPaperCardBackground });


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
        public override bool RespondsToResolveOnBoard()
        {
            return  true;
        }
        public CardInfo GetRandomFriend()
        {
            if (FriendCardCreator.friends == null || FriendCardCreator.friends.Count <= FriendCardCreator.friendIndex) { FriendCardCreator.Initialize(30); }
            CardInfo friend = NextFriendToCard(8);
            friend.appearanceBehaviour.Add(CustomAppearances.TechPaperCardBackground);
            return friend;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            yield return base.PreSuccessfulTriggerSequence();
            if (toLeftValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toLeft);
            }
            if (toRightValid)
            {
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toRight);
            }
            if (toLeftValid || toRightValid)
            {
                yield return base.LearnAbility(0f);
            }
            yield break;
        }
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {
            CardInfo cardByName = GetRandomFriend();
            this.ModifySpawnedCard(cardByName);
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardByName, slot, 0.15f, true);
            yield break;
        }
        private void ModifySpawnedCard(CardInfo card)
        {
            List<Ability> abilities = base.Card.Info.Abilities;
            foreach (CardModificationInfo cardModificationInfo in base.Card.TemporaryMods)
            {
                abilities.AddRange(cardModificationInfo.abilities);
            }
            abilities.RemoveAll((Ability x) => x == this.Ability);
            if (abilities.Count > 4)
            {
                abilities.RemoveRange(3, abilities.Count - 4);
            }
            CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
            cardModificationInfo2.fromCardMerge = true;
            cardModificationInfo2.abilities = abilities;
            card.Mods.Add(cardModificationInfo2);
        }
        public static CardInfo NextFriendToCard(int statPoints)
        {
            FriendCardCreator.friendIndex++;
            if (FriendCardCreator.friends.Count > FriendCardCreator.friendIndex)
            {
                return FriendToCard(FriendCardCreator.friends[FriendCardCreator.friendIndex], statPoints);
            }
            return null;
        }

        private static CardInfo FriendToCard(OnlineFriend friend, int statPoints)
        {
            CardInfo cardByName = CardLoader.GetCardByName("SigilNevernamed Act1Friend");
            cardByName.alternatePortrait = friend.avatar;
            CardModificationInfo cardModificationInfo = Tools.GetActAsInt() == 1 ? CardInfoGenerator.CreateRandomizedAbilitiesStatsMod(ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.metaCategories.Contains(AbilityMetaCategory.Part1Modular)), statPoints, 1, 1) : CardInfoGenerator.CreateRandomizedAbilitiesStatsMod(ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.metaCategories.Contains(AbilityMetaCategory.Part3Modular)), statPoints, 1, 1);
            cardModificationInfo.nameReplacement = friend.name;
            if (Tools.GetActAsInt() == 1)
            {
                if (UnityEngine.Random.value <= 0.5f) { cardModificationInfo.bonesCostAdjustment = statPoints / 2; }
                else { cardModificationInfo.bloodCostAdjustment = 1; };
            }
            else { cardModificationInfo.energyCostAdjustment = statPoints / 2; }
            cardByName.Mods.Add(cardModificationInfo);
            return cardByName;
        }
    }
}
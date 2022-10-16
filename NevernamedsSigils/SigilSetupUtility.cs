using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    class SigilSetupUtility
    {
        public static AbilityInfo MakeNewSigil(string name, string description, Type behaviour, List<AbilityMetaCategory> categories = null, int powerLevel = 0, bool stackable = false, bool opponentUsable = false, Texture tex = null, Texture2D pixelTex = null,
            bool isConduit = false, bool isActivated = false)
        {
            AbilityInfo info = AbilityManager.New("nevernamed.inscryption.sigils",
                name, description,
               behaviour, tex != null ? tex : Tools.LoadTex("NevernamedsSigils/Resources/Sigils/placeholdersigil.png"));
            info.powerLevel = powerLevel;
            info.canStack = stackable;
            info.opponentUsable = opponentUsable;
            if (pixelTex != null) info.pixelIcon = Tools.ConvertTexToSprite(pixelTex);
            info.metaCategories = categories != null ? categories : new List<AbilityMetaCategory>() { };
            info.conduit = isConduit;
            info.activated = isActivated;
            return info;
        }

        public static StatIconInfo MakeNewStatIcon(string name, string description, Type behaviour, Texture2D tex = null, Texture2D pixelTex = null, bool isForHealth = false, List<AbilityMetaCategory> categories = null)
        {
            StatIconInfo info = StatIconManager.New("nevernamed.inscryption.sigils", name, description, behaviour);

            info.iconGraphic = tex != null ? tex : Tools.LoadTex("NevernamedsSigils/Resources/Sigils/placeholdersigil.png");
            info.pixelIconGraphic = pixelTex != null ? Tools.GenerateAct2Portrait(pixelTex) : null;

            info.appliesToAttack = !isForHealth;
            info.appliesToHealth = isForHealth;

            if (categories != null) info.metaCategories.AddRange(categories);

            return info;
        }

        public static CardInfo NewCard(string internalName, string displayname, int power, int health, List<CardMetaCategory> categories, CardTemple temple, string description,
            int bloodCost = 0, int bonesCost = 0, int energyCost = 0, List<GemType> gemsCost = null, List<Ability> abilities = null, Texture2D defaultTex = null, Texture2D emissionTex = null, Texture2D pixelTex = null, Texture2D altTexture = null, Texture2D altTextureEmission = null,
           Texture2D titleGraphic = null, List<Texture> decals = null, List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = null, List<Tribe> tribes = null, bool preventSignature = false, List<Trait> traits = null,
            string defaultTailCard = null, string defaultEvolutionCard = null, string defaultEvolutionName = null, int defaultEvolutionTurns = 1, string defaultFrozenCard = null, Texture2D tailLostTexture = null,
            List<SpecialTriggeredAbility> specialAbilities = null, SpecialStatIcon variableStat = SpecialStatIcon.None, bool hideStats = false, bool onePerDeck = false, GameObject animatedPortrait = null, bool flipPortraitWhenStrafing = false)
        {
            CardInfo newInfo = CardManager.New("Nevernamed", internalName, displayname, power, health, description);
            newInfo.metaCategories = categories;
            newInfo.temple = temple;
            newInfo.cost = bloodCost;
            newInfo.cardComplexity = CardComplexity.Simple;
            newInfo.bonesCost = bonesCost;
            newInfo.gemsCost = gemsCost != null ? gemsCost : new List<GemType>();
            newInfo.energyCost = energyCost;
            newInfo.abilities = abilities != null ? abilities : new List<Ability>();
            newInfo.tribes = tribes != null ? tribes : new List<Tribe>();
            newInfo.traits = traits != null ? traits : new List<Trait>();

            if (specialAbilities != null && specialAbilities.Count > 0) newInfo.AddSpecialAbilities(specialAbilities.ToArray());

            if (variableStat != SpecialStatIcon.None) newInfo.specialStatIcon = variableStat;


            //Setup Tails, Frozens, and Evolutions         
            if (!string.IsNullOrEmpty(defaultEvolutionName)) newInfo.defaultEvolutionName = defaultEvolutionName;
            if (!string.IsNullOrEmpty(defaultEvolutionCard)) newInfo.SetEvolve(defaultEvolutionCard, defaultEvolutionTurns);
            if (!string.IsNullOrEmpty(defaultFrozenCard)) newInfo.SetIceCube(defaultFrozenCard);
            //if (!string.IsNullOrEmpty(defaultTailCard)) newInfo.SetTail(defaultTailCard);
            if (!string.IsNullOrEmpty(defaultTailCard)) newInfo.tailParams = new TailParams() { tail = CardLoader.GetCardByName(defaultTailCard) };



            //Setup Textures
            if (defaultTex != null) newInfo.SetPortrait(defaultTex);
            if (emissionTex != null) newInfo.SetEmissivePortrait(emissionTex);
            if (pixelTex != null) newInfo.SetPixelPortrait(pixelTex);
            if (altTexture != null) newInfo.SetAltPortrait(altTexture);
            if (altTextureEmission != null) newInfo.SetEmissiveAltPortrait(altTextureEmission);
            if (titleGraphic != null) newInfo.titleGraphic = titleGraphic;
            if (tailLostTexture != null) newInfo.SetLostTailPortrait(tailLostTexture, FilterMode.Point);

            newInfo.hideAttackAndHealth = hideStats;
            newInfo.onePerDeck = onePerDeck;
            newInfo.animatedPortrait = animatedPortrait;
            newInfo.flipPortraitForStrafe = flipPortraitWhenStrafing;

            //Handle Decals
            newInfo.decals = decals != null ? decals : new List<Texture>();

            if (appearanceBehaviour != null) newInfo.AddAppearances(appearanceBehaviour.ToArray());

            CardManager.Add("Nevernamed", newInfo);

            return newInfo;
        }
    }
}

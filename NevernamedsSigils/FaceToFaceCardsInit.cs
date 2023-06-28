using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.Text;

namespace NevernamedsSigils
{
    public static class FaceToFaceCardsInit
    {
        public static void Init()
        {
            //Act 1

            SigilSetupUtility.NewCard("SigilNevernamed ShadowyFigure", "Shadowy Figure", 1, 1,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Ability.RandomAbility },
               traits: new List<Trait>() { },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                   CardAppearanceBehaviour.Appearance.RareCardBackground },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/shadowyfigure.png"),
              emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/shadowyfigure_emission.png"));

            SigilSetupUtility.NewCard("SigilNevernamed Angler", "Angler", 1, 3,
              new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
              abilities: new List<Ability>() { GiftBearerCustom.ability  },
              traits: new List<Trait>() { },
              bonesCost: 3,
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                   CardAppearanceBehaviour.Appearance.RareCardBackground },
              defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/angler.png"),
             emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/angler_emission.png")).SetExtendedProperty("GiftBearerCustomPoolIdentifier", "FishbotFishPool");

            SigilSetupUtility.NewCard("SigilNevernamed Trader", "Trader", 2, 2,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { FairTrade.ability },
               traits: new List<Trait>() { },
               bloodCost: 2,
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                   CardAppearanceBehaviour.Appearance.RareCardBackground },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/trader.png"),
              emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/trader_emission.png"));

            SigilSetupUtility.NewCard("SigilNevernamed Trapper", "Trapper", 3, 3,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Ability.SteelTrap, GraveyardShift.ability },
               traits: new List<Trait>() { },
               bloodCost: 2,
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                   CardAppearanceBehaviour.Appearance.RareCardBackground },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/trapper.png"),
              emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/trapper_emission.png"),
              defaultFrozenCard: "SigilNevernamed Trader");       

            SigilSetupUtility.NewCard("SigilNevernamed FakeFinalBoss", "Leshy", 2, 2,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { BloodArtist.ability },
               traits: new List<Trait>() { },
               bloodCost: 2,
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/leshyplaceholder.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.LeshyCardBackground });

            //Act 2 Scrybes

            SigilSetupUtility.NewCard("SigilNevernamed PixelLeshy", "Leshy", 3, 3,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { BloodArtist.ability },
               traits: new List<Trait>() { },
               bloodCost: 2,
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/leshy_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelRareBackground });

            SigilSetupUtility.NewCard("SigilNevernamed PixelP03", "P03", 3, 3,
               new List<CardMetaCategory> { }, CardTemple.Tech, description: "",
               abilities: new List<Ability>() { Globetrotter.ability, Revolve.ability },
               traits: new List<Trait>() { },
               energyCost: 3,
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/p03_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelRareBackground }).SetExtendedProperty("GlobetrotterLeaveBehind", "Automaton");

            SigilSetupUtility.NewCard("SigilNevernamed PixelMagnificus", "Magnificus", 0, 3,
               new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
               abilities: new List<Ability>() { Bleach.ability },
               traits: new List<Trait>() { Trait.Gem },
               gemsCost: new List<GemType>() { GemType.Green },
               specialAbilities: new List<SpecialTriggeredAbility>() { MagickePower.ability },
               variableStat: MagickePower.specialStatIcon,
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/magnificus_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelRareBackground });

            SigilSetupUtility.NewCard("SigilNevernamed PixelGrimora", "Grimora", 3, 3,
              new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
              abilities: new List<Ability>() { OtherSide.ability, SkullSwarm.ability },
              traits: new List<Trait>() { },
              bonesCost: 6,
              pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/grimora_pixel.png"),
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelRareBackground });

            //Act 2 Ghouls

            SigilSetupUtility.NewCard("SigilNevernamed Sawyer", "Sawyer", 2, 2,
               new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
               abilities: new List<Ability>() { Ability.GuardDog },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/sawyer_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelBoneHeader, CustomAppearances.PixelDogBiteDecal });

            SigilSetupUtility.NewCard("SigilNevernamed Kaycee", "Kaycee", 1, 1,
               new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
               abilities: new List<Ability>() { },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/kaycee_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelBoneHeader, CustomAppearances.PixelSnowflakeDecal });

            CardInfo royalInf = SigilSetupUtility.NewCard("SigilNevernamed RoyalPixel", "Royal", 3, 3,
                 new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
                 abilities: new List<Ability>() { },
                 specialAbilities: new List<SpecialTriggeredAbility>() { InherentCardShedder.ability },
                 traits: new List<Trait>() { },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/kaycee_pixel.png"),
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                   CustomAppearances.PixelShipDecal });
            royalInf.SetExtendedProperty("InherentCardShedderLeaveBehind", "Skeleton");

            //Act 2 Pupils

            SigilSetupUtility.NewCard("SigilNevernamed TrainingDummy", "Training Dummy", 0, 2,
               new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
               abilities: new List<Ability>() { TemptingTarget.ability },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/trainingdummy_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelSparkleHeader });

            SigilSetupUtility.NewCard("SigilNevernamed Goobert", "Goobert", 1, 1,
               new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
               abilities: new List<Ability>() { },
                 specialAbilities: new List<SpecialTriggeredAbility>() { InherentGooey.ability },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/goobert_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelGoobertBackground });

            SigilSetupUtility.NewCard("SigilNevernamed PikeMage", "Pike Mage", 2, 2,
               new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
               abilities: new List<Ability>() { },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/pikemage_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelPikeMageBackground });

            SigilSetupUtility.NewCard("SigilNevernamed LonelyWizard", "Lonely Wizard", 2, 1,
               new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
               abilities: new List<Ability>() { Ability.MoveBeside },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/lonelywiz_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelLonelyWizBackground });

            //Act 2 Bots

            SigilSetupUtility.NewCard("SigilNevernamed Inspector", "Inspector", 1, 1,
               new List<CardMetaCategory> { }, CardTemple.Tech, description: "",
               abilities: new List<Ability>() { },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/inspector_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelTechHeader });

            SigilSetupUtility.NewCard("SigilNevernamed Melter", "Melter", 2, 2,
               new List<CardMetaCategory> { }, CardTemple.Tech, description: "",
               abilities: new List<Ability>() { Ability.IceCube },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/melter_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelTechHeader, CustomAppearances.PixelCharredBackground }, defaultFrozenCard: "MeatBot");

            SigilSetupUtility.NewCard("SigilNevernamed Dredger", "Dredger", 3, 3,
               new List<CardMetaCategory> { }, CardTemple.Tech, description: "",
               abilities: new List<Ability>() { Ability.Submerge },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/dredger_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelTechHeader, CustomAppearances.PixelWaterDamageDecal });

            //Act 2 Pioneers

            SigilSetupUtility.NewCard("SigilNevernamed Act2Prospector", "Prospector", 1, 1,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Ability.GuardDog },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/prospector_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelScrappyHeader });

            SigilSetupUtility.NewCard("SigilNevernamed Act2Angler", "Angler", 2, 2,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Ability.Submerge },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/angler_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {
                  CustomAppearances.PixelAnglerBackground });

            SigilSetupUtility.NewCard("SigilNevernamed Act2Trapper", "Trapper", 3, 3,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Ability.SteelTrap },
               traits: new List<Trait>() { },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/FaceToFace/trapper_pixel.png"),
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>()
               {
               });
        }
    }
}

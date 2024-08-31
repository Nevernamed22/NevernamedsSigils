using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public static class Act2ItemCards
    {
        public static void Init()
        {
            SigilSetupUtility.NewCard("SigilNevernamed PixelItemPliers", "Pliers", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Desperate.ability, InstantEffect.ability },
               traits: new List<Trait>() { },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() {  CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/pliers_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemScissors", "Scissors", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { Snip.ability, InstantEffect.ability },
               traits: new List<Trait>() { },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/scissors_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemBleach", "Bleach", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
              abilities: new List<Ability>() { Bleach.ability, InstantEffect.ability },
              traits: new List<Trait>() { },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
              pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/bleachitem_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemSkinningKnife", "Skinning Knife", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                  abilities: new List<Ability>() { Ability.SteelTrap, InstantEffect.ability },
                  traits: new List<Trait>() { },
                  appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                  pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/skinningknife_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemFishhook", "Fish Hook", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
              abilities: new List<Ability>() { HookLineAndSinker.ability, InstantEffect.ability },
              traits: new List<Trait>() { },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
              pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/fishhook_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemDeadHand", "Dead Hand", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { Ability.DrawNewHand, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/deadhanditem_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemHarpiesBirdlegFan", "Harpie's Birdleg Fan", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { FanTailed.ability, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/harpiesbirdlegfan_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemHoggyBank", "Hoggy Bank", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { Ability.QuadrupleBones, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/hoggybank_pixel.png")).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemExtraBattery", "Extra Battery", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { Supercharge.ability, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/extrabattery_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemShieldGenerator", "Shield Generator", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { Omniguardian.ability, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/shieldgenerator_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemWiseclock", "Wiseclock", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { Revolve.ability, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/wiseclock_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemMrsBombsRemote", "Mrs Bombs Remote", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                 abilities: new List<Ability>() { Ability.BombSpawner, InstantEffect.ability },
                 traits: new List<Trait>() { },
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats }, defaultFrozenCard: "Bombbot",
                 specialAbilities: new List<SpecialTriggeredAbility>() { InherentGraveyardShift.ability },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/mrsbombsremote_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemHourglass", "Hourglass", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                     abilities: new List<Ability>() { TimeTravel.ability, InstantEffect.ability },
                     traits: new List<Trait>() { },
                     appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                     pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/hourglassitem_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

            SigilSetupUtility.NewCard("SigilNevernamed PixelItemMagpiesGlass", "Magpies Glass", 0, 0, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                    abilities: new List<Ability>() { Ability.Tutor, InstantEffect.ability },
                    traits: new List<Trait>() { },
                    appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelInstantEffectBackground, CustomAppearances.HiddenStats },
                    pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/PixelItems/magpiesglassitem_pixel.png"), preventBones: true).SetExtendedProperty("Act2TrinketBearerItemCard", "true");

        }
    }
}

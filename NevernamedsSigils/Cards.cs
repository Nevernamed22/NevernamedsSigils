using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public static class Cards
    {
        public static void Init()
        {
            SigilSetupUtility.NewCard("SigilNevernamed WrigglingPereiopod",
                "Wriggling Pereiopod",
                0,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/wrigglingpereiopod.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed WormyTail",
                "Wormy Tail",
                0,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/wormytail.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed Web",
                "Web",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/web.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/web_emission.png"),
                abilities: new List<Ability>() { WebSigil.ability, Remove.ability, Ability.Reach },
                traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout },
                preventBones: true
                );

            SigilSetupUtility.NewCard("SigilNevernamed Guts",
                "Guts",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/guts.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/guts_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/guts_pixel.png"),
                abilities: new List<Ability>() { Ability.Evolve },
                traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout },
                defaultEvolutionCard: "SigilNevernamed FesteringGuts",
                defaultEvolutionTurns: 3
                );

            SigilSetupUtility.NewCard("SigilNevernamed FesteringGuts",
                "Festering Guts",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/festeringguts.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/festeringguts_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/festeringguts_pixel.png"),
                abilities: new List<Ability>() { Doomed.ability },
                traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout }
                );

            SigilSetupUtility.NewCard("SigilNevernamed Act2EmptyVessel",
                "Empty Vessel",
                0,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Tech,
                description: "",
                energyCost: 1,
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/act2emptyves_pixel.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed ClawLeft",
                "Claw",
                1,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/claw_left.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/clawleft_pixel.png"),
                abilities: new List<Ability>() { CrookedStrikeRight.ability, Claw.ability },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { }
                );

            SigilSetupUtility.NewCard("SigilNevernamed ClawRight",
                "Claw",
                1,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/claw_right.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/clawright_pixel.png"),
                abilities: new List<Ability>() { CrookedStrikeLeft.ability, Claw.ability },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { }
                );

            SigilSetupUtility.NewCard("SigilNevernamed CloneGrub",
                "Clone Grub",
                0,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/clonegrub.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/clonegrub_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/clonegrub_pixel.png"),
                abilities: new List<Ability>() { Ability.Evolve }
                );

            SigilSetupUtility.NewCard("SigilNevernamed EnchantedPine",
                "Enchanted Pine",
                0,
                3,
                new List<CardMetaCategory> { },
                CardTemple.Wizard,
                description: "",
                abilities: new List<Ability>() { Ability.Reach, Ability.GemDependant },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/enchantedpine_pixel.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed UnnaturalCreature",
               "Unnatural Creature",
               1,
               1,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/unnaturalcreature.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/unnaturalcreature_emission.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed ShadowedCreature",
               "Shadowed Creature",
               1,
               1,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               abilities: new List<Ability>() { Ability.Brittle },
               description: "",
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/shadowedcreature.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/shadowedcreature_emission.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed Mess",
               "Mess",
               0,
               2,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
                abilities: new List<Ability>() { Ability.DebuffEnemy },
                specialAbilities: new List<SpecialTriggeredAbility>() { PoopDeck.ability },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/mess.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/mess_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed Termite",
               "Termite",
               1,
               2,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bonesCost: 2,
                tribes: new List<Tribe>() { Tribe.Insect },
                abilities: new List<Ability>() { },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/termite.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/termite_pixel.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/termite_emission.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed SwarmedMayfly",
               "Mayfly",
               1,
               1,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bonesCost: 1,
                tribes: new List<Tribe>() { Tribe.Insect },
                abilities: new List<Ability>() { Ability.Flying, Ability.Brittle },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/mayfly.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/mayfly_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/mayfly_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed CopiedProspecter",
               "The Prospector",
               2,
               3,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bloodCost: 2,
                tribes: new List<Tribe>() { },
                abilities: new List<Ability>() { Ability.WhackAMole, GoldRush.ability },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/copiedprospector.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/copiedprospector_emission.png"),
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground }
               );

            SigilSetupUtility.NewCard("SigilNevernamed CopiedRoyal",
               "Royal Dominguez",
               1,
               3,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bonesCost: 5,
                tribes: new List<Tribe>() { },
                abilities: new List<Ability>() { Ability.SkeletonStrafe, FireInTheHole.ability },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/copiedroyal.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/copiedroyal_emission.png"),
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground },
                overrideSkeletonCrewID: "SkeletonPirate"
               );

            SigilSetupUtility.NewCard("SigilNevernamed RemoteController",
               "Remote Controller",
               0,
               2,
               new List<CardMetaCategory> { },
               CardTemple.Tech,
               description: "",
               energyCost: 1,
                tribes: new List<Tribe>() { },
                abilities: new List<Ability>() { },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/remotecontroller.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/remotecontroller_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed SpiritBeast",
               "Spirit Beast",
               1,
               2,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bloodCost: 1,
                tribes: new List<Tribe>() { },
                abilities: new List<Ability>() { },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/spiritbeast.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/spiritbeast_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/spiritbeast_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed Bud",
                "Bud",
                0,
                3,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/bud.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/bud_emission.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed GrimoraBud",
                "Bud",
                0,
                3,
                new List<CardMetaCategory> { },
                CardTemple.Undead,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/grimorabud.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/grimorabud_emission.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed Vomit",
                "Vomit",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                abilities: new List<Ability>() { Ability.PreventAttack, Doomed.ability },
                traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout },
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/vomit.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/vomit_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/vomit_pixel.png")
                );

            SigilSetupUtility.NewCard("SigilNevernamed VomitGrimora",
                "Vomit",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Undead,
                description: "",
                abilities: new List<Ability>() { Ability.PreventAttack, Doomed.ability },
                traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout },
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/vomitgrimora.png")
                );

            CardInfo morefish = SigilSetupUtility.NewCard("SigilNevernamed MoreFish",
                  "More Fish",
                  0,
                  1,
                  new List<CardMetaCategory> { },
                  CardTemple.Nature,
                  bonesCost: 3,
                  description: "The bloated, everpregnant guppy... it always contains more fish...",
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/morefish.png"),
                 emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/morefish_emission.png"),
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.RareCardBackground },
                 abilities: new List<Ability>() { GiftBearerCustom.ability }
                  ).SetExtendedProperty("GiftBearerCustomPoolIdentifier", "FishbotFishPool");
            morefish.SetExtendedProperty("FishbotFishPool", "true");

            SigilSetupUtility.NewCard("SigilNevernamed BadFish",
                  "Bad Fish",
                  0,
                  1,
                  new List<CardMetaCategory> { },
                  CardTemple.Nature,
                  bonesCost: 3,
                  description: "",
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/badfish.png"),
                 emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/badfish_emission.png"),
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.RareCardBackground },
                 abilities: new List<Ability>() { Ability.BuffEnemy, Ability.Brittle }
                  ).SetExtendedProperty("FishbotFishPool", "true");

            SigilSetupUtility.NewCard("SigilNevernamed GoodFish",
                  "Good Fish",
                  2,
                  2,
                  new List<CardMetaCategory> { },
                  CardTemple.Nature,
                  description: "",
                  bonesCost: 6,
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/goodfish.png"),
                 emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/goodfish_emission.png"),
                 appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.RareCardBackground },
                 abilities: new List<Ability>() { Ability.Reach, Ability.Sentry, Ability.Sharp, Ability.Sniper }
                  ).SetExtendedProperty("FishbotFishPool", "true");

            SigilSetupUtility.NewCard("SigilNevernamed WolfPelt",
               "Wolf Pelt", 0, 2, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                abilities: new List<Ability>() { }, traits: new List<Trait>() { Trait.Pelt, Trait.Terrain },
                specialAbilities: new List<SpecialTriggeredAbility>() { Act2SpawnLice.ability },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/wolfpelt_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed Payload",
               "Payload", 0, 1, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                abilities: new List<Ability>() { Endangered.ability, Ability.ExplodeOnDeath }, traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.TerrainBackground, CardAppearanceBehaviour.Appearance.TerrainLayout },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/payload.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/payload_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed Components",
               "Components", 0, 1, new List<CardMetaCategory> { }, CardTemple.Tech, description: "",
                abilities: new List<Ability>() { }, traits: new List<Trait>() { Trait.Terrain },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/components.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed GutsGrimora",
               "Guts", 0, 1, new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
                abilities: new List<Ability>() { Doomed.ability }, traits: new List<Trait>() { },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/gutsgrimora.png")
               ).SetExtendedProperty("CustomDoomedDuration", "4");

            SigilSetupUtility.NewCard("SigilNevernamed RawMeat",
              "Raw Meat", 0, 1, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { }, traits: new List<Trait>() { },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/rawmeat_pixel.png"),
                 preventBones: true
              );

            SigilSetupUtility.NewCard("SigilNevernamed ExcisedPowerCell",
              "Excised Power Cell", 0, 1, new List<CardMetaCategory> { }, CardTemple.Tech, description: "",
               abilities: new List<Ability>() { Ability.GainBattery, Recharge.ability, InstantEffect.ability }, traits: new List<Trait>() { Trait.Terrain },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/excisedpowercell_pixel.png"),
                 preventBones: true
              );

            SigilSetupUtility.NewCard("SigilNevernamed BoneEffigy",
              "Bone Effigy", 0, 1, new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
               abilities: new List<Ability>() { BoneDuke.ability, Unhammerable.ability }, traits: new List<Trait>() { Trait.Terrain },
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                 pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/boneeffigy_pixel.png")
              );

            SigilSetupUtility.NewCard("SigilNevernamed DesecratedMox",
             "Desecrated Mox", 0, 1, new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
              abilities: new List<Ability>() { MysteryMox.ability }, traits: new List<Trait>() { Trait.Terrain, Trait.Gem },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/desecratedmox_pixel.png"),
                preventBones: true
             );

            SigilSetupUtility.NewCard("SigilNevernamed LooseFlesh",
               "Loose Flesh", 0, 1, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                abilities: new List<Ability>() { Ability.Morsel },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/looseflesh.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/looseflesh_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed LooseFleshGrimora",
               "Loose Flesh", 0, 1, new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
                abilities: new List<Ability>() { BoneSpurs.ability, Remove.ability },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/loosefleshgrimora.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed RubyPillar",
             "Ruby Pillar", 0, 4, new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
              abilities: new List<Ability>() { Ability.GainGemOrange, Ability.Reach }, traits: new List<Trait>() { Trait.Terrain, Trait.Gem },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/rubypillar_pixel.png")
             );

            SigilSetupUtility.NewCard("SigilNevernamed EmeraldPillar",
             "Emerald Pillar", 0, 4, new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
              abilities: new List<Ability>() { Ability.GainGemGreen, Ability.Reach }, traits: new List<Trait>() { Trait.Terrain, Trait.Gem },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/emeraldpillar_pixel.png")
             );

            SigilSetupUtility.NewCard("SigilNevernamed SapphirePillar",
             "Sapphire Pillar", 0, 4, new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
              abilities: new List<Ability>() { Ability.GainGemBlue, Ability.Reach }, traits: new List<Trait>() { Trait.Terrain, Trait.Gem },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/sapphirepillar_pixel.png")
             );

            SigilSetupUtility.NewCard("SigilNevernamed Act2GoldNugget",
             "Gold Nugget", 0, 2, new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
              abilities: new List<Ability>() { }, traits: new List<Trait>() { Trait.Terrain },
              appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CustomAppearances.PixelGoldBackground },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/goldnugget_pixel.png")
             );

            SigilSetupUtility.NewCard("SigilNevernamed MagicBomb",
            "Magic Bomb", 0, 1, new List<CardMetaCategory> { }, CardTemple.Wizard, description: "",
             abilities: new List<Ability>() { Ability.ExplodeOnDeath }, traits: new List<Trait>() { Trait.Terrain },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/magicbomb.png"),
               pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/magicbomb_pixel.png")
            );

            SigilSetupUtility.NewCard("SigilNevernamed SapphireSkeleton",
             "Sapphire Skeleton", 1, 1, new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
              abilities: new List<Ability>() { Ability.Brittle, Ability.GainGemBlue },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/sapphireskeleton_pixel.png"));

            SigilSetupUtility.NewCard("SigilNevernamed RubySkeleton",
             "Ruby Skeleton", 1, 1, new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
              abilities: new List<Ability>() { Ability.Brittle, Ability.GainGemOrange },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/rubyskeleton_pixel.png"));

            SigilSetupUtility.NewCard("SigilNevernamed EmeraldSkeleton",
             "Emerald Skeleton", 1, 1, new List<CardMetaCategory> { }, CardTemple.Undead, description: "",
              abilities: new List<Ability>() { Ability.Brittle, Ability.GainGemGreen },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/emeraldskeleton_pixel.png"));

            SigilSetupUtility.NewCard("SigilNevernamed PinnacleMox",
             "Pinnacle Mox", 0, 1, new List<CardMetaCategory> { CardMetaCategory.Rare }, CardTemple.Wizard, description: "",
              abilities: new List<Ability>() { PerfectForm.ability },
              traits: new List<Trait>() { Trait.Terrain, Trait.Gem },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/pinnaclemox_pixel.png"));

            SigilSetupUtility.NewCard("SigilNevernamed Flea",
               "Flea",
               0,
               1,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bonesCost: 2,
                tribes: new List<Tribe>() { Tribe.Insect },
                abilities: new List<Ability>() { Eager.ability, Frail.ability },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/flea.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/flea_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/flea_pixel.png")
               );

            SigilSetupUtility.NewCard("SigilNevernamed Swabber",
               "Swabber", 2, 2, new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
                abilities: new List<Ability>() { },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { },
                  defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/swabber.png")
               );
        }
    }
}

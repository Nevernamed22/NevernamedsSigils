using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;

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
               abilities: new List<Ability>() { Ability.Brittle},
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
               "The Prospecter",
               2,
               3,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               bloodCost: 2,
                tribes: new List<Tribe>() {  },
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
                tribes: new List<Tribe>() {  },
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
        }
    }
}

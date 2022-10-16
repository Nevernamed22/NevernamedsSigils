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
            SigilSetupUtility.NewCard("Nevernamed Web",
                "Web",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/web.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/web_emission.png"),
                abilities: new List<Ability>() { WebSigil.ability, Remove.ability, Ability.Reach },
                traits: new List<Trait>() { Trait.Terrain, NevernamedsTraits.NoBones },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground }
                );

            SigilSetupUtility.NewCard("Nevernamed Guts",
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
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground },
                defaultEvolutionCard: "Nevernamed FesteringGuts",
                defaultEvolutionTurns: 3
                );

            SigilSetupUtility.NewCard("Nevernamed FesteringGuts",
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
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.TerrainBackground }
                );

            SigilSetupUtility.NewCard("Nevernamed Act2EmptyVessel",
                "Empty Vessel",
                0,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Tech,
                description: "",
                energyCost: 1,
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/act2emptyves_pixel.png")
                );

            SigilSetupUtility.NewCard("Nevernamed ClawLeft",
                "Claw",
                2,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/claw_left.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/claw_left_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/clawleft_pixel.png"),
                abilities: new List<Ability>() { CrookedStrikeRight.ability, Claw.ability },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground }
                );

            SigilSetupUtility.NewCard("Nevernamed ClawRight",
                "Claw",
                2,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/claw_right.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/claw_right_emission.png"),
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/clawright_pixel.png"),
                abilities: new List<Ability>() { CrookedStrikeLeft.ability, Claw.ability },
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground }
                );

            SigilSetupUtility.NewCard("Nevernamed CloneGrub",
                "Clone Grub",
                0,
                2,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/clonegrub.png"),
                emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/clonegrub_emission.png"),
                abilities: new List<Ability>() { Ability.Evolve }
                );

            SigilSetupUtility.NewCard("Nevernamed EnchantedPine",
                "Enchanted Pine",
                0,
                3,
                new List<CardMetaCategory> { },
                CardTemple.Wizard,
                description: "",
                abilities: new List<Ability>() { Ability.Reach, Ability.GemDependant },
                pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/enchantedpine_pixel.png")
                );

            SigilSetupUtility.NewCard("Nevernamed EarthwormSegment",
                "Earthworm Segment",
                0,
                1,
                new List<CardMetaCategory> { },
                CardTemple.Nature,
                description: "",
                defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/earthwormsegment.png"),
                appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance> { CardAppearanceBehaviour.Appearance.RareCardBackground }
                );

            SigilSetupUtility.NewCard("Nevernamed Ratling",
               "Ratling",
               1,
               1,
               new List<CardMetaCategory> { },
               CardTemple.Nature,
               description: "",
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/ratling.png"),
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/ratling_emission.png")
               );

            SigilSetupUtility.NewCard("Nevernamed Mess",
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

            SigilSetupUtility.NewCard("Nevernamed Termite",
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
               emissionTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/termite_emission.png")
               );

            SigilSetupUtility.NewCard("Nevernamed SwarmedMayfly",
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
        }
    }
}

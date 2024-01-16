using BepInEx;
using BepInEx.Configuration;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static InscryptionAPI.Card.AbilityManager;

namespace NevernamedsSigils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("nevernamed.inscryption.opponentbones", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("nevernamed.inscryption.graveyardhandler", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "nevernamed.inscryption.sigils";
        private const string PluginName = "NevernamedsSigils";
        private const string PluginVersion = "1.10.0.0";

        public static AssetBundle bundle;
        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");

            bundle = LoadBundle("NevernamedsSigils/Resources/sigilassetbundle");

            Harmony harmony = new Harmony("NevernamedsSigils.harmonypatcher");
            harmony.PatchAll();

            NoBonesDecal = Tools.LoadTex("NevernamedsSigils/Resources/Other/preventbonesdecal.png");

            arachnophobiaMode = base.Config.Bind<bool>("General", "Arachnophobia Mode", false, "Replaces certain icons to resemble webs rather than spiders.");


            NevernamedsTribes.InitTribes();
            CustomAppearances.Init();
            CustomDeathcardPortrait.InitialiseDictionaries();

            Copier.Init();
            Flighty.Init();
            Harbinger.Init();
            GutSpewer.Init();
            ExplodingCorpseCustom.Init();
            Doomed.Init();
            UnfocusedStrike.Init();
            Commander.Init();
            BandageWeaver.Init();
            Endangered.Init();
            ToothPuller.Init();
            TrophyHunter.Init();
            Fearsome.Init();
            Trio.Init();
            Hypermorphic.Init();
            Enraged.Init();
            Medicinal.Init();
            Burning.Init();
            FireResistant.Init();
            Ignition.Init();
            SavageRitual.Init();
            Trapjaw.Init();
            SkinAnimator.Init();
            OrganThief.Init();
            Snapshot.Init();
            EnergyDependent.Init();
            CrookedStrikeLeft.Init();
            CrookedStrikeRight.Init();
            Lonesome.Init();
            DeusHoof.Init();
            Ripper.Init();
            HaunterCustom.Init();
            FrailSacrifice.Init();
            Mockery.Init();
            ImmortalCoil.Init();
            Clawed.Init();
            Claw.Init();
            RunningStrike.Init();
            Abstain.Init();
            Cute.Init();
            WebSigil.Init();
            Trampler.Init();
            SharperQuills.Init();
            ClawingHand.Init();
            Weaver.Init();
            ReturnStrike.Init();
            InspiringSacrifice.Init();
            DeckedOut.Init();
            BoneDependent.Init();
            OtherSide.Init();
            ExceptionalSacrifice.Init();
            TriflingSacrifice.Init();
            Visceral.Init();
            TuckAndRoll.Init();
            BoneSpurs.Init();
            Crossbones.Init();
            HomeRun.Init();
            SquirrelDependent.Init();
            Vampiric.Init();
            Allure.Init();
            Traitor.Init();
            TermiteKing.Init();
            Termatriarch.Init();
            Parthenogenesis.Init();
            Landscaper.Init();
            Diver.Init();
            FanTailed.Init();
            TransformerCustom.Init();
            Twister.Init();
            TwinBond.Init();
            SubaquaticSpines.Init();
            Frail.Init();
            Caustic.Init();
            Unlucky.Init();
            AntGuardian.Init();
            Drop.Init();
            Moxcraft.Init();
            WoodsieLord.Init();
            FringeStrike.Init();
            Mason.Init();
            Dupeglitch.Init();
            Downdraft.Init();
            Recharge.Init();
            VesselShedder.Init();
            GiftBearerCustom.Init();
            KinBearer.Init();
            HartsWithin.Init();
            ArtisticLicense.Init();
            ChaosStrike.Init();
            BloodDependent.Init();
            DeathSnatch.Init();
            AculeateGrip.Init();
            Motivator.Init();
            Paralytic.Init();
            BuzzOff.Init();
            CoastGuard.Init();
            SpidersWeb.Init();
            TripleStrike.Init();
            Bonelust.Init();
            Moxwarp.Init();
            TemptingTarget.Init();
            SwoopingStrike.Init();
            GraveyardShift.Init();
            Incontinent.Init();
            BloodFromStone.Init();
            Tug.Init();
            PointNemo.Init();
            Gooey.Init();
            Immaterial.Init();
            Stalwart.Init();
            GoldRush.Init();
            FireInTheHole.Init();
            Erratic.Init();
            Soak.Init();
            DogGone.Init();
            Snakebite.Init();
            DeerlyDeparted.Init();
            FowlPlay.Init();
            Insectivore.Init();
            Crusher.Init();
            RockEater.Init();
            FatalFlank.Init();
            Eager.Init();
            Lurk.Init();
            LashOut.Init();
            Ravenous.Init();
            EspritDeCorp.Init();
            Sanguine.Init();
            UnbalancedLeadership.Init();
            BoneDuke.Init();
            Sturdy.Init();
            Resilient.Init();
            Mauler.Init();
            FossilRecord.Init();
            CallToArms.Init();
            SplashDamage.Init();
            Phantasmic.Init();
            FilterFeeder.Init();
            WaveringStrike.Init();
            PinnacleStrike.Init();
            OddStrike.Init();
            EvenStrike.Init();
            Valuable.Init();
            Thief.Init();
            CenterStrike.Init();
            Herald.Init();
            Revolve.Init();
            DivisibilityStrike.Init();
            Venator.Init();
            Summoner.Init();
            SharpestQuills.Init();
            SinisterStrike.Init();
            DexterStrike.Init();
            RemoteControlled.Init();
            Pushover.Init();
            Healshield.Init();
            Sentriple.Init();
            Bloodrunner.Init();
            SurgingQuills.Init();
            SplitSentry.Init();
            Snare.Init();
            Bully.Init();
            Legion.Init();
            TrajectileQuills.Init();
            BloodGusher.Init();
            Propagation.Init();
            EyeForBattle.Init();
            Wingrider.Init();
            Bisection.Init();
            Emancipation.Init();
            Cheater.Init();
            Collector.Init();
            CollateralDamage.Init();
            Siphon.Init();
            HookLineAndSinker.Init();
            SquirrelKing.Init();
            Spurred.Init();
            SharpShot.Init();
            EnemyLines.Init();
            PainfulPresence.Init();
            BloodBorn.Init();
            Armortify.Init();
            Distraction.Init();
            FlankBlast.Init();
            SplitDetonation.Init();
            CardShedder.Init();
            SapphireHeart.Init();
            EmeraldHeart.Init();
            RubyCore.Init();
            SapphireCore.Init();
            EmeraldCore.Init();
            TestSigil.Init();
            Docile.Init();
            Reroute.Init();
            Globetrotter.Init();
            Expulsion.Init();
            Crunchy.Init();
            InstantEffect.Init();
            Wimpy.Init();
            Osteoklepty.Init();
            Annihilation.Init();
            Telepathic.Init();
            OrangeInspiration.Init();
            Transmogrification.Init();
            Divination.Init();
            Freefall.Init();
            Disembowel.Init();
            Exsanguination.Init();
            FaceToFace.Init();
            BloodArtist.Init();
            FriendFinder.Init();
            NatureOfTheBeast.Init();
            Quickdraw.Init();
            Bleach.Init();
            SkullSwarm.Init();
            KillingJoke.Init();
            UnstableGems.Init();
            GreenInspiration.Init();
            BlueInspiration.Init();
            WildShape.Init();
            EasyPickings.Init();
            Antimagic.Init();
            SplashZone.Init();
            Obedient.Init();
            Snip.Init();
            Desperate.Init();
            Act2TrinketBearer.Init();
            Omniguardian.Init();
            Supercharge.Init();
            Cleaving.Init();
            MorphMover.Init();
            Backup.Init();
            SapphireDependant.Init();
            RubyDependant.Init();
            EmeraldDependant.Init();
            BombsAway.Init();
            SigilMimic.Init();
            MysteryMox.Init();
            Recycle.Init();
            BatchDelete.Init();
            Unhammerable.Init();
            Absorber.Init();
            Marcescent.Init();
            DoubleTap.Init();
            Dripping.Init();
            UnderPressure.Init();
            Regenerator.Init();
            Exhume.Init();
            Lockdown.Init();
            Magitech.Init();
            QuadStrike.Init();
            GemShedder.Init();
            SwitchStrike.Init();
            Bombjuration.Init();
            Firebomb.Init();
            Piercing.Init();
            PreemptiveStrike.Init();
            Bloodguzzler.Init();
            Eternal.Init();
            Giant.Init();
            HighPowered.Init();
            Rupture.Init();
            Ranger.Init();
            Rememberance.Init();
            Toxic.Init();
            Waterbird.Init();
            Bloated.Init();
            Draw.Init();
            FrogFriend.Init();
            L33pLeaver.Init();
            Deadringer.Init();
            PutSentryHere.Init();
            CircuitMaker.Init();
            VivaLaRevolution.Init();
            MoxMax.Init();
            Goated.Init();
            WingClipper.Init();
            Gravity.Init();
            Bastion.Init();
            Healer.Init();
            PerfectForm.Init();
            ImmaculateForm.Init();
            PureHeart.Init();
            GemSkeptic.Init();
            Defiler.Init();
            BifurcatedWhenGempowered.Init();
            SweepingStrikeLeft.Init();
            SweepingStrikeRight.Init();
            Entangle.Init();
            //Slimy.Init();
            Coward.Init();
            Reflective.Init();
            Mirrific.Init();
            Unspeakable.Init();
            Prophecy.Init();
            DiagonalStrike.Init();
            Retreat.Init();

            //LATCH SIGILS
            WaterborneLatch.Init();
            SprinterLatch.Init();
            BurningLatch.Init();
            BurrowerLatch.Init();
            WeirdLatch.Init();
            NullLatch.Init(); 
            FrailLatch.Init();
            HaunterLatch.Init();
            OverclockedLatch.Init();
            GemLatch.Init();
            AirborneLatch.Init();
            AnnoyingLatch.Init();
            SniperLatch.Init();
            StalwartLatch.Init();
            TotemLatch.Init(); 

            //CONDUIT SIGILS
            //Act 1 "Bonds"
            MeaninglessBond.Init();
            FriendshipBond.Init();
            TenderBond.Init();
            BillowingBond.Init();
            //Other Conduits
            GraveConduit.Init();
            StimConduit.Init();
            LootConduit.Init();
            MalfunctioningConduit.Init();
            GunConduit.Init();
            ThornyConduit.Init();
            ElderConduit.Init();
            NanoConduit.Init();
            HotConduit.Init();
            SapphireConduit.Init();
            RubyConduit.Init();
            EmeraldConduit.Init();

            //CONDUITREACTIVE
            RepulsiveWhenPowered.Init();
            SplashDamageWhenPowered.Init();
            GiftWhenPoweredCustom.Init();
            DoubleStrikeWhenPowered.Init();
            OmnipotentWhenPowered.Init();
            MorselWhenPowered.Init();
            PoweredQuills.Init();
            PrintWhenPowered.Init();
            ScavengeWhenPowered.Init();
            StimulateWhenPowered.Init();

            //ACTIVATED SIGILS
            Fetch.Init();
            Remove.Init();
            EternalGallop.Init();
            Flip.Init();
            TrainedFlier.Init();
            EnlargeCustom.Init();
            Dredge.Init();
            Bonefed.Init();
            Bonestrike.Init();
            UpgradeSubroutine.Init();
            Causality.Init();
            Carnivore.Init();
            Deadbeat.Init();
            Bloodbait.Init();
            TrainedSwimmer.Init();
            Gorge.Init();
            PunchingBag.Init();
            FairTrade.Init();
            Broodfeast.Init();
            PickyEater.Init();
            Escape.Init();
            Spellsword.Init();
            Sharpen.Init();
            ExaltedRune.Init();

            //VARIABLE STATS
            AntPlusTwo.Init();
            HealthDamage.Init();
            BloodAndBone.Init();
            DirectorOfTheBoard.Init();
            DamageDice.Init();
            OneDFour.Init();
            BirdsOfAFeather.Init();
            PackHunter.Init();
            HerdingBeast.Init();
            WorldWideWeb.Init();
            Boned.Init();
            OneHalfSquirrels.Init();
            SinEater.Init();
            Ambitious.Init();
            StrengthInNumbers.Init();
            PackPower.Init();
            PackPowerPlus.Init();
            TrinketVitality.Init();
            HalfCharged.Init();
            Greenhorn.Init();
            Fabled.Init();
            MagickePower.Init();
            CrabDance.Init();
            HandOnHeart.Init();
            Lithophile.Init();
            Firepower.Init();
            FollowTheLeader.Init();
            Starved.Init();
            DrawnOut.Init();
            SanguineBond.Init();

            //SPECIAL ABILITIES
            InherentFecundity.Init();
            ContinualEvolution.Init();
            AbsorbOtherCards.Init();
            SigilShedder.Init();
            InherentUndying.Init();
            InherentCardOnHit.Init();
            InherentGooey.Init();
            InherentCardShedder.Init();
            BetterRandomCard.Init();
            InherentGraveyardShift.Init();
            Act2SpawnLice.Init();
            Act2Shapeshifter.Init();

            Cards.Init();
            FaceToFaceCardsInit.Init();
            Act2ItemCards.Init();

            CardManager.BaseGameCards.CardByName("EmptyVessel").SetPixelPortrait(Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/act2emptyves_pixel.png"));
            CardManager.BaseGameCards.CardByName("EmptyVessel_GreenGem").SetPixelPortrait(Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/emeraldvessel_pixel.png"));
            CardManager.BaseGameCards.CardByName("EmptyVessel_OrangeGem").SetPixelPortrait(Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/rubyvessel_pixel.png"));
            CardManager.BaseGameCards.CardByName("EmptyVessel_BlueGem").SetPixelPortrait(Tools.LoadTex("NevernamedsSigils/Resources/PixelCards/sapphirevessel_pixel.png"));



            CardManager.ModifyCardList += delegate (List<CardInfo> cards)
            {
                foreach (CardInfo card in cards.Where(c => c.GetExtendedProperty("PreventBones") != null))
                {
                    if (card.decals == null) card.decals = new List<Texture>();
                    card.decals.Add(NoBonesDecal);
                }
                bool addArachnid = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.tribes.Contains(NevernamedsTribes.Arachnid)).Count > 0;
                bool addCrustacean = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.tribes.Contains(NevernamedsTribes.Crustacean)).Count > 0;
                bool addRodent = ScriptableObjectLoader<CardInfo>.AllData.FindAll((CardInfo x) => x.tribes.Contains(NevernamedsTribes.Rodent)).Count > 0;
                foreach (CardInfo inf in cards)
                {
                    if (toBeMadeArachnid.Contains(inf.name) && addArachnid) { inf.tribes.Add(NevernamedsTribes.Arachnid); }
                    if (toBeMadeCrustacean.Contains(inf.name) && addCrustacean) { inf.tribes.Add(NevernamedsTribes.Crustacean); }
                    if (toBeMadeRodent.Contains(inf.name) && addRodent) { inf.tribes.Add(NevernamedsTribes.Rodent); }
                    if (inf.GetExtendedProperty("SigilariumGemified") != null) { inf.Mods.Add(new CardModificationInfo() { gemify = true }); }
                }

                return cards;
            };
            AbilityManager.ModifyAbilityList += delegate (List<FullAbility> abilities)
            {
                return abilities;
            };
        }
        public static List<string> toBeMadeRodent = new List<string>()
        {
            "Amalgam",
            "Hydra",
            "PackRat",
            "Porcupine",
            "Beaver",
            "FieldMouse",
            "RatKing"
        };
        public static List<string> toBeMadeArachnid = new List<string>()
        {
            "Amalgam",
            "Hydra"
        };
        public static List<string> toBeMadeCrustacean = new List<string>()
        {
            "Amalgam",
            "Hydra"
        };
        public static AssetBundle LoadBundle(string path)
        {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(path.Replace("\\", ".").Replace("/", ".")))
            {
                return AssetBundle.LoadFromStream(s);
            }
        }

        public static List<CardInfo> mycoRequiredCardAdded = new List<CardInfo>();
        public static List<CardInfo> mycoFusedCardAdded = new List<CardInfo>();

        internal static ConfigEntry<bool> arachnophobiaMode;

        public static Texture2D NoBonesDecal;
        public static readonly AbilityMetaCategory GrimoraModChair1 = (AbilityMetaCategory)GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1");
        public static readonly AbilityMetaCategory GrimoraModChair2 = (AbilityMetaCategory)GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2");
        public static readonly AbilityMetaCategory GrimoraModChair3 = (AbilityMetaCategory)GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3");
        public static readonly AbilityMetaCategory Part2Modular = (AbilityMetaCategory)GuidManager.GetEnumValue<AbilityMetaCategory>("cyantist.inscryption.api", "Part2Modular");

        public static readonly CardMetaCategory GrimoraChoiceNode = GuidManager.GetEnumValue<CardMetaCategory>("arackulele.inscryption.grimoramod", "GrimoraModChoiceNode");

        public static readonly CardMetaCategory P03KayceesWizardRegion = (CardMetaCategory)GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "WizardRegionCards");
        public static readonly CardMetaCategory P03KayceesBastionRegion = (CardMetaCategory)GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "TechRegionCards");
        public static readonly CardMetaCategory P03KayceesNatureRegion = (CardMetaCategory)GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "NatureRegionCards");
        public static readonly CardMetaCategory P03KayceesUndeadRegion = (CardMetaCategory)GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "UndeadRegionCards");
        public static readonly CardMetaCategory P03KayceesNeutralRegion = GuidManager.GetEnumValue<CardMetaCategory>("zorro.inscryption.infiniscryption.p03kayceerun", "NeutralRegionCards");

    }
}

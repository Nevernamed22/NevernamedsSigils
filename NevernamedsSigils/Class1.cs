using BepInEx;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NevernamedsSigils
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "nevernamed.inscryption.sigils";
        private const string PluginName = "NevernamedsSigils";
        private const string PluginVersion = "1.0.0.0";

        public static AssetBundle bundle;
        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");

            bundle = LoadBundle("NevernamedsSigils/Resources/sigilassetbundle");

            Harmony harmony = new Harmony("NevernamedsSigils.harmonypatcher");
            harmony.PatchAll();

            NoBonesDecal = Tools.LoadTex("NevernamedsSigils/Resources/Other/preventbonesdecal.png");

            NevernamedsTribes.InitTribes();

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
            Act2Amorphous.Init();
            Unlucky.Init();
            AntGuardian.Init();
            Drop.Init();
            Moxcraft.Init();
            WoodsieLord.Init();
            FringeStrike.Init();
            Mason.Init();
            Dupeglitch.Init();
            Downdraft.Init();
            Act2VesselPrinter.Init();
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

            //LATCH SIGILS
            WaterborneLatch.Init();
            SprinterLatch.Init();
            BurningLatch.Init();
            BurrowerLatch.Init();
            WeirdLatch.Init();

            //CONDUIT SIGILS
            //Act 1 "Bonds"
            MeaninglessBond.Init();
            FriendshipBond.Init();
            TenderBond.Init();
            BillowingBond.Init();
            //Other Conduits

            //CONDUITREACTIVE
            RepulsiveWhenPowered.Init();

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

            //VARIABLE STATS
            AntPlusTwo.Init();
            HealthDamage.Init();
            BloodAndBone.Init();
            DirectorOfTheBoard.Init();
            DamageDice.Init();
            BirdsOfAFeather.Init();
            PackHunter.Init();
            HerdingBeast.Init();
            WorldWideWeb.Init();
            Boned.Init();
            OneHalfSquirrels.Init();
            SinEater.Init();
            Ambitious.Init();

            //SPECIAL ABILITIES
            InherentFecundity.Init();
            ContinualEvolution.Init();
            AbsorbOtherCards.Init();
            SigilShedder.Init();
            InherentUndying.Init();

            CustomAppearances.Init();

            Cards.Init();

            CardManager.ModifyCardList += delegate (List<CardInfo> cards)
            {
                foreach (CardInfo card in cards.Where(c => c.GetExtendedProperty("PreventBones") != null))
                {
                    if (card.decals == null) card.decals = new List<Texture>();
                    card.decals.Add(NoBonesDecal);
                }
                return cards;
            };
        }
        public static AssetBundle LoadBundle(string path)
        {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(path.Replace("\\", ".").Replace("/", ".")))
            {
                return AssetBundle.LoadFromStream(s);
            }
        }
        public static Texture2D NoBonesDecal;
    }
}

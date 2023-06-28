using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class CustomDeathcardPortrait : DynamicCardPortrait
    {       
        public static string GenerateRandomName()
        {
            string toReturn = "Jeff";
            switch (Tools.GetActAsInt())
            {
                case 1:
                    toReturn = Tools.RandomElement(syllable1) + (UnityEngine.Random.value <= 0.5f ? Tools.RandomElement(syllable2) : "") + Tools.RandomElement(syllable3);
                    break;
                case 2:
                    toReturn = "Amalgam";
                    break;
                case 3:
                    toReturn = Tools.RandomElement(RobotSyllable1) + Tools.RandomElement(RobotSyllable2) + Tools.RandomElement(RobotSyllable3);
                    break;
                case 4:
                    toReturn = (UnityEngine.Random.value <= 0.5f ? Tools.RandomElement(UndeadSyllable1) : "") + Tools.RandomElement(UndeadSyllable2) + Tools.RandomElement(UndeadSyllable3);
                    break;
            }
            return toReturn;
        }
        public static CardInfo CompletelyRandomAnimalDeathcard(int statPoints, int minattck = 0)
        {
            CardModificationInfo inf = new CardModificationInfo();
            switch (Tools.GetActAsInt())
            {
                case 1:
                    inf = CardInfoGenerator.CreateRandomizedAbilitiesStatsMod(
                        ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.metaCategories.Contains(AbilityMetaCategory.Part1Modular)),
                        statPoints, UnityEngine.Random.value < 0.5f ? 1 : minattck, 1);
                    break;
                case 2:
                    break;
                case 3:
                    inf = CardInfoGenerator.CreateRandomizedAbilitiesStatsMod(
                        ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.metaCategories.Contains(AbilityMetaCategory.Part3Modular)),
                       Math.Min(statPoints, 10), UnityEngine.Random.value < 0.5f ? 1 : minattck, 1);
                    break;
                case 4:
                    inf = CardInfoGenerator.CreateRandomizedAbilitiesStatsMod(
                        ScriptableObjectLoader<AbilityInfo>.AllData.FindAll((AbilityInfo x) => x.metaCategories.Contains(Plugin.GrimoraModChair1) || x.metaCategories.Contains(Plugin.GrimoraModChair2) || x.metaCategories.Contains(Plugin.GrimoraModChair3)),
                        statPoints, UnityEngine.Random.value < 0.5f ? 1 : minattck, 1);
                    break;
            }

            List<int> tribes = new List<int>() { 10, 11, 12, 13, 14, 16, 17, 18 };

            //Determine Cost in here
            int bloodCost = 0;
            int boneCost = 0;
            int energyCost = 0;
            List<GemType> gemsCost = new List<GemType>();

            switch (Tools.GetActAsInt())
            {
                case 1: //Act 1                  
                    int remaining = statPoints - 1;
                    while (remaining > 0)
                    {
                        if (remaining < 2) { remaining = 0; break; }
                        if (remaining < 4) { boneCost += remaining; remaining = 0; break; }

                        float preference = 0.5f;
                        if (bloodCost > 0) { preference = 0.8f; }
                        else if (boneCost > 0) { preference = 0.2f; }
                        if (UnityEngine.Random.value <= preference) { bloodCost++; remaining -= 4; }
                        else { boneCost++; remaining--; }
                    }
                    break;
                case 2:
                    break;
                case 3:
                    energyCost = Math.Min(6, statPoints - 1);
                    break;
                case 4:
                    if (UnityEngine.Random.value <= 0.25f) { boneCost = statPoints - 1; break; }
                    else
                    {
                        int grimoraRemaining = statPoints - 1;
                        float energyPreference = 0.5f;
                        if (UnityEngine.Random.value <= 0.5f) { energyPreference += 0.25f; }
                        while (grimoraRemaining > 0)
                        {
                            if (UnityEngine.Random.value <= energyPreference && energyCost < 6) { energyCost++; grimoraRemaining--; }
                            else { boneCost++; grimoraRemaining--; }
                        }
                    }
                    break;
            }

            List<Tribe> tribeList = new List<Tribe>
            {
                Tribe.Bird,
                Tribe.Canine,
                Tribe.Hooved,
                Tribe.Insect,
                Tribe.Reptile,
                NevernamedsTribes.Arachnid,
                NevernamedsTribes.Crustacean,
                NevernamedsTribes.Rodent
            };

            int tribe1 = Tools.RandomElement(tribes);
            int tribe2 = Tools.RandomElement(tribes);
            int tribe3 = Tools.RandomElement(tribes);
            if (Tools.GetActAsInt() == 3) { tribe1 = 19; tribe2 = 19; tribe3 = 19; }
            if (Tools.GetActAsInt() == 4) { tribe1 = 20; tribe2 = 20; tribe3 = 20; }

            return GenerateAnimalDeathcard(
                bloodCost, boneCost, energyCost, gemsCost, 0, //Costs
                inf.attackAdjustment, inf.healthAdjustment, //Stats
                false, SpecialStatIcon.None, new List<SpecialTriggeredAbility>() { }, //Special Stats
                inf.abilities, //Sigils
                tribe1, tribe2, tribe3, //Tribes
                GenerateRandomName(),
                new List<Tribe>() { Tools.RandomElement(tribeList) }
                );
        }
        public static CardInfo GenerateAnimalDeathcard(int bloodcost, int bonecost, int energyCost, List<GemType> gemsCost, int lightCost, int power, int health, bool hasSpecialStat, SpecialStatIcon specialStat, List<SpecialTriggeredAbility> variableStat, List<Ability> sigils, int tribe1, int tribe2, int tribe3, string name, List<Tribe> tribes)
        {
            //Debug.Log("Generating Deathcard 1");
            CardInfo cardByName = Tools.GetActAsInt() == 2 ? CardLoader.GetCardByName("SigilNevernamed CrypticBeast") : CardLoader.GetCardByName("SigilNevernamed AnimalDeathcard");
            CardModificationInfo deathcardDefinition = new CardModificationInfo();

            //cardByName.tribes.AddRange(tribes);

            deathcardDefinition.healthAdjustment = health;
            deathcardDefinition.attackAdjustment = power;
            deathcardDefinition.bloodCostAdjustment = bloodcost;
            deathcardDefinition.bonesCostAdjustment = bonecost;
            deathcardDefinition.energyCostAdjustment = energyCost;
            deathcardDefinition.addGemCost = gemsCost;
            deathcardDefinition.nameReplacement = name;
            if (hasSpecialStat)
            {
                deathcardDefinition.statIcon = specialStat;
                deathcardDefinition.specialAbilities.AddRange(variableStat);
            }
            deathcardDefinition.abilities = sigils;

            //Debug.Log("Generating Deathcard 2");
            List<int> tribeOrder = new List<int>() { tribe1, tribe2, tribe3 };
            tribeOrder.Add(Tools.RandomElement(tribeOrder));
            List<int> newTribeOrder = Shuffle(tribeOrder);

            if (Tools.GetActAsInt() != 2)
            {
                deathcardDefinition.bountyHunterInfo = new BountyHunterInfo()
                {
                    tier = int.Parse($"{newTribeOrder[0]}" + $"{newTribeOrder[1]}" + $"{newTribeOrder[2]}" + $"{newTribeOrder[3]}"), //Indexes which determine the tribe
                    faceIndex = UnityEngine.Random.Range(0, BodySprites[newTribeOrder[0]].Count), //Body
                    mouthIndex = UnityEngine.Random.Range(0, HeadSprites[newTribeOrder[1]].Count), //head
                    eyesIndex = UnityEngine.Random.Range(0, EyesSprites[newTribeOrder[2]].Count), //face
                    hatIndex = UnityEngine.Random.Range(0, BodySprites[newTribeOrder[3]].Count), //ears
                };
            }

            cardByName.mods.Add(deathcardDefinition);
            return cardByName;
        }
        [SerializeField]
        public SpriteRenderer bodyRenderer;
        [SerializeField]
        public SpriteRenderer headRenderer;
        [SerializeField]
        public SpriteRenderer faceRenderer;
        [SerializeField]
        public SpriteRenderer earsRenderer;
        [SerializeField]
        public SpriteRenderer emissionRenderer;

        private static List<int> Shuffle(List<int> original)
        {
            List<int> newList = new List<int>();
            for (int i = original.Count - 1; i >= 0; i--)
            {
                int index = UnityEngine.Random.Range(0, original.Count);
                newList.Add(original[index]);
                original.RemoveAt(index);
            }
            return newList;
        }
        public int[] ParseTribeCode(int toDecode)
        {
            int[] individuals = GetDigits3(toDecode);
            List<int> revised = new List<int>();
            revised.Add(int.Parse($"{individuals[0]}" + $"{individuals[1]}"));
            revised.Add(int.Parse($"{individuals[2]}" + $"{individuals[3]}"));
            revised.Add(int.Parse($"{individuals[4]}" + $"{individuals[5]}"));
            revised.Add(int.Parse($"{individuals[6]}" + $"{individuals[7]}"));
            return revised.ToArray();
        }
        CardModificationInfo bountyHunterInfo;
        public override void ApplyCardInfo(CardInfo card)
        {
            bodyRenderer = base.transform.Find("bodysprite").GetComponent<SpriteRenderer>();
            headRenderer = base.transform.Find("headsprite").GetComponent<SpriteRenderer>();
            faceRenderer = base.transform.Find("Eyes").GetComponent<SpriteRenderer>();
            earsRenderer = base.transform.Find("earssprite").GetComponent<SpriteRenderer>();
            emissionRenderer = base.transform.Find("Eyes").Find("Emission").GetComponent<SpriteRenderer>();

            CardModificationInfo cardModificationInfo = card.Mods.Find((CardModificationInfo x) => x.bountyHunterInfo != null);
            if (cardModificationInfo != null)
            {
                bountyHunterInfo = cardModificationInfo;
                DisplayFace(
                   cardModificationInfo.bountyHunterInfo.faceIndex,
                   cardModificationInfo.bountyHunterInfo.mouthIndex,
                   cardModificationInfo.bountyHunterInfo.eyesIndex,
                   cardModificationInfo.bountyHunterInfo.hatIndex,
                   cardModificationInfo.bountyHunterInfo.tier);
            }
        }
        private void DisplayFace(int bodyind, int headind, int faceind, int earsind, int tribe)
        {
            if (Tools.GetActAsInt() == 3 || Tools.GetActAsInt() == 4)
            {
                // 1.35 1.8444 0.88520 0.6395 0 

                // 1.25 1.7444 0.8852 
                //0 0.5995 0

                // scale 1.2 1.0434 0.8852
                // local pos 0 0.2 0
                // Vector3 newScale = Tools.GetActAsInt() == 4 ? new Vector3(1.25f, 1.7444f, 0.8852f) : new Vector3(1.2f, 1.0434f, 0.8852f);
                // Vector3 newPos = Tools.GetActAsInt() == 4 ? new Vector3(0, 0.5995f, 0) : new Vector3(0, 0.2f, 0);
                Vector3 newScale = new Vector3(1.25f, 1.7444f, 0.8852f);
                Vector3 newPos = new Vector3(0, 0.5995f, 0);
                bodyRenderer.transform.localScale = newScale;
                bodyRenderer.transform.localPosition = newPos;
                headRenderer.transform.localScale = newScale;
                headRenderer.transform.localPosition = newPos;
                faceRenderer.transform.localScale = newScale;
                faceRenderer.transform.localPosition = newPos;
                earsRenderer.transform.localScale = newScale;
                earsRenderer.transform.localPosition = newPos;
                //emissionRenderer.transform.localScale = newScale;
                //emissionRenderer.transform.localPosition = newPos;
            }

            //Debug.Log($"Body ({bodyind}) Head ({headind}) Face ({faceind}) Ears ({earsind}) Tribe Indeces ({tribe})");
            int[] tribeindex = ParseTribeCode(tribe);

            // Debug.Log($"Setting body {bodyind} for tribe {tribeindex[0]}.");
            bodyRenderer.sprite = BodySprites[tribeindex[0]][bodyind];

            //Debug.Log($"Setting head {headind} for tribe {tribeindex[1]}.");
            headRenderer.sprite = HeadSprites[tribeindex[1]][headind];

            //Debug.Log($"Setting face {faceind} for tribe {tribeindex[2]}.");
            faceRenderer.sprite = EyesSprites[tribeindex[2]][faceind];

            if (tribeindex[3] == 19) { earsRenderer.enabled = false; }
            else
            {
                //Debug.Log($"Setting ears {earsind} for tribe {tribeindex[3]}.");
                earsRenderer.sprite = EarsSprites[tribeindex[3]][earsind];
            }

            if (tribeindex[2] == 19) { emissionRenderer.enabled = false; }
            else
            {
                //Debug.Log($"Setting emission {faceind} for tribe {tribeindex[2]}.");
                emissionRenderer.sprite = EmissionSprites[tribeindex[2]][faceind];
            }
        }
        public static int[] GetDigits3(int source)
        {
            Stack<int> digits = new Stack<int>();
            while (source > 0)
            {
                var digit = source % 10;
                source /= 10;
                digits.Push(digit);
            }

            return digits.ToArray();
        }
        public static Dictionary<int, List<Sprite>> BodySprites;
        public static Dictionary<int, List<Sprite>> HeadSprites;
        public static Dictionary<int, List<Sprite>> EyesSprites;
        public static Dictionary<int, List<Sprite>> EmissionSprites;
        public static Dictionary<int, List<Sprite>> EarsSprites;

        public static Dictionary<Tribe, int> tribeToInt = new Dictionary<Tribe, int>()
        {
            {Tribe.Canine, 10},
            {Tribe.Bird, 11},
            {Tribe.Reptile, 12},
            {Tribe.Insect, 13},
            {Tribe.Hooved, 14},
            {Tribe.Squirrel, 15},
            {NevernamedsTribes.Arachnid, 16},
            {NevernamedsTribes.Crustacean, 17},
            {NevernamedsTribes.Rodent, 18},
        };
        public static List<int> validRandomAct1Tribes = new List<int>() { 10, 11, 12, 13, 14, 16, 17, 18 };
        public static void InitialiseDictionaries()
        {

            GameObject friendlyFace = Plugin.bundle.LoadAsset<GameObject>("AnimalDeathcardPortrait");
            friendlyFace.AddComponent<CustomDeathcardPortrait>();
            friendlyFace.layer = LayerMask.NameToLayer("CardOffscreen");
            friendlyFace.transform.Find("bodysprite").gameObject.layer = LayerMask.NameToLayer("CardOffscreen");
            friendlyFace.transform.Find("headsprite").gameObject.layer = LayerMask.NameToLayer("CardOffscreen");
            friendlyFace.transform.Find("Eyes").gameObject.layer = LayerMask.NameToLayer("CardOffscreen");
            friendlyFace.transform.Find("earssprite").gameObject.layer = LayerMask.NameToLayer("CardOffscreen");
            friendlyFace.transform.Find("Eyes").Find("Emission").gameObject.layer = LayerMask.NameToLayer("CardOffscreenEmission");
            SigilSetupUtility.NewCard("SigilNevernamed AnimalDeathcard", "", 0, 0,
               new List<CardMetaCategory> { }, CardTemple.Nature, description: "",
               abilities: new List<Ability>() { },
               traits: new List<Trait>() { },
               defaultTex: Tools.LoadTex("NevernamedsSigils/Resources/Cards/customdeathcarddummy.png"),
               animatedPortrait: friendlyFace,
               appearanceBehaviour: new List<CardAppearanceBehaviour.Appearance>() { CardAppearanceBehaviour.Appearance.DynamicPortrait });

            //Key
            // 10 - Canine
            // 11 - Bird
            // 12 - Reptile
            // 13 - Insect
            // 14 - Hooved
            // 15 - Squirrel - Special
            // 16 - Arachnid
            // 17 - Crustacean
            // 18 - Rodent
            // 19 - ROBOT - Special
            // 20 - UNDEAD - Special

            BodySprites = new Dictionary<int, List<Sprite>>() { };
            BodySprites.Add(10, SetupSpritesForPartAndTribe(3, "body", "canine"));
            BodySprites.Add(11, SetupSpritesForPartAndTribe(3, "body", "bird"));
            BodySprites.Add(12, SetupSpritesForPartAndTribe(3, "body", "reptile"));
            BodySprites.Add(13, SetupSpritesForPartAndTribe(3, "body", "insect"));
            BodySprites.Add(14, SetupSpritesForPartAndTribe(3, "body", "hooved"));
            BodySprites.Add(15, SetupSpritesForPartAndTribe(1, "body", "squirrel"));
            BodySprites.Add(16, SetupSpritesForPartAndTribe(3, "body", "arachnid"));
            BodySprites.Add(17, SetupSpritesForPartAndTribe(3, "body", "crustacean"));
            BodySprites.Add(18, SetupSpritesForPartAndTribe(3, "body", "rodent"));

            HeadSprites = new Dictionary<int, List<Sprite>>() { };
            HeadSprites.Add(10, SetupSpritesForPartAndTribe(3, "head", "canine"));
            HeadSprites.Add(11, SetupSpritesForPartAndTribe(3, "head", "bird"));
            HeadSprites.Add(12, SetupSpritesForPartAndTribe(3, "head", "reptile"));
            HeadSprites.Add(13, SetupSpritesForPartAndTribe(3, "head", "insect"));
            HeadSprites.Add(14, SetupSpritesForPartAndTribe(3, "head", "hooved"));
            HeadSprites.Add(15, SetupSpritesForPartAndTribe(1, "head", "squirrel"));
            HeadSprites.Add(16, SetupSpritesForPartAndTribe(3, "head", "arachnid"));
            HeadSprites.Add(17, SetupSpritesForPartAndTribe(3, "head", "crustacean"));
            HeadSprites.Add(18, SetupSpritesForPartAndTribe(3, "head", "rodent"));

            EyesSprites = new Dictionary<int, List<Sprite>>() { };
            EyesSprites.Add(10, SetupSpritesForPartAndTribe(3, "face", "canine"));
            EyesSprites.Add(11, SetupSpritesForPartAndTribe(3, "face", "bird"));
            EyesSprites.Add(12, SetupSpritesForPartAndTribe(3, "face", "reptile"));
            EyesSprites.Add(13, SetupSpritesForPartAndTribe(3, "face", "insect"));
            EyesSprites.Add(14, SetupSpritesForPartAndTribe(3, "face", "hooved"));
            EyesSprites.Add(15, SetupSpritesForPartAndTribe(1, "face", "squirrel"));
            EyesSprites.Add(16, SetupSpritesForPartAndTribe(3, "face", "arachnid"));
            EyesSprites.Add(17, SetupSpritesForPartAndTribe(3, "face", "crustacean"));
            EyesSprites.Add(18, SetupSpritesForPartAndTribe(3, "face", "rodent"));

            EarsSprites = new Dictionary<int, List<Sprite>>() { };
            EarsSprites.Add(10, SetupSpritesForPartAndTribe(3, "ears", "canine"));
            EarsSprites.Add(11, SetupSpritesForPartAndTribe(3, "ears", "bird"));
            EarsSprites.Add(12, SetupSpritesForPartAndTribe(3, "ears", "reptile"));
            EarsSprites.Add(13, SetupSpritesForPartAndTribe(3, "ears", "insect"));
            EarsSprites.Add(14, SetupSpritesForPartAndTribe(3, "ears", "hooved"));
            EarsSprites.Add(15, SetupSpritesForPartAndTribe(1, "ears", "squirrel"));
            EarsSprites.Add(16, SetupSpritesForPartAndTribe(3, "ears", "arachnid"));
            EarsSprites.Add(17, SetupSpritesForPartAndTribe(3, "ears", "crustacean"));
            EarsSprites.Add(18, SetupSpritesForPartAndTribe(3, "ears", "rodent"));

            EmissionSprites = new Dictionary<int, List<Sprite>>() { };
            EmissionSprites.Add(10, SetupSpritesForPartAndTribe(3, "emission", "canine"));
            EmissionSprites.Add(11, SetupSpritesForPartAndTribe(3, "emission", "bird"));
            EmissionSprites.Add(12, SetupSpritesForPartAndTribe(3, "emission", "reptile"));
            EmissionSprites.Add(13, SetupSpritesForPartAndTribe(3, "emission", "insect"));
            EmissionSprites.Add(14, SetupSpritesForPartAndTribe(3, "emission", "hooved"));
            EmissionSprites.Add(15, SetupSpritesForPartAndTribe(1, "emission", "squirrel"));
            EmissionSprites.Add(16, SetupSpritesForPartAndTribe(3, "emission", "arachnid"));
            EmissionSprites.Add(17, SetupSpritesForPartAndTribe(3, "emission", "crustacean"));
            EmissionSprites.Add(18, SetupSpritesForPartAndTribe(3, "emission", "rodent"));

            //Undead
            BodySprites.Add(20, SetupSpritesForPartAndTribe(23, "body", "undead", "GrimoraDeathcards"));
            HeadSprites.Add(20, SetupSpritesForPartAndTribe(25, "head", "undead", "GrimoraDeathcards"));
            EyesSprites.Add(20, SetupSpritesForPartAndTribe(28, "face", "undead", "GrimoraDeathcards"));
            EarsSprites.Add(20, SetupSpritesForPartAndTribe(30, "ears", "undead", "GrimoraDeathcards"));
            EmissionSprites.Add(20, SetupSpritesForPartAndTribe(28, "emission", "undead", "GrimoraDeathcards"));


            //Robot
            BodySprites.Add(19, SetupSpritesForPartAndTribe(37, "body", "robot", "RobotDeathcards"));
            HeadSprites.Add(19, SetupSpritesForPartAndTribe(31, "head", "robot", "RobotDeathcards"));
            EyesSprites.Add(19, SetupSpritesForPartAndTribe(41, "face", "robot", "RobotDeathcards"));
        }
        public static List<Sprite> SetupSpritesForPartAndTribe(int amount, string part, string tribe, string sourceFolder = "AnimalDeathcards")
        {
            List<Sprite> toReturn = new List<Sprite>();
            for (int i = 1; i <= amount; i++) { toReturn.Add(Tools.ConvertTexToSprite(Tools.LoadTex($"NevernamedsSigils/Resources/{sourceFolder}/{part}_{tribe}_{i}.png"))); }
            return toReturn;
        }

        //Beast Names
        private static List<string> syllable1 = new List<string>() { "Bull", "Mos", "Man", "The ", "Deadly ", "Wild ", "Ho", "Am", "Amph", "Co", "Kar", "Fi", "Ra", "Spar", "Blood ", "King", "Coy", "Al", "El", "Moo", "Wa", "Pa", "Wor", "Mag", "Lar", "Strange ", "Stink", "Our", "Add", "Ura", "Squi", "Por", "Ara", "Bea", "Ot", "War" };
        private static List<string> syllable2 = new List<string>() { "qui", "fro", "ti", "digi", "al", "pe", "ki", "sher", "wal", "fi", "o", "loo", "ker", "got", "goat", "va", "lar", "ink", "yu", "oe", "cu", "cku", "pu" };
        private static List<string> syllable3 = new List<string>() { "frog", "to", "s", "amy", "gam", "pod", "pog", "nos", "pa", "ven", "row", "ker", "te", "pha", "se", "sie", "ant", "va", "bug", "fly", "boros", "li", "ba", "rrel", "pine", "lele", "ver", "ter", "ren" };
        //Robot Names
        private static List<string> RobotSyllable1 = new List<string>() { "The ", "Not ", "Holy ", "Tall ", "Lil ", "Big ", "Ms. ", "Mr. ", "Mrs. ", "Master ", "Dr. ", "St. ", "Dat ", "Prof. ", "Hon. ", "Grim", "Super ", "Capt. ", "Rev. ", "Gen. ", "My ", "Your ", "Msgr. ", "King ", "Queen ", "Mistress ", "Tiny ", "Crazy " };
        private static List<string> RobotSyllable2 = new List<string>() { "Lol", "The", "Toot", "Loot", "Shoot", "Help", "Good", "Smart", "Fat", "Fire", "Screw", "Bomb", "Gun", "Sad", "Happy", "Killer", "Death", "Steel", "Tin", "Gold", "Iron", "Green", "Engi", "Zap", "Buzz", "Clunker", "Angry", "Eat", "Old" };
        private static List<string> RobotSyllable3 = new List<string>() { " 2000", " 3000", "matic", "Bot", " bot", " You", " thing", "robo", " Esq.", " X", " Boi", " Jr.", " Man", "tron", "tronic", " .v4", "man", "droid", " droid", " Toe", " Me" };
        //Undead Names
        private static List<string> UndeadSyllable1 = new List<string>() { "Dr. ", "Master ", "The ", "Sir ", "Horrid ", "Festering ", "Gruesome ", "Corpulent ", "Lady ", "Madame ", "Capt. ", "First Mate ", "Loathesome ", "Dear ", "The Late ", "Mad ", "Msgr. ", "Prof. ", "Unholy ", "Evil ", "Dead ", "Wild ", "Lt. ", "Baron ", "Duke ", "Duke of ", "Starved ", "Smooth " };
        private static List<string> UndeadSyllable2 = new List<string>() { "Bones", "Graves", "Deads", "Ghoul", "Frost", "Dirt", "Heart", "Rip", "Dark", "Cold", "Limb", "Bone", "Skull", "Skulls", "Femur", "Rot", "Pus", "Blood", "Bile", "Murder", "Ooze", "Tomb", "Crypt", "Coffin", "Dung", "Wight", "Phantom", "Ghost", "Spirit", "Salt", "Sea", "Grim", "Prim", "Slick", "Boar", "Skin", "Arm", "Leg", "Spore", "Shroom", "Scum", "Fire", "Fly", "Maggot", "Worm", "Sport", "Dog", "Hound", "Doom", "Woof", "Cann", "Tumor", "Snow", "Brain" };
        private static List<string> UndeadSyllable3 = new List<string>() { "alot", "fly", "ley", "ton", "wick", "man", " Esq.", "lin", "walker", "smith", "snag", "break", "rot", "pour", "drip", "lord", "ra", "ora", "ick", "sley", "thief", "eater", "wick", "yer", "yar", " Jones", "digger", "ish", "erra", "icus", "ica", "starved", "mouth", "sucker", "meister", "beard", "bark", "shaw", "jaw", "tooth", "oli", "olas" };
    }
}


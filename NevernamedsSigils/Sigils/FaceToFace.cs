using APIPlugin;
using DiskCardGame;
using GBC;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using InscryptionAPI.Card;

namespace NevernamedsSigils
{
    public class FaceToFace : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Face To Face", "When [creature] is drawn, it morphs to mimic the current opponent.",
                      typeof(FaceToFace),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.GrimoraRulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/facetoface.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/facetoface_pixel.png"));

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
        public override bool RespondsToDrawn()
        {
            return true;
        }
        public override IEnumerator OnDrawn()
        {
            if (Singleton<PlayerHand>.Instance is PlayerHand3D)
            {
                (Singleton<PlayerHand>.Instance as PlayerHand3D).MoveCardAboveHand(base.Card);
                bool flipped = false;
                yield return base.Card.FlipInHand(delegate ()
                {
                    flipped = true;
                });
                yield return new WaitUntil(() => flipped);
                yield return Transform();
            }
            else
            {
                yield return Transform();
            }
            yield return base.LearnAbility(0.5f);

            if (base.Card.HasAbility(Ability.RandomAbility) && base.Card.GetComponent<RandomAbility>())
            {
                yield return base.Card.GetComponent<RandomAbility>().OnDrawn();
            }
            yield break;
        }
        private IEnumerator Transform()
        {
            CardInfo evolution = this.GetTransformCardInfo();

            foreach (CardModificationInfo cardModificationInfo in base.Card.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo cardModificationInfo2 = (CardModificationInfo)cardModificationInfo.Clone();
                if (cardModificationInfo2.HasAbility(FaceToFace.ability))
                {
                    cardModificationInfo2.abilities.Remove(FaceToFace.ability);
                }
                evolution.Mods.Add(cardModificationInfo2);
            }

            base.Card.SetInfo(evolution);
            yield break;
        }
        public static Dictionary<Opponent.Type, string> predefinedOpponents = new Dictionary<Opponent.Type, string>()
        {
            { Opponent.Type.ProspectorBoss, "SigilNevernamed CopiedProspecter" },
            { Opponent.Type.AnglerBoss, "SigilNevernamed Angler" },
            { Opponent.Type.TrapperTraderBoss, "SigilNevernamed Trapper" },
            { Opponent.Type.LeshyBoss, "SigilNevernamed FakeFinalBoss" },
            { Opponent.Type.PirateSkullBoss, "SigilNevernamed CopiedRoyal" },


            { Opponent.Type.PixelLeshyBoss, "SigilNevernamed PixelLeshy" },
            { Opponent.Type.GrimoraBoss, "SigilNevernamed PixelGrimora" },
            { Opponent.Type.MagnificusBoss, "SigilNevernamed PixelMagnificus" },
            { Opponent.Type.P03Boss, "SigilNevernamed PixelP03" },
            { Opponent.Type.PixelP03FinaleBoss, "SigilNevernamed PixelP03" },
        };
        public static Dictionary<DialogueSpeaker.Character, string> pixelUnderlings = new Dictionary<DialogueSpeaker.Character, string>()
        {
            //Weirdos
            { DialogueSpeaker.Character.Prospector, "SigilNevernamed Act2Prospector" },
            { DialogueSpeaker.Character.Angler, "SigilNevernamed Act2Angler" },
            { DialogueSpeaker.Character.Trapper, "SigilNevernamed Act2Trapper" },
            //Bots
            { DialogueSpeaker.Character.Inspector, "SigilNevernamed Inspector" },
            { DialogueSpeaker.Character.Smelter, "SigilNevernamed Melter" },         
            { DialogueSpeaker.Character.Dredger, "SigilNevernamed Dredger" },
            //Pupils
            { DialogueSpeaker.Character.GreenWizard, "SigilNevernamed Goobert" },
            { DialogueSpeaker.Character.OrangeWizard, "SigilNevernamed PikeMage" },
            { DialogueSpeaker.Character.BlueWizard, "SigilNevernamed LonelyWizard" },
            //Ghouls
            { DialogueSpeaker.Character.GhoulBriar, "SigilNevernamed Kaycee" },
            { DialogueSpeaker.Character.GhoulSawyer, "SigilNevernamed Sawyer" },
            { DialogueSpeaker.Character.GhoulRoyal, "SigilNevernamed RoyalPixel" },
        };
        public CardInfo GetTransformCardInfo()
        {
            CardInfo toReturn = null;
            Opponent.Type bossType = Opponent.Type.Default;
            DialogueSpeaker.Character characterType = DialogueSpeaker.Character.Woodcarver;


            if (Singleton<TurnManager>.Instance.opponent != null)
            {
                bossType = Singleton<TurnManager>.Instance.opponent.OpponentType;
                //Debug.Log(bossType);
                if (Singleton<TurnManager>.Instance.opponent.gameObject.GetComponent<PixelOpponent>() != null)
                {
                    PixelOpponent opponent = Singleton<TurnManager>.Instance.opponent.gameObject.GetComponent<PixelOpponent>();
                    if (opponent.DialogueSpeaker != null)
                    {
                    characterType = Singleton<TurnManager>.Instance.opponent.gameObject.GetComponent<PixelOpponent>().DialogueSpeaker.characterId;
                    //Debug.Log(characterType);
                    }
                }
            }

            if (predefinedOpponents.ContainsKey(bossType)) { toReturn = CardLoader.GetCardByName(predefinedOpponents[bossType]); } //Boss Definitions
            else if (pixelUnderlings.ContainsKey(characterType)) { toReturn = CardLoader.GetCardByName(pixelUnderlings[characterType]); } //Underlings Definitions
            else //Fallback encounter definitions
            {
                switch (Tools.GetActAsInt())
                {
                    case 1:
                        toReturn = CardLoader.GetCardByName("SigilNevernamed ShadowyFigure");
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }

            if (toReturn == null) { toReturn = Tools.GetActAsInt() == 2 ? CardLoader.GetCardByName("SigilNevernamed TrainingDummy") : CardLoader.GetCardByName("Amalgam"); }

            return toReturn;
        }
    }
}
using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class GoldRush : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Gold Rush", "When [creature] is played, all opponent cards on the board perish, and are replaced by Gold Nuggets.",
                      typeof(GoldRush),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/goldrush.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/goldrush_pixel.png"));

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
        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }
        public override IEnumerator OnResolveOnBoard()
        {
            List<CardSlot> slots = base.Card.OpponentCard ? new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(true)) : new List<CardSlot>(Singleton<BoardManager>.Instance.GetSlots(false));
			Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            if (slots != null && slots.Count > 0)
            {
                foreach (CardSlot indiv in slots)
                {
                    if (indiv && indiv.Card && !indiv.Card.HasTrait(Trait.Uncuttable) && !indiv.Card.HasTrait(Trait.Giant))
                    {
                        AudioController.Instance.PlaySound3D("metal_object_hit#1", MixerGroup.TableObjectsSFX, indiv.transform.position, 1f, 0f, new AudioParams.Pitch(AudioParams.Pitch.Variation.Medium), null, null, null, false);
                        yield return indiv.Card.Die(false, null, true);
                        if (indiv.Card == null)
                        {
                            yield return new WaitForSeconds(0.25f);
                            if (Tools.GetActAsInt() == 2)
                            {
                                yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("SigilNevernamed Act2GoldNugget"), indiv, 0.1f, true);
                            }
                            else
                            {
                            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("GoldNugget"), indiv, 0.1f, true);
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.35f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }
    }
}
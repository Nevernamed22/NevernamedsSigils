using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Bleach : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bleach", "When [creature] is played, all opponent creatures lose all sigils.",
                      typeof(Bleach),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 5,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/bleachsigil.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/bleachsigil_pixel.png"));

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
        private List<CardSlot> GetValidOpponentSlots()
        {
            List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
            opponentSlotsCopy.RemoveAll((CardSlot x) => x.Card == null || this.CardHasNoAbilities(x.Card));
            return opponentSlotsCopy;
        }
        private bool CardHasNoAbilities(PlayableCard card)
        {
            return !card.TemporaryMods.Exists((CardModificationInfo t) => t.abilities.Count > 0) && card.Info.Abilities.Count <= 0;
        }
        private void SpawnSplatter(PlayableCard card)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Items/BleachSplatter"));
            gameObject.transform.position = card.transform.position + new Vector3(0f, 0.1f, -0.25f);
            UnityEngine.Object.Destroy(gameObject, 5f);
        }
        public override IEnumerator OnResolveOnBoard()
        {
            yield return new WaitForSeconds(0.1f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
            yield return new WaitForSeconds(0.1f);
            if (Tools.GetActAsInt() != 2) Singleton<FirstPersonController>.Instance.AnimController.SpawnFirstPersonAnimation("FirstPersonBleachBrush", null);
            yield return new WaitForSeconds(0.41f);
            AudioController.Instance.PlaySound2D("magnificus_brush_splatter_bleach", MixerGroup.None, 0.5f, 0f, null, null, null, null, false);
            List<CardSlot> validSlots = this.GetValidOpponentSlots();

            foreach (CardSlot slot in validSlots)
            {
                if (Tools.GetActAsInt() != 2) this.SpawnSplatter(slot.Card);
                if (slot.Card.FaceDown)
                {
                    slot.Card.SetFaceDown(false, true);
                }
                slot.Card.Anim.PlayTransformAnimation();
                CustomCoroutine.WaitThenExecute(0.15f, delegate
                {
                    this.RemoveCardAbilities(slot.Card);
                }, false);
                yield return new WaitForSeconds(0.04166f);
            }
            yield return new WaitForSeconds(1.5f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield break;
        }
        private void RemoveCardAbilities(PlayableCard card)
        {
            CardModificationInfo cardModificationInfo = new CardModificationInfo();
            cardModificationInfo.negateAbilities = new List<Ability>();
            foreach (CardModificationInfo cardModificationInfo2 in card.TemporaryMods)
            {
                if (base.Card.gameObject.GetComponent<MagickePower>()) { base.Card.gameObject.GetComponent<MagickePower>().RemovedSigilAmount += cardModificationInfo2.abilities.Count; }
                cardModificationInfo.negateAbilities.AddRange(cardModificationInfo2.abilities);
            }
                if (base.Card.gameObject.GetComponent<MagickePower>()) { base.Card.gameObject.GetComponent<MagickePower>().RemovedSigilAmount += card.Info.Abilities.Count; }
            cardModificationInfo.negateAbilities.AddRange(card.Info.Abilities);
            card.AddTemporaryMod(cardModificationInfo);
        }
    }
}
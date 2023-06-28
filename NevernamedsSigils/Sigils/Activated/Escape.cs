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
    public class Escape : ActivatedAbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Escape", "If you have a green gem, return [creature] to your hand.",
                      typeof(Escape),
                      categories: new List<AbilityMetaCategory> { },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/escape.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/escape_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;

        
        public override bool CanActivate()
        {
            return Singleton<ResourcesManager>.Instance.HasGem(GemType.Green);
        }
        public override IEnumerator Activate()
        {
            yield return new WaitForSeconds(0.15f);

            CardInfo toDraw = base.Card.Info;
            List<CardModificationInfo> tempMods = new List<CardModificationInfo>();
            tempMods.AddRange(base.Card.temporaryMods);
            int damageTaken = base.Card.Status.damageTaken;

            base.Card.ExitBoard(0.25f, Vector3.zero);
            yield return new WaitForSeconds(0.75f);
            if (Tools.GetActAsInt() == 2)
            {
                Tween.Position(base.transform, base.transform.position + Vector3.down * 2f, 0.25f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate ()
                {
                    UnityEngine.Object.Destroy(base.gameObject);
                }, true);
            }
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                yield return new WaitForSeconds(0.2f);
            }

            PlayableCard playableCard = CardSpawner.SpawnPlayableCard(toDraw);
            foreach (CardModificationInfo mod in tempMods) { playableCard.AddTemporaryMod(mod); }
            playableCard.Status.damageTaken = damageTaken;

            yield return Singleton<PlayerHand>.Instance.AddCardToHand(playableCard, Singleton<CardSpawner>.Instance.spawnedPositionOffset, 0.25f);
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

    }
}
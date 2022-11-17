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
    public class Bloodbait : BloodActivatedAbility
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Bloodbait", "Pay 1 blood to return [creature] to your hand.",
                      typeof(Bloodbait),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook },
                      powerLevel: 1,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/Activated/bloodbait.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/Activated/bloodbait_pixel.png"),
                      isActivated: true);

            ability = newSigil.ability;
        }

        public static Ability ability;
        public override int BloodRequired()
        {
            return 1;
        }
        public override IEnumerator OnBloodAbilityPostAllSacrifices()
        {
            yield return new WaitForSeconds(0.15f);

            CardInfo toDraw = base.Card.Info;
            List<CardModificationInfo> tempMods = new List<CardModificationInfo>();
            tempMods.AddRange(base.Card.temporaryMods);
            int damageTaken = base.Card.Status.damageTaken;
            
            base.Card.ExitBoard(0.25f, Vector3.zero);
            yield return new WaitForSeconds(0.75f);
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
            yield break;
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
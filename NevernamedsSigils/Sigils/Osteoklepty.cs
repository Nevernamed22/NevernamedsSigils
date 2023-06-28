using APIPlugin;
using DiskCardGame;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using OpponentBones;

namespace NevernamedsSigils
{
    public class Osteoklepty : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Osteoklepty", "When [creature] is played, it steals all of the opponents bones for it's owner.",
                      typeof(Osteoklepty),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair3 },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/osteoklepty.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/osteoklepty_pixel.png"));

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
            if (OpponentResourceManager.instance)
            {
                if (base.Card.OpponentCard)
                {
                    if (Singleton<ResourcesManager>.Instance.PlayerBones == 0) { base.Card.Anim.StrongNegationEffect(); yield break; }
                    Singleton<ViewManager>.Instance.SwitchToView(View.BoneTokens, false, true);
                    yield return new WaitForSeconds(0.1f);
                    base.Card.Anim.LightNegationEffect();
                    yield return base.PreSuccessfulTriggerSequence();

                    int Bones = Singleton<ResourcesManager>.Instance.PlayerBones;
                    yield return Singleton<ResourcesManager>.Instance.SpendBones(Bones);
                    yield return new WaitForSeconds(0.5f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    yield return new WaitForSeconds(0.1f);

                    yield return OpponentResourceManager.instance.AddOpponentBones(base.Card.Slot, Bones);

                    yield return new WaitForSeconds(0.1f);
                    yield return base.LearnAbility(0.1f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }
                else
                {
                    if (OpponentResourceManager.instance.OpponentBones == 0) { base.Card.Anim.StrongNegationEffect(); yield break; }
                    Singleton<ViewManager>.Instance.SwitchToView(View.OpponentTotem, false, true);
                    yield return new WaitForSeconds(0.1f);
                    base.Card.Anim.LightNegationEffect();
                    yield return base.PreSuccessfulTriggerSequence();

                    int Bones = OpponentResourceManager.instance.OpponentBones;
                    yield return OpponentResourceManager.instance.RemoveOpponentBones(Bones);
                    yield return new WaitForSeconds(0.5f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, true);
                    yield return new WaitForSeconds(0.1f);

                    yield return Singleton<ResourcesManager>.Instance.AddBones(Bones, base.Card.Slot);

                    yield return new WaitForSeconds(0.1f);
                    yield return base.LearnAbility(0.1f);
                    Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                }

            }
            yield break;
        }
    }
}

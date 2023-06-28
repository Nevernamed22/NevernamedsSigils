using APIPlugin;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NevernamedsSigils
{
    public class Expulsion : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/expulsion.png");
            basePixelIcon = Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/expulsion_pixel.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Expulsion", "When [creature] takes damage, it regurgitates it's contents into the adjacent space to the right.",
                      typeof(Expulsion),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.GrimoraRulebook, Plugin.GrimoraModChair2, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: baseIcon,
                      pixelTex: basePixelIcon);

            ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {100, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/expulsioninf.png") },
                {1, baseIcon },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/expulsion2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/expulsion3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/expulsion4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/expulsion5.png") },
            };
            countDownPixelIcons = new Dictionary<int, Texture>()
            {
                {100, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/expulsioninf_pixel.png") },
                {1, basePixelIcon },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/expulsion2_pixel.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/expulsion3_pixel.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/expulsion4_pixel.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/expulsion5_pixel.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Dictionary<int, Texture> countDownPixelIcons;
        public static Texture baseIcon;
        public static Texture2D basePixelIcon;
        public static Ability ability;
        private bool initialised;
        private int tailsLeft;
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        private int Counter
        {
            get
            {
                int customLifespan = 1;
                if (base.Card.Info.GetExtendedProperty("CustomExpulsionMax") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomExpulsionMax"), out customLifespan);
                    customLifespan = succeed ? customLifespan : 1;
                }
                return customLifespan;
            }
        }
        private IEnumerator Initialise()
        {
            tailsLeft = Counter;
            initialised = true;
            ReRenderCard();
            yield break;
        }
        private void ReRenderCard()
        {
            if (Tools.GetActAsInt() == 2)
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Expulsion.ability, countDownPixelIcons.ContainsKey(tailsLeft) ? countDownPixelIcons[tailsLeft] : basePixelIcon);
            }
            else
            {
                base.Card.RenderInfo.OverrideAbilityIcon(Expulsion.ability, countDownIcons.ContainsKey(tailsLeft) ? countDownIcons[tailsLeft] : baseIcon);
            }
            base.Card.RenderCard();
        }
        public override bool RespondsToDrawn() { return true; }
        public override IEnumerator OnDrawn()
        {
            if (!initialised) { yield return Initialise(); }
            yield break;
        }
        public override bool RespondsToResolveOnBoard() { return true; }
        public override IEnumerator OnResolveOnBoard()
        {
            if (!initialised) { yield return Initialise(); }
            yield break;
        }

        public override bool RespondsToTakeDamage(PlayableCard source)
        {
            return tailsLeft > 0;
        }
        public override IEnumerator OnTakeDamage(PlayableCard source)
        {
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            base.Card.Anim.StrongNegationEffect();

            if (toLeftValid || toRightValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.2f);

                if (tailsLeft != 100) tailsLeft--;
                if (tailsLeft <= 0)
                {
                    base.Card.Status.hiddenAbilities.Add(this.Ability);
                }
                ReRenderCard();

                string cardIdentifier = Tools.GetActAsInt() == 4 ? "SigilNevernamed VomitGrimora" : "SigilNevernamed Vomit";
                if (base.Card.Info.GetExtendedProperty("ExpulsionOverride") != null) { cardIdentifier = base.Card.Info.GetExtendedProperty("ExpulsionOverride"); }

                CardInfo inf = CardLoader.GetCardByName(cardIdentifier);
                if (inf.name == "Tadpole") { inf.evolveParams.turnsToEvolve = 2; }

                PlayableCard tail = CardSpawner.SpawnPlayableCardWithCopiedMods(inf, base.Card, Expulsion.ability);
                tail.transform.position = (toRightValid ? toRight : toLeft).transform.position + Vector3.back * 2f + Vector3.up * 2f;
                tail.transform.rotation = Quaternion.Euler(110f, 90f, 90f);

                yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(tail, toRightValid ? toRight : toLeft, 0.1f, null, true);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
                tail.Anim.StrongNegationEffect();
                yield return base.StartCoroutine(base.LearnAbility(0.5f));
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }
    }
}
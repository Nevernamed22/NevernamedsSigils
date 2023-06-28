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
    public class Distraction : AbilityBehaviour
    {
        public static void Init()
        {
            baseIcon = Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction.png");
            basePixelIcon = Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction_pixel.png");
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Distraction", "When [creature] would be struck, a distraction is left in its place and [creature] moves to the right.",
                      typeof(Distraction),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: true,
                      tex: baseIcon,
                      pixelTex: basePixelIcon);

            Distraction.ability = newSigil.ability;
            countDownIcons = new Dictionary<int, Texture>()
            {
                {1, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction.png") },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction2.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction3.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction4.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction5.png") },
                {6, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction6.png") },
                {7, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction7.png") },
                {8, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction8.png") },
                {9, Tools.LoadTex("NevernamedsSigils/Resources/Sigils/distraction9.png") },
            };
            pixelCountDownIcons = new Dictionary<int, Texture>()
            {
                {1, basePixelIcon },
                {2, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction2_pixel.png") },
                {3, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction3_pixel.png") },
                {4, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction4_pixel.png") },
                {5, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction5_pixel.png") },
                {6, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction6_pixel.png") },
                {7, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction7_pixel.png") },
                {8, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction8_pixel.png") },
                {9, Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/distraction9_pixel.png") },
            };
        }
        public static Dictionary<int, Texture> countDownIcons;
        public static Dictionary<int, Texture> pixelCountDownIcons;
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
                if (base.Card.Info.GetExtendedProperty("CustomDistractionMax") != null)
                {
                    bool succeed = int.TryParse(base.Card.Info.GetExtendedProperty("CustomDistractionMax"), out customLifespan);
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
                base.Card.RenderInfo.OverrideAbilityIcon(Distraction.ability, pixelCountDownIcons.ContainsKey(tailsLeft) ? pixelCountDownIcons[tailsLeft] : basePixelIcon);
            }
            else
            {
            base.Card.RenderInfo.OverrideAbilityIcon(Distraction.ability, countDownIcons.ContainsKey(tailsLeft) ? countDownIcons[tailsLeft] : baseIcon);
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









        public override bool RespondsToCardGettingAttacked(PlayableCard source)
        {
            return source == base.Card && tailsLeft > 0;
        }
        public override IEnumerator OnCardGettingAttacked(PlayableCard card)
        {
            CardSlot slot = base.Card.Slot;
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            bool flag = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            if (flag || toRightValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                yield return new WaitForSeconds(0.2f);
                if (toRightValid)
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, toRight, 0.1f, null, true);
                }
                else
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, toLeft, 0.1f, null, true);
                }
                base.Card.Anim.StrongNegationEffect();

                tailsLeft--;
                if (tailsLeft <= 0)
                {
                    base.Card.Status.hiddenAbilities.Add(this.Ability);
                }
                ReRenderCard();

                yield return new WaitForSeconds(0.2f);
                CardInfo info;
                if (base.Card.Info.tailParams != null) { info = (base.Card.Info.tailParams.tail.Clone() as CardInfo); }
                else { info = TailParams.GetDefaultTail(base.Card.Info); }

                PlayableCard tail = CardSpawner.SpawnPlayableCardWithCopiedMods(info, base.Card, Distraction.ability);
                tail.transform.position = slot.transform.position + Vector3.back * 2f + Vector3.up * 2f;
                tail.transform.rotation = Quaternion.Euler(110f, 90f, 90f);

                yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(tail, slot, 0.1f, null, true);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
                tail.Anim.StrongNegationEffect();
                yield return base.StartCoroutine(base.LearnAbility(0.5f));
                yield return new WaitForSeconds(0.2f);
                tail = null;
            }
            yield break;
        }
    }
}
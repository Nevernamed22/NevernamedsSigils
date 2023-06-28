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
    public class Cleaving : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Cleaving", "When [creature] strikes another creature, that creature will be cut in half. Half of the creature will move to the right, while the other half remains in place.",
                      typeof(Cleaving),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook, Plugin.Part2Modular },
                      powerLevel: 2,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/cleaving.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/cleaving_pixel.png"),
                      triggerText: "[creature] cleaves it's target in half!");

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
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return attacker == base.Card && target.Health > 0 && !target.Dead && !target.HasTrait(Trait.Uncuttable) && !target.HasTrait(Trait.Giant);
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            CardSlot slot = target.Slot;
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(target.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(target.Slot, false);

            bool toLeftValid = toLeft != null && toLeft.Card == null;
            bool toRightValid = toRight != null && toRight.Card == null;
            if (toLeftValid || toRightValid)
            {
                yield return base.PreSuccessfulTriggerSequence();
                if (toRightValid)
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(target, toRight, 0.1f, null, true);
                }
                else
                {
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(target, toLeft, 0.1f, null, true);
                }
                PlayableCard tail = CardSpawner.SpawnPlayableCardWithCopiedMods(target.Info.Clone() as CardInfo, target, Ability.None);
                tail.transform.position = slot.transform.position;
                yield return new WaitForSeconds(0.1f);

                CardInfo newSelf = target.Info.Clone() as CardInfo;
                foreach(CardModificationInfo inf in target.Info.Mods)
                {
                    newSelf.Mods.Add(inf.Clone() as CardModificationInfo);
                }
                target.SetInfo(newSelf);

                yield return new WaitForSeconds(0.1f);

                //I have no mouth, and I must modify cards
                int powerForOrig = 0;
                int powerForBehind = 0;
                int healthForOrig = 0;
                int healthForBehind = 0;
                if (target.Info.SpecialStatIcon == SpecialStatIcon.None || !StatIconInfo.GetIconInfo(target.Info.SpecialStatIcon).appliesToAttack)
                {
                    powerForOrig = (target.Attack / 2) + (target.Attack.IsEven() ? 0 : 1);
                    powerForBehind = Mathf.FloorToInt((float)target.Attack / 2f);
                }
                if (target.Info.SpecialStatIcon == SpecialStatIcon.None || !StatIconInfo.GetIconInfo(target.Info.SpecialStatIcon).appliesToHealth)
                {
                    healthForOrig = (target.Health / 2) + (target.Health.IsEven() ? 0 : 1);
                    healthForBehind = Mathf.FloorToInt((float)target.Health / 2f);
                }

                List<Ability> removeFromOrig = new List<Ability>();
                List<Ability> removeFromBehind = new List<Ability>();
                List<Ability> abilities = target.GetAllAbilities();
                int iterator = 2;
                foreach (Ability checking in abilities)
                {
                    if (iterator.isEven()) { removeFromBehind.Add(checking); }
                    else { removeFromOrig.Add(checking); }
                    iterator++;
                }


                target.AddTemporaryMod(new CardModificationInfo(
                    -(target.Attack - powerForOrig),
                    -(target.Health - healthForOrig)));
                tail.AddTemporaryMod(new CardModificationInfo(
                    -(tail.Attack - powerForBehind),
                    -(tail.Health - healthForBehind)));

                target.AddTemporaryMod(new CardModificationInfo() { negateAbilities = removeFromOrig });
                tail.AddTemporaryMod(new CardModificationInfo() { negateAbilities = removeFromBehind });
                target.Status.hiddenAbilities.AddRange(removeFromOrig);
                tail.Status.hiddenAbilities.AddRange(removeFromBehind);

                yield return new WaitForSeconds(0.1f);

                if (Tools.GetActAsInt() == 1)
                {
                    target.Info.Mods.Add(new CardModificationInfo()
                    {
                        DecalIds =
                            {
                                AlternatingBloodDecal.GetBloodDecalId(),
                                 "decal_stitches"
                            }});
                    tail.Info.Mods.Add(new CardModificationInfo()
                    {
                        DecalIds =
                            {
                                AlternatingBloodDecal.GetBloodDecalId(),
                                 "decal_stitches"
                            }
                    });
                }
                tail.RenderCard();
                target.RenderCard();

                yield return Singleton<BoardManager>.Instance.ResolveCardOnBoard(tail, slot, 0.1f, null, true);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.2f);
                tail.Anim.StrongNegationEffect();
                if (tail.Health == 0 && !tail.Dead)
                {
                    yield return tail.Die(false, base.Card);
                }
                yield return base.StartCoroutine(base.LearnAbility(0.5f));
                yield return new WaitForSeconds(0.2f);
            }
            yield break;
        }

    }
}
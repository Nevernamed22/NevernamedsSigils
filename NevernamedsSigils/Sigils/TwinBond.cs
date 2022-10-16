using APIPlugin;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Sirenix;

namespace NevernamedsSigils
{
    public class TwinBond : AbilityBehaviour
    {
        public static void Init()
        {
            AbilityInfo newSigil = SigilSetupUtility.MakeNewSigil("Twin Bond", "When [creature] is played, a copy of it is created adjacent, killing other creatures to make room if there is none. These cards share a health pool, and when either card dies, it's twin will die as well.",
                      typeof(TwinBond),
                      categories: new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular },
                      powerLevel: 4,
                      stackable: false,
                      opponentUsable: false,
                      tex: Tools.LoadTex("NevernamedsSigils/Resources/Sigils/twinbond.png"),
                      pixelTex: Tools.LoadTex("NevernamedsSigils/Resources/PixelSigils/twinbond_pixel.png"));

            TwinBond.ability = newSigil.ability;
        }
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }
        public static Ability ability;
        private IEnumerator SpawnCardOnSlot(CardSlot slot)
        {

            CardInfo inf = Tools.TrueClone(base.Card.Info);
            inf.mods.Add(new CardModificationInfo() { negateAbilities = new List<Ability>() { TwinBond.ability } });
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(inf, slot, 0.15f, true);
            while (slot.Card == null) yield return null;
            if (slot.Card)
            {
                //Debug.Log("Got into the if");
                PlayableCard twin = slot.Card;
                foreach (CardModificationInfo mod in Card.Info.Mods.FindAll((x) => x != null && x.negateAbilities.Contains(TwinBond.ability)))
                {
                    mod.negateAbilities.Remove(TwinBond.ability);
                    twin.Status.hiddenAbilities.Remove(TwinBond.ability);
                }
                twin.AddTemporaryMod(new CardModificationInfo(TwinBond.ability));
                twin.GetComponent<TwinBond>().twinCard = base.Card;
                twinCard = twin;
                twinset = true;
                twin.GetComponent<TwinBond>().twinset = true;
                twin.GetComponent<TwinBond>().twindamagetakenlastchecked = base.Card.Status.damageTaken;
                twindamagetakenlastchecked = twin.Status.damageTaken;

            }
            yield break;
        }
        public override bool RespondsToResolveOnBoard()
        {
            return !twinset;
        }
        public PlayableCard twinCard;
        public bool twinset = false;
        public IEnumerator RecalculateTwinStatus()
        {
            if (twinCard != null && !twinCard.Dead)
            {
                if (twinCard.Status.damageTaken != twindamagetakenlastchecked)
                {
                    if (twinCard.Status.damageTaken != base.Card.Status.damageTaken)
                    {
                        base.Card.Status.damageTaken = twinCard.Status.damageTaken;
                    }
                    twindamagetakenlastchecked = twinCard.Status.damageTaken;
                }
            }
            if (twinCard == null || twinCard.Dead || !twinCard.OnBoard)
            {               
                if (!base.Card.Dead) yield return base.Card.Die(false);
            }
            yield break;
        }
        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return base.Card.OnBoard;
        }
        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return RecalculateTwinStatus();
            yield break;
        }
        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return base.Card.OnBoard;
        }
        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return RecalculateTwinStatus();
            yield break;
        }
        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return base.Card.OnBoard;
        }
        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return RecalculateTwinStatus();
            yield break;
        }
        int twindamagetakenlastchecked = 0;
        public override IEnumerator OnResolveOnBoard()
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            CardSlot toLeft = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, true);
            CardSlot toRight = Singleton<BoardManager>.Instance.GetAdjacent(base.Card.Slot, false);
            yield return new WaitForSeconds(0.1f);

            yield return base.PreSuccessfulTriggerSequence();

            if (toLeft != null && toLeft.Card == null)
            {
                yield return this.SpawnCardOnSlot(toLeft);
            }
            else if (toRight != null && toRight.Card == null)
            {
                yield return this.SpawnCardOnSlot(toRight);
            }
            else if (toLeft != null)
            {
                yield return toLeft.Card.Die(false);
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toLeft);
            }
            else if (toRight != null)
            {
                yield return toLeft.Card.Die(false);
                yield return new WaitForSeconds(0.1f);
                yield return this.SpawnCardOnSlot(toRight);
            }

            yield return base.LearnAbility(0f);

            yield break;
        }
    }
}
